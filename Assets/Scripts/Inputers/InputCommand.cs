using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Inputers
{
    public class InputCommand
    {
        public Vector3 worldDirection;
    }

    public sealed class MoveCommand:InputCommand
    {
    }
    public sealed class ShootCommand : InputCommand
    {
    }
    public sealed class RotateCommand : InputCommand
    {
    }
    public sealed class SwitchWeaponCommand : InputCommand
    {
    }
    public sealed class ResetLevelCommand : InputCommand { }
}
