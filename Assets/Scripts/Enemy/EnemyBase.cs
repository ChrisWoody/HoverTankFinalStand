using System;
using Game;
using Player.Weapons;
using UnityEngine;

namespace Enemy
{
    public abstract class EnemyBase : MonoBehaviour
    {
        protected int Heath { get; private set; }
        protected abstract int InitialHealth { get; }
        public bool Alive { get; private set; }
        public abstract int PlayerDamageToApply { get; }

        protected virtual void OnAwake() {}

        private void Awake()
        {
            ResetForGame();
            OnAwake();
        }

        public void Hit(int damage, WeaponType weaponType, Vector3 hitPosition)
        {
            if (Heath <= 0 || !Alive)
                return;

            Heath -= damage;
            if (Heath <= 0)
            {
                OnDeath(weaponType, hitPosition);
                ResetForGame();
                GameController.Instance.EnemyKilled();
            }
        }

        public void Spawn()
        {
            Heath = InitialHealth;
            Alive = true;
        }

        public void ResetForGame()
        {
            Alive = false;
            Heath = 0;
            transform.position = Vector3.down * 20f;
        }

        protected abstract void OnDeath(WeaponType weaponType, Vector3 hitPosition);
    }
}
