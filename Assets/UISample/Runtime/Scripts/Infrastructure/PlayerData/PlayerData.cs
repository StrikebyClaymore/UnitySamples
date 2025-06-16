using Plugins.ServiceLocator;

namespace UISample.Infrastructure
{
    public class PlayerData : IService
    {
        public PlayerPrefsData Data { get; } = new();
        public readonly PlayerDataValue<float> SoundVolume;
        public readonly PlayerDataValue<float> MusicVolume;
        public readonly PlayerDataValue<float> UIVolume;
        public readonly PlayerDataValue<string> Language;
        public readonly PlayerDataValue<int> Acorns;
        public readonly PlayerDataValue<int> Gems;
        public readonly PlayerDataValue<bool> LastReward;

        public PlayerData(PlayerDataDefaultSettings defaultSettings)
        {
            SoundVolume = new PlayerDataValue<float>(Data, PlayerDataConstants.SoundVolume, defaultSettings.SoundVolume);
            MusicVolume = new PlayerDataValue<float>(Data, PlayerDataConstants.MusicVolume, defaultSettings.MusicVolume);
            UIVolume = new PlayerDataValue<float>(Data, PlayerDataConstants.UIVolume, defaultSettings.UIVolume);
            Language = new PlayerDataValue<string>(Data, PlayerDataConstants.Language, defaultSettings.Language);
            Acorns = new PlayerDataValue<int>(Data, PlayerDataConstants.Acorns, 0);
            Gems = new PlayerDataValue<int>(Data, PlayerDataConstants.Gems, 0);
        }

        public void Initialize()
        {
            SoundVolume.Initialize();
            MusicVolume.Initialize();
            UIVolume.Initialize();
            Language.Initialize();
            Acorns.Initialize();
            Gems.Initialize();
        }
    }
}