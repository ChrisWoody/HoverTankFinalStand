using System;
using Player.Weapons;
using UnityEngine;

namespace Enemy
{
    public class SmallEnemyDeath : MonoBehaviour
    {
        private Rigidbody[] _rigidbodies;
        private Vector3 _originalScale;
        private bool _dying;

        private float _elapsed;
        private const float Timeout = 4f;

        private void Awake()
        {
            _rigidbodies = GetComponentsInChildren<Rigidbody>();
            _originalScale = new Vector3(100f, 100f, 100f);
        }

        private void Update()
        {
            if (!_dying)
                return;

            _elapsed += Time.deltaTime;
            if (_elapsed >= Timeout)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                foreach (var rigidbody1 in _rigidbodies)
                {
                    rigidbody1.transform.localScale = _originalScale * (-(_elapsed / Timeout) + 1f);
                }
            }
        }

        public void Die(WeaponType weaponType, Vector3 hitPosition)
        {
            _dying = true;

            var force = weaponType switch
            {
                WeaponType.PlayerCollision => transform.forward * 3f,
                WeaponType.RapidLaser => transform.forward * 5f,
                WeaponType.ChargedPlasma => -(hitPosition - transform.position).normalized * 25f,
                WeaponType.ChargedLaser => -(hitPosition - transform.position).normalized * 10f,
                _ => throw new ArgumentOutOfRangeException(nameof(weaponType), weaponType, null)
            };

            foreach (var rigidbody1 in _rigidbodies)
            {
                rigidbody1.AddForce(force, ForceMode.Impulse);
            }
        }
    }
}
