using Infrastructure.Services.Input.Core;
using UnityEngine;
using VContainer;

namespace Gameplay.CameraScripts
{
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target;

        [Header("Follow")]
        [SerializeField] private float followSmooth = 10f;
        [SerializeField] private Vector3 offset = new Vector3(0f, 5f, -10f);

        [Header("Rotation")]
        [SerializeField] private float sensitivity = 2f;
        [SerializeField] private float minPitch = -30f;
        [SerializeField] private float maxPitch = 60f;

        private IInputService _input;

        private float _yaw;
        private float _pitch = 15f;

        private Vector3 _vel;

        [Inject]
        public void Construct(IInputService inputService)
        {
            _input = inputService;
        }

        private void FixedUpdate()
        {
            if (target == null) return;

            HandleInput();
            UpdateRotation();
            UpdatePosition();
        }

        private void HandleInput()
        {
            if (_input?.Gameplay?.Look == null) return;

            Vector2 look = _input.Gameplay.Look.Value;

            _yaw += look.x * sensitivity;
            _pitch -= look.y * sensitivity;

            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);
        }

        private void UpdateRotation()
        {
            transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }

        private void UpdatePosition()
        {
            Vector3 desiredPos =
                target.position +
                transform.rotation * offset;

            transform.position = Vector3.SmoothDamp(
                transform.position,
                desiredPos,
                ref _vel,
                1f / followSmooth);
        }
    }
}