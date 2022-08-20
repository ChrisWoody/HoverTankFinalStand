using UnityEngine;

namespace Player.Weapons
{
    public class RapidLaserTrail : MonoBehaviour
    {
        private LineRenderer _lineRenderer;

        private bool _firing;

        private const float Lifetime = 0.2f;
        private float _elapsed;

        private float _initialWidth;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _initialWidth = _lineRenderer.startWidth;
            _lineRenderer.startWidth = 0f;
            _lineRenderer.endWidth = 0f;
        }

        private void Update()
        {
            if (!_firing)
                return;
            
            _elapsed += Time.deltaTime;
            if (_elapsed >= Lifetime)
            {
                _firing = false;
                _elapsed = 0f;
                _lineRenderer.startWidth = 0f;
                _lineRenderer.endWidth = 0f;
            }
            else
            {
                var scale = -(_elapsed / Lifetime) + 1f;
                _lineRenderer.startWidth = scale * _initialWidth;
                _lineRenderer.endWidth = scale * _initialWidth;
            }
        }

        public void Fire(Vector3 start, Vector3 end)
        {
            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, end);
            _lineRenderer.startWidth = _initialWidth;
            _lineRenderer.endWidth = _initialWidth;
            _firing = true;
            _elapsed = 0f;
        }
    }
}