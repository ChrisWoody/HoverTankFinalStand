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

        public override void ConstantFire()
        {
            if (!_canFire)
                return;

            _canFire = false;
            _elapsed = 0f;
            
            _rapidLaserFlash.Flash();
            _rapidLaserTrail.Fire(transform.position, transform.position + (transform.forward * 100f));
            // if (Physics.Raycast(BolterPoint.position, BolterPoint.forward, out var hit, 100f, _enemyLayerMask))
            // {
            //     //hit.transform.GetComponent<EnemyBase>().Hit(1);
            //     
            //     //var bolterImpact = Instantiate(BolterImpact);
            //     //bolterImpact.position = hit.point;
            //     
            //     //var bolterExplosion = Instantiate(BolterExplosion);
            //     //bolterExplosion.position = hit.point;
            //     //Destroy(bolterExplosion.gameObject, 1f);
            //     
            //     //var bolterTrail = Instantiate(BolterTrail);
            //     //bolterTrail.Fire(BolterPoint.position, hit.point);
            // }
        }
    }
}
