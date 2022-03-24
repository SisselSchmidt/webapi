using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Json;

using Data;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Swagger-halløj der tilføjer nogle udviklingsværktøjer direkte i app'en.
// Se mere her: https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS skal slåes til i app'en. Ellers kan man ikke hente data fra den
// fra et andet domæne.
// Se mere her: https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0
var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSomeStuff, builder => {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Tilføj DbContext factory som service.
// Det gør at man kan få TodoContext ind via dependecy injection - fx 
// i DataService (smart!)
builder.Services.AddDbContext<ProjektContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ProjektContextSQLite")));

// Kan vise flotte fejlbeskeder i browseren hvis der kommer fejl fra databasen
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Tilføj DataService så den kan bruges i endpoints
builder.Services.AddScoped<DataService>();

// Her kan man styrer hvordan den laver JSON.
builder.Services.Configure<JsonOptions>(options =>
{
    // Super vigtig option! Den gør, at programmet ikke smider fejl
    // når man returnerer JSON med objekter, der refererer til hinanden.
    // (altså dobbelrettede associeringer)
    options.SerializerOptions.ReferenceHandler = 
        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
}); 

// Byg app'ens objekt
var app = builder.Build();

// Seed data hvis nødvendigt
using (var scope = app.Services.CreateScope())
{
    // Med scope kan man hente en service.
    var dataService = scope.ServiceProvider.GetRequiredService<DataService>();
    dataService.SeedData(); // Fylder data på hvis databasen er tom.
}

// Sæt Swagger og alt det andet halløj op
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseCors(AllowSomeStuff);

// Middlware der kører før hver request. Alle svar skal have ContentType: JSON.
app.Use(async (context, next) =>
{
    context.Response.ContentType = "application/json; charset=utf-8";
    await next(context);
});

// Herunder alle endpoints i API'en
app.MapGet("/", (HttpContext context, DataService service) =>
{
    context.Response.ContentType = "text/html;charset=utf-8";
    return "Hejsa. Her er der intet at se. Prøv i stedet: " + 
            "<a href=\"/api/tasks\">/api/tasks</a>";
});

app.MapGet("/api/questions", (DataService service) =>
{
    return service.GetQuestions();
});

app.MapGet("/api/questions/{id}", (DataService service, int id) =>
{
    return service.GetQuestionById(id);
});

app.MapPost("/api/question/", (QuestionData data, DataService service) =>
{
    return service.CreateQuestion(data.date, data.headline, data.question, data.name);
});

app.MapGet("/api/users", (DataService service) =>
{
    return service.GetUsers();
});

app.MapGet("/api/users/{id}", (DataService service, int id) =>
{
    return service.GetUserById(id);
});

app.MapPost("/api/users/", (UserData data, DataService service) =>
{
    return service.CreateUser(data.name);
});

app.Run();

// Records til input data (svarende til input JSON)
record QuestionData(DateTime date, string headline, string question, string name);
record UserData(string name);