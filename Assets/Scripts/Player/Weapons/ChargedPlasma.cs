using UnityEngine;

namespace Player.Weapons
{
    public class ChargedPlasma : WeaponBase
    {
        [SerializeField] private ChargedPlasmaBall _chargedPlasmaBall;

        private float _chargingElapsed;
        private const float ChargingTimeout = 4f; 

        private float _coolingDownElapsed;
        private const float CoolingDownTimeout = 10f;

        private float _cancellingChargeElapsed;
        private const float CancellingChargeTimeout = ChargingTimeout;

        private State _state = State.Idle;

        private void Update()
        {
            switch (_state)
            {
                case State.Idle:
                    break;
                case State.Charging:
                    _chargingElapsed += Time.deltaTime;
                    if (_chargingElapsed >= ChargingTimeout)
                    {
                        _chargingElapsed = 0f;
                        _coolingDownElapsed = 0f;
                        _state = State.CoolingDown;
                    
                        _chargedPlasmaBall.ScaleAndSetBall(transform.position, transform.forward, 1f);
                        _chargedPlasmaBall.Fire();
                    }
                    else
                    {
                        var scale = _chargingElapsed / ChargingTimeout;
                        _chargedPlasmaBall.ScaleAndSetBall(transform.position, transform.forward, scale);
                    }
                    break;
                case State.CoolingDown:
                    _coolingDownElapsed += Time.deltaTime;
                    if (_coolingDownElapsed >= CoolingDownTimeout)
                    {
                        _state = State.Idle;
                        _coolingDownElapsed = 0f;
                    }
                    break;
                case State.Cancelling:
                    _cancellingChargeElapsed -= Time.deltaTime;
                    if (_cancellingChargeElapsed <= 0f)
                    {
                        _state = State.Idle;
                        _cancellingChargeElapsed = 0f;
                    }
                    else
                    {
                        var scale = _cancellingChargeElapsed / CancellingChargeTimeout;
                        _chargedPlasmaBall.ScaleAndSetBall(transform.position, transform.forward, scale);
                    }
                    break;
            }
        }

        public override void StartFire()
        {
            if (_state == State.Cancelling || _state == State.CoolingDown)
                return;

            if (!_chargedPlasmaBall.CanFire())
                return;

            _state = State.Charging;
            _chargingElapsed = 0f;
        }

        public override void EndFire()
        {
            if (_state == State.Cancelling || _state == State.CoolingDown)
                return;

            if (!_chargedPlasmaBall.CanFire())
                return;

            if (_state == State.Charging)
            {
                _state = State.Cancelling;
                _cancellingChargeElapsed = _chargingElapsed;
                _chargingElapsed = 0f;
            }
        }

        public override string Name => $"Charged Plasma: ({_state})";

        private enum State
        {
            Idle,
            Charging,
            CoolingDown,
            Cancelling
        }
    }
}
