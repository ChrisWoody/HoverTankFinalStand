using Enemy;
using UnityEngine;

namespace Player.Weapons
{
    public class RapidLaser : WeaponBase
    {
        //public Transform BolterPoint;
        //public Transform BolterImpact;
        //public Transform BolterExplosion;
        //public BolterTrail BolterTrail;
        
        private RapidLaserFlash _rapidLaserFlash;
        private RapidLaserTrail _rapidLaserTrail;

        private bool _canFire = true;
        private const float Cooldown = 0.25f;
        private float _elapsed;
        private int _enemyLayerMask;

        private void Awake()
        {
            _rapidLaserFlash = GetComponentInChildren<RapidLaserFlash>();
            _rapidLaserTrail = GetComponentInChildren<RapidLaserTrail>();
            _enemyLayerMask = LayerMask.GetMask("Enemy");
        }

        private void Update()
        {
            if (!_canFire)
            {
                _elapsed += Time.deltaTime;
                if (_elapsed >= Cooldown)
                {
                    _canFire = true;
                    _elapsed = 0f;
                }
            }
        }

        public override string Name => "Rapid Laser";

        public override void ConstantFire()
        {
            if (!_canFire)
                return;

            _canFire = false;
            _elapsed = 0f;
            
            _rapidLaserFlash.Flash();
            if (Physics.Raycast(transform.position, transform.forward, out var hit, 100f, _enemyLayerMask))
            {
                 hit.transform.GetComponent<EnemyBase>().Hit(1);
                 
                 _rapidLaserTrail.Fire(transform.position, hit.point);
            }
            else
            {
                _rapidLaserTrail.Fire(transform.position, transform.position + (transform.forward * 100f));
            }
        }
    }
}
