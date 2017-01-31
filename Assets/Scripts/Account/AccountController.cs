using UnityEngine;
using Assets.Scripts;

public class AccountController : MonoBehaviour
{

    public bool ResetAccountDataOnStart = false;
    public bool ResetItemShop = false;
    public AccountManager manager;
    public string dataName = "Account";
    public string inventaryName = "inventrary";
    public void ResetAccountInfo()
    {
        manager = AccountManager.Default_CreateAccount(dataName);
    }

    // Use this for initialization
    public void LoadAccount()
    {
        if (ResetAccountDataOnStart)
        {
            ResetAccountInfo();// manager = AccountManager.Default_CreateAccount(dataName);
        }
        else
        {
            manager = AccountManager.LoadAccaunt(dataName);            
            if (manager == null)
                manager = AccountManager.Default_CreateAccount(dataName);
//            manager.settings.iniDefaults();
        }
        
    }
}
