using MongoDBEssentials.Models;
using MongoDBEssentials.Services;
using Microsoft.AspNetCore.Mvc;

namespace MongoDBEssentials.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketingMembersController : ControllerBase
    {
        private readonly MarketingMemberService _marketingMemberService;

        public MarketingMembersController(MarketingMemberService marketingMemberService) =>
            _marketingMemberService = marketingMemberService;


         // EJERCICIO 1 - CREAR UN MIEMBRO
        [HttpPost]
        public async Task<IActionResult> Post(MarketingMember newMember)
        {
            await _marketingMemberService.CreateAsync(newMember);

            return CreatedAtAction(nameof(Get), new { id = newMember.Id }, newMember);
        }


        // EJERCICIO 2 - OBTENER TODOS LOS MIEMBROS
        [HttpGet]
        public async Task<List<MarketingMember>> Get() =>
            await _marketingMemberService.GetAsync();

        // EJERCICIO 3 - OBTENER UN MIEMBRO POR ID
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<MarketingMember>> Get(string id)
        {
            var member = await _marketingMemberService.GetAsync(id);

            if (member is null)
            {
                return NotFound();
            }

            return member;
        }


        // EJERCICIO 4 - ACTUALIZAR UN MIEMBRO POR ID
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, MarketingMember updatedMember)
        {
            var member = await _marketingMemberService.GetAsync(id);

            if (member is null)
            {
                return NotFound();
            }

            updatedMember.Id = member.Id;

            await _marketingMemberService.UpdateAsync(id, updatedMember);

            return NoContent();
        }


        // EJERCICIO 5 - DELETE A MEMBER
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var member = await _marketingMemberService.GetAsync(id);

            if (member is null)
            {
                return NotFound();
            }

            await _marketingMemberService.RemoveAsync(id);

            return NoContent();
        }
    }

    
}
