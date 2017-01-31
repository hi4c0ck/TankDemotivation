using Assets.Scripts.Controllers;
using Assets.Scripts.Notifications;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public abstract class AnyEnemie : MonoBehaviour, IPublisher
    {
        public GameStats stats;
        public Vector3 MoveDirection;
        Rigidbody body;
        Collider collider;
        Animator animator;
        float atck_timer = 0;
        float calculateDamage(float damage)
        {
            return damage * (1 - stats.Armour);
        }

        public virtual void OnEnemieAlive()
        {
            stats.CurrentHP = stats.HealthPoints;
        }

        public void OnEnable()
        {
            OnEnemieAlive();
        }
        void Start()
        {
            body = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
            animator = GetComponent<Animator>();
            var nc = FindObjectOfType<NotificationCenter>();
            SetUpSubscriber(nc);

        }
        Vector3 zeroground = new Vector3(1, 0, 1);
        void Update()
        {
            //            transform.LookAt(transform.position + MoveDirection);
            transform.forward = MoveDirection; //Vector3.Lerp(transform.forward, MoveDirection, Time.deltaTime * 3f);
            transform.position += transform.forward * (Time.deltaTime * stats.Velocity);
            atck_timer += Time.deltaTime;
        }

        public void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("HeroBullet"))
            {
                var bc = collision.gameObject.GetComponent<BulletController>();
                float sufferDamage = calculateDamage(bc.bulletInfo.damage);
                stats.CurrentHP -= sufferDamage;
                if (stats.CurrentHP <= 0)
                {
                    SendMessage(new OnEnemyKilledNotification
                    {
                        killedPoint = transform.position
                    });
                    gameObject.SetActive(false);
                }
                else
                {
                    SendMessage(new OnEnemyDamagedNotification
                    {
                        damagedTarget = transform,
                        damageSuffer = sufferDamage,
                        targetStats = stats
                    });
                }
                collision.gameObject.SetActive(false);
            }

            if (collision.gameObject.layer == LayerMask.NameToLayer("Hero"))
                if (atck_timer > 1f)
                {
                    atck_timer = 0;
                    animator.SetBool("isAttack", true);
                    StartCoroutine(ResetAnimation());
                    SendMessage(new OnDealDamageToHero
                    {
                        DealDamage = stats.Damage
                    });

                }

        }

        IEnumerator ResetAnimation()
        {
            yield return new WaitForSeconds(.5f);
            animator.SetBool("isAttack", false);
        }

        ISubscriber notificationCenter;
        public void SendMessage(INotification notification)
        {
            notificationCenter.OnNotify(notification);
        }

        public void SetUpSubscriber(ISubscriber subscriber)
        {
            notificationCenter = subscriber;
        }
    }
}
