using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace Assets.Scripts.Tank
{
    [RequireComponent(typeof(Rigidbody))]
    public class TankMoveModel:MonoBehaviour
    {
        public List<WheelCollider> moveTrucks;
        public List<WheelCollider> steerTrucks;

        public float maxSteerAngle = 12f;
        public float steerLerpKoef = 0.3f;
        float actuelMaxSteer = 0f;

        Vector3 tmpV;
        float currentSteer;
        float currentMotor;
        public void Move(Vector3 direction, Vector3 forward, float weight)
        {
            currentMotor = direction.z * weight; 
            for (int i = 0; i < moveTrucks.Count; i++)
            {
                moveTrucks[i].motorTorque = currentMotor;
            }

            actuelMaxSteer = maxSteerAngle*(moveTrucks[0].motorTorque + 1) / moveTrucks[0].motorTorque;
            actuelMaxSteer = Mathf.Clamp(actuelMaxSteer, -maxSteerAngle, maxSteerAngle);
            currentSteer = Mathf.Lerp(currentSteer, actuelMaxSteer*direction.x,steerLerpKoef);
            currentSteer = Mathf.Clamp(currentSteer, -actuelMaxSteer, actuelMaxSteer);
            for (int i = 0; i < steerTrucks.Count; i++)
            {
                steerTrucks[i].steerAngle= currentSteer;
            }
//            currentSteer = tmpV.x;
        }
        void FixedUpdate()
        {
            currentSteer = Mathf.Lerp(currentSteer, 0, steerLerpKoef);
            if (Mathf.Abs(currentSteer) < .1f) currentSteer = 0;
            for (int i = 0; i < steerTrucks.Count; i++)
            {
                steerTrucks[i].steerAngle = currentSteer;
            }
            currentMotor = Mathf.Lerp(currentMotor, 0, Time.deltaTime);
            if (Mathf.Abs(currentMotor) < 1f) currentMotor = 0;
            for (int i = 0; i < moveTrucks.Count; i++)
            {
                moveTrucks[i].motorTorque = currentMotor;
            }
        }

        void Update()
        {
            /*
            for (int i = 0; i < steerTrucks.Count; i++)
            {
                steerTrucks[i].transform.Rotate(Vector3.forward *
                        moveTrucks[i].rpm * Time.deltaTime );
            }
            for (int i = 0; i < moveTrucks.Count; i++)
            {
                //                moveTrucks[i].transform.Rotate(Vector3.forward * currentMotor * Time.deltaTime);
                moveTrucks[i].transform.Rotate(Vector3.forward*
                        moveTrucks[i].rpm * Time.deltaTime );
            }
            */
        }
    }
}
