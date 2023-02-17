using AutoMapper;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models.Frequency;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Enums;
using Pomodoro.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Services.Services
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

        public async Task<Guid> FindFrequencyTypeId(FrequencyModel freq)
        {
            var freqTypeData = await this.freqTypeRepo.FindAsync(x =>
                x.Value == (FrequencyValue)Enum.Parse(typeof(FrequencyValue), freq.FrequencyTypeValue)
            );

            return freqTypeData.FirstOrDefault().Id;
        }

        public async Task<Guid> FindFrequencyId(FrequencyModel freq)
        {
            Guid freqTypeId = await this.FindFrequencyTypeId(freq);

            var freqData = await this.freqRepo.FindAsync(
                f => f.FrequencyTypeId == freqTypeId
                && f.IsCustom == freq.IsCustom && f.Every == freq.Every
            );

            if (freqData.Count() == 0)
            {
                return Guid.Empty;
            } 
            return freqData.FirstOrDefault().Id;


        }

        public async Task<IEnumerable<FrequencyModel>> FindAllFrequenciesAsync(FrequencyModel freq)
        {
            var freqId = await this.FindFrequencyTypeId(freq);
            var result = await this.freqRepo.FindAsync(
                f => f.FrequencyTypeId == freqId &&
                f.Every == freq.Every && f.IsCustom == freq.IsCustom
            );

            return this.mapper.Map<IEnumerable<Frequency>, IEnumerable<FrequencyModel>>(result);
        }

        public async Task<Guid> AddFrequencyAsync(FrequencyModel freq)
        {
            Guid freqTypeId = await this.FindFrequencyTypeId(freq);

            Frequency newFreq = new Frequency();
            newFreq.Every = freq.Every;
            newFreq.IsCustom = freq.IsCustom;

            if (freqTypeId != Guid.Empty)
            {
                newFreq.FrequencyTypeId = freqTypeId;
            }

            Guid insertedFreqId = await this.freqRepo.AddFrequencyAsync(newFreq);
            await this.freqRepo.SaveChangesAsync();
            return insertedFreqId;
        } 
    }
}
