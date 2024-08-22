// using KafkaExample.Services;
// using Microsoft.AspNetCore.Mvc;

// namespace KafkaExample.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class KafkaController : ControllerBase
//     {
//         private readonly KafkaProducerService _producerService;

//         public KafkaController(KafkaProducerService producerService)
//         {
//             _producerService = producerService;
//         }

//         [HttpPost("send")]
//         public async Task<IActionResult> SendMessage([FromQuery] string topic, [FromQuery] string message)
//         {
//             await _producerService.SendMessageAsync(topic, message);
//             return Ok(new { Message = "Message sent to Kafka", Topic = topic, Content = message });
//         }
//     }
// }
