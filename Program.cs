using MongoDB.Driver;
using oodb_mongo_server.database.config;
using oodb_mongo_server.database.context;
using oodb_project.controllers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

MongoClient client = new MongoClient("mongodb://root:example@localhost:27017");

using (var cursor = await client.ListDatabasesAsync())
{
    var databases = cursor.ToList();
    foreach (var database in databases)
    {
        Console.WriteLine(database);
    }
}

var context = new DbContext(new MongoDbConfig(null)
{
    Database = "oodb",
    Host = "localhost",
    Password = "example",
    User = "root",
    Port = 27017
});

var initMongoController = new InitMongoController(app, context);
initMongoController.InitRoutes();

app.Run();
