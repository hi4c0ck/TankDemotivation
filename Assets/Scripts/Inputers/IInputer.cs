using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Inputers
{
    interface IInputer
    {
        Vector3 SwitchWeapon { get; }
        bool Wpn_switched { get; }
        Vector3 RotationDirection { get; }
        bool Rotated { get; }
        Vector3 MoveDirection { get; }
        bool Inputed { get; }
        bool Shooted { get; }
    }
}
