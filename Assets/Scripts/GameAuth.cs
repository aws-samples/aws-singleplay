using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using static GamePlayManager;
using static GamePlayModel;

public class GameAuth : MonoBehaviour
{
    [SerializeField]
    public InputField UserIdInput;
    [SerializeField]
    public InputField UserPwInput;

    [SerializeField]
    public InputField SignUpIdInput;
    [SerializeField]
    public InputField SignUpPwInput;
    [SerializeField]
    public InputField SignUPEmailInput;
    public Text SignUpDisplayText;
    public Text SignInDisplayText;

    public void OnClickSignInBtn()
    {
        string inputId = UserIdInput.text;
        string inputPw = UserPwInput.text;

        Debug.Log("[GameAuth] Click Login Button");

        string jwt = SignInAsync(inputId, inputPw).Result;
        if (jwt.Equals(""))
        {
            SignInDisplayText.text = "Login Error!";
            return;
        }
        GamePlayManager.Singleton.Token = jwt;
        GamePlayManager.Singleton.ApiModule.CallHomeAPI(GamePlayManager.Singleton.Token, (string result) =>
        {
            Debug.Log("[GameAuth] Call Home API - " + result);
            GamePlayManager.Singleton.Token = jwt;
            GamePlayManager.Singleton.HomePlayData = JsonUtility.FromJson<HomeData>(result);
            SceneManager.LoadScene("HomeScene");
        });
    }

    public void OnClickSignUpBtn()
    {
        string inputId = SignUpIdInput.text;
        string inputPw = SignUpPwInput.text;
        string inputEmail = SignUPEmailInput.text;

        bool signUpResult = SignUpAsync(inputId, inputPw, inputEmail).Result;
        if (signUpResult)
        {
            SignUpIdInput.text = "";
            SignUpPwInput.text = "";
            SignUPEmailInput.text = "";
        }
        SignUpDisplayText.text = signUpResult ? "Sign Up Success" : "Sign Up Failed";
    }

    /***
     * Default SignUp Method using email. It will return error, if the given information is invalid.
     */
    public async Task<bool> SignUpAsync(string userName, string password, string email)
    {
        try
        {
            var provider = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), RegionEndpoint.APNortheast2);

            SignUpRequest signUpRequest = new SignUpRequest()
            {
                ClientId = GamePlayManager.Singleton.CLIENT_ID,
                Username = userName,
                Password = password
            };
            signUpRequest.UserAttributes.Add(new Amazon.CognitoIdentityProvider.Model.AttributeType()
            {
                Name = "email",
                Value = email
            });

            SignUpResponse signUpResponse = await provider.SignUpAsync(signUpRequest).ConfigureAwait(false);
            //provider.ConfirmSignUpAsync()         //TODO : It is able to implement confirmation logic
            Debug.Log(signUpResponse.ResponseMetadata);
            return signUpResponse.UserConfirmed;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            return false;
        }
    }

    public async Task<string> SignInAsync(string userName, string password)
    {
        try
        {
            var provider = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), RegionEndpoint.APNortheast2);

            //Pool ID, Client App ID, Provider, ClientSecret
            CognitoUserPool userPool =
                new CognitoUserPool(GamePlayManager.Singleton.USERPOOL_ID,
                GamePlayManager.Singleton.CLIENT_ID,
                provider
                );
            CognitoUser user = new CognitoUser(userName, GamePlayManager.Singleton.CLIENT_ID, userPool, provider);        //Client ID does not mean App Client ID in User Pool
            AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync(new InitiateSrpAuthRequest()
            {
                Password = password
            }).ConfigureAwait(false);

            //CognitoAWSCredentials credentials =
            //    user.GetCognitoAWSCredentials(GamePlayManager.Singleton.IDENTITYPOOL_ID, RegionEndpoint.APNortheast2);       //identityPoolID
            string jwt = authResponse.AuthenticationResult.AccessToken;
            return jwt;
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            return "";
        }
    }
}