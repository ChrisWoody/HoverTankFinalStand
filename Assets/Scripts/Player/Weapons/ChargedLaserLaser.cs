using Cinemachine;
using UnityEngine;

namespace Player.Weapons
{
    public class ChargedLaserLaser : MonoBehaviour
    {
        private CinemachineImpulseSource _cinemachineImpulseSource;

        private LineRenderer _lineRenderer;
        private Light _startLight;
        private Light _endLight;
        private float _initialIntensity;

        private float _elapsed;
        private const float Timeout = 0.1f;

        private float _cameraShakeElapsed = CameraShakeTimeout;
        private const float CameraShakeTimeout = 0.1f; 
        private float _cameraShakeAmount = 0.1f;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.startWidth = 0f;
            _lineRenderer.endWidth = 0f;
            var lights = GetComponentsInChildren<Light>();
            _startLight = lights[0];
            _endLight = lights[1];
            _initialIntensity = _startLight.intensity;
            _startLight.intensity = _endLight.intensity = 0f;
            _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public void Fire(Vector3 start, Vector3 end, Vector3 dir)
        {
            _lineRenderer.startWidth = 0.5f;
            _lineRenderer.endWidth = 0.5f;
            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, end);

            _elapsed += Time.deltaTime;
            if (_elapsed >= Timeout)
            {
                _elapsed = 0f;
            }

            _startLight.intensity = _endLight.intensity = _initialIntensity;
            _startLight.transform.position = start + (dir * 0.5f);
            _endLight.transform.position = end - (dir * 0.5f);

            _cameraShakeElapsed += Time.deltaTime;
            if (_cameraShakeElapsed >= CameraShakeTimeout)
            {
                _cameraShakeElapsed = 0f;
                _cameraShakeAmount = -_cameraShakeAmount;
                _cinemachineImpulseSource.GenerateImpulse(_cameraShakeAmount);
            }
        }

        public void StopFiring()
        {
            _lineRenderer.startWidth = 0f;
            _lineRenderer.endWidth = 0f;
            _startLight.intensity = _endLight.intensity = 0f;
            _elapsed = 0f;
            _cameraShakeElapsed = CameraShakeTimeout;
        }
    }
}
