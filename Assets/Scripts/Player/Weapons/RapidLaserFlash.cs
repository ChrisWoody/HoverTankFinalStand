using UnityEngine;

namespace Player.Weapons
{
    public class RapidLaserFlash : MonoBehaviour
    {
        private const float DimTimeout = 0.25f;
        private float _elapsed;
        private bool _diming;

        private Light _light;
        private float _maxIntensity;

        private void Awake()
        {
            _light = GetComponent<Light>();
            _maxIntensity = _light.intensity;
            _light.intensity = 0f;
        }

        private void Update()
        {
            if (_diming)
            {
                _elapsed += Time.deltaTime;
                if (_elapsed >= DimTimeout)
                {
                    _elapsed = 0f;
                    _diming = false;
                    _light.intensity = 0f;
                }
                else
                {
                    _light.intensity = (-(_elapsed / DimTimeout) + 1) * _maxIntensity;
                }
            }
        }

        public void Flash()
        {
            _diming = true;
            _elapsed = 0f;
        }
    }
}