using System.Collections;
using System.Collections.Generic;
using Assets._PROJECT.Scripts.HelperClasses;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DataValidatorTests
    {
        // A Test behaves as an ordinary method
        [Test]
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

        [Test]
        public void TestNonMatchingPasswords()
        {
            // Arrange
            var password = "password";
            var passwordConfirm = "password1";

            // Act
            bool passwordsMatch = DataValidator.PasswordsMatch(password, passwordConfirm);

            // Assert
            Assert.False(passwordsMatch);
        }

        [Test]
        public void TestPasswordLength()
        {
            // Arrange
            var password = "password";

            // Act
            bool lengthIsOk = DataValidator.PasswordLengthIsValid(password);

            // Assert
            Assert.True(lengthIsOk);
        }

        [Test]
        public void TestTooShortPassword()
        {
            // Arrange
            var password = "passwor";

            // Act
            bool lengthIsOk = DataValidator.PasswordLengthIsValid(password);

            // Assert
            Assert.False(lengthIsOk);
        }

        [Test]
        public void TestTooLongPassword()
        {
            // Arrange
            var password = "dRrroijoypd*x5RSVkHYPclx2*szyonf248Fwu3ltzW#1BPzVP3e#8z2fxzRJOjg6atIR$g$Y0W0BGPal$XgY5px5WF*tE@^ar*$tBhv&211eKXkCUevK0n1liY!$@vdd";

            // Act
            bool lengthIsOk = DataValidator.PasswordLengthIsValid(password);

            // Assert
            Assert.False(lengthIsOk);
        }

        [Test]
        public void TestValidEmail()
        {
            // Arrange
            var email = "A@a.com";

            // Act
            bool emailValid = DataValidator.EmailAddressIsValid(email);

            // Assert
            Assert.True(emailValid);
        }

        [Test]
        public void TestInvalidEmail()
        {
            // Arrange
            var email = "thisisnotanemailaddress";

            // Act
            bool emailValid = DataValidator.EmailAddressIsValid(email);

            // Assert
            Assert.False(emailValid);
        }

        //// A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        //// `yield return null;` to skip a frame.
        //[UnityTest]
        //public IEnumerator TestSuiteWithEnumeratorPasses()
        //{
        //    // Use the Assert class to test conditions.
        //    // Use yield to skip a frame.
        //    yield return null;
        //}
    }
}
