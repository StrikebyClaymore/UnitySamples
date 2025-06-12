using UISample.Infrastructure;

namespace UISample.Features
{
    public class PlayerController : IUpdatable
    {
        private readonly PlayerView _view;
        private readonly PlayerMovement _movement;
        
        public PlayerController(PlayerView view, MapGeneratorMono mapGenerator)
        {
            _view = view;
            _movement = new PlayerMovement(_view, mapGenerator);
        }

        public void CustomUpdate()
        {
            _movement.CustomUpdate();
        }

        public void CustomFixedUpdate() {
}

        public void CustomLateUpdate() { }
    }
}