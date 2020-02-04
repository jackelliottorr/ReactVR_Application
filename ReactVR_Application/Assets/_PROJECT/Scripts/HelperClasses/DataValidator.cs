using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Assets._PROJECT.Scripts.HelperClasses
{
    public class DataValidator
    {
        public static bool ValidateCreateAccountData(string emailAddress, string password, string passwordConfirm)
        {
            if (!DataValidator.EmailAddressIsValid(emailAddress))
            {
                // Log reason somehow
                return false;
            }

            if (!DataValidator.PasswordLengthIsValid(password))
            {
                // Log reason somehow
                return false;
            }

            if (!DataValidator.PasswordsMatch(password, passwordConfirm))
            {
                // Log reason somehow
                return false;
            }

            return true;
        }

        public static bool ValidateLoginData(string emailAddress, string password)
        {
            if (!DataValidator.EmailAddressIsValid(emailAddress))
            {
                // Log reason somehow
                return false;
            }

            if (!DataValidator.PasswordLengthIsValid(password))
            {
                // Log reason somehow
                return false;
            }

            return true;
        }

        public static bool EmailAddressIsValid(string emailAddress)
        {
            try
            {
                var mailAddress = new MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public static bool PasswordLengthIsValid(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            else if (password.Length > 128 || password.Length < 8)
            {
                return false;
            }

            return true;
        }

        public static bool PasswordsMatch(string password, string passwordConfirm)
        {
            if (password != passwordConfirm)
            {
                return false;
            }

            return true;
        }
    }
}
