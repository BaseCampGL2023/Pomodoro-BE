using AutoMapper;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models.Frequency;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.Services.Services.Realizations
{
    public class FrequencyService : IFrequencyService
    {
        private readonly IFrequencyTypeRepository freqTypeRepo;
        private readonly IFrequencyRepository freqRepo;
        private readonly IMapper mapper;
        public FrequencyService(
            IFrequencyTypeRepository freqTypeRepo,
            IFrequencyRepository freqRepo,
            IMapper mapper)
        {
            this.mapper = mapper;
            this.freqTypeRepo = freqTypeRepo;
            this.freqRepo = freqRepo;
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
            var freqTypeData = await freqTypeRepo.FindAsync(x =>
                x.Value == freq.FrequencyTypeValue
            );

            return freqTypeData.FirstOrDefault().Id;
        }

        public async Task<Guid> FindFrequencyId(FrequencyModel freq)
        {
            Guid freqTypeId = await FindFrequencyTypeId(freq);

            var freqData = await freqRepo.FindAsync(
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
            var result = await freqRepo.FindAsync(
                f => f.FrequencyTypeId == freqId &&
                f.Every == freq.Every && f.IsCustom == freq.IsCustom
            );

            return mapper.Map<IEnumerable<Frequency>, IEnumerable<FrequencyModel>>(result);
        }

        public async Task<Guid> AddFrequencyAsync(FrequencyModel freq)
        {
            Guid freqTypeId = await FindFrequencyTypeId(freq);

            Frequency newFreq = mapper.Map<FrequencyModel, Frequency>(freq);

            if (freqTypeId != Guid.Empty)
            {
                newFreq.FrequencyTypeId = freqTypeId;
            }

            await freqRepo.AddAsync(newFreq);
            await freqRepo.SaveChangesAsync();
            return newFreq.Id;
        }
    }
}
