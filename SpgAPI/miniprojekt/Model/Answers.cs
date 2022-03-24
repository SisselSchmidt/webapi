using System;

namespace Model
{

	public class Answers
	{
		public Answers(DateTime date, string answer, int rating, User user)
		{
			this.Date = date;
			this.Answer = answer;	
			this.Rating = rating;
			this.User = user;
		}
		public Answers(DateTime date, string answer, int rating)
		{
			this.Date = date;
			this.Answer = answer;
			this.Rating = rating;
		}

		public long AnswersId { get; set; }
		public DateTime Date { get; set; }
		public string Answer { get; set; }
		public int Rating { get; set; }
		public User? User { get; set; }
		public Questions Questions { get; set; }
	}
}
