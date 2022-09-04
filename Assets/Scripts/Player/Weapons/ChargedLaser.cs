using Enemy;
using UnityEngine;

namespace Player.Weapons
{
    public class ChargedLaser : WeaponBase
    {
        [SerializeField] private ChargedLaserLaser _chargedLaserLaser;

        private float _chargingElapsed;
        private const float ChargingTimeout = 4f; 

        private float _coolingDownElapsed;
        private const float CoolingDownTimeout = 10f;

        private float _firingElapsed;
        private const float FiringTimeout = 5f;

        private float _damagedCooldownElapsed;
        private const float DamageCooldownTimeout = 0.1f;

        private float _cancellingChargeElapsed;
        private const float CancellingChargeTimeout = ChargingTimeout;

        private State _state = State.Idle;
        private int _enemyLayerMask;

        private void Awake()
        {
            _enemyLayerMask = LayerMask.GetMask("Enemy");
        }

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
                        _state = State.Firing;
                        _firingElapsed = 0f;
                    }
                    break;
                case State.CoolingDown:
                    _coolingDownElapsed += Time.deltaTime;
                    if (_coolingDownElapsed >= CoolingDownTimeout)
                    {
                        _coolingDownElapsed = 0f;
                        _state = State.Idle;
                    }
                    break;
                case State.Firing:
                    _firingElapsed += Time.deltaTime;
                    if (_firingElapsed >= FiringTimeout)
                    {
                        _firingElapsed = 0f;
                        _state = State.CoolingDown;
                        _coolingDownElapsed = 0f;
                        _damagedCooldownElapsed = 0f;
                        _chargedLaserLaser.StopFiring();
                    }
                    else
                    {
                        _damagedCooldownElapsed += Time.deltaTime;
                        var end = transform.position + (transform.forward * 50f);
                        if (Physics.Raycast(transform.position, transform.forward, out var hit, 100f, _enemyLayerMask))
                        {
                            end = hit.point;

                            // If we've hit an enemy, and our non-framerate dependant damage is ready, hit it
                            if (_damagedCooldownElapsed >= DamageCooldownTimeout)
                            {
                                _damagedCooldownElapsed = 0f;
                                hit.transform.GetComponent<EnemyBase>().Hit(1);
                            }
                        }

                        _chargedLaserLaser.Fire(transform.position, end, transform.forward);
                    }
                    break;
                case State.Cancelling:
                    _cancellingChargeElapsed -= Time.deltaTime;
                    if (_cancellingChargeElapsed <= 0f)
                    {
                        _cancellingChargeElapsed = 0f;
                        _state = State.Idle;
                    }
                    break;
            }
        }

        public override void StartFire()
        {
            if (_state != State.Idle)
                return;

            _state = State.Charging;
            _chargingElapsed = 0f;
        }

        public override void EndFire()
        {
            if (_state != State.Charging)
                return;

            _state = State.Cancelling;
            _cancellingChargeElapsed = _chargingElapsed;
            _chargingElapsed = 0f;
        }
        
        public override string Name => $"Charged Laser, {_state}";

        private enum State
        {
            Idle,
            Charging,
            CoolingDown,
            Firing,
            Cancelling
        }
    }
}
