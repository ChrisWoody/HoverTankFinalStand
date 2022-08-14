using System;
using System.Diagnostics;

namespace Game
{
    public class GameController : Singleton<GameController>
    {
        public TimeSpan CurrentLifetime { get; private set; }
        public TimeSpan HighestLifetime { get; private set; }

        private Stopwatch _stopwatch = new Stopwatch();
        
        public void StartGame()
        {
            CurrentLifetime = TimeSpan.Zero;
            _stopwatch.Restart();
        }

        public void GameOver()
        {
            _stopwatch.Stop();
            CurrentLifetime = _stopwatch.Elapsed;
            if (CurrentLifetime > HighestLifetime)
                HighestLifetime = CurrentLifetime;
            UiController.Instance.GameOver();
        }
    }
}
