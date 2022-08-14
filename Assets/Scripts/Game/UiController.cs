using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UiController : Singleton<UiController>
    {
        [SerializeField]
        private Button _startGameButton;

        private void Awake()
        {
            _startGameButton.onClick.AddListener(StartGame);
        }

        private static void StartGame()
        {
            GameController.Instance.StartGame();
        }
    }
}
