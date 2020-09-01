using System.Collections;
using System.Collections.Generic;
using Assets._PROJECT.Scripts.Controllers;
using Assets._PROJECT.Scripts.HelperClasses;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class UserAccountTesting
    {
        [Test]
        public void TestUpdateUserAccount()
        {
            // Arrange
            var tokenManager = new TokenManager();
            //var accessToken = tokenManager.RetrieveToken();
            var name = "Jack Orr";
            var email = "test@email.com";
            var password = "password";
            var jwt = tokenManager.Login(email, password);

            // Act
            var userAccountController = new UserAccountController();
            bool updated = userAccountController.UpdateUserAccount(name, jwt);

            // Assert
            Assert.True(updated);
        }
    }
}
