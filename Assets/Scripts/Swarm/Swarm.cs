using Assets.Scripts.Controllers;
using Assets.Scripts.Enemy;
using Assets.Scripts.Maze;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Swarm
{
    public class Swarm:MonoBehaviour
    {
        public PrefabsManager prefabsManager;
        public List<StaticObjectController> BlocksPool;
        public GameObject blockPrefab;

        public List<BulletController> BulletsPool;

        public List<AnyEnemie> EnemiesPool;

        public GameObject blocksHolder;
        public GameObject enemiesHolder;
        public GameObject bulletsHolder;
        void Start()
        {
            IniBlocksPool(20);
            IniBulletsPool(20);
            EnemiesPool = new List<AnyEnemie>();
        }

        public int AliveEnemies
        {
            get
            {
                int ret = 0;
                for (int i = 0; i < EnemiesPool.Count; i++)
                    if (EnemiesPool[i].gameObject.activeSelf)
                        ret++;
                return ret;
            }
        }

        public void StartIniBlocks(MazeObject[,] objs)
        {
            for (int i = 0; i < objs.GetLength(0); i++)
                for (int j = 0; j < objs.GetLength(1); j++)
                    if (objs[i,j].type==MazeObjectType.Block)
            {

                SetUpBlock(objs[i,j].position, objs[i,j].scales);
            }
        }

        public void PopulateMazeObjects(MazeObject[,] maze)
        {
            for (int i = 0; i < maze.GetLength(0); i++)
            {
                for (int j = 0; j < maze.GetLength(1); j++)
                    if (!maze[i,j].is_Alive)
                        if (maze[i,j].type==MazeObjectType.Block)
                {
                            maze[i, j].is_Alive = true;
                            SetUpBlock(maze[i, j].position, maze[i, j].scales);
                }

            }      
        }

        int max_iterations=10;
        public void SpawnEnemieByMaze(MazeObject[,] maze,int bounds,GameStats stats)
        {
            int x, y,it;
            it = 0;
            do
            {
                x =  Random.Range(0, bounds)+Random.Range(0,2)*(maze.GetLength(0)-1-bounds);
                y = Random.Range(0, bounds) + Random.Range(0, 2) * (maze.GetLength(1) - 1 - bounds);
                it++;
            } while (maze[x, y].type != MazeObjectType.Empty || it>max_iterations);
            if (!(it < max_iterations)) return;
            SpawnEnemie(maze[x, y].position,stats);
        }

        public void SpawnEnemie(Vector3 pos,GameStats stats)
        {
            GameObject go = findDeactivatedBlock<AnyEnemie>();
            if (go == null)
            {
                //                go = SpawnObject("AnyEnemie_" + EnemiesPool.Count);
                go = Instantiate(prefabsManager.monstersPrefabs[0]);
                go.transform.SetParent(enemiesHolder.transform, false);
                go.SetActive(false);
                var bc = go.GetComponent<AnyEnemie>();
                EnemiesPool.Add(bc);
            }
            var bca = go.GetComponent<AnyEnemie>();
            bca.stats= stats;
            go.SetActive(true);
            pos.y = go.transform.localPosition.y;
            go.transform.localPosition = pos;
            //Vector3.right - Условимся, что снаряд распологается вдоль оси Х
//            go.transform.rotation = Quaternion.LookRotation(Vector3.right, info.shootDirection);

        }

        public void SpawnBullet(BulletInfo info)
        {
            GameObject go = findDeactivatedBlock<BulletController>();
            if (go == null)
            {
                go =SpawnBullet("Bullet_" + BulletsPool.Count);
                go.transform.SetParent(bulletsHolder.transform, false);

                var bc = go.GetComponent<BulletController>();
                BulletsPool.Add(bc);
            }
            go.SetActive(true);
            var bca = go.GetComponent<BulletController>();
            bca.bulletInfo = info;
            go.transform.localPosition= info.startPosition;
            //Vector3.right - Условимся, что снаряд распологается вдоль оси Х
            go.transform.rotation = Quaternion.LookRotation(Vector3.right, info.shootDirection);

        }
        public void SetUpBlock(Vector3 pos, Vector3 scales)
        {
            GameObject go = findDeactivatedBlock<StaticObjectController>();
            if (go == null)
            {
                go = SpawnBlock();
            }
            
            go.SetActive(true);
            go.transform.position = pos +Vector3.up*(scales.y/2f);
            go.transform.localScale = scales;
        }

        public void UpdateEnemiesMoveDirection(Vector3 aim_position)
        {


        }

        GameObject findDeactivatedBlock<T>() where T:MonoBehaviour
        {
            if (typeof(T) == typeof(StaticObjectController))
            {
                for (int i = 0; i < BlocksPool.Count; i++)
                    if (!BlocksPool[i].gameObject.activeSelf)
                        return BlocksPool[i].gameObject;
            }
            else if (typeof(T) == typeof(BulletController))
            {
                for (int i = 0; i < BulletsPool.Count; i++)
                    if (!BulletsPool[i].gameObject.activeSelf)
                        return BulletsPool[i].gameObject;
            }
            else if (typeof(T) == typeof(AnyEnemie))
            {
                for (int i = 0; i < EnemiesPool.Count; i++)
                    if (!EnemiesPool[i].gameObject.activeSelf)
                        return EnemiesPool[i].gameObject;
            }

            return null;
        }

        void IniBlocksPool(int amount)
        {
            BlocksPool = new List<StaticObjectController>(amount);
            for(int i = 0; i < amount; i++)
			{
                SpawnBlock();
            };
        }
        void IniBulletsPool(int amount)
        {
            BulletsPool = new List<BulletController>(amount);
            for (int i = 0; i < amount; i++)
            {
                //                var go = SpawnObject("Bullet_"+i.ToString());
                var go = SpawnBullet("bullet_" + i.ToString()) ;
                go.transform.localScale = Vector3.one * .2f;
                var bc= go.AddComponent<BulletController>();
                go.SetActive(false);
                BulletsPool.Add(bc);
            };
        }

        GameObject SpawnBullet(string name)
        {
            var go = Instantiate(prefabsManager.bulletPrefabs[0]);
            go.transform.SetParent(bulletsHolder.transform, false);

            go.name=name;
            return go;
        }

        GameObject SpawnObject(string name)
        {
            var go = new GameObject(name);
            return go;
        }
        GameObject SpawnBlock()
        {
            var go = Instantiate(blockPrefab);
            go.transform.SetParent(blocksHolder.transform,false);
            go.SetActive(false);
            BlocksPool.Add(go.GetComponent<StaticObjectController>());
            return go;
        }

        public void ReleaseDistantedObjects(Vector3 center, float distance)
        {
            for (int i = 0; i < BlocksPool.Count; i++)
                if (BlocksPool[i].gameObject.activeSelf)
                    if ((Mathf.Abs(BlocksPool[i].gameObject.transform.localPosition.x - center.x) > distance)
                        || (Mathf.Abs(BlocksPool[i].gameObject.transform.localPosition.y - center.y) > distance))
                        BlocksPool[i].gameObject.SetActive(false);

            for (int i = 0; i < BulletsPool.Count; i++)
                if (BulletsPool[i].gameObject.activeSelf)
                    if ((Mathf.Abs(BulletsPool[i].gameObject.transform.localPosition.x - center.x) > distance)
                        || (Mathf.Abs(BulletsPool[i].gameObject.transform.localPosition.y - center.y) > distance))
                        BulletsPool[i].gameObject.SetActive(false);

            for (int i = 0; i < EnemiesPool.Count; i++)
                if (EnemiesPool[i].gameObject.activeSelf)
                    if ((Mathf.Abs(EnemiesPool[i].gameObject.transform.localPosition.x - center.x) > distance)
                        || (Mathf.Abs(EnemiesPool[i].gameObject.transform.localPosition.y - center.y) > distance))
                        EnemiesPool[i].gameObject.SetActive(false);
        }


        public void ResetAllPools()
        {
            for (int i = 0; i < BlocksPool.Count; i++)
                BlocksPool[i].gameObject.SetActive(false);

            for (int i = 0; i < BulletsPool.Count; i++)
                BulletsPool[i].gameObject.SetActive(false);

            for (int i = 0; i < EnemiesPool.Count; i++)
                EnemiesPool[i].gameObject.SetActive(false);
        }
    }
}
