using Confluent.Kafka;
using consumidor.config;
using consumidor.service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ConsumerConfig>(ConsumerConfiguration.getConsumerCofig());
builder.Services.AddHostedService<KafkaConsumerService>();//hosted : um seviço que roda em background, essencial para aplicações que precisam consumir mensagens de forma contínua, como é o caso de um consumidor Kafka.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();