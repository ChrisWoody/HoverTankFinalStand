using Game;
using Player.Weapons;
using UnityEngine;

namespace Enemy
{
    public class MediumEnemy : EnemyBase
    {
        [SerializeField] private Transform _weaponPosition;
        [SerializeField] private MediumEnemyShot _mediumEnemyShot;
        [SerializeField] private SmallEnemyDeath _death;

        private float _firedElapsed;
        private float _firedCooldown;
        
        private float _speed; 

        protected override void OnAwake()
        {
            _speed = Random.value * 2f + 3f;
            _firedCooldown = Random.value * 5f + 2f;
        }

        private void Update()
        {
            if (!GameController.Instance.IsPlaying)
                return;

            if (Heath <= 0 || !Alive)
                return;

            var playerPos = Player.PlayerMovement.Instance.transform.position;
            playerPos.y = transform.position.y;

            var playerEnemyDiff = playerPos - transform.position;
            transform.forward = playerEnemyDiff.normalized;

            _firedElapsed += Time.deltaTime;
            if (playerEnemyDiff.magnitude < 25f)
            {
                if (_firedElapsed >= _firedCooldown)
                {
                    _firedElapsed = 0f;
                    var shot = Instantiate(_mediumEnemyShot);
                    shot.Fire(_weaponPosition.position, _weaponPosition.forward);
                }
            }
            else
            {
                transform.position += (transform.forward * (Time.deltaTime * _speed));                
            }
        }

        protected override int InitialHealth => 10;
        public override int PlayerDamageToApply => 5;
        protected override void OnDeath(WeaponType weaponType, Vector3 hitPosition)
        {
            var death = Instantiate(_death);
            death.transform.position = transform.position;
            death.transform.forward = transform.forward;
            death.Die(weaponType, hitPosition);
        }
    }
}
