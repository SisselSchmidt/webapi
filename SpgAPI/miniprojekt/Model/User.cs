using System;

namespace Model
{
	public class User
	{
		public long UserId { get; set; }
		public string Name { get; set; }

		public User(string name)
		{
			this.Name = name;
		}
	
	}
}
