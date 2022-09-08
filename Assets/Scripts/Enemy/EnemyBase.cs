using System;
using Player.Weapons;
using UnityEngine;

namespace Enemy
{
    public abstract class EnemyBase : MonoBehaviour
    {
        private int _health;

        protected abstract int InitialHealth { get; }
        public abstract int PlayerDamageToApply { get; }

        protected virtual void OnAwake() {}

        private void Awake()
        {
            _health = InitialHealth;
            OnAwake();
        }

        public void Hit(int damage, WeaponType weaponType, Vector3 hitPosition)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _health = 0;
                // kill enemy/return to pool
                OnDeath(weaponType, hitPosition);
                Destroy(gameObject);
            }
        }

        protected abstract void OnDeath(WeaponType weaponType, Vector3 hitPosition);
    }
}
