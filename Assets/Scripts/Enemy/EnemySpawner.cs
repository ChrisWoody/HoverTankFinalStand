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

        private static Vector3 GetSpawnPosition(float distance = 170f)
        {
            float x;
            float z;

            if (Random.value > 0.5f)
            {
                if (Random.value > 0.5f)
                {
                    x = -distance;
                    z = Random.Range(-distance, distance);
                }
                else
                {
                    x = distance;
                    z = Random.Range(-distance, distance);
                }
            }
            else
            {
                if (Random.value > 0.5f)
                {
                    x = Random.Range(-distance, distance);
                    z = -distance;
                }
                else
                {
                    x = Random.Range(-distance, distance);
                    z = -distance;
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

            for (var i = 1; i <= 3; i++)
            {
                var smallEnemy = _smallEnemies[i];
                var spawnPosition = GetSpawnPosition(60f * i);
                smallEnemy.transform.position = spawnPosition;
                smallEnemy.Spawn();
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
