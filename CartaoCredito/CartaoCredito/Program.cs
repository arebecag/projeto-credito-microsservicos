using CartaoCredito.Consumers;
using CartaoCredito.Services;
using CartaoCredito.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<PropostaAprovadaConsumer>();
        services.AddSingleton<ClienteCadastradoConsumer>();
        services.AddSingleton<ICartaoCreditoService, CartaoCreditoService>();
        services.AddSingleton<RabbitMQMessageBus>();

        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    })
    .Build();

var propostaAprovadaConsumer = host.Services.GetRequiredService<PropostaAprovadaConsumer>();
Task.Run(() => propostaAprovadaConsumer.StartConsuming());

var clienteCadastradoConsumer = host.Services.GetRequiredService<ClienteCadastradoConsumer>();
Task.Run(() => clienteCadastradoConsumer.StartConsuming());

await host.RunAsync();
