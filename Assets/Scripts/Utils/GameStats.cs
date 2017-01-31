using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Utils
{
    [Serializable]
    public struct GameStats
    {
        public float HealthPoints;
        public float CurrentHP;
        public bool isAlive;
        public float Armour;
        public float Damage;
        public int Level;
        public float Velocity;
    }
}
