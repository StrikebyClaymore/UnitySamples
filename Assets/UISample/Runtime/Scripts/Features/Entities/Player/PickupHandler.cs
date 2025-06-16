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
        
        public PickupHandler(MapGenerator mapGenerator)
        {
            _playerData = ServiceLocator.Get<PlayerData>();
            _gameplayData = ServiceLocator.Get<GameplayData>();
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
                    break;
            }
        }
    }
}