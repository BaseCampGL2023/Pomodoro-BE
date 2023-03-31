using AutoMapper;
using Pomodoro.Core.Enums;
using Pomodoro.Core.Models;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Services.Mapping
{

	public class FrequencyProfile : Profile
	{
        public FrequencyProfile()
        {
            CreateMap<Frequency, FrequencyModel>()
                .ForMember(dist => dist.FrequencyValue, act => act.MapFrom(src => GetFrequencyValue(src)));
            CreateMap<FrequencyModel, Frequency>()
                .ForMember(dist => dist.Id, act => act.MapFrom(src => Guid.Empty));

        }

        private FrequencyValue GetFrequencyValue(Frequency frequency)
		{
            if (frequency == null || frequency.FrequencyType == null)
                return 0;

            return frequency.FrequencyType.Value;
		}
	}
}