using Plugins.ServiceLocator;
using Pool;
using UISample.Infrastructure;

namespace UISample.Features
{
    public class PickupHandler
    {
        private readonly PlayerData _playerData;
        private readonly GameplayData _gameplayData;
        private readonly MonoPool<Acorn> _acornsPool;
        private readonly AudioPlayer _audioPlayer;
        
        public PickupHandler(MapGenerator mapGenerator)
        {
            _playerData = ServiceLocator.Get<PlayerData>();
            _gameplayData = ServiceLocator.Get<GameplayData>();
            _audioPlayer = ServiceLocator.Get<AudioPlayer>();
            _acornsPool = mapGenerator.AcornsPool;
        }
        
        public void Pickup(PickupItem item)
        {
            switch (item)
            {
                case Acorn acorn:
                    _playerData.Acorns.Value++;
                    _gameplayData.Acorns.Value++;
                    _acornsPool.Release(acorn);
                    EventBus.OnAddQuestValue.Invoke(EQuestType.Collect, EQuestTarget.Acorns, 1);
                    break;
            }

            _audioPlayer.PlaySound(_audioPlayer.Config.ItemPickupClip);
        }
    }
}