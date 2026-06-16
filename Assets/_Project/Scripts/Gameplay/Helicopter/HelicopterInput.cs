using Infrastructure.Services.Input.Core;
using UnityEngine;
using VContainer;

namespace Gameplay.Helicopter
{
    public class HelicopterInput : MonoBehaviour
    {
        public Vector2 Move { get; private set; }
        public float Throttle { get; private set; }
        public float Yaw { get; private set; }

        private IInputService _inputService;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Update()
        {
            if (_inputService?.Gameplay == null)
                return;

            Move = _inputService.Gameplay.Move?.Value ?? Vector2.zero;
            Throttle = _inputService.Gameplay.Throttle?.Value ?? 0f;
            Yaw = _inputService.Gameplay.Yaw?.Value ?? 0f;
        }
    }
}