using Enemy;
using UnityEngine;

namespace Player.Weapons
{
    public class ChargedPlasmaBall : MonoBehaviour
    {
        [SerializeField] private ChargedPlasmaExplosion _chargedPlasmaExplosion;

        private Vector3 _initialScale;
        private Light _light;
        private float _initialLightIntensity;
        
        private const float Lifetime = 5f;
        private float _elapsed;

        private bool _fired;

        private Rigidbody _rb;
        private int _enemyLayerMask;

        private readonly Collider[] _enemiesHit = new Collider[50];  

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _initialScale = transform.localScale;
            _light = GetComponentInChildren<Light>();
            _initialLightIntensity = _light.intensity;

            transform.localScale = Vector3.zero;
            _light.intensity = 0f;
            _enemyLayerMask = LayerMask.GetMask("Enemy");
        }

        private void Update()
        {
            if (!_fired)
                return;

            _elapsed += Time.deltaTime;
            if (_elapsed >= Lifetime)
            {
                Explode();
            }
        }

        private void FixedUpdate()
        {
            if (_fired)
            {
                _rb.MovePosition(_rb.position + transform.forward * (5f * Time.fixedDeltaTime));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_fired)
                return;

            Explode();
        }

        private void Explode()
        {
            transform.localScale = Vector3.zero;
            _light.intensity = 0f;
            _fired = false;
            _elapsed = 0f;

            _chargedPlasmaExplosion.Explode(transform.position);
            
            var size = Physics.OverlapSphereNonAlloc(transform.position, 15f, _enemiesHit, _enemyLayerMask);
            if (size == 0)
                return;

            for (var i = 0; i < size; i++)
            {
                _enemiesHit[i].GetComponent<EnemyBase>().Hit(10, WeaponType.ChargedPlasma, transform.position);
            }
        }

        public void ScaleAndSetBall(Vector3 pos, Vector3 dir, float scale)
        {
            if (_fired)
                return;
 
            transform.position = pos;
            transform.forward = dir;

            transform.localScale = _initialScale * scale;
            _light.intensity = _initialLightIntensity * scale;
        }

        public void Fire()
        {
            _fired = true;
            _elapsed = 0f;
        }

        public bool CanFire()
        {
            return !_fired;
        }
    }
}
