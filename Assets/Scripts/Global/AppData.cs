using UnityEngine;
using System.Collections;

public enum AppState
{
    Register,
    AddChild,
    AddPet,
    ChangeInfo,
    Main
}

namespace Gululu{
    
    public class AppData
    {
        public AppState currentState = AppState.Main;

        private static AppData instance;

        public static AppData getInstance()
        {
            if (instance == null)
            {
                instance = new AppData();
                checkServices();
            }
            return instance;
        }

        public static void checkServices()
        {
            if (ServiceConfig.service != ServiceType.Product){
                Debug.LogWarning("Now is Not Product Service");
            }else{
                Debug.Log("Now is In " + ServiceConfig.service.ToString() + " service"); 
            }
        }

        public static string GetAppName()
        {
            return "GameMercury";
        }

    }

}

