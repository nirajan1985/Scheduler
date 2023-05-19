using AutoMapper;
using SchedulerApi.Model;
using SchedulerApi.Model.dto;

namespace SchedulerApi
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<Schedule,ScheduleDTO>().ReverseMap();
            CreateMap<Schedule,ScheduleCreateDTO>().ReverseMap();
            CreateMap<Schedule,ScheduleUpdateDTO>().ReverseMap();
        }
    }
}
