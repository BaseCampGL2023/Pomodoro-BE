using AutoMapper;
using Pomodoro.Core.Interfaces.IServices;
using Pomodoro.Core.Models;
using Pomodoro.DataAccess.Entities;
using Pomodoro.DataAccess.Repositories.Interfaces;

namespace Pomodoro.Services.Realizations
{
    public class SettingsService : ISettingsService
    {
        private readonly IMapper mapper;
        private readonly ISettingsRepository settingsRepository;

        public SettingsService(
            IMapper mapper,
            ISettingsRepository settingsRepository)
        {
            this.mapper = mapper;
            this.settingsRepository = settingsRepository;
        }

        public async Task<SettingsModel> CreateSettingsAsync(SettingsModel settingsModel)
        {
            if (settingsModel is null)
            {
                throw new ArgumentNullException(nameof(settingsModel));
            }

            var settings = mapper.Map<Settings>(settingsModel);

            await RemoveExistingSettings(settings.UserId);
            await settingsRepository.AddAsync(settings);
            await settingsRepository.SaveChangesAsync();

            return mapper.Map<SettingsModel>(settings);
        }

        public async Task<SettingsModel?> GetSettingsAsync(Guid id)
        {
            var settings = await settingsRepository.GetByIdAsync(id);
            return mapper.Map<SettingsModel>(settings);
        }

        public async Task<SettingsModel?> GetUserSettingsAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("Argument value is all zeroes.", nameof(userId));
            }

            var settings = await settingsRepository
                .FindAsync(s => s.UserId == userId);
            
            return mapper.Map<SettingsModel>(settings?.FirstOrDefault());
        }

        public async Task<SettingsModel?> UpdateSettingsAsync(SettingsModel settingsModel)
        {
            if (settingsModel is null)
            {
                throw new ArgumentNullException(nameof(settingsModel));
            }

            var settingsToUpdate = await settingsRepository.GetByIdAsync(settingsModel.Id);
            if (settingsToUpdate is null)
            {
                return null;
            }

            var settings = mapper.Map<Settings>(settingsModel);

            settingsRepository.Update(settings);
            await settingsRepository.SaveChangesAsync();

            return mapper.Map<SettingsModel>(settings);
        }

        private async Task RemoveExistingSettings(Guid userId)
        {
            var settings = await settingsRepository
                .FindAsync(s => s.UserId == userId);

            if (settings?.Count() > 0)
            {
                settingsRepository.RemoveRange(settings);
                await settingsRepository.SaveChangesAsync();
            }   
        }
    }
}