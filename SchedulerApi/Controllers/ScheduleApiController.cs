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
    }
}
