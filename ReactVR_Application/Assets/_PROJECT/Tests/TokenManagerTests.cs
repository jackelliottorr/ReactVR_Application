using System.Collections;
using System.Collections.Generic;
using Assets._PROJECT.Scripts.HelperClasses;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class TokenManagerTests
    {
        [Test]
        public void TestTokenStorage()
        { // Arrange
            var fakeJwt = "fake";

            // Act
            var tokenManager = new TokenManager();
            tokenManager.StoreToken(fakeJwt);

            // Assert
            Assert.True(tokenManager.RetrieveToken() == "fake");
        }

        [Test]
        public void TestCreateAccount()
        {
            // Arrange
            var name = "Jack";
            var email = "newemail@test.com";
            var password = "password";
            var passwordConfirm = "password";

            // Act
            var dataIsValid = DataValidator.ValidateCreateAccountData(email, password, passwordConfirm);
            string jwt = null;

            if (dataIsValid)
            {
                var tokenManager = new TokenManager();
                jwt = tokenManager.CreateAccount(name, email, password, passwordConfirm);
            }

            // Assert
            Assert.False(string.IsNullOrEmpty(jwt));
        }

        [Test]
        public void TestLogin()
        {
            // Arrange
            var email = "test@email.com";
            var password = "password";

            // Act
            var dataIsValid = DataValidator.ValidateLoginData(email, password);
            string jwt = null;

            if (dataIsValid)
            {
                var tokenManager = new TokenManager();
                jwt = tokenManager.Login(email, password);

                //var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(jwt);                
                //bool stored = tokenManager.StoreToken(jwt);
                //string readToken = tokenManager.RetrieveToken();
            }

            // Assert
            Assert.False(string.IsNullOrEmpty(jwt));
        }
    }
}
