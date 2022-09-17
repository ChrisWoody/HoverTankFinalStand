using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : Singleton<EnemySpawner>
    {
        [SerializeField] private EnemyBase _smallEnemyPrefab;

        private EnemyBase[] _smallEnemies;

        private float _elapsed;
        private const float Spawnrate = 2f;
        
        private bool _playing;

        private void Awake()
        {
            _smallEnemies = Enumerable.Range(0, 20).Select(x => Instantiate(_smallEnemyPrefab)).ToArray();
        }

        private void Update()
        {
            if (!_playing)
                return;

            _elapsed += Time.deltaTime;
            if (_elapsed >= Spawnrate)
            {
                _elapsed = 0f;
                foreach (var smallEnemy in _smallEnemies)
                {
                    if (!smallEnemy.Alive)
                    {
                        var spawnPosition = GetSpawnPosition();
                        smallEnemy.transform.position = spawnPosition;
                        smallEnemy.Spawn();
                        break;
                    }
                }
            }
        }

        private static Vector3 GetSpawnPosition()
        {
            float x;
            float z;

            const float f = 125f;
            if (Random.value > 0.5f)
            {
                if (Random.value > 0.5f)
                {
                    x = -f;
                    z = Random.Range(-f, f);
                }
                else
                {
                    x = f;
                    z = Random.Range(-f, f);
                }
            }
            else
            {
                if (Random.value > 0.5f)
                {
                    x = Random.Range(-f, f);
                    z = -f;
                }
                else
                {
                    x = Random.Range(-f, f);
                    z = -f;
                }
            }

            return new Vector3(x, 0.5f, z);
        }

        public void StartGame()
        {
            _playing = true;
            _elapsed = 0f;

            foreach (var smallEnemy in _smallEnemies)
            {
                smallEnemy.ResetForGame();
            }
        }

        public void GameOver()
        {
            _playing = false;
            _elapsed = 0f;
        }
    }
}
