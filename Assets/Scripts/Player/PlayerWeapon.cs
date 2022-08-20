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
        
        private int _currentWeapon;
        
        private void Awake()
        {
            _weapons = GetComponents<WeaponBase>();
        }

        private void Update()
        {
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
        }
        
        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started || context.phase == InputActionPhase.Performed)
            {
                _started = true;
                _fire = true;
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                _finished = true;
            }
        }

        public void OnChangeNextWeapon(InputAction.CallbackContext context)
        {
            _currentWeapon++;
            if (_currentWeapon >= _weapons.Length)
                _currentWeapon = 0;
        }

        public void OnChangePreviousWeapon(InputAction.CallbackContext context)
        {
            _currentWeapon--;
            if (_currentWeapon < 0)
                _currentWeapon = _weapons.Length - 1;
        }
    }
}
