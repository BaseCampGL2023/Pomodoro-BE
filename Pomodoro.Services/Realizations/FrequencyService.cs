using AutoMapper;
using Microsoft.Extensions.Logging;
using Pomodoro.Core.Enums;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Pomodoro.Services.Realizations
{
    public class FrequencyService : IFrequencyService
    {
        private readonly IFrequencyTypeRepository _freqTypeRepo;
        private readonly IFrequencyRepository _freqRepo;
        private readonly IMapper _mapper;
        private readonly ILogger _log;

        public FrequencyService(
            IFrequencyTypeRepository freqTypeRepo,
            IFrequencyRepository freqRepo,
            IMapper mapper,
            ILogger logger)
        {
            _mapper = mapper;
            _freqTypeRepo = freqTypeRepo;
            _freqRepo = freqRepo;
            _log = logger;
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

            try
            {
                await _freqRepo.AddAsync(freq);
            }
            catch (Exception e)
            {
                _log.LogError(e.Message + " - occureed while adding frequency to db.");
                throw;
            }

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

            try
            {
                _freqRepo.Update(freq);
            }
            catch (Exception e)
            {
                _log.LogError(e.Message + " - occureed while updating frequency in db.");
                throw;
            }

            return _mapper.Map<FrequencyModel>(freq);
        }

        public async Task DeleteFrequencyAsync(FrequencyModel freqModel)
        {
            var freq = await _freqRepo.GetByIdAsync(freqModel.Id);

            if (freq == null)
            {
                throw new InvalidOperationException("Can`t find frequency in db.");
            }

            try
            {
                _freqRepo.Remove(freq);
            }
            catch (Exception e)
            {
                _log.LogError(e.Message + " - occureed while deleting frequency from db.");
                throw;
            }
        }

        private async Task<Guid> GetFrequencyTypeIdAsync(FrequencyValue freqValue)
        {
            var freqTypes = await _freqTypeRepo.FindAsync(ft => ft.Value == freqValue);

            return freqTypes.Select(ft => ft.Id).FirstOrDefault();
        }
    }
}
