using System;

namespace MessageInspector
{
	public class User
	{
		public readonly long id;
		public bool stringName;
		public String name;
		
		public User(long id)
		{
			this.id = id;
			this.stringName = false;
			this.name = id.ToString();
		}
		
		public void Lookup() {
			if(this.stringName)
				return;
			if(new Twitter().Lookup(id, out name)) {
				stringName = true;
			} else {
				name = id.ToString();
				stringName = false;
			}
		}
	}
}
