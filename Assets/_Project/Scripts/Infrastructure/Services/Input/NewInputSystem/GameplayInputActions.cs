using Infrastructure.Services.Input.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Infrastructure.Services.Input.NewInputSystem
{
    public class GameplayInputActions : IGameplayInputActions
    {
        public GameplayInputActions(InputActions.GameplayActions playerActions)
        {
            Move = new FuncInputAction<Vector2>(() => playerActions.Move.ReadValue<Vector2>());
            Look = new FuncInputAction<Vector2>(() => playerActions.Look.ReadValue<Vector2>());
            Fire = new FuncInputAction<bool>(() => playerActions.Fire.IsPressed());
            Pause = new FuncInputAction<bool>(() => playerActions.Pause.WasPressedThisFrame());
            Throttle = new FuncInputAction<float>(() => playerActions.Throttle.ReadValue<float>());
            Yaw = new FuncInputAction<float>(() => playerActions.Yaw.ReadValue<float>());
        }

        public IInputAction<Vector2> Move { get; }
        public IInputAction<Vector2> Look { get; }
        public IInputAction<bool> Fire { get; }
        public IInputAction<bool> Pause { get; }
        public IInputAction<float> Throttle { get; }
        public IInputAction<float> Yaw { get; }

        public void SetActive(bool active)
        {
            Move.Enabled = active;
            Look.Enabled = active;
            Fire.Enabled = active;
            Pause.Enabled = active;
            Throttle.Enabled = active;
            Yaw.Enabled = active;
        }
    }
}