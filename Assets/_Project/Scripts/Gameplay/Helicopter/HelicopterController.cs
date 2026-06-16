using UnityEngine;

namespace Gameplay.Helicopter
{
    [RequireComponent(typeof(HelicopterPhysics))]
    public class HelicopterController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform Visuals;

        [Header("Smoothing")]
        [SerializeField] private float PositionSmoothing = 10f;
        [SerializeField] private float RotationSmoothing = 10f;

        private HelicopterPhysics _physics;
        private Vector3 _visualPosVel;
      
        private void Awake()
        {
            _physics = GetComponent<HelicopterPhysics>();
            if (Visuals == null) Visuals = transform;
        }

        private void Update()
        {
            if (Visuals != null && Visuals != transform)
            {
                Visuals.position = Vector3.SmoothDamp(Visuals.position, transform.position, ref _visualPosVel, 1f / Mathf.Max(0.0001f, PositionSmoothing));
                Visuals.rotation = Quaternion.Slerp(Visuals.rotation, transform.rotation, Time.deltaTime * RotationSmoothing);
            }
        }
    }
}
