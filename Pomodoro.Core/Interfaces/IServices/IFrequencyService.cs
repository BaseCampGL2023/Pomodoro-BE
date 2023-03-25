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
        public Task<FrequencyModel?> CreateFrequencyAsync(FrequencyModel freqModel);
        public Task<FrequencyModel?> UpdateFrequencyAsync(FrequencyModel freqModel);
        public Task DeleteFrequencyAsync(FrequencyModel freqModel);
    }
}
