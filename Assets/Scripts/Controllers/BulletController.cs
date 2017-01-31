using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class BulletController : MonoBehaviour
    {
        public BulletInfo bulletInfo;

        void Update()
        {
            transform.localPosition += bulletInfo.shootDirection * (bulletInfo.velocity * Time.deltaTime);
        }

        public void OnCollisionEnter(Collision collision)
        {
            Debug.Log("1");
            if (collision.gameObject.layer == LayerMask.NameToLayer("enviorment"))
            {
                Debug.Log("2");
                gameObject.SetActive(false);
            }

        }
    }
}
