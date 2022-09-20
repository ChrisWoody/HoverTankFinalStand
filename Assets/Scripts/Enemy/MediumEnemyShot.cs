using UnityEngine;

namespace Enemy
{
    public class MediumEnemyShot : MonoBehaviour
    {
        private float _elapsed;
        private const float Lifetime = 30f;

        private bool _fired;

        private void Update()
        {
            if (!_fired)
                return;

            _elapsed += Time.deltaTime;
            if (_elapsed >= Lifetime)
                Destroy(gameObject);

            transform.position += transform.forward * (Time.deltaTime * 4f);
        }

        public void Fire(Vector3 position, Vector3 forward)
        {
            transform.position = position;
            transform.forward = forward;
            _fired = true;
        }
    }
}