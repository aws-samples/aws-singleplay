using UnityEngine;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static GamePlayModel;

public class HttpModule
{

    private static HttpClient client = new HttpClient();

    public delegate void CallbackDelegate(string result);

    public static async Task<string> GetAsync(string path, string jwt, CallbackDelegate callback)
    {
        string resp = "";

        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(path),
            Headers =
            {
                {
                    HttpRequestHeader.Authorization.ToString(), jwt
                },
                {
                    "ClientInfo", JsonUtility.ToJson(GameSystem.GetClientInfo())
                }
            }
        };

        HttpResponseMessage response = await client.SendAsync(httpRequestMessage);
        if (response.IsSuccessStatusCode)
        {
            resp = response.Content.ReadAsStringAsync().Result;
            callback(resp);
        }
        else
        {
            Debug.Log("[HttpModule] Send Error - " + response.Content);
        }
        return resp;
    }

    public static async Task<string> PostAsync(string path, string jwt, CallbackDelegate callback)
    {
        string resp = "";

        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(path),
            Headers =
            {
                {
                    HttpRequestHeader.Authorization.ToString(), jwt
                },
                {
                    "ClientInfo", JsonUtility.ToJson(GameSystem.GetClientInfo())
                }
            }
        };

        HttpResponseMessage response = await client.SendAsync(httpRequestMessage);
        if (response.IsSuccessStatusCode)
        {
            resp = response.Content.ReadAsStringAsync().Result;
            callback(resp);
        }
        else
        {
            Debug.Log("[HttpModule] Send Error - " + response.Content);
        }
        return resp;
    }

    public static async Task<string> PostAsync(string path, string jwt, UserData userdata, CallbackDelegate callback)
    {
        string resp = "";

        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri(path),
            Headers =
            {
                {
                    HttpRequestHeader.Authorization.ToString(), jwt
                },
                {
                    "ClientInfo", JsonUtility.ToJson(GameSystem.GetClientInfo())
                },
                {
                    "UserData", JsonUtility.ToJson(userdata)
                }
            }
        };

        HttpResponseMessage response = await client.SendAsync(httpRequestMessage);
        if (response.IsSuccessStatusCode)
        {
            resp = response.Content.ReadAsStringAsync().Result;
            callback(resp);
        }
        else
        {
            Debug.Log("[HttpModule] Send Error - " + response.Content);
        }
        return resp;
    }
}
