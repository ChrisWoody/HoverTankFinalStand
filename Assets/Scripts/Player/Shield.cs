using UnityEngine;

namespace Player
{
    public class Shield : Singleton<Shield>
    {
        public const float FadeoutTimeout = 0.3f;
        private float _elapsed;

        private Material _material;
        private bool _flashing;

        private void Awake()
        {
            _material = GetComponent<MeshRenderer>().material;
            _material.SetFloat("_fadeoutval", 0f);
        }

        private void Update()
        {
            if (!_flashing)
                return;

            _elapsed += Time.deltaTime;
            if (_elapsed >= FadeoutTimeout)
            {
                _flashing = false;
                _elapsed = 0f;
                _material.SetFloat("_fadeoutval", 0f);
            }
            else
            {
                var scale = -(_elapsed / FadeoutTimeout) + 1f;
                _material.SetFloat("_fadeoutval", scale);
            }
        }

        public void Flash()
        {
            _flashing = true;
            _elapsed = 0f;
        }
    }
}
