using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Assets._PROJECT.Scripts.HelperClasses;
using Newtonsoft.Json;
using ReactVR_API.Common.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    #region Private Fields
    
    [SerializeField]
    private InputField _inputEmailAddress;

    [SerializeField]
    private InputField _inputPassword;

    #endregion


    #region Public Methods

    /// <summary>
    /// If the user has already logged in and their current access token is still valid,
    /// load the next scene so that they don't have to re-enter their password
    /// </summary>
    private void Awake()
    {
        TokenManager tokenManager = new TokenManager();
        var accessToken = tokenManager.RetrieveToken();

        try
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var jsonResponse = httpClient.PostAsync(new Uri("http://localhost:7071/api/UserAccount/ValidateAccessToken"), null).Result;

                if (jsonResponse.IsSuccessStatusCode)
                {
                    var responseContent = jsonResponse.Content.ReadAsStringAsync();
                    string updatedAccessToken = responseContent.Result;

                    tokenManager.StoreToken(updatedAccessToken);

                    SceneManager.LoadSceneAsync("Main Menu Scene", LoadSceneMode.Single);
                }
            }
        }        
        catch (Exception ex)
        {
            var message = ex.Message;
            throw ex;
        }
    }

    //var emailAddress = "jackelliottorr@gmail.com";
    //var password = "password1";
    //var passwordConfirm = "password1";
    private UserAccountCreateModel BuildLoginModel()
    {
        var emailAddress = _inputEmailAddress.text;
        var password = _inputPassword.text;


        var userAccountCreateModel = new UserAccountCreateModel()
        {
            EmailAddress = emailAddress,
            Password = password
        };

        return userAccountCreateModel;
    } 

    public bool HandleLoginAttempt(UserAccountCreateModel userAccount, string url)
    {
        try
        {
            var jsonString = JsonConvert.SerializeObject(userAccount);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var jsonResponse = httpClient.PostAsync(new Uri(url), content).Result;

                if (jsonResponse.IsSuccessStatusCode)
                {
                    var responseContent = jsonResponse.Content.ReadAsStringAsync();
                    string jwt = responseContent.Result;

                    TokenManager tokenManager = new TokenManager();
                    tokenManager.StoreToken(jwt);

                    return true;
                }
                else
                {
                    var responseContent = jsonResponse.Content.ReadAsStringAsync();
                    string errorMessage = responseContent.Result;
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            throw ex;
        }
    }

    public void Login()
    {
        var userAccountCreateModel = BuildLoginModel();
        string url = "http://localhost:7071/api/UserAccount/Login";

        if (HandleLoginAttempt(userAccountCreateModel, url))
        {
            SceneManager.LoadSceneAsync("Main Menu Scene", LoadSceneMode.Single);
        }
    }

    public void CreateAccount()
    {
        var userAccountCreateModel = BuildLoginModel();
        string url = "http://localhost:7071/api/UserAccount/CreateUserAccount";

        if (HandleLoginAttempt(userAccountCreateModel, url))
        {
            SceneManager.LoadSceneAsync("Main Menu Scene", LoadSceneMode.Single);
        }
    }

    public void PlayOffline()
    {
        SessionData.DemoMode = true;

        SceneManager.LoadSceneAsync("Main Menu Scene", LoadSceneMode.Single);
    }

    #endregion
}
