using Microsoft.AspNetCore.Mvc;
using SchedulerApi.Data;
using SchedulerApi.Model.dto;

namespace SchedulerApi.Controllers
{
    [ApiController]
    [Route("api/ScheduleApi")]
    public class ScheduleApiController:ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public ScheduleApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult <IEnumerable<ScheduleDTO>> GetSchedules()
        {
            return Ok(_db.Schedules.ToList());
        }
    }
}
