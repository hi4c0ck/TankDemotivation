using Assets.Scripts.Enemy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Impl.Enemies
{
    public class LevelColoredEnemy : AnyEnemie
    {
        public Color tint;
        public List<Color> level_colors;
        public MeshRenderer enemyMesh;
        
        public override void OnEnemieAlive()
        {
            base.OnEnemieAlive();
            SetUpColorByLevel();
            enemyMesh.material.SetColor("_Color", tint);
        }
        
        void SetUpColorByLevel()
        {
            if (stats.Level < level_colors.Count)
                tint = level_colors[stats.Level];
            else
            {
                tint = level_colors[0];
            }
        }


    }
}
