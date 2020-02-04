using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Assets._PROJECT.Scripts.Controllers
{
    public class UserAccountController
    {
        public bool UpdateUserAccount(string name, string accessToken)
        {
            try
            {
                var updateModel = new
                {
                    Name = name
                };

                var jsonString = JsonConvert.SerializeObject(updateModel);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var url = "http://localhost:7071/api/UserAccount/UpdateUserAccount";

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                    var jsonResponse = httpClient.PutAsync(new Uri(url), content).Result;

                    if (jsonResponse.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
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

        public bool UpdateUserAccountEmailAddress(string name, string emailAddress, string newEmailAddress, string password)
        {
            try
            {
                var updateModel = new
                {
                    Name = name,
                    EmailAddress = newEmailAddress
                };

                var jsonString = JsonConvert.SerializeObject(updateModel);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var url = "http://localhost:7071/api/UserAccount/UpdateUserAccount";

                using (var httpClient = new HttpClient())
                {
                    var jsonResponse = httpClient.PutAsync(new Uri(url), content).Result;

                    if (jsonResponse.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
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

        public bool UpdateUserAccountPassword(string name, string emailAddress, string password, string newPassword)
        {
            try
            {
                var updateModel = new
                {
                    Name = name,
                    Password = newPassword
                };

                var jsonString = JsonConvert.SerializeObject(updateModel);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var url = "http://localhost:7071/api/UserAccount/UpdateUserAccount";

                using (var httpClient = new HttpClient())
                {
                    var jsonResponse = httpClient.PutAsync(new Uri(url), content).Result;

                    if (jsonResponse.IsSuccessStatusCode)
                    {
                        return true;
                    }
                    else
                    {
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
    }
}
