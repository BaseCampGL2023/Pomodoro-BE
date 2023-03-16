using AutoMapper;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models.Frequency;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.Services.Services.Realizations
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
            this._mapper = mapper;
            this._freqTypeRepo = freqTypeRepo;
            this._freqRepo = freqRepo;
        }

        public async Task<Guid> GetFrequencyId(FrequencyModel freqModel)
        {
            Guid freqId = await FindFrequencyId(freqModel);

            if (freqId == Guid.Empty)
            {
                freqId = await AddFrequencyAsync(freqModel);
            }

            return freqId;
        }

        public async Task<Guid> FindFrequencyTypeId(FrequencyModel freq)
        {
            var freqTypeData = await _freqTypeRepo.FindAsync(x =>
                x.Value == freq.FrequencyTypeValue
            );

            return freqTypeData.FirstOrDefault().Id;
        }

        public async Task<Guid> FindFrequencyId(FrequencyModel freq)
        {
            Guid freqTypeId = await FindFrequencyTypeId(freq);

            var freqData = await _freqRepo.FindAsync(
                f => f.FrequencyTypeId == freqTypeId
                && f.IsCustom == freq.IsCustom && f.Every == freq.Every
            );

            if (freqData.Count() == 0)
            {
                return Guid.Empty;
            }
            return freqData.FirstOrDefault() == null ? Guid.Empty : freqData.FirstOrDefault().Id;


        }

        public async Task<IEnumerable<FrequencyModel>> FindAllFrequenciesAsync(FrequencyModel freq)
        {
            var freqId = await FindFrequencyTypeId(freq);
            var result = await _freqRepo.FindAsync(
                f => f.FrequencyTypeId == freqId &&
                f.Every == freq.Every && f.IsCustom == freq.IsCustom
            );

            return _mapper.Map<IEnumerable<Frequency>, IEnumerable<FrequencyModel>>(result);
        }

        public async Task<Guid> AddFrequencyAsync(FrequencyModel freq)
        {
            Guid freqTypeId = await FindFrequencyTypeId(freq);

            Frequency newFreq = _mapper.Map<FrequencyModel, Frequency>(freq);

            if (freqTypeId != Guid.Empty)
            {
                newFreq.FrequencyTypeId = freqTypeId;
            }

            await _freqRepo.AddAsync(newFreq);
            await _freqRepo.SaveChangesAsync();
            return newFreq.Id;
        }
    }
}
