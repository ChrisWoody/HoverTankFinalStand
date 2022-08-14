using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UiController : Singleton<UiController>
    {
        [SerializeField] private Canvas _mainMenuCanvas;
        [SerializeField] private Button _startGameButton;

        [SerializeField] private Canvas _gameMenuCanvas;

        [SerializeField] private Canvas _gameOverCanvas;
        [SerializeField] private TMP_Text _gameOverScore;
        [SerializeField] private TMP_Text _gameOverHighscore;
        [SerializeField] private Button _playAgainButton;
        [SerializeField] private Button _mainMenuButton;

        [SerializeField] private Button _gameOverButton;

        private void Awake()
        {
            _gameMenuCanvas.enabled = false;
            _gameOverCanvas.enabled = false;
            
            _startGameButton.onClick.AddListener(StartGame);
            _playAgainButton.onClick.AddListener(StartGame);
            _mainMenuButton.onClick.AddListener(MainMenu);
            
            _gameOverButton.onClick.AddListener(DebugGameOver);
        }

        private void MainMenu()
        {
            _mainMenuCanvas.enabled = true;
            _gameMenuCanvas.enabled = false;
            _gameOverCanvas.enabled = false;
        }
        
        private void StartGame()
        {
            _mainMenuCanvas.enabled = false;
            _gameMenuCanvas.enabled = true;
            _gameOverCanvas.enabled = false;
            
            GameController.Instance.StartGame();
        }

        private void DebugGameOver()
        {
            GameController.Instance.GameOver();
        }
        
        public void GameOver()
        {
            _mainMenuCanvas.enabled = false;
            _gameMenuCanvas.enabled = false;
            _gameOverCanvas.enabled = true;

            _gameOverScore.text = $"Score: {Math.Truncate(GameController.Instance.CurrentLifetime.TotalSeconds)}";
            _gameOverHighscore.text = $"Highscore: {Math.Truncate(GameController.Instance.HighestLifetime.TotalSeconds)}";
        }
    }
}
