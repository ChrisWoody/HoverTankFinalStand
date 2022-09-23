using Game;
using Player.Weapons;
using UnityEngine;

namespace Enemy
{
    public class SmallEnemy : EnemyBase
    {
        [SerializeField] private SmallEnemyDeath _death;

        protected override int InitialHealth => 3;
        public override int PlayerDamageToApply => 1;

        private float _speed; 

        protected override void OnAwake()
        {
            _speed = Random.value * 2f + 7f;
        }

        protected override void OnDeath(WeaponType weaponType, Vector3 hitPosition)
        {
            var death = Instantiate(_death);
            death.transform.position = transform.position;
            death.transform.forward = transform.forward;
            death.Die(weaponType, hitPosition);
        }

        private void Update()
        {
            if (!GameController.Instance.IsPlaying)
                return;

            if (Heath <= 0 || !Alive)
                return;

            var playerPos = Player.PlayerMovement.Instance.transform.position;
            playerPos.y = transform.position.y;

            transform.forward = (playerPos - transform.position).normalized;
            transform.position += (transform.forward * (Time.deltaTime * _speed));
        }
    }
}
