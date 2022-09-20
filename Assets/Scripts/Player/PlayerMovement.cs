using System;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : Singleton<PlayerMovement>
    {
        private Rigidbody _rigidbody;
        private Vector3 _moveDir;
        private int _groundLayerMask;
        private Quaternion _rotation;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _groundLayerMask = LayerMask.GetMask("Ground");
        }

        private void Update()
        {
            if (!GameController.Instance.IsPlaying)
                return;
            
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out var hit, 50f, _groundLayerMask))
            {
                // Based on https://www.youtube.com/watch?v=mKLp-2iseDc
                var targetPosition = hit.point;
                targetPosition.y = transform.position.y;
                var targetDirection = targetPosition - transform.position;
                var angle = MathF.Atan2(targetDirection.x, targetDirection.z) * Mathf.Rad2Deg;
                _rotation = Quaternion.AngleAxis(angle, Vector3.up);
            }
        }

        private void LateUpdate()
        {
            if (!GameController.Instance.IsPlaying)
                return;
            
            if ((_rigidbody.velocity + _moveDir).magnitude < 20f)
                _rigidbody.velocity += _moveDir;
            _rigidbody.MoveRotation(Quaternion.Slerp(_rigidbody.rotation, _rotation, Time.fixedDeltaTime));
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            var move = context.ReadValue<Vector2>();
            _moveDir = new Vector3(move.x, 0f, move.y) * (40f * Time.deltaTime);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            // todo
        }

        public void TempStartGame(InputAction.CallbackContext context)
        {
            UiController.Instance.StartGame();
        }

        public void TempGameOver(InputAction.CallbackContext context)
        {
            UiController.Instance.DebugGameOver();
        }
    }
}
