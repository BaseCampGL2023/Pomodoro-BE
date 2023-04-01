using Pomodoro.Core.Models;

namespace Pomodoro.Core.Interfaces.IServices
{
    public interface IFrequencyService
    {
        public Task<FrequencyModel?> CreateFrequencyAsync(FrequencyModel freqModel);
        public Task<Guid> GetFrequencyIdAsync(FrequencyModel freqModel);
        public Task<FrequencyModel?> UpdateFrequencyAsync(FrequencyModel freqModel);
        public Task DeleteFrequencyAsync(FrequencyModel freqModel);
    }
}
