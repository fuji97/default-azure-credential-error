using System.Text.Json;
using Azure.Identity;
using Azure.ResourceManager;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => {
        services.AddAzureClients(builder => {
            builder.UseCredential(new DefaultAzureCredential());
            builder.AddClient<ArmClient, ArmClientOptions>((options, creds) => new ArmClient(creds));
        });
    })
    .Build();

var armClient = new ArmClient(new DefaultAzureCredential());
var sub = await armClient.GetDefaultSubscriptionAsync();
Console.WriteLine("Sub ID from direct DefaultAzureCredential(): " + sub.Id);

armClient = host.Services.GetRequiredService<ArmClient>();
sub = await armClient.GetDefaultSubscriptionAsync();
Console.WriteLine("Sub ID from DI: " + sub.Id);
