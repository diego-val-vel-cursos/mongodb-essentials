using Microsoft.AspNetCore.Mvc;
using Practice.Services.mslogs.Models;
using Practice.Services.mslogs.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Practice.Services.mslogs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly LogService _logService;

        public LogController(LogService logService)
        {
            _logService = logService;
        }

        // GET: api/Log
        [HttpGet]
        public async Task<ActionResult<List<Log>>> GetLogs()
        {
            var logs = await _logService.GetAsync();
            return Ok(logs);
        }

        // GET: api/Log/:id
        [HttpGet("{id:length(24)}", Name = "GetLogById")]
        public async Task<ActionResult<Log>> GetLogById(string id)
        {
            var log = await _logService.GetAsync(id);

            if (log == null)
            {
                return NotFound();
            }

            return Ok(log);
        }

        // POST: api/Log
        [HttpPost]
        public async Task<ActionResult<Log>> CreateLog(Log log)
        {
            await _logService.CreateAsync(log);
            return CreatedAtRoute("GetLogById", new { id = log.Id.ToString() }, log);
        }

        // PUT: api/Log/:id
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateLog(string id, Log updatedLog)
        {
            var log = await _logService.GetAsync(id);

            if (log == null)
            {
                return NotFound();
            }

            updatedLog.Id = log.Id;
            await _logService.UpdateAsync(id, updatedLog);

            return NoContent();
        }

        // DELETE: api/Log/:id
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteLog(string id)
        {
            var log = await _logService.GetAsync(id);

            if (log == null)
            {
                return NotFound();
            }

            await _logService.RemoveAsync(id);

            return NoContent();
        }
    }
}
