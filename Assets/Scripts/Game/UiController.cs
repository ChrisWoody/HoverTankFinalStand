using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game
{
    public class UiController : Singleton<UiController>
    {
        [SerializeField] private Canvas _mainMenuCanvas;
        //[SerializeField] private Button _startGameButton;

        [SerializeField] private Canvas _gameMenuCanvas;
        [SerializeField] private TMP_Text _gameMenuShieldsHealth;

        [SerializeField] private Canvas _gameOverCanvas;
        [SerializeField] private TMP_Text _gameOverScore;
        [SerializeField] private TMP_Text _gameOverHighscore;
        //[SerializeField] private Button _playAgainButton;
        //[SerializeField] private Button _mainMenuButton;

        //[SerializeField] private Button _gameOverButton;

        [SerializeField] private PlayerInput _playerInput;

        private void Awake()
        {
            Time.timeScale = 0f;
            _gameMenuCanvas.enabled = false;
            _gameOverCanvas.enabled = false;
            
            // _startGameButton.onClick.AddListener(StartGame);
            // _playAgainButton.onClick.AddListener(StartGame);
            // _mainMenuButton.onClick.AddListener(MainMenu);
            //
            // _gameOverButton.onClick.AddListener(DebugGameOver);
            
            //_playerInput.SwitchCurrentActionMap("UI");
        }

        private void MainMenu()
        {
            Time.timeScale = 0f;
            _mainMenuCanvas.enabled = true;
            _gameMenuCanvas.enabled = false;
            _gameOverCanvas.enabled = false;
            //_playerInput.SwitchCurrentActionMap("UI");
        }
        
        public void StartGame()
        {
            Time.timeScale = 1f;
            _mainMenuCanvas.enabled = false;
            _gameMenuCanvas.enabled = true;
            _gameOverCanvas.enabled = false;
            //_playerInput.SwitchCurrentActionMap("Player");
            
            GameController.Instance.StartGame();
        }

        public void DebugGameOver()
        {
            GameController.Instance.GameOver();
        }
        
        public void GameOver()
        {
            //_playerInput.SwitchCurrentActionMap("UI");
            Time.timeScale = 0f;
            _mainMenuCanvas.enabled = false;
            _gameMenuCanvas.enabled = false;
            _gameOverCanvas.enabled = true;

            _gameOverScore.text = $"Score: {Math.Truncate(GameController.Instance.CurrentLifetime.TotalSeconds)}";
            _gameOverHighscore.text = $"Highscore: {Math.Truncate(GameController.Instance.HighestLifetime.TotalSeconds)}";
        }

        public void UpdateShieldHealth(int shields, int health)
        {
            _gameMenuShieldsHealth.text = $"Shields: {shields} Health: {health}";
        }
    }
}
