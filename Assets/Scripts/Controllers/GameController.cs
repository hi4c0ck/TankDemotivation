using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
using Assets.Scripts.Maze;
using Assets.Scripts.Swarm;
using System.Collections.Generic;
using Assets.Scripts.Notifications;

public class GameController : MonoBehaviour,ISubscriber,IPublisher {
    int GAME_LEVEL = 0;
    public int GAME_LEVELUP_KILLS = 5;
    public int game_plane_width = 20;
    public int game_plane_height = 20;
    public float cellSize = 1f;
    GameObject plane_obj;
    GameObject outer_plane;
    public Material plane_material;
    public Material outer_plane_material;
    public Material outer_objects_material;
    public Camera playCamera;

    public MazeHolder mazeHolder;
    public Swarm swarm;
    public PhysicMaterial earthPhysMaterial;
    public List<GameStats> EnemiesStats;
    int kills_counter = 0;
    // Use this for initialization
    void Start () {
        // Генерирует Plane в осях: width => X, heigth => Z, перпендикуляр (высота) => Y
        plane_obj = GameFieldGenerator.GenPlane(game_plane_width, game_plane_height, cellSize, plane_material);//new GameObject();//GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane_obj.layer = LayerMask.NameToLayer("enviorment");
        var bxc=plane_obj.AddComponent<BoxCollider>();
        plane_obj.transform.localPosition = Vector3.zero;            
        bxc.material = earthPhysMaterial;
        outer_plane = GameFieldGenerator.GenOuterPlane(game_plane_width, game_plane_height, cellSize, outer_plane_material, 1);
        outer_plane.transform.SetParent(plane_obj.transform, true);
        mazeHolder = new MazeHolder(game_plane_width, game_plane_height);
        swarm.StartIniBlocks(mazeHolder.Mazes);
        kills_counter = 0;
        //notifications ini
        var nc = FindObjectOfType<NotificationCenter>();
        nc.SetUpSubscriber(this);
        SetUpSubscriber(nc);

    }
    public void ResetGameLevel()
    {
        swarm.ResetAllPools();
        mazeHolder = new MazeHolder(game_plane_width, game_plane_height);
        swarm.PopulateMazeObjects(mazeHolder.Mazes);
        kills_counter = 0;
        GAME_LEVEL = 0;
        SendMessage(new OnHeroLevelUP
        {
            achievedLevel = GAME_LEVEL,
            NextLevelTargetKills = GAME_LEVELUP_KILLS
        });

    }
    Vector3 _ppos = Vector3.zero;
    public void OnShoot(BulletInfo bulletInfo)
    {
        swarm.SpawnBullet(bulletInfo);
    }
    public void UpdateHeroPosition(Vector3 pos)
    {
        _ppos.y = plane_obj.transform.position.y;
        _ppos.x = pos.x;
        _ppos.z = pos.z;
    }

    // Update is called once per frame
    void Update() {

        plane_obj.transform.position = _ppos;
        outer_objects_material.SetVector("_HeroPos", _ppos);
        outer_plane.GetComponent<MeshRenderer>().material.SetVector("_HeroPos", _ppos);
        

        mazeHolder.UpdateMaze(_ppos);
        swarm.PopulateMazeObjects(mazeHolder.Mazes);

        plane_obj.GetComponent<MeshRenderer>().material.SetTextureOffset("_MainTex", new Vector2(_ppos.x/game_plane_width
            , _ppos.z/game_plane_height));
        outer_plane.GetComponent<MeshRenderer>().material.SetVector("_MainOffset", new Vector4(_ppos.x / game_plane_width
            , _ppos.z / game_plane_height,0,0));

        release_do_timer += Time.deltaTime;
        if (release_do_timer > release_distante_objects_time)
        {
            release_do_timer = 0;
            swarm.ReleaseDistantedObjects(_ppos, game_plane_width / 2f + 5.5f);
        }
        enemies_spr_timer += Time.deltaTime;
        if (enemies_spr_timer > EnemiesSpawnRate)
            if (swarm.AliveEnemies < MAX_ENEMIES_ALIVE)
            {
                enemies_spr_timer = 0;
                swarm.SpawnEnemieByMaze(mazeHolder.Mazes,3,EnemiesStats[Mathf.Clamp(GAME_LEVEL,0,EnemiesStats.Count-1)]);
            }

        path_find_timer+= Time.deltaTime;
        if (path_find_timer > PATH_FIND_RATE)
        {
            path_find_timer = 0;
            for (int i = 0; i < swarm.EnemiesPool.Count; i++)
                if(swarm.EnemiesPool[i].gameObject.activeSelf)
            {
                    mazeHolder.CalculateToPointDirection(swarm.EnemiesPool[i].transform.position,
                        _ppos, ref swarm.EnemiesPool[i].MoveDirection);
            }
        }

    }

    public void OnNotify(INotification notification)
    {
        if (notification is OnEnemyKilledNotification)
        {
            kills_counter++;
            if (kills_counter > 4)
            {
                GAME_LEVEL++;
                kills_counter = 0;
                SendMessage(new OnHeroLevelUP
                {
                    achievedLevel = GAME_LEVEL,
                    NextLevelTargetKills = GAME_LEVELUP_KILLS
                });
            }
        }
    }
    ISubscriber NCenter;
    public void SetUpSubscriber(ISubscriber subscriber)
    {
        NCenter=subscriber;
    }

    public void SendMessage(INotification notification)
    {
        NCenter.OnNotify(notification);
    }

    float release_distante_objects_time=.2f;
    float release_do_timer = 0;

    public int MAX_ENEMIES_ALIVE = 10;
    public float EnemiesSpawnRate=2f;
    float enemies_spr_timer = 0;

    public float PATH_FIND_RATE= 1.47f;
    float path_find_timer = 0;
}
