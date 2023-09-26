using EFCoreRelationshipsDemo.Dtos;
using EFCoreRelationshipsDemo.EntityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreRelationshipsDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly DataContext _context;
        public CharacterController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Character>>> Get(int userId)
        {
            var characters = await _context.Characters
                .Where(x => x.UserId == userId)
                
                .Include(w => w.Weapon)
                .Include(w => w.Skills)
                .ToListAsync();

            return Ok(characters);
        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> Create(CreateCharacterDto input)
        {
            var character = new Character
            {
                UserId = input.UserId,
                Name = input.Name,
                RpgClass = input.RpgClass
            };
            _context.Characters.Add(character);
            await _context.SaveChangesAsync();
            return await Get(character.UserId);

        }

        [HttpPost("weapon")]
        public async Task<ActionResult<Character>> AddWeapon(AddWeaponDto input)
        {
            var character = await _context.Characters.FindAsync(input.CharacterId);
            if (character is null)
                return NotFound();


            var weapon = new Weapon
            {
                Name = input.Name,
                Damage = input.Damage,
                Character = character
            };
            _context.Weapons.Add(weapon);
            await _context.SaveChangesAsync();
            return character;

        }

        [HttpPost("skill")]
        public async Task<ActionResult<Character>> AddCharacterSkill(AddCharacterSkillDto input)
        {
            var character = await _context.Characters.Where(x => x.Id == input.CharacterId)
                .Include(x => x.Skills)
                .FirstOrDefaultAsync();
            if (character is null)
                return NotFound();

            var skill = await _context.Skills.FindAsync(input.SkillId);
            if (skill is null)
                return NotFound();

            character.Skills.Add(skill);
            await _context.SaveChangesAsync();
            return character;

        }


    }
}
