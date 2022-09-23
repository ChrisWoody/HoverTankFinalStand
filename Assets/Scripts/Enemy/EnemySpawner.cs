using System.Linq;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : Singleton<EnemySpawner>
    {
        [SerializeField] private EnemyBase _smallEnemyPrefab;
        [SerializeField] private EnemyBase _mediumEnemyPrefab;

        private EnemyBase[] _smallEnemies;
        private EnemyBase[] _mediumEnemies;

        private float _gameElapsed;
        private float _elapsed;
        private float _spawnrate;

        private bool _playing;

        private void Awake()
        {
            _smallEnemies = Enumerable.Range(0, 40).Select(x => Instantiate(_smallEnemyPrefab)).ToArray();
            _mediumEnemies = Enumerable.Range(0, 30).Select(x => Instantiate(_mediumEnemyPrefab)).ToArray();
        }

        private void Update()
        {
            if (!_playing)
                return;

            _gameElapsed += Time.deltaTime;
            if (_gameElapsed > 60f)
            {
                _spawnrate = 2f;
            }
            else if (_gameElapsed > 120f)
            {
                _spawnrate = 1f;
            }

            _elapsed += Time.deltaTime;
            if (_elapsed >= _spawnrate)
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

                foreach (var mediumEnemy in _mediumEnemies)
                {
                    if (!mediumEnemy.Alive)
                    {
                        var spawnPosition = GetSpawnPosition();
                        mediumEnemy.transform.position = spawnPosition;
                        mediumEnemy.Spawn();
                        break;
                    }
                }
            }
        }

        private static Vector3 GetSpawnPosition()
        {
            float x;
            float z;

            const float f = 170f;
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
            _spawnrate = 3f;

            foreach (var smallEnemy in _smallEnemies)
            {
                smallEnemy.ResetForGame();
            }

            foreach (var mediumEnemy in _mediumEnemies)
            {
                mediumEnemy.ResetForGame();
            }
        }

        public void GameOver()
        {
            _playing = false;
            _elapsed = 0f;
        }
    }
}
