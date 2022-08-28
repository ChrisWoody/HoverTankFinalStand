using UnityEngine;

namespace Player.Weapons
{
    public class ChargedPlasmaExplosion : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _sizeCurve;
        [SerializeField] private AnimationCurve _fadeOutCurve;

        private Material _material;

        private float _elapsed;
        private const float Timeout = 2f;

        private Light _light;
        private float _initialLightIntensity;
        private float _maxLightRange;

        private bool _exploding;

        private void Awake()
        {
            _material = GetComponent<MeshRenderer>().material;
            _light = GetComponentInChildren<Light>();
            _initialLightIntensity = _light.intensity;
            _maxLightRange = _light.range;
            _light.intensity = 0f;
            _material.SetFloat("_distance", 0f);
            _material.SetFloat("_fadeoutval", 0f);
            transform.localScale = Vector3.zero;
        }

        private void Update()
        {
            if (!_exploding)
                return;

            _elapsed += Time.deltaTime;
            if (_elapsed >= Timeout)
            {
                _exploding = false;
                _elapsed = 0f;
                _light.intensity = 0f;
                _material.SetFloat("_distance", 0f);
                _material.SetFloat("_fadeoutval", 0f);
                transform.localScale = Vector3.zero;
                return;
            }

            var scale = _elapsed / Timeout;

            _material.SetFloat("_distance", _sizeCurve.Evaluate(scale) * 5f);
            _material.SetFloat("_fadeoutval", _fadeOutCurve.Evaluate(scale));
            if (scale >= 0.5f)
            {
                _light.intensity = _initialLightIntensity * (-scale + 1f) * 2f;
            }
            else
            {
                _light.range = _maxLightRange * scale * 2f;
            }
        }

        public void Explode(Vector3 pos)
        {
            transform.localScale = Vector3.one;
            transform.position = pos;
            _exploding = true;
            _elapsed = 0f;
            _light.intensity = _initialLightIntensity;
        }
    }
}
