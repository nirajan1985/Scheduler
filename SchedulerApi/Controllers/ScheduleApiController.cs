using Microsoft.AspNetCore.Mvc;
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
        public ScheduleApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<ScheduleDTO>> GetSchedules()
        {
            return Ok(_db.Schedules.ToList());
        }

        [HttpGet("{id:int}", Name = "GetSchedule")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<ScheduleDTO> GetSchedule(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var schedule = _db.Schedules.FirstOrDefault(u => u.Id == id);
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
        public ActionResult<ScheduleCreateDTO>CreateSchedule([FromBody]ScheduleCreateDTO createDTO) 
        {
            if(_db.Schedules.FirstOrDefault(u=>u.Name.ToLower()== createDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Schedule already exixts");
                return BadRequest(ModelState);
            }
            Schedule schedule = new()
            {
                Name = createDTO.Name,
                AppointmentDate = createDTO.AppointmentDate
            };
            _db.Schedules.Add(schedule);
            _db.SaveChanges();

            return CreatedAtRoute("GetSchedule", new { id = schedule.Id }, schedule);
        }
        
        [HttpDelete("{id:int}",Name ="DeleteSchedule")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteSchedule(int id)
        {
            if(id== 0)
            {
                return BadRequest();
            }
            var schedule = _db.Schedules.FirstOrDefault(u => u.Id == id);
            _db.Schedules.Remove(schedule);
            _db.SaveChanges();
            return NoContent();
        }
        [HttpPut("{id:int}",Name ="UpdateSchedule")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public ActionResult<ScheduleUpdateDTO>UpdateSchedule(int id,[FromBody]ScheduleUpdateDTO updateDTO)
        {
            if(id!=updateDTO.Id)
            {
                return BadRequest();
            }
            Schedule schedule = new Schedule()
            {
                Id = updateDTO.Id,
                Name = updateDTO.Name,
                AppointmentDate = updateDTO.AppointmentDate,
            };
            _db.Schedules.Update(schedule);
            _db.SaveChanges();
           return NoContent();
        }
    }
}
