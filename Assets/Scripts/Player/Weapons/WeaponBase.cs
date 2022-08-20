using UnityEngine;

namespace Player.Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        public virtual void StartFire()
        {
        }

        public virtual void ConstantFire()
        {
        }

        public virtual void EndFire()
        {
        }
    }
}
