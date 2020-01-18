using ReactVR_API.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using UnityEngine;

public class UserAccountController : MonoBehaviour
{
    private UserAccount _userAccount;

    // Start is called before the first frame update
    void Start()
    {
        _userAccount = GetUserAccount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public UserAccount GetUserAccount()
    {
        string url = "localhost:7071/api/useraccount/17874532-544f-4f41-8fd2-954cf7d122ac";
        try
        {
            using (var httpClient = new HttpClient())
            {
                //var jsonResponse = httpClient.GetStringAsync(new Uri(url)).Result;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string jsonResponse = reader.ReadToEnd();

                var userAccount = JsonUtility.FromJson<UserAccount>(jsonResponse);

                return userAccount;
            }
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            throw ex;
        }
    }
}
