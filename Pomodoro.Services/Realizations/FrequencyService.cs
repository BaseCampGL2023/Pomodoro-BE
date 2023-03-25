using AutoMapper;
using Pomodoro.Core.Enums;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models.Frequency;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.Services.Realizations
{
    public class FrequencyService : IFrequencyService
    {
        private readonly IFrequencyTypeRepository _freqTypeRepo;
        private readonly IFrequencyRepository _freqRepo;
        private readonly IMapper _mapper;
        public FrequencyService(
            IFrequencyTypeRepository freqTypeRepo,
            IFrequencyRepository freqRepo,
            IMapper mapper)
        {
            _mapper = mapper;
            _freqTypeRepo = freqTypeRepo;
            _freqRepo = freqRepo;
        }

        public async Task<FrequencyModel?> CreateFrequencyAsync(FrequencyModel freqModel)
        {
            if (freqModel == null)
            {
                return null;
            }

            var freqType = await _freqTypeRepo.FindAsync(ft => ft.Value == freqModel.FrequencyValue);

            if (freqType == null)
            {
                return null;
            }

            var freqTypeId = freqType.Select(ft => ft.Id).FirstOrDefault();

            var freq = _mapper.Map<Frequency>(freqModel);

            freq.FrequencyTypeId = freqTypeId;

            await _freqRepo.AddAsync(freq);
            await _freqRepo.SaveChangesAsync();

            return _mapper.Map<FrequencyModel>(freq);
        }

        public async Task DeleteFrequencyAsync(FrequencyModel freqModel)
        {
            var freq = await _freqRepo.GetByIdAsync(freqModel.Id);

            if (freq == null)
            {
                return;
            }

            _freqRepo.Remove(freq);
            await _freqRepo.SaveChangesAsync();
        }

        public async Task<FrequencyModel?> UpdateFrequencyAsync(FrequencyModel freqModel)
        {
            var freq = await _freqRepo.GetByIdAsync(freqModel.Id);

            if (freq == null || freq.FrequencyType == null)
            {
                return null;
            }

            if(freq.FrequencyType.Value != freqModel.FrequencyValue)
            {
                var freqType = await _freqTypeRepo.FindAsync(ft => ft.Value == freqModel.FrequencyValue);

                if (freqType == null)
                {
                    return null;
                }

                freq.FrequencyTypeId = freqType.Select(ft => ft.Id).FirstOrDefault();
            }

            freq.IsCustom = freqModel.IsCustom;
            freq.Every = freqModel.Every;

            _freqRepo.Update(freq);
            await _freqRepo.SaveChangesAsync();

            return _mapper.Map<FrequencyModel>(freq);
        }
    }
}
