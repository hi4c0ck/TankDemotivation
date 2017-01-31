using System;
using Assets.Scripts.Controllers;
using Assets.Scripts.Equipement.Abstract;
using Assets.Scripts.Inputers;
using Assets.Scripts.Notifications;
using Assets.Scripts.Tank;
using Assets.Scripts.Utils;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class HeroController : MonoBehaviour,ISubscriber,IPublisher {

    public AnyTank selectedTank;

    Rigidbody body;
    public GameController gameController;
    public CamerController camController;
    public InputController inputController;
    public PrefabsManager prefabs;
    GameStats heroConditions;
    GameStateMachine stateMachine;
    
    // Use this for initialization
    void Start () {
        var nt = FindObjectOfType<NotificationCenter>();
        nt.AddSubscriber(this);
        SetUpSubscriber(nt);
        inputController.onInput += MoveHero;
        InitializeTank();
        stateMachine = GameStateMachine.Hold;
    }
    public void ResetGame()
    {
        ResetStats();
        stateMachine = GameStateMachine.Prepare;
    }
    void InitializeTank()
    {
        if (selectedTank==null)
            selectedTank=Instantiate(prefabs.tanksPrefabs[0]).GetComponent<AnyTank>();
        selectedTank.gameObject.SetActive(true);
        var weapon = Instantiate(prefabs.weaponsPrefabs[0]).GetComponent<TankWeapon>();
        weapon.gameObject.SetActive(true);
        selectedTank.SetUpWeapon(weapon);
        var weapon2 = Instantiate(prefabs.weaponsPrefabs[1]).GetComponent<TankWeapon>();
        weapon2.gameObject.SetActive(false);
        selectedTank.SetUpWeapon(weapon2);
    }
    void ResetStats()
    {
        selectedTank.transform.SetParent(transform, true);
        selectedTank.transform.localPosition = Vector3.zero;
        heroConditions = selectedTank.tankStats;
        heroConditions.CurrentHP = heroConditions.HealthPoints;
        heroConditions.isAlive = true;
        if (NCenter != null)
        {
            NCenter.OnNotify(new OnWeaponChangedNotification { activeWeaponIndex = selectedTank.SelectedWeaponIndext });
            NCenter.OnNotify(new OnHeroChangeStatsNotification { heroStats = heroConditions });
        }
    }

    BulletInfo current_bullet;
    void MoveHero(InputCommand command)
    {
        if (command is MoveCommand)
        {
            selectedTank.MoveTankCommand(command.worldDirection, .4f);
        }
        else if (command is ShootCommand)
        {
            if (selectedTank.TryShoot(ref current_bullet))
                gameController.OnShoot(current_bullet);
        }
        else if (command is RotateCommand)
        {
            selectedTank.RotateTurret(command.worldDirection);
        }
        else if (command is SwitchWeaponCommand)
        {
            if (command.worldDirection.x > 0)
                selectedTank.SelectNextWeapon();
            else
            {
                selectedTank.SelectPreviousWeapon();
            }
            NCenter.OnNotify(new OnWeaponChangedNotification { activeWeaponIndex = selectedTank.SelectedWeaponIndext });

        }
        else if (command is ResetLevelCommand)
        {
            NCenter.OnNotify(new OnRestartGameNotification());
        }

    }
    Vector3 tmpPos=Vector3.zero;
    // Update is called once per frame
    void Update () {
        if (stateMachine==GameStateMachine.Prepare)
        {
            Time.timeScale = 0f;
            ResetGame();
            stateMachine = GameStateMachine.Running;
            gameController.ResetGameLevel();
        }
        else if (stateMachine==GameStateMachine.Running)
        {
            Time.timeScale = 1f;
            tmpPos = selectedTank == null ? transform.position : selectedTank.transform.position;
            gameController.UpdateHeroPosition(tmpPos);
            camController.SetUpCam(tmpPos);
        }
        else if (stateMachine == GameStateMachine.Hold)
        {
            Time.timeScale = 0f;
        }
        else if (stateMachine == GameStateMachine.HeroDied)
        {
            Time.timeScale = 0f;
            NCenter.OnNotify(new OnEndGameNotification { totalKills= 123});
            stateMachine = GameStateMachine.Hold;
        }
        

    }
    void OnDestroy()
    {
        inputController.onInput -= MoveHero;
    }

    float calcDamage(float input_damage)
    {
        return input_damage * (1 - heroConditions.Armour);
    }
    float current_HP = 0;
    public void OnNotify(INotification notification)
    {
        if (notification is OnHeroLevelUP)
        {

        }
        else if (notification is OnDealDamageToHero)
        {
            if (!heroConditions.isAlive)
            {
                return;
            }
            var n = notification as OnDealDamageToHero;
            heroConditions.CurrentHP -= calcDamage(n.DealDamage);
            NCenter.OnNotify(new OnHeroChangeStatsNotification { heroStats = heroConditions });
            if (heroConditions.CurrentHP<0)
            {
                stateMachine = GameStateMachine.HeroDied;
                heroConditions.isAlive = false;
            }
        }
    }
    ISubscriber NCenter;
    public void SetUpSubscriber(ISubscriber subscriber)
    {
        NCenter = subscriber;
    }

    public void SendMessage(INotification notification)
    {
        if (NCenter != null)
            NCenter.OnNotify(notification);
    }

}
