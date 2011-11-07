using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OAuth.ChannelElements;

namespace TestMvcApp.Services
{
    public class TwitterTokenManager : ITokenManager
    {
        public void ExpireRequestTokenAndStoreNewAccessToken(string consumerKey, string requestToken, string accessToken, string accessTokenSecret)
        {
            throw new NotImplementedException();
        }

        public string GetTokenSecret(string token)
        {
            throw new NotImplementedException();
        }

        public TokenType GetTokenType(string token)
        {
            throw new NotImplementedException();
        }

        public void StoreNewRequestToken(DotNetOpenAuth.OAuth.Messages.UnauthorizedTokenRequest request, DotNetOpenAuth.OAuth.Messages.ITokenSecretContainingMessage response)
        {
            throw new NotImplementedException();
        }
    }
}