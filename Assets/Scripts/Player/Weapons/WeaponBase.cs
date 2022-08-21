using UnityEngine;

namespace Player.Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        public abstract string Name { get; }
    
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
