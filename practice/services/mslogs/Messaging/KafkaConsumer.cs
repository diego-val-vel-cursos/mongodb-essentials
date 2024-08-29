using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Practice.Services.mslogs.Models;
using Practice.Services.mslogs.Services;
using System;
using System.Threading;
using System.Text.Json;

namespace Practice.Services.mslogs.Messaging
{
    public class KafkaConsumer
    {
        private readonly IConsumer<Null, string> _consumer;
        private readonly LogService _logService;

        public KafkaConsumer(IOptions<KafkaSettings> kafkaSettings, LogService logService)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = kafkaSettings.Value.BootstrapServers,
                GroupId = "mslogs-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Null, string>(config).Build();
            _consumer.Subscribe(kafkaSettings.Value.Topic);
            _logService = logService;

            Console.WriteLine("KafkaConsumer initialized and subscribed to topic: " + kafkaSettings.Value.Topic);
        }

        public void Consume(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = _consumer.Consume(cancellationToken);
                    Console.WriteLine($"Received message: {result.Message.Value}");

                    try
                    {
                        // Intentar deserializar el mensaje recibido
                        var logEvent = JsonSerializer.Deserialize<Log>(result.Message.Value);

                        if (logEvent != null)
                        {
                            // Guardar el evento en la base de datos de MongoDB
                            _logService.CreateAsync(logEvent).Wait();
                            Console.WriteLine("Log saved successfully in MongoDB.");
                        }
                        else
                        {
                            Console.WriteLine("Deserialization resulted in a null object.");
                        }
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"Error deserializing message: {jsonEx.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error saving log to MongoDB: {ex.Message}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Consumption loop cancelled. Closing consumer...");
                _consumer.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in Consume method: {ex.Message}");
            }
        }
    }

    public class KafkaSettings
    {
        public string BootstrapServers { get; set; } = null!;
        public string Topic { get; set; } = null!;
    }
}
