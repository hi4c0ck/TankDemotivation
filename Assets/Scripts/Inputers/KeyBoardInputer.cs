using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Inputers
{
    public class KeyBoardInputer : MonoBehaviour, IInputer
    {
        bool inputed = false;
        Vector3 inputVector=Vector3.zero;
        bool rotated = false;
        Vector3 rotatedVector = Vector3.zero;
        bool wpn_switched = false;
        Vector3 weaponVector = Vector3.zero;
        bool shooted;

        void Update()
        {
            #region Axis move
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                inputVector.x = -1;
                inputed = true;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                inputVector.x = 1;
                inputed = true;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                inputVector.z = 1;
                inputed = true;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                inputVector.z = -1;
                inputed = true;
            }
            #endregion
            #region Rotate

            if (Input.GetKey(KeyCode.A))
            {
                rotatedVector.x = -1;
                rotated = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                rotatedVector.x = 1;
                rotated = true;
            }
            if (Input.GetKey(KeyCode.W))
            {
                rotatedVector.z = 1;
                rotated = true;
            }
            if (Input.GetKey(KeyCode.S))
            {
                rotatedVector.z = -1;
                rotated = true;
            }

            #endregion
            #region Shoot


            if (Input.GetKey(KeyCode.X))
            {
                shooted = true;
            }
            #endregion
            #region SwitchWeapon
            if (Input.GetKeyDown(KeyCode.Q))
            {
                weaponVector = Vector3.left;
                wpn_switched=true;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                weaponVector = Vector3.right;
                wpn_switched = true;
            }

            #endregion



            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
            if (!inputed)
            {
                inputVector.x = 0;
                inputVector.z = 0;
            }
            if (!rotated)
            {
                rotatedVector.x = 0;
                rotatedVector.z = 0;
            }
        }
        public bool Inputed
        {
            get
            {
                if (inputed)
                {
                    inputed = false;
                    return true;
                }
                return inputed;
            }
        }
        public Vector3 MoveDirection
        {
            get
            {
                return inputVector.normalized;
            }
        }

        public bool Shooted
        {
            get
            {
                if (shooted)
                {
                    shooted = false;
                    return true;
                }
                return shooted;
            }
        }

        public Vector3 RotationDirection
        {
            get
            {
                return rotatedVector.normalized;
            }
        }

        public bool Rotated
        {
            get
            {
                if (rotated)
                {
                    rotated = false;
                    return true;
                }
                return rotated;
            }
        }

        public Vector3 SwitchWeapon
        {
            get
            {
                return weaponVector;
            }
        }

        public bool Wpn_switched
        {
            get
            {
                if (wpn_switched)
                {
                    wpn_switched = false;
                    return true;
                }
                return wpn_switched;
            }
        }
    }
}
