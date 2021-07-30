using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static GamePlayModel;

public class GameHome : MonoBehaviour
{
    private Texture homeTexture;

    public Text GameDataText;
    public Text UserDataText;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Game Home");
        UpdateGameEvent();
        StartCoroutine(GetTexture());
    }

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(Screen.width/2, 10, 60, 60), homeTexture, ScaleMode.StretchToFill, true, 10.0f);

        var gameDataText = "Resource URL : " + GamePlayManager.Singleton.HomePlayData.GameData.Resource + "\r\nEvent Data : \r\n";
        //foreach (var key in GamePlayManager.Singleton.HomePlayData.GameData.GameEvent.Keys)
        //{
        //    gameDataText += key + " : " + GamePlayManager.Singleton.HomePlayData.GameData.GameEvent[key] + "\r\n";
        //}
        gameDataText += "Login Reward Ruby : " + GamePlayManager.Singleton.HomePlayData.GameData.LoginRuby;

        var userDataText = "";
        userDataText += "User Name : " + GamePlayManager.Singleton.HomePlayData.UserData.UserName + "\r\n";
        userDataText += "User Play Score : " + GamePlayManager.Singleton.HomePlayData.UserData.UserPlayScore + "\r\n";
        userDataText += "User Ruby : " + GamePlayManager.Singleton.HomePlayData.UserData.UserRuby + "\r\n";
        userDataText += "Last Play Time : " + GamePlayManager.Singleton.HomePlayData.UserData.LastPlayTime + "\r\n";

        GameDataText.text = gameDataText;
        UserDataText.text = userDataText;
    }

    private void UpdateGameEvent()
    {
        //LoginEventData loginEventData = JsonUtility.FromJson<LoginEventData>(GamePlayManager.Singleton.HomePlayData.GameData.GameEvent["Login"]);
        Debug.Log("Update Game Event");
        Debug.Log(GamePlayManager.Singleton.HomePlayData.GameData.LoginRuby);
        GamePlayManager.Singleton.HomePlayData.UserData.UserRuby += GamePlayManager.Singleton.HomePlayData.GameData.LoginRuby;
    }

    IEnumerator GetTexture()
    {
        // Draw Texture based on CDN resource.
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(GamePlayManager.Singleton.HomePlayData.GameData.Resource);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            homeTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
        }
    }

    public void OnClickGamePlayBtn()
    {
        // Play Game
        GamePlayManager.Singleton.HomePlayData.UserData.UserPlayScore += (10 + GamePlayManager.Singleton.HomePlayData.UserData.UserRuby);
        GamePlayManager.Singleton.HomePlayData.UserData.UserRuby += 1;
    }

    public void OnClickUserDataSyncBtn()
    {
        // Call WebSyncAPI
        Debug.Log("Click Sync Button");

        GamePlayManager.Singleton.ApiModule.CallSyncAPI(
            GamePlayManager.Singleton.Token, GamePlayManager.Singleton.HomePlayData.UserData, (string result) =>
        {
            Debug.Log("Call Shop API - Callback");
            Debug.Log(result);
            SyncData syncDataResponse = JsonUtility.FromJson<SyncData>(result);
            GamePlayManager.Singleton.HomePlayData.UserData = syncDataResponse.UserData;
            Debug.Log("Sync Callback end");
        });
    }

    public void OnClickShopBtn()
    {
        // Call WebPurchase API. Update UserRuby at Callback
        Debug.Log("Click Shop Button");

        GamePlayManager.Singleton.ApiModule.CallPurchaseAPI(GamePlayManager.Singleton.Token, (string result) =>
        {
            Debug.Log("Call Purchase API - Callback");
            Debug.Log(result);
            ShopData shopDataResponse = JsonUtility.FromJson<ShopData>(result);
            GamePlayManager.Singleton.HomePlayData.UserData = shopDataResponse.UserData;
            Debug.Log("Shop Callback end");
        });
    }

    public void OnClickExitGameBtn()
    {
        // Close the application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
