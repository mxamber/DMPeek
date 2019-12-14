/*
 * Created by SharpDevelop.
 * User: SuperUser
 * Date: Mi, 11.12.2019
 * Time: 22:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace MessageInspector
{
	public class Message
	{
		public readonly String text;
		public readonly long id;
		public readonly User sender;
		public readonly User recipient;
		public readonly DateTime date;
		
		public Message(String text, long id, User sender, User recipient, DateTime date)
		{
			this.text = text;
			this.id = id;
			this.sender = sender;
			this.recipient = recipient;
			this.date = date;
		}
	}
}
