using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Equipement
{
    public interface IAnimatedWeapon
    {
        void SetAnimationState(WeaponAnimateState state);
        void OnStateEnd(WeaponAnimateState current_state);
        IEnumerator WaitForAnimationEnd();
    }

    public enum WeaponAnimateState
    {
        Idle,
        PickUp,
        Drop,
        Fire
    }
}
