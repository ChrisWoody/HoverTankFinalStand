using Game;
using Player.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerWeapon : Singleton<PlayerWeapon>
    {
        private WeaponBase[] _weapons;

        private bool _fire;
        private bool _started;
        private bool _finished;
        private bool _canShootAfterChangingWeapon;
        
        private int _currentWeapon;
        
        private void Awake()
        {
            _weapons = GetComponents<WeaponBase>();
            UiController.Instance.UpdateSelectedWeapon(_weapons[_currentWeapon].Name);
        }

        private void Update()
        {
            if (!GameController.Instance.IsPlaying)
                return;

            if (!_canShootAfterChangingWeapon)
                return;
            
            if (_fire)
            {
                if (_started)
                {
                    _started = false;
                    _weapons[_currentWeapon].StartFire();
                }
                else if (_finished)
                {
                    _finished = false;
                    _fire = false;
                    _weapons[_currentWeapon].EndFire();
                }
                else
                {
                    _weapons[_currentWeapon].ConstantFire();                    
                }
            }
            UiController.Instance.UpdateSelectedWeapon(_weapons[_currentWeapon].Name);
        }
        
        public void OnFire(InputAction.CallbackContext context)
        {
            if (!GameController.Instance.IsPlaying)
                return;
            
            if (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed)
            {
                _started = true;
                _fire = true;
                _canShootAfterChangingWeapon = true;
                _finished = false;
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                _finished = true;
            }
        }

        public void OnChangeNextWeapon(InputAction.CallbackContext context)
        {
            if (!GameController.Instance.IsPlaying)
                return;

            if (context.phase != InputActionPhase.Started && context.phase != InputActionPhase.Performed)
                return;

            _canShootAfterChangingWeapon = false;
            _finished = false;
            _fire = false;
            _weapons[_currentWeapon].EndFire();
            
            _currentWeapon++;
            if (_currentWeapon >= _weapons.Length)
                _currentWeapon = 0;

            UiController.Instance.UpdateSelectedWeapon(_weapons[_currentWeapon].Name);
        }

        public void OnChangePreviousWeapon(InputAction.CallbackContext context)
        {
            if (!GameController.Instance.IsPlaying)
                return;

            if (context.phase != InputActionPhase.Started && context.phase != InputActionPhase.Performed)
                return;

            _canShootAfterChangingWeapon = false;
            _finished = false;
            _fire = false;
            _weapons[_currentWeapon].EndFire();

            _currentWeapon--;
            if (_currentWeapon < 0)
                _currentWeapon = _weapons.Length - 1;

            UiController.Instance.UpdateSelectedWeapon(_weapons[_currentWeapon].Name);
        }
    }
}
