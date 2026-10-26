using VotePreferentiel;
using VotePreferentiel.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHttpClient<VoteProvider>();
builder.Services.AddHostedService<Worker>();
builder.Build().Run();
