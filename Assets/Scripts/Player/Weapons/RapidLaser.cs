using Enemy;
using UnityEngine;

namespace Player.Weapons
{
    public class RapidLaser : WeaponBase
    {
        public Transform RapidLaserImpact;
        [SerializeField] private RapidLaserFlash _rapidLaserFlashEnd;
    
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

            var transform1 = transform;
            if (Physics.Raycast(transform1.position, transform1.forward, out var hit, 100f, _enemyLayerMask))
            {
                 hit.transform.GetComponent<EnemyBase>().Hit(1);
                 
                 _rapidLaserTrail.Fire(transform1.position, hit.point);
                 var impact = Instantiate(RapidLaserImpact);
                 impact.position = hit.point;
                 impact.forward = -transform1.forward;
                 Destroy(impact.gameObject, 1f);
                 _rapidLaserFlashEnd.transform.position = hit.point - (transform1.forward * 0.5f);
            }
            else
            {
                _rapidLaserTrail.Fire(transform1.position, transform1.position + (transform1.forward * 100f));
                _rapidLaserFlashEnd.transform.position = Vector3.zero + Vector3.down;
            }

            _rapidLaserFlash.Flash();
            _rapidLaserFlashEnd.Flash();
        }
    }
}
