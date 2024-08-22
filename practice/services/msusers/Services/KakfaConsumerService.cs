// using Confluent.Kafka;

// namespace KafkaExample.Services
// {
//     public class KafkaConsumerService
//     {
//         private readonly IConsumer<Null, string> _consumer;

//         public KafkaConsumerService(ConsumerConfig config)
//         {
//             _consumer = new ConsumerBuilder<Null, string>(config).Build();
//         }

//         public void ConsumeMessages(string topic)
//         {
//             _consumer.Subscribe(topic);

//             while (true)
//             {
//                 var consumeResult = _consumer.Consume();
//                 Console.WriteLine($"Consumed message '{consumeResult.Value}' from topic '{topic}'");
//             }
//         }
//     }
// }
