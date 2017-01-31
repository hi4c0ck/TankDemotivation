using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Maze
{
    public struct MazeObject
    {
        public MazeObjectType type;
        public bool is_Alive;
        public Vector3 position;
        public Vector3 scales;
        public string prefabName;
        
    }
}
