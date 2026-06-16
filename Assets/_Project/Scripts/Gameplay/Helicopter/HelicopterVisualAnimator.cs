using UnityEngine;

namespace Gameplay.Helicopter
{
    public class HelicopterVisualAnimator : MonoBehaviour
    {
        [SerializeField] private HelicopterPhysics physics;

        [Header("References")]
        [SerializeField] private Transform mainRotor;
        [SerializeField] private Transform tailRotor;

        [Header("Rotor Speeds")]
        [SerializeField] private float minMainRotorSpeed = 300f;
        [SerializeField] private float maxMainRotorSpeed = 2000f;

        [SerializeField] private float minTailRotorSpeed = 500f;
        [SerializeField] private float maxTailRotorSpeed = 3500f;

        private void FixedUpdate()
        {
            if (physics == null)
                return;

            float collective = physics.Collective;

            float mainRotorSpeed =
                Mathf.Lerp(
                    minMainRotorSpeed,
                    maxMainRotorSpeed,
                    collective);

            float tailRotorSpeed =
                Mathf.Lerp(
                    minTailRotorSpeed,
                    maxTailRotorSpeed,
                    collective);

            tailRotorSpeed +=
                Mathf.Abs(physics.YawVelocity) * 20f;

            if (mainRotor != null)
            {
                mainRotor.Rotate(
                    Vector3.up,
                    mainRotorSpeed * Time.deltaTime,
                    Space.Self);
            }

            if (tailRotor != null)
            {
                tailRotor.Rotate(
                    Vector3.right,
                    tailRotorSpeed * Time.deltaTime,
                    Space.Self);
            }
        }
    }
}