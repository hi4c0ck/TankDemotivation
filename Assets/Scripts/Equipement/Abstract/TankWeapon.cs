using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Equipement.Abstract
{
    public abstract class TankWeapon:MonoBehaviour,IAnimatedWeapon
    {
        public Transform shootPosition;
        public BulletInfo bullet;
        public Animator weaponAnimator;
        WeaponAnimateState animationState= WeaponAnimateState.Idle;
        
        
        public virtual void SetUpPosition(Transform position_transform)
        {
            transform.localPosition = position_transform.localPosition;
            transform.localRotation= position_transform.localRotation;
        }
        public float fireRate;
        /// <summary>
        /// return shoot position in world coordinates
        /// </summary>
        /// <returns></returns>
        public Vector3 ShootPosition()
        {
            return shootPosition.position;
        }

        float reload_delay = 0;
        bool ready_to_shoot = false;
        public bool Shoot
        {
            get
            {
                if (ready_to_shoot && animationState == WeaponAnimateState.Idle)
                {
                    ready_to_shoot = false;
                    bullet.startPosition = shootPosition.position;
                    bullet.shootDirection = transform.forward;
                    SetAnimationState(WeaponAnimateState.Fire);
                    reload_delay = 0;
                    return true;
                }
                return false;
            }
        }
        void Update()
        {
            if (!ready_to_shoot)
                if (reload_delay < fireRate)
                    reload_delay += Time.deltaTime;
                else
                {
                    reload_delay = 0;
                    ready_to_shoot = true;
                }


        }

        public void SetAnimationState(WeaponAnimateState state)
        {
            animationState = state;
            switch(animationState)
            {
                case WeaponAnimateState.Idle:
                    break;
                case WeaponAnimateState.Drop:
                    weaponAnimator.SetBool("DropState", true);
                    StartCoroutine(WaitForAnimationEnd());
                    break;
                case WeaponAnimateState.PickUp:
                    weaponAnimator.SetBool("PickUpState", true);
                    StartCoroutine(WaitForAnimationEnd());
                    break;
                case WeaponAnimateState.Fire:
                    weaponAnimator.SetBool("FireState", true);
                    StartCoroutine(WaitForAnimationEnd());
                    break;
            }
        }

        public void OnStateEnd(WeaponAnimateState new_state)
        {
            weaponAnimator.SetBool("FireState", false);
            weaponAnimator.SetBool("DropState", false);
            weaponAnimator.SetBool("PickUpState", false);
            if (animationState == WeaponAnimateState.Drop)
                gameObject.SetActive(false);
            animationState = new_state;
        }



        public IEnumerator WaitForAnimationEnd()
        {
            yield return new WaitForEndOfFrame();
            do
            {
                yield return null;
            } while (gameObject.activeSelf && weaponAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);
            OnStateEnd(WeaponAnimateState.Idle);
        }
    }
}
