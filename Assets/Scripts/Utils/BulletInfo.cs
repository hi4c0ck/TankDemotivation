using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    [Serializable]
    public struct BulletInfo
    {
        [SerializeField]
        public string prefabName { get; set; }
        public Vector3 startPosition { get; set; }
        public Vector3 shootDirection { get; set; }
        [SerializeField]
        public float damage;
        [SerializeField]
        public float velocity;

    }
}
