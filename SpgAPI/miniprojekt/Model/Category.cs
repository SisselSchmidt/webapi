using System;

namespace Model
{
	public class Category
	{
		public Category(string name)
		{
			this.Name = name;
		}

		public long CategoryId { get; set; }
		public string Name { get; set; }

		public List<Questions> Questions { get; set; } = new List<Questions>();
	}
}
