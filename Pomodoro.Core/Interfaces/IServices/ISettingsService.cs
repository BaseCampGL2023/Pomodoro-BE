using Pomodoro.Core.Models;

namespace Pomodoro.Core.Interfaces.IServices
{
    public interface ISettingsService
    {
        Task<SettingsModel?> GetSettingsAsync(Guid id);
        Task<SettingsModel?> GetUserSettingsAsync(Guid userId);
        Task<SettingsModel> CreateSettingsAsync(SettingsModel settingsModel);
        Task<SettingsModel?> UpdateSettingsAsync(SettingsModel settingsModel);
    }
}
