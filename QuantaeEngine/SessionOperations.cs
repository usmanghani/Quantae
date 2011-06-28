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

            UserSession session = Repositories.Repositories.Sessions.GetSessionFromToken(token);

            if (session != null)
            {
                return session.Token;
            }

            session = new UserSession() { Token = token, Timestamp = DateTime.UtcNow, UserID = profile.UserID, UserProfile = new UserProfileHandle(profile) };

            Repositories.Repositories.Sessions.Save(session);

            return session.Token;
        }
    }
}
