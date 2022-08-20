using Game;

namespace Player
{
    public class PlayerHealth : Singleton<PlayerHealth>
    {
        private int _shields;
        private int _health;

        public void Reset()
        {
            _shields = 3;
            _health = 1;
            UiController.Instance.UpdateShieldHealth(_shields, _health);
        }

        public void Hit()
        {
            if (_shields > 0)
            {
                _shields--;
            }
            else if (_health > 0)
            {
                _health--;
            }

            if (_health <= 0)
            {
                GameController.Instance.GameOver();
            }
            else
            {
                UiController.Instance.UpdateShieldHealth(_shields, _health);
            }
        }
    }
}