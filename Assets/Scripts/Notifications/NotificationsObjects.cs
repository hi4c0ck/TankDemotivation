using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Notifications
{
    public sealed class OnEnemyKilledNotification : INotification
    {
        public Vector3 killedPoint;
    }
    public sealed class OnEnemyDamagedNotification : INotification
    {
        public Transform damagedTarget;
        public GameStats targetStats;
        public float damageSuffer;
    }
    public sealed class OnHeroLevelUP : INotification
    {
        public int achievedLevel;
        public int NextLevelTargetKills;
    }
    public sealed class OnEndGameNotification:INotification
    {
        public int totalKills;
    }
    public sealed class OnRestartGameNotification : INotification
    {
    }

    public sealed class OnDealDamageToHero : INotification
    {
        public float DealDamage;
    }

    public sealed class OnHeroChangeStatsNotification : INotification
    {
        public GameStats heroStats;
    }

    public sealed class OnWeaponChangedNotification : INotification
    {
        public int activeWeaponIndex;
    }
}
