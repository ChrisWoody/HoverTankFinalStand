using UnityEngine;

namespace Player.Weapons
{
    public class ChargedPlasmaBall : MonoBehaviour
    {
        private Vector3 _initialScale;
        private Light _light;
        private float _initialLightIntensity;
        
        private const float Lifetime = 30f;
        private float _elapsed;

        private bool _fired;

        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _initialScale = transform.localScale;
            _light = GetComponentInChildren<Light>();
            _initialLightIntensity = _light.intensity;

            transform.localScale = Vector3.zero;
            _light.intensity = 0f;
        }

        private void Update()
        {
            if (!_fired)
                return;

            _elapsed += Time.deltaTime;
            if (_elapsed >= Lifetime)
            {
                transform.localScale = Vector3.zero;
                _light.intensity = 0f;
                _fired = false;

                // show explosion and damage enemies
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
            
            // assume enemy because of configured layers
            // show explosion and damage enemies
            
            transform.localScale = Vector3.zero;
            _light.intensity = 0f;
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
        }
    }
}
