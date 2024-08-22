// using Confluent.Kafka;

// namespace KafkaExample.Services
// {
//     public class KafkaProducerService
//     {
//         private readonly IProducer<Null, string> _producer;

//         public KafkaProducerService(ProducerConfig config)
//         {
//             _producer = new ProducerBuilder<Null, string>(config).Build();
//         }

//         public async Task SendMessageAsync(string topic, string message)
//         {
//             var kafkaMessage = new Message<Null, string> { Value = message };
//             await _producer.ProduceAsync(topic, kafkaMessage);
//         }
//     }
// }
