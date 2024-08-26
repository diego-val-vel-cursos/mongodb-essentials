using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Practice.Services.msusers.Messaging
{
    public class KafkaProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly string _topic;

        public KafkaProducer(IOptions<KafkaSettings> kafkaSettings)
        {
            var config = new ProducerConfig { BootstrapServers = kafkaSettings.Value.BootstrapServers };
            _producer = new ProducerBuilder<Null, string>(config).Build();
            _topic = kafkaSettings.Value.Topic;
        }

        public async Task ProduceEventAsync<T>(T @event)
        {
            var message = new Message<Null, string> { Value = JsonSerializer.Serialize(@event) };
            await _producer.ProduceAsync(_topic, message);
        }
    }

    public class KafkaSettings
    {
        public string BootstrapServers { get; set; } = null!;
        public string Topic { get; set; } = null!;
    }
}
