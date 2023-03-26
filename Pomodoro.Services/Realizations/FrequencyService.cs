using AutoMapper;
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

            var freqTypes = await _freqTypeRepo.FindAsync(ft => ft.Value == freqModel.FrequencyValue);

            if (freqTypes == null)
            {
                throw new InvalidOperationException("Can`t find frequency type in db.");
            }

            var freqTypeId = freqTypes.Select(ft => ft.Id).FirstOrDefault();

            var freq = _mapper.Map<Frequency>(freqModel);

            freq.FrequencyTypeId = freqTypeId;

            await _freqRepo.AddAsync(freq);

            return _mapper.Map<FrequencyModel>(freq);
        }

        public async Task<FrequencyModel> GetFrequencyByIdAsync(Guid freqId) 
        {
            var freq = await _freqRepo.GetByIdAsync(freqId);

            if (freq == null)
            {
                throw new InvalidOperationException("Can`t find frequency in db.");
            }

            return _mapper.Map<FrequencyModel>(freq);
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
                var freqType = await _freqTypeRepo.FindAsync(ft => ft.Value == freqModel.FrequencyValue);

                if (freqType == null)
                {
                    throw new InvalidOperationException("Can`t find frequency type in db.");
                }

                freq.FrequencyTypeId = freqType.Select(ft => ft.Id).FirstOrDefault();
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
    }
}
