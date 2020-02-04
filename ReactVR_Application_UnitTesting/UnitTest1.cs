using Assets._PROJECT.Scripts.HelperClasses;
using System;
using Xunit;

namespace ReactVR_Application_UnitTesting
{
    public class UnitTest1
    {
        [Fact]
        public void TestMatchingPasswords()
        {
            // Arrange
            var password = "password";
            var passwordConfirm = "password";

            // Act
            bool passwordsMatch = DataValidator.PasswordsMatch(password, passwordConfirm);

            // Assert
            Assert.True(passwordsMatch);
        }
    }
}
