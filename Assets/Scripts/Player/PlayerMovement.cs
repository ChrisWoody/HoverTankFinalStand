using System;
using Game;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Vector3 _moveDir;
        private Vector3 _lookDir;
        private int _groundLayerMask;

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
                var targetPosition = hit.point;
                targetPosition.y = transform.position.y;
                var targetDirection = (targetPosition - transform.position).normalized;
                var dot = Vector3.Dot(targetDirection, transform.right);
                var dir = dot < -0.05f ? -1f : dot > 0.05f ? 1f : 0f;
                _lookDir = new Vector3(0f, dir * 20f, 0f);
            }
        }

        private void LateUpdate()
        {
            if (!GameController.Instance.IsPlaying)
                return;
            
            if ((_rigidbody.velocity + _moveDir).magnitude < 20f)
                _rigidbody.velocity += _moveDir;
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(_lookDir * Time.fixedDeltaTime));
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