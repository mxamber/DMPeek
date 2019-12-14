using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace MessageInspector
{
	public class Twitter
	{
		public static DmConversation ParseConversation(String input) {
			// is there even anything in { } brackets?
			// if no json -> return empty convo
			Regex json_regex = new Regex("(?'json'{.*})", RegexOptions.Singleline);
			Match json_match = json_regex.Match(input);
			if(!json_match.Groups["json"].Success)
				return null;
			
			// check for conversation id
			// if no conversation id -> return empty convo
			String json = json_match.Groups["json"].Value;
			Regex id_regex = new Regex("\"conversationId\"\\s*:\\s*\"(?'one'\\d*)-(?'two'\\d*)\"");
			Match id_match = id_regex.Match(json);
			if(!id_match.Success || !id_match.Groups["one"].Success || !id_match.Groups["two"].Success)
				return null;
			
			long id_one = Int64.Parse(id_match.Groups["one"].Value);
			long id_two = Int64.Parse(id_match.Groups["two"].Value);
			
			// find msg array
			Regex msg_ar_regex = new Regex("\"messages\"\\s*:\\s*\\[\\s*{.*}\\s*\\]", RegexOptions.Singleline);
			Match msg_ar_match = msg_ar_regex.Match(json);
			if(!msg_ar_match.Success)
				return null;
			
			
			// {\s*"messageCreate"[^}]*"recipientId"\s*:\s*"(?'recip'\d*)"[^}]*"text"\s*:\s*"(?'text'[^"]*)"[^}]*"senderId"\s*:\s*"(?'sender'\d*)"[^}]*"id"\s*:\s*"(?'id'\d*)"[^}]*"createdAt"\s*:\s*"(?'date'[^"]*)"[^}]*}
			// check for entire messageCreate objects, gather them in a match collection, return null if none are found
			Regex msg_regex = new Regex("{\\s*\"messageCreate\"[^}]*\"recipientId\"\\s*:\\s*\"(?'recip'\\d*)\"[^}]*\"text\"\\s*:\\s*\"(?'text'[^\"]*)\"[^}]*\"senderId\"\\s*:\\s*\"(?'sender'\\d*)\"[^}]*\"id\"\\s*:\\s*\"(?'id'\\d*)\"[^}]*\"createdAt\"\\s*:\\s*\"(?'date'[^\"]*)\"[^}]*}", RegexOptions.Singleline);
			MatchCollection msg_matches = msg_regex.Matches(msg_ar_match.Value);
			if(msg_matches.Count == 0)
				return null;
			
			DmConversation convo = new DmConversation(new User(id_one), new User(id_two));
			
			
			for(int i = msg_matches.Count -1; i > -1; i--){
				Match current = msg_matches[i];
				String text = current.Groups["text"].Value;
				long sender = Int64.Parse(current.Groups["sender"].Value);
				long recipient = Int64.Parse(current.Groups["recip"].Value);
				long id = Int64.Parse(current.Groups["id"].Value);
				DateTime sent = new DateTime(1970, 01, 01, 12, 00, 00);
				DateTime.TryParse(current.Groups["date"].Value, out sent);
				Message msg = new Message(text, id, new User(sender), new User(recipient), sent);
				convo.messages.Add(msg);
				
			}
			
			
			return convo;
		}
		
		
		public bool network;
		const String user_url = "https://twitter.com/intent/user?user_id=";
		readonly Regex nickname_regex = new Regex("<span[^>]*class=\\\"[^\\\"]*nickname[^\\\"]*\\\"[^>]*>(?'name'[^<]*)<\\/span>");
		
		public Twitter()
		{
			Ping();
		}
		
		public void Ping() {
			Ping ping = new Ping();
			try {
				PingReply resp = ping.Send("www.twitter.com", 1500);
				
				if(resp.Status == IPStatus.Success) {
					network = true;
				} else {
					network = false;
				}
			} catch {
				network = false;
			}
		}
		
		public bool Lookup(long id, out String store) {
			store = null;
			if(!network) {
				return false;
			}
			
			
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(user_url + id);
			req.UserAgent = "Console";
			Console.WriteLine(user_url + id);
			req.Method = "GET";
			WebResponse resp = null;
			
			try {
				resp = req.GetResponse();
				StreamReader reader = new StreamReader(resp.GetResponseStream(), System.Text.Encoding.UTF8);
				String res = reader.ReadToEnd();
				reader.Close();
				resp.Close();
				
				Console.WriteLine(res);
				
				Match match = nickname_regex.Match(res);
				if(match.Groups["name"].Success) {
					store = match.Groups["name"].Value;
					return true;
				}
				
				return false;
			} catch (Exception e) {
				Console.WriteLine("No lookup possible!");
				Console.WriteLine(e);
				return false;
			}
		}
	}
}
