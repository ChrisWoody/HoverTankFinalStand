using System;
using System.Diagnostics;
using Enemy;
using Player;

namespace Game
{
    public class GameController : Singleton<GameController>
    {
        public TimeSpan CurrentLifetime { get; private set; }
        public TimeSpan HighestLifetime { get; private set; }

        public bool IsPlaying { get; private set; }

        public int EnemiesKilled { get; private set; }

        private Stopwatch _stopwatch = new Stopwatch();
        
        public void StartGame()
        {
            IsPlaying = true;
            CurrentLifetime = TimeSpan.Zero;
            PlayerHealth.Instance.Reset();
            EnemySpawner.Instance.StartGame();
            EnemiesKilled = 0;
            UiController.Instance.UpdateEnemiesKilled(EnemiesKilled);
            _stopwatch.Restart();
        }

        public void GameOver()
        {
            _stopwatch.Stop();
            IsPlaying = false;
            CurrentLifetime = _stopwatch.Elapsed;
            if (CurrentLifetime > HighestLifetime)
                HighestLifetime = CurrentLifetime;
            UiController.Instance.GameOver();
            EnemySpawner.Instance.GameOver();
        }

        public void EnemyKilled()
        {
            EnemiesKilled++;
            UiController.Instance.UpdateEnemiesKilled(EnemiesKilled);
        }
    }
}
