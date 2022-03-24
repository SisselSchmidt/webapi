using System;

namespace Model
{

	public class Questions
	{
		public Questions(DateTime date, string headline, string question, int rating, User user)
		{
			this.Date = date;
			this.Headline = headline;
			this.Question = question;
			this.Rating = rating;
			this.User = user;
		}

		public Questions(DateTime date, string headline, string question, int rating)
		{
			this.Date = date;
			this.Headline = headline;
			this.Question = question;
			this.Rating = rating;
		}

		public long QuestionsId { get; set; }
		public DateTime Date { get; set; }
		public string Headline { get; set; }
		public string Question { get; set; }
		public int Rating { get; set; }
		public User? User { get; set; }

		public List<Category> Category { get; set; } = new List<Category>();

		public List<Answers> Answers { get; set; } = new List<Answers>();
	}


}