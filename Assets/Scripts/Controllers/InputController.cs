using UnityEngine;
using System.Collections;
using Assets.Scripts.Inputers;

public class InputController : MonoBehaviour {

    IInputer input;
    public KeyBoardInputer key_inputer;
    // Use this for initialization
    void Start () {
        input = key_inputer;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            onInput(rstLevel);

        }
        if (input.Inputed)
        {
            mv_cmd.worldDirection = input.MoveDirection;
            onInput(mv_cmd);
        }
        if (input.Shooted)
        {
            onInput(sht_cmd);
        }
        if (input.Rotated)
        {
            rot_cmd.worldDirection = input.RotationDirection;
            onInput(rot_cmd);
        }


        if (input.Wpn_switched)
        {
            switch_wpn_cmd.worldDirection = input.SwitchWeapon;
            onInput(switch_wpn_cmd);
        }
    }
    MoveCommand mv_cmd=new MoveCommand();
    ShootCommand sht_cmd = new ShootCommand();
    RotateCommand rot_cmd = new RotateCommand();
    SwitchWeaponCommand switch_wpn_cmd = new SwitchWeaponCommand();
    ResetLevelCommand rstLevel= new ResetLevelCommand();
    public OnInput onInput;
    public delegate void OnInput(InputCommand command);

}
