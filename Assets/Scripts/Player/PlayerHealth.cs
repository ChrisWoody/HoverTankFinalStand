using Enemy;
using Game;
using Player.Weapons;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : Singleton<PlayerHealth>
    {
        private int _shields;
        private int _health;

        public void Reset()
        {
            _shields = 3;
            _health = 1;
            UiController.Instance.UpdateShieldHealth(_shields, _health);
        }

        private void ApplyDamage(int damage)
        {
            if (_shields - damage > 0)
            {
                _shields -= damage;
            }
            else
            {
                var diff = damage - _shields;
                _health -= diff;
                _shields = 0;
            }

            if (_health <= 0)
            {
                GameController.Instance.GameOver();
            }
            else
            {
                UiController.Instance.UpdateShieldHealth(_shields, _health);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Enemy") // or enemy weapon later
            {
                var enemyBase = collision.transform.GetComponent<EnemyBase>();
                ApplyDamage(enemyBase.PlayerDamageToApply);
                enemyBase.Hit(100, WeaponType.PlayerCollision, transform.position);
            }
            else if (collision.transform.tag == "MediumEnemyShot")
            {
                ApplyDamage(1);
                Destroy(collision.gameObject);
            }
        }
    }
}