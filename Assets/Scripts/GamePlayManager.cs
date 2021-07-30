using System;
using System.Collections.Generic;
using UnityEngine;
using static GamePlayModel;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager Singleton { get; protected set; }

    public string Token { get; set; }
    public HomeData HomePlayData { get; set; }

    public ApiModule ApiModule = new ApiModule();

    public string USERPOOL_ID = "";
    //public string IDENTITYPOOL_ID = "";
    public string CLIENT_ID = "";

    [System.Serializable]
    public class GamePlayConfig
    {
        public string ApiUrl;
        public string UserPoolId;
        public string IdentityPoolId;
        public string ClientId;
    }

    public void Awake()
    {
        if (Singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Singleton = this;
    }

    public void Start()
    {
        Debug.Log("[PlayManager] Singleton Start");
        HomePlayData = new HomeData();

        ReadConfigFile("config.json");
    }

    private void ReadConfigFile(string fileName)
    {
        string contents = "";
        using (System.IO.StreamReader configFile = new System.IO.StreamReader(Application.streamingAssetsPath + "/" + fileName))
        {
            contents = configFile.ReadToEnd();
        }
        GamePlayConfig gamePlayConfig = JsonUtility.FromJson<GamePlayConfig>(contents);

        // Set application config variables
        this.ApiModule.API_URL = gamePlayConfig.ApiUrl;
        this.USERPOOL_ID = gamePlayConfig.UserPoolId;
        //this.IDENTITYPOOL_ID = gamePlayConfig.IdentityPoolId;
        this.CLIENT_ID = gamePlayConfig.ClientId;
    }
}
