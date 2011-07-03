using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;
using System.Security.Cryptography;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace Quantae.Engine
{
    public class UserOperations
    {
        /// <summary>
        /// Creates a new user and returns a token that can be used to track a user's session.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="facebookToken"></param>
        /// <param name="twitterToken"></param>
        /// <returns></returns>
        public static CreateUserResult CreateUser(string username, string email, string password, string facebookToken = null, string twitterToken = null)
        {
            CreateUserResult result = new CreateUserResult();

            string salt = ComputeSalt(password);

            // HACK: Really scary. Put a time-bound here.
            while (SaltExists(salt))
            {
                salt = ComputeSalt(password);
            }

            string encryptedPassword = EncryptPassword(password, salt);
            string token = CreateToken(username, salt);

            UserProfile profile = Repositories.Repositories.Users.GetUserByUserName(username);

            if (profile != null)
            {
                result.Reason = CreateUserFailureReason.AlreadyExists;
                result.Success = false;
                return result;
            }

            profile = Repositories.Repositories.Users.GetUserByEmail(email);

            if (profile != null)
            {
                result.Reason = CreateUserFailureReason.EmailInUse;
                result.Success = false;
                return result;
            }

            profile = new UserProfile();
            profile.UserID = username;
            profile.PasswordHash = encryptedPassword;
            profile.Email = email;
            profile.FacebookToken = facebookToken;
            profile.TwitterToken = twitterToken;

            Repositories.Repositories.Users.Save(profile);

            result.Token = SessionOperations.CreateOrReturnSession(profile);
            result.Success = true;
            return result;
        }

        public static bool SaltExists(string salt)
        {
            var cursor = Repositories.Repositories.Users.Collection.Find(Query.EQ("PasswordHash", new BsonString(salt)));
            return cursor.Count() > 0;
        }

        public static LoginUserResult LoginUser(string usernameOrEmail, string password)
        {
            UserProfile profile = Repositories.Repositories.Users.GetUserByUserName(usernameOrEmail);
            LoginUserResult result = new LoginUserResult();

            if (profile == null)
            {
                profile = Repositories.Repositories.Users.GetUserByEmail(usernameOrEmail);
            }

            if (profile == null)
            {
                // this means the user is not found.
                result.Reason = LoginFailureReason.NotAUser;
                return result;
            }

            string encryptedPassword = EncryptPassword(password, profile.Salt);
            if (encryptedPassword != profile.PasswordHash)
            {
                // we failed to log in the user.
                result.Reason = LoginFailureReason.InvalidPassword;
                return result;
            }

            result.Token = SessionOperations.CreateOrReturnSession(profile);
            result.Success = true;

            return result;
        }

        public static void LogoutUser(string token)
        {
            try
            {
                SessionManager.Current.RemoveSession(token);
            }
            catch (SessionNotFoundException)
            {
                // ignore.
            }
        }

        private static string CalculateHash(string str)
        {
            return BitConverter.ToString(new SHA512CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(str)));
        }

        private static string ComputeSalt(string password)
        {
            return CalculateHash(string.Format("{0}--{1}", DateTime.UtcNow, password));
        }

        private static string EncryptPassword(string password, string salt)
        {
            return CalculateHash(string.Format("{0}--{1}", salt, password));
        }

        private static string CreateToken(string username, string salt)
        {
            return string.Concat(username, ",", salt);
        }

        public static string CreateToken(UserProfile profile)
        {
            return CreateToken(profile.UserID, profile.Salt);
        }
    }
}
