using Pomodoro.Core.Models.Frequency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Core.Interfaces.IServices
{
    public interface IFrequencyService
    {
        public Task<Guid> FindFrequencyTypeId(FrequencyModel freq);
        public Task<IEnumerable<FrequencyModel>> FindAllFrequenciesAsync(FrequencyModel freq);
        public Task<Guid> AddFrequencyAsync(FrequencyModel freq);
        public Task<Guid> FindFrequencyId(FrequencyModel freq);
    }
}
