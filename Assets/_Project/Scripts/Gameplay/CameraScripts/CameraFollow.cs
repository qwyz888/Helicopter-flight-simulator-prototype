using Gameplay.Helicopter;
using UnityEngine;

namespace Gameplay.CameraScripts
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new(0f, 5f, -10f);

        private HelicopterPhysics _physics;

        private void Awake()
        {
            if (target != null)
            {
                _physics = target.GetComponent<HelicopterPhysics>();
            }
        }

        private void FixedUpdate()
        {
            if (target == null)
                return;

            if (_physics == null)
            {
                transform.position =
                    target.position +
                    target.TransformDirection(offset);

                transform.LookAt(target);

                return;
            }

            float alpha =
                (Time.time - Time.fixedTime) /
                Time.fixedDeltaTime;

            alpha = Mathf.Clamp01(alpha);

            Vector3 interpolatedPosition =
                Vector3.Lerp(
                    _physics.PreviousPosition,
                    _physics.CurrentPosition,
                    alpha);

            Vector3 desiredPosition =
                interpolatedPosition +
                target.TransformDirection(offset);

            transform.position = desiredPosition;

            transform.LookAt(interpolatedPosition);
        }
    }
}