using UnityEngine;

namespace Gameplay.Helicopter
{
    public class HelicopterPhysics : MonoBehaviour
    {
        [Header("Physics")]
        [SerializeField] private float mass = 1200f;
        [SerializeField] private float maxLift = 20000f;
        [SerializeField] private float linearDrag = 4f;

        [Header("Collective")]
        [SerializeField] private float collectiveChangeSpeed = 0.5f;
        [SerializeField] private float startCollective = 0.6f;

        [Header("Angular Physics")]
        [SerializeField] private float pitchTorque = 40f;
        [SerializeField] private float rollTorque = 40f;

        [SerializeField] private float angularDrag = 3f;
        [SerializeField] private float yawAcceleration = 50f;
        [SerializeField] private float yawDamping = 2f;

        [Header("Ground")]
        [SerializeField] private float groundCheckDistance = 3f;
        [SerializeField] private LayerMask groundLayers = ~0;

        public Vector3 PreviousPosition { get; private set; }
        public Vector3 CurrentPosition { get; private set; }
        public float Collective => _collective;
        public float YawVelocity => _yawVelocity;


        private Vector3 _velocity;

        private float _collective;

        private float _pitch;
        private float _roll;

        private float _pitchVelocity;
        private float _rollVelocity;

        private float _yawAngle;
        private float _yawVelocity;

        private HelicopterInput _helicopterInput;

        private void Awake()
        {
            _helicopterInput = GetComponent<HelicopterInput>();
            _collective = startCollective;
        }
        private void FixedUpdate()
        {
            PreviousPosition = transform.position;

            float dt = Time.fixedDeltaTime;

            ReadInput(out Vector2 move, out float throttleInput, out float yawInput);

            UpdateCollective(throttleInput, dt);
            UpdateYaw(yawInput, dt);
            UpdateRotation(move, dt);

            Vector3 nextPosition = CalculateMovement(dt);

            HandleGroundCollision(ref nextPosition);

            CurrentPosition = nextPosition;
            transform.position = nextPosition;
        }

        private void ReadInput(out Vector2 move, out float throttleInput, out float yawInput)
        {
            move = Vector2.zero;
            throttleInput = 0f;
            yawInput = 0f;

            if (_helicopterInput == null)
                return;

            move = Vector2.ClampMagnitude(_helicopterInput.Move, 1f);

            throttleInput = _helicopterInput.Throttle;

            yawInput = Mathf.Clamp(_helicopterInput.Yaw, -1f, 1f);
        }
        private void UpdateCollective(float throttleInput, float dt)
        {
            _collective += throttleInput * collectiveChangeSpeed * dt;
            _collective = Mathf.Clamp01( _collective);
        }

        private void UpdateYaw(float yawInput,float dt)
        {
            _yawVelocity += yawInput * yawAcceleration * dt;
            _yawVelocity -=  _yawVelocity * yawDamping * dt;
            _yawAngle += _yawVelocity *  dt;
        }

        private void UpdateRotation(Vector2 move, float dt)
        {
            // Pilot input creates torque

            _pitchVelocity += move.y * pitchTorque * dt;
            _rollVelocity += -move.x * rollTorque * dt;

            // Angular damping

            _pitchVelocity -= _pitchVelocity * angularDrag * dt;
            _rollVelocity -= _rollVelocity * angularDrag * dt;

            // Integrate

            _pitch += _pitchVelocity * dt;
            _roll += _rollVelocity * dt;

            transform.rotation =
                Quaternion.Euler(
                    _pitch,
                    _yawAngle,
                    _roll);
        }

        private Vector3 CalculateMovement(float dt)
        {
            Vector3 liftForce = transform.up *(_collective * maxLift);

            Vector3 gravityForce = Physics.gravity * mass;

            Vector3 dragForce = -_velocity * linearDrag;

            Vector3 totalForce = liftForce + gravityForce + dragForce;

            Vector3 acceleration =totalForce / mass;

            _velocity += acceleration * dt;

            return transform.position + _velocity * dt;
        }

        private void HandleGroundCollision( ref Vector3 nextPosition)
        {
            if (!Physics.Raycast(
                    nextPosition + Vector3.up,
                    Vector3.down,
                    out RaycastHit hit,
                    groundCheckDistance + 1f,
                    groundLayers))
            {
                return;
            }

            float groundY = hit.point.y + 0.05f;

            if (nextPosition.y >= groundY)
                return;

            nextPosition.y = groundY;

            if (_velocity.y < 0f)
            {
                _velocity.y = 0f;
            }
        }
    }
}