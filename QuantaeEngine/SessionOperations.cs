using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class SessionOperations
    {
        public static string CreateOrReturnSession(UserProfile profile)
        {
            string token = UserOperations.CreateToken(profile);

            if (SessionManager.Current.SessionExists(token))
            {
                return token;
            }

            UserSession session = new UserSession() { Token = token, CreationTimestamp = DateTime.UtcNow, UserID = profile.UserID, UserProfile = new UserProfileHandle(profile) };

            SessionManager.Current.CreateOrReturnSession(session);

            return session.Token;
        }
    }
}
