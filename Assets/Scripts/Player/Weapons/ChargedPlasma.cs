using UnityEngine;

namespace Player.Weapons
{
    public class ChargedPlasma : WeaponBase
    {
        [SerializeField] private ChargedPlasmaBall _chargedPlasmaBall;
        
        private bool _charging;
        private float _chargingElapsed;
        private const float ChargingTimeout = 4f; 

        private bool _coolingDown;
        private float _coolingDownElapsed;
        private const float CoolingDownTimeout = 10f;

        private bool _cancellingCharge;
        private float _cancellingChargeElapsed;
        private const float CancellingChargeTimeout = ChargingTimeout;

        private void Update()
        {
            if (_charging)
            {
                _chargingElapsed += Time.deltaTime;
                if (_chargingElapsed >= ChargingTimeout)
                {
                    _charging = false;
                    _chargingElapsed = 0f;
                    _coolingDown = true;
                    _coolingDownElapsed = 0f;
                    
                    _chargedPlasmaBall.ScaleAndSetBall(transform.position, transform.forward, 1f);
                    _chargedPlasmaBall.Fire();
                }
                else
                {
                    var scale = _chargingElapsed / ChargingTimeout;
                    _chargedPlasmaBall.ScaleAndSetBall(transform.position, transform.forward, scale);
                }
            }
            else if (_cancellingCharge)
            {
                _cancellingChargeElapsed -= Time.deltaTime;
                if (_cancellingChargeElapsed <= 0f)
                {
                    _cancellingCharge = false;
                    _cancellingChargeElapsed = 0f;
                }
                else
                {
                    var scale = _cancellingChargeElapsed / CancellingChargeTimeout;
                    _chargedPlasmaBall.ScaleAndSetBall(transform.position, transform.forward, scale);
                }
            }
            else if (_coolingDown)
            {
                _coolingDownElapsed += Time.deltaTime;
                if (_coolingDownElapsed >= CoolingDownTimeout)
                {
                    _coolingDown = false;
                    _coolingDownElapsed = 0f;
                }
            }
        }

        public override void StartFire()
        {
            if (_cancellingCharge || _coolingDown)
                return;

            if (!_chargedPlasmaBall.CanFire())
                return;

            _charging = true;
            _chargingElapsed = 0f;
        }

        public override void EndFire()
        {
            if (_cancellingCharge || _coolingDown)
                return;

            if (!_chargedPlasmaBall.CanFire())
                return;

            if (_charging)
            {
                _cancellingCharge = true;
                _cancellingChargeElapsed = _chargingElapsed;
                _charging = false;
                _chargingElapsed = 0f;
            }
        }

        public override string Name => $"Charged Plasma: {_charging} {_cancellingCharge} {_coolingDown} {_chargedPlasmaBall.CanFire()}";
    }
}
