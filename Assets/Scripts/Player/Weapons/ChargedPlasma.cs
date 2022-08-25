using UnityEngine;

namespace Player.Weapons
{
    public class ChargedPlasma : WeaponBase
    {
        [SerializeField] private ChargedPlasmaBall _chargedPlasmaBall;
        
        private bool _charging;
        private float _chargingElapsed;
        private const float ChargingTimeout = 5f; 

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

            _charging = true;
            _chargingElapsed = 0f;
        }

        public override void EndFire()
        {
            if (_cancellingCharge || _coolingDown)
                return;

            if (_charging)
            {
                _cancellingCharge = true;
                _cancellingChargeElapsed = _chargingElapsed;
                _charging = false;
                _chargingElapsed = 0f;
            }
        }

        // can check the other project how it was done, but player needs to charge weapon before it is fired
        // could have it fire as soon as its charged. Can also spawn the ball as charging is started, increasing its size and the light intestity up until its fully charged
        // this is cool actually as helps indicate that its charging. Also if they stop charging it can decrease, though unsure how that impacts swapping to other weapons
        // yeah swapping away or stop charging will cause the charged ball to descrease in size/light intesity but very quickly (not related to how long they were charging for)

        public override string Name => "Charged Plasma";
    }
}
