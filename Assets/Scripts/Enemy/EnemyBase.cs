using System;
using UnityEngine;

namespace Enemy
{
    public abstract class EnemyBase : MonoBehaviour
    {
        private int _health;

        protected abstract int InitialHealth { get; }

        private void Awake()
        {
            _health = InitialHealth;
        }

        public void Hit(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _health = 0;
                // kill enemy/return to pool
            }
        }
    }
}
