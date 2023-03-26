using AutoMapper;
using Pomodoro.Core.Enums;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models;
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

        public async Task<FrequencyModel> CreateFrequencyAsync(FrequencyModel freqModel)
        {
            if (freqModel == null)
            {
                throw new ArgumentNullException(nameof(freqModel), "Can`t be Null.");
            }

            var freqTypeId = await GetFrequencyTypeIdAsync(freqModel.FrequencyValue);

            if (freqTypeId == Guid.Empty)
            {
                throw new InvalidOperationException("Can`t find frequency type in db.");
            }

            var freq = _mapper.Map<Frequency>(freqModel);

            freq.FrequencyTypeId = freqTypeId;

            await _freqRepo.AddAsync(freq);

            return _mapper.Map<FrequencyModel>(freq);
        }

        public async Task<Guid> GetFrequencyIdAsync(FrequencyModel freqModel)
        {
            if (freqModel == null)
            {
                throw new ArgumentNullException(nameof(freqModel), "Can`t be Null.");
            }

            var freqTypeId = await GetFrequencyTypeIdAsync(freqModel.FrequencyValue);

            if (freqTypeId == Guid.Empty)
            {
                throw new InvalidOperationException("Can`t find frequency with frequency type in db.");
            }

            var freqs = await _freqRepo.FindAsync(f => 
            f.FrequencyTypeId == freqTypeId && 
            f.IsCustom == freqModel.IsCustom && 
            f.Every == freqModel.Every);

            var freq = freqs.FirstOrDefault();

            if (freq == null)
            {
                throw new InvalidOperationException("Can`t find frequency in db.");
            }

            return freq.Id;
        }

        public async Task<FrequencyModel> UpdateFrequencyAsync(FrequencyModel freqModel)
        {
            var freq = await _freqRepo.GetByIdAsync(freqModel.Id);

            if (freq == null || freq.FrequencyType == null)
            {
                throw new InvalidOperationException("Can`t find frequency with frequency type in db.");
            }

            if (freq.FrequencyType.Value != freqModel.FrequencyValue)
            {

                var freqTypeId = await GetFrequencyTypeIdAsync(freqModel.FrequencyValue);

                if (freqTypeId == Guid.Empty)
                {
                    throw new InvalidOperationException("Can`t find frequency type in db.");
                }

                freq.FrequencyTypeId = freqTypeId;
            }

            freq.IsCustom = freqModel.IsCustom;
            freq.Every = freqModel.Every;

            _freqRepo.Update(freq);

            return _mapper.Map<FrequencyModel>(freq);
        }

        public async Task DeleteFrequencyAsync(FrequencyModel freqModel)
        {
            var freq = await _freqRepo.GetByIdAsync(freqModel.Id);

            if (freq == null)
            {
                throw new InvalidOperationException("Can`t find frequency in db.");
            }

            _freqRepo.Remove(freq);
        }

        private async Task<Guid> GetFrequencyTypeIdAsync(FrequencyValue freqValue)
        {
            var freqTypes = await _freqTypeRepo.FindAsync(ft => ft.Value == freqValue);

            return freqTypes.Select(ft => ft.Id).FirstOrDefault();
        }
    }
}
