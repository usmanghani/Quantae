using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class SessionOperations
    {
        public static string CreateOrReturnSession(UserProfile profile, bool rememberMe = false)
        {
            string token = UserOperations.CreateToken(profile);

            UserSession session = null;
            if (SessionManager.Current.SessionExists(token))
            {
                session = SessionManager.Current.GetSession(token);
                session.LastLoginTimestamp = DateTime.UtcNow;
                if (rememberMe)
                {
                    session.ExpirationTimestamp = session.CreationTimestamp.AddDays(14);
                }
            }
            else
            {
                session = new UserSession() { Token = token, CreationTimestamp = DateTime.UtcNow, ExpirationTimestamp = DateTime.UtcNow.AddDays(1), LastLoginTimestamp = DateTime.UtcNow, UserID = profile.UserID, UserProfile = new UserProfileHandle(profile) };
                session = SessionManager.Current.CreateOrReturnSession(session);
            }

            SessionManager.Current.UpdateSession(token, session);
            return session.Token;
        }
    }
}
