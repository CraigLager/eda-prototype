using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

// hook up dependency inversion - register interfaces against concrete types
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<UserDomain.Common.Events.IMessageBus, UserDomain.MessageBus.MessageBus>();
        services.AddScoped<UserDomain.Common.Repositories.IRepository<UserDomain.Entities.User>, UserDomain.Entities.UserRepository>();
        services.AddScoped<UserDomain.Entities.UserService>();
        services.AddScoped<UserDomain.Common.Encryption.IEncryptor, UserDomain.Encryption.Encyptor>();
    })
    .Build();

var scope = host.Services.CreateScope();

var messageBus = scope.ServiceProvider.GetService<UserDomain.Common.Events.IMessageBus>();

// simulate an event listener
messageBus.OnUserEvent += MessageBus_DebugUserEvent;
messageBus.OnUserEvent += MessageBus_WriteUserEvent;

// simulate some UI work - create and update some users using the service layer
var userService = scope.ServiceProvider.GetService<UserDomain.Entities.UserService>();
var repo = scope.ServiceProvider.GetService<UserDomain.Common.Repositories.IRepository<UserDomain.Entities.User>>();

Console.WriteLine("-------------------------------------");
Console.WriteLine("UI INTERACTIONS");
var user1 = userService.Create(new UserDomain.Common.UserData.UserData() { FirstName = "F1", LastName = "L1" });
Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(repo.GetById(user1.Id)));

var user2 = userService.Create(new UserDomain.Common.UserData.UserData() { FirstName = "F11", LastName = "L11" });
Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(repo.GetById(user2.Id)));

user1 = userService.Update(user1.Id, new UserDomain.Common.UserData.UserData() { FirstName = "F2", LastName = "L2" });
Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(repo.GetById(user1.Id)));

user1 = userService.Update(user1.Id, new UserDomain.Common.UserData.UserData() { FirstName = "F3", LastName = "L3" });
Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(repo.GetById(user1.Id)));

user1 = userService.Update(user1.Id, new UserDomain.Common.UserData.UserData() { FirstName = "CRAIG", LastName = "LAGER" });
Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(repo.GetById(user1.Id)));

user2 = userService.Update(user2.Id, new UserDomain.Common.UserData.UserData() { FirstName = "F21", LastName = "L21" });
Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(repo.GetById(user2.Id)));

Console.WriteLine("-------------------------------------");
Console.WriteLine("USER 1 HISTORY");
Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(repo.GetById(user1.Id, 1)));
Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(repo.GetById(user1.Id, 2)));
Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(repo.GetById(user1.Id, 3)));
Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(repo.GetById(user1.Id, 4)));
Console.WriteLine("-------------------------------------");

// check the data (todo: move to unit test)
userService.Delete(user1.Id);
Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(repo.GetById(user1.Id)));


Console.ReadLine();


/// <summary>
/// Event handler - outputs all events raised to debut output
/// </summary>
void MessageBus_DebugUserEvent(object? sender, UserDomain.Common.Events.IUserEvent e)
{
    Debug.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(e));
}

void MessageBus_WriteUserEvent(object? sender, UserDomain.Common.Events.IUserEvent e)
{
    Console.ForegroundColor = e.Message == "CREATE" ?  ConsoleColor.Green : Console.ForegroundColor;
    Console.ForegroundColor = e.Message == "UPDATE" ? ConsoleColor.Blue : Console.ForegroundColor;
    Console.ForegroundColor = e.Message == "DELETE" ? ConsoleColor.Red : Console.ForegroundColor;

    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(e));
    Console.ResetColor();
}