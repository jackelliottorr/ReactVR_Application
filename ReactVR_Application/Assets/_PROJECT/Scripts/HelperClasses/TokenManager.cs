using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._PROJECT.Scripts.HelperClasses
{
    public class TokenManager
    {
        public bool StoreToken(string jwt)
        {
            try
            {
                PlayerPrefs.SetString("jwt", jwt);
                return true;
            }
            catch (Exception ex)
            {
                // log ex/
                return false;
            }
        }

        public string RetrieveToken()
        {
            try
            {
               var jwt =  PlayerPrefs.GetString("jwt");
                return jwt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CreateAccount(string name, string emailAddress, string password, string passwordConfirm)
        {
            var createModel = new
            {
                Name = name,
                Emailaddress = emailAddress,
                Password = password
            };

            var jsonString = JsonConvert.SerializeObject(createModel);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var url = "http://localhost:7071/api/useraccount/CreateUserAccount";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    // should probably use UnityWebRequest?
                    var jsonResponse = httpClient.PostAsync(new Uri(url), content).Result;


                    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    //StreamReader reader = new StreamReader(response.GetResponseStream());
                    //string jsonResponse = reader.ReadToEnd();

                    //var userAccount = JsonConvert.DeserializeObject<UserAccount>(jsonResponse);
                    
                    var jwt = jsonResponse.Content.ReadAsStringAsync().Result;

                    // get token and store locally
                    // get id from token also

                    return jwt;
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw ex;
            }
        }

        public string Login(string emailAddress, string password)
        {
            try
            {
                var loginModel = new
                {
                    Emailaddress = emailAddress,
                    Password = password
                };

                var jsonString = JsonConvert.SerializeObject(loginModel);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var url = "http://localhost:7071/api/useraccount/Login";

                using (var httpClient = new HttpClient())
                {
                    var jsonResponse = httpClient.PostAsync(new Uri(url), content).Result;

                    var jwt = jsonResponse.Content.ReadAsStringAsync().Result;

                    return jwt;
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw ex;
            }
        }
    }
}
