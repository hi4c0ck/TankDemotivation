using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class AccountManager
    {
        private static string fileName = "Account.json";
        

        
        [SerializeField]
        public Settings settings;
        public void iniSetts()
        {
            settings = new Settings();
        }

        public static AccountManager Default_CreateAccount(string name)
        {
            AccountManager man = new AccountManager()
            {
                //inventary = new List<GameObject>() { Resources.Load<GameObject>("Prefabs/Brevno") },
            };
            man.iniSetts();
            AccountManager.SaveGame(man,name);
            return man;
        }


        public static bool SaveGame(AccountManager saveGame, string name)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(GetSavePath(name), FileMode.Create))
            {
                try
                {
                    var v = JsonUtility.ToJson(saveGame);
                    formatter.Serialize(stream, v);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public static AccountManager LoadAccaunt(byte[] bytes)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                try
                {
                    var v = formatter.Deserialize(stream) as string;
                    return JsonUtility.FromJson<AccountManager>(v);
                    //                    return formatter.Deserialize(stream) as AccountManager;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static AccountManager LoadAccaunt(string name)
        {
            if (!DoesSaveGameExist(name))
            {
                return null;
            }

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(GetSavePath(name), FileMode.Open))
            {
                try
                {
                    var v = formatter.Deserialize(stream) as string;
                    return  JsonUtility.FromJson<AccountManager>(v);
//                    return formatter.Deserialize(stream) as AccountManager;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public static bool DeleteSaveGame(string name)
        {
            try
            {
                File.Delete(GetSavePath(name));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static bool DoesSaveGameExist(string name)
        {
            return File.Exists(GetSavePath(name));
        }

        private static string GetSavePath(string name)
        {
            return Path.Combine(Application.persistentDataPath, name + ".json");
        }
    }

    [Serializable]
    public class RopedObject
    {
        public bool avaliable { get { return avaliable; } }
        public GameObject prefabObject;
        public string Name;
        public Sprite iconObject;
    }






}
