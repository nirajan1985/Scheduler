using System.ComponentModel.DataAnnotations;

namespace SchedulerApi.Model.dto
{
    public class ScheduleCreateDTO
    {
        [Required]
        public string Name { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
