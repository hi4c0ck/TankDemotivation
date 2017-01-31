using Assets.Scripts.Tank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Equipement.Abstract;

namespace Assets.Scripts.Impl
{
    public class PartizanTank : AnyTank
    {
        public override void ChangeWeapon(int slotNumber, TankWeapon newWeapon)
        {
            if (slotNumber > weapons.Count - 1)
            {
                Console.WriteLine("Wrong weapons change trying!{0}",slotNumber);
                return;
            };
            weapons[slotNumber] = newWeapon;
        }

    }
}
