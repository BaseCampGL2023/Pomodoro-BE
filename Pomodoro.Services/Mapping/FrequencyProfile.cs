using AutoMapper;
using Pomodoro.Core.Models.Frequency;
using Pomodoro.DataAccess.Entities;

namespace Pomodoro.Services.Mapping
{

	public class FrequencyProfile : Profile
	{

		public FrequencyProfile()
		{
			this.CreateMap<Frequency, FrequencyModel>()
				.ForMember(f => f.FrequencyTypeValue, o => o.MapFrom(s => s.FrequencyType.Value));
			this.CreateMap<FrequencyModel, Frequency>();
		}
	}
}