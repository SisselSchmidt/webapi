using Microsoft.EntityFrameworkCore;
using System.Text.Json;

using Data;
using Model;

namespace Service;

public class DataService
{
    private ProjektContext db { get; }

    public DataService(ProjektContext db)
    {
        this.db = db;
    }
    /// <summary>
    /// Seeder noget nyt data i databasen hvis det er nødvendigt.
    /// </summary>
    public void SeedData()
    {
        User user = db.User.FirstOrDefault()!;
        if (user == null)
        {
            user = new User("Kristian");
            db.User.Add(user);
            db.User.Add(new User("Søren"));
        }

        Category category = db.Category.FirstOrDefault()!;
        if (category == null)
        {
            category = new Category("Alt muligt");
            db.Category.Add(category);
        }

        Questions questions = db.Questions.FirstOrDefault()!;
        if (questions == null)
        {
            var question = new Questions(DateTime.Now, "Hjælp mig", "Jeg kan ikke finde ud af at kode", 21, user);
            var answer = new Answers(DateTime.Now, "Det var træls", 1, user);
            question.Answers.Add(answer);
            question.Answers.Add(new Answers(DateTime.Now, "Så må du øve dig", 4, user));
            category.Questions.Add(question);
            db.Questions.Add(question);
        }

        db.SaveChanges();
    }

    // Metode til at hente alle spørgsmål
    public List<Questions> GetQuestions()
    {
        return db.Questions
            /*.Include(task => task.User)*/
            .ToList();
    } 

    //Metode til at hente enkelt spørgsmål ud fra ID
    public Questions GetQuestionById(int id)
    {
        var question = db
            .Questions
            .Where(question => question.QuestionsId == id)
            .Include(t => t.User)
            .First();
        return question;
    }

    public string CreateQuestion(DateTime date, string headline, string question, string name)
    {
        User user = db.User.Where(user => user.Name == name).First();
        Questions questions = new Questions(date, headline, question, 0, user);
        db.Questions.Add(questions);
        db.SaveChanges();
        return JsonSerializer.Serialize(
            new { msg = "New questions created", newQuestion = questions }
        );
    }

    public DbSet<User> GetUsers()
    {
        return db.User;
    }

    public User GetUserById(int id)
    {
        var user = db
               .User
               .Where(user => user.UserId == id)
               .First();
        return user;
    }

    public string CreateUser(string name)
    {
        var user = new User(name);
        db.User.Add(user);
        db.SaveChanges();
        return JsonSerializer.Serialize(
            new { msg = "New user created", newUser = user });
    }
}