using UnityEngine;
using static GamePlayModel;

public class ApiModule
{
    public string API_URL = "";

    public async void CallHomeAPI(string jwt, HttpModule.CallbackDelegate callback)
    {
        Debug.Log("Call Home API");
        string result = await HttpModule.GetAsync(API_URL + "/home", jwt, callback);
        Debug.Log("API Call Success. result: " + result);
    }

    public async void CallPurchaseAPI(string jwt, HttpModule.CallbackDelegate callback)
    {
        Debug.Log("Call Shop API");
        string result = await HttpModule.PostAsync(API_URL + "/shop", jwt, callback);
        Debug.Log("API Call Success. result: " + result);
    }

    public async void CallSyncAPI(string jwt, UserData userData, HttpModule.CallbackDelegate callback)
    {
        Debug.Log("Call Sync API");
        string result = await HttpModule.PostAsync(API_URL + "/sync", jwt, userData, callback);
        Debug.Log("API Call Success. result : " + result);
    }
}
