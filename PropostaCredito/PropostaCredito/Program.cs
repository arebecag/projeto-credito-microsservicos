using PropostaCredito.Consumers;
using PropostaCredito.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ClienteCadastradoConsumer>();
builder.Services.AddSingleton<RabbitMQMessageBus>();

var app = builder.Build();

var clienteCadastradoConsumer = app.Services.GetRequiredService<ClienteCadastradoConsumer>();
Task.Run(() => clienteCadastradoConsumer.StartConsuming());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
