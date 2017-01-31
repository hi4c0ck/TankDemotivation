using Assets.Scripts.Equipement.Abstract;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Tank
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(TankMoveModel))]
    public abstract class AnyTank:MonoBehaviour
    {
        Rigidbody body;
        protected List<TankWeapon> weapons;
        int selecteWeaponIndex = 0;
        public int SelectedWeaponIndext { get { return selecteWeaponIndex; } }

        public List<Transform> weaponsPoints;
        public Transform mass_center;
        public Transform turret_point;
        public GameStats tankStats;
        TankMoveModel moveModel;
        void Start()
        {
            body = GetComponent<Rigidbody>();
            moveModel = GetComponent<TankMoveModel>();
            body.centerOfMass = mass_center.localPosition;
        }

        public bool TryShoot(ref BulletInfo info)
        {
            if (weapons==null || weapons.Count==0)
            {
                Debug.Log("weapons not initialized");
                return false;
            }
            if (selecteWeaponIndex>weapons.Count-1)
            {
                Console.WriteLine("Trying to shoot with weapon index {0} with object {1} ", selecteWeaponIndex, gameObject.name);
                return false;
            }
            if (weapons[selecteWeaponIndex] == null)
                return false;
            if (weapons[selecteWeaponIndex].Shoot)
            {
                info = weapons[selecteWeaponIndex].bullet;
                return true;
            }
            return false;
        }
        public bool SetUpWeapon(TankWeapon weapon)
        {
            if (weapons == null)
                weapons = new List<TankWeapon>();
            if (weaponsPoints == null)
                return false;
//            if (!(weapons.Count < weaponsPoints.Count))
//                return false;
            weapons.Add(weapon);
            weapon.transform.SetParent(transform, true);
            weapon.SetUpPosition(weaponsPoints[0]);
//            weapon.SetUpPosition(weaponsPoints[weapons.Count - 1]);
            return true;
        }
        public abstract void ChangeWeapon(int slotNumber,TankWeapon newWeapon);

        public void SelectNextWeapon()
        {
            if (weapons == null || weapons.Count < 2) return;
            int new_index=selecteWeaponIndex+1;
            new_index = new_index >=weapons.Count?0:new_index;
            SelectWeaponByIndex(selecteWeaponIndex, new_index);
            selecteWeaponIndex = new_index;
        }

        public void SelectPreviousWeapon()
        {
            if (weapons == null || weapons.Count < 2) return;
            int new_index = selecteWeaponIndex-1;
            new_index = new_index < 0 ? weapons.Count-1 : new_index;
            SelectWeaponByIndex(selecteWeaponIndex, new_index);
            selecteWeaponIndex = new_index;
        }
        private void SelectWeaponByIndex(int prev_index,int new_index)
        {
            weapons[prev_index].SetAnimationState(Equipement.WeaponAnimateState.Drop);
            weapons[new_index].gameObject.SetActive(true);
            weapons[new_index].SetAnimationState(Equipement.WeaponAnimateState.PickUp);
        }

        public void MoveTankCommand(Vector3 direction, float weight)
        {
            moveModel.Move(direction, transform.forward, body.mass * weight);
        }
        
        public void RotateTurret(Vector3 direction)
        {
            weaponsPoints[0].localRotation = Quaternion.Lerp(
                weaponsPoints[0].localRotation, Quaternion.LookRotation(direction),Time.deltaTime);
            weapons[selecteWeaponIndex].SetUpPosition(weaponsPoints[0]);
        }
    }
}
