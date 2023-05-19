using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchedulerApi.Data;
using SchedulerApi.Model;
using SchedulerApi.Model.dto;

namespace SchedulerApi.Controllers
{
    [ApiController]
    [Route("api/ScheduleApi")]
    public class ScheduleApiController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ScheduleApiController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task< ActionResult<IEnumerable<ScheduleDTO>>> GetSchedules()
        {
            return Ok(await _db.Schedules.ToListAsync());
        }

        [HttpGet("{id:int}", Name = "GetSchedule")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task< ActionResult<ScheduleDTO>> GetSchedule(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var schedule = await _db.Schedules.FirstOrDefaultAsync(u => u.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }
            return Ok(schedule);
        }

        [HttpPost(Name = "CreateSchedule")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task< ActionResult<ScheduleCreateDTO>>CreateSchedule([FromBody]ScheduleCreateDTO createDTO) 
        {
            if(await _db.Schedules.FirstOrDefaultAsync(u=>u.Name.ToLower()== createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Schedule already exixts");
                return BadRequest(ModelState);
            }
            
            Schedule schedule= _mapper.Map<Schedule>(createDTO);

            await _db.Schedules.AddAsync(schedule);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetSchedule", new { id = schedule.Id }, schedule);
        }
        
        [HttpDelete("{id:int}",Name ="DeleteSchedule")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public async Task< ActionResult> DeleteSchedule(int id)
        {
            if(id== 0 )
            {
                return BadRequest();
            }
            if(await _db.Schedules.FirstOrDefaultAsync(u => u.Id != id) != null)
            {
                return BadRequest();
            }
            var schedule =await _db.Schedules.FirstOrDefaultAsync(u => u.Id == id);
            _db.Schedules.Remove(schedule);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut("{id:int}",Name ="UpdateSchedule")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public async Task< ActionResult<ScheduleUpdateDTO>>UpdateSchedule(int id,[FromBody]ScheduleUpdateDTO updateDTO)
        {
            if(id!=updateDTO.Id)
            {
                return BadRequest();
            }
            
            Schedule schedule=_mapper.Map<Schedule>(updateDTO);

            _db.Schedules.Update(schedule);
            await _db.SaveChangesAsync();
           return NoContent();
        }
    }
}
