using System.ComponentModel.DataAnnotations;

namespace SchedulerApi.Model.dto
{
    public class ScheduleUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
