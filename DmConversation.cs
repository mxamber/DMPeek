using System;
using System.Collections.Generic;

namespace MessageInspector
{
	public class DmConversation
	{
		public List<Message> messages;
		public User userOne;
		public User userTwo;
		
		public DmConversation(User one, User two)
		{
			this.userOne = one;
			this.userTwo = two;
			messages = new List<Message> {};
		}
		
		public bool Involves(User user) {
			if(user.id == userTwo.id || user.id == userOne.id)
				return true;
			return false;
		}
		public bool Involves(long id) {
			return Involves(new User(id));
		}
		public bool Involves(String id_str) {
			long id;
			if(!Int64.TryParse(id_str, out id))
				return false;
			return Involves(id);
		}
	}
}
