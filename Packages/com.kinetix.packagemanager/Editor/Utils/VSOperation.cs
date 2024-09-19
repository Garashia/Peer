
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.VSAttribution.Kinetix;
using UnityEngine;
using UnityEngine.Networking;

namespace UnityEditor.VSAttribution.Kinetix
{
public static class VSOperation
{
    public const  string LOGIN_PREF_KEY = "Kinetix_Logged_In_Kinetix_Package_Manager";
    private const string URL_COGNITO    = "https://cognito-idp.eu-west-1.amazonaws.com/";

    public static async Task SendRequest(string _Mail, string _Password)
    {
        string data = GetPostData(_Mail, _Password);

        using (UnityWebRequest request = UnityWebRequest.PostWwwForm(URL_COGNITO, data))
        {
            request.SetRequestHeader("Content-Type", "application/x-amz-json-1.1");
            request.SetRequestHeader("X-Amz-Target", "AWSCognitoIdentityProviderService.InitiateAuth");

            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));

            UnityWebRequestAsyncOperation asyncOperation = request.SendWebRequest();
            while (!asyncOperation.isDone)
                await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                throw new Exception(request.downloadHandler.text);
            }

            JObject jsonData = JObject.Parse(request.downloadHandler.text);
            string  idToken  = (string)jsonData["AuthenticationResult"]?["IdToken"];
            
            JWTDecoder            jwtDecoder = new JWTDecoder();
            JWTDecoder.JWTPayload payload    = jwtDecoder.Decode(idToken);

            string developerId = payload.sub;
            VSAttribution.SendAttributionEvent("login", "Kinetix", developerId);
            EditorPrefs.SetBool(LOGIN_PREF_KEY, true);
        }
    }

    [Serializable]
    public class AuthParameters
    {
        public string USERNAME;
        public string PASSWORD;
    }

    [Serializable]
    public class PostData
    {
        public string         AuthFlow = "USER_PASSWORD_AUTH";
        public string         ClientId = "68scgk0e4nvmh9tntq4l1nkmi7";
        public AuthParameters AuthParameters;
        public List<string>   ClientMetadata = new List<string>(1);
    }

    private static string GetPostData(string _Mail, string _Password)
    {
        PostData postData = new PostData
        {
            AuthParameters = new AuthParameters
            {
                USERNAME = _Mail,
                PASSWORD = _Password
            }
        };

        string data = JsonUtility.ToJson(postData, true);
        data = data.Replace("\"ClientMetadata\": []", "\"ClientMetadata\": {}");
        return data;
    }
}
}
