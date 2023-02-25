/*
 * Точка входа в сервис ooodb-mongo-server
 * **/

using MongoDB.Driver;
using oodb_mongo_server.database.config;
using oodb_mongo_server.database.context;
using oodb_project.controllers;

// Создание приложения
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Создание экземпляра MongoClient (подключение к БД)
MongoClient client = new MongoClient("mongodb://root:example@localhost:27017");

// Выпод коллекций из базы данных (проверка работы подключения)
using (var cursor = await client.ListDatabasesAsync())
{
    var databases = cursor.ToList();
    foreach (var database in databases)
    {
        Console.WriteLine(database);
    }
}

// Получение контекста базы данных на основе собранной конфигурации
var context = new DbContext(new MongoDbConfig(null)
{
    Database = "oodb",
    Host = "localhost",
    Password = "example",
    User = "root",
    Port = 27017
});

// Инициализация маршрутов API сервиса
var initMongoController = new InitMongoController(app, context);
initMongoController.InitRoutes();

// Запуск сервиса
app.Run();
