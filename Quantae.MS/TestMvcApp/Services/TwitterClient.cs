﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.OAuth;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OAuth.ChannelElements;

namespace TestMvcApp.Services
{
    public class TwitterClient
    {
        public string UserName { get; set; }

        private static readonly ServiceProviderDescription ServiceDescription =
            new ServiceProviderDescription
            {
                RequestTokenEndpoint = new MessageReceivingEndpoint(
                                           "http://twitter.com/oauth/request_token",
                                           HttpDeliveryMethods.GetRequest |
                                           HttpDeliveryMethods.AuthorizationHeaderRequest),
                UserAuthorizationEndpoint = new MessageReceivingEndpoint(
                                          "http://twitter.com/oauth/authorize",
                                          HttpDeliveryMethods.GetRequest |
                                          HttpDeliveryMethods.AuthorizationHeaderRequest),
                AccessTokenEndpoint = new MessageReceivingEndpoint(
                                          "http://twitter.com/oauth/access_token",
                                          HttpDeliveryMethods.GetRequest |
                                          HttpDeliveryMethods.AuthorizationHeaderRequest),
                TamperProtectionElements = new ITamperProtectionChannelBindingElement[] { new HmacSha1SigningBindingElement() },
            };

        IConsumerTokenManager _tokenManager;

        public TwitterClient(IConsumerTokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public void StartAuthentication()
        {
            var request = HttpContext.Current.Request;
            using (var twitter = new WebConsumer(ServiceDescription, _tokenManager))
            {
                var callBackUrl = new Uri(request.Url.Scheme + "://" +
                                  request.Url.Authority + "/Account/TwitterCallback");
                twitter.Channel.Send(
                    twitter.PrepareRequestUserAuthorization(callBackUrl, null, null)
                );
            }
        }

        public bool FinishAuthentication()
        {
            using (var twitter = new WebConsumer(ServiceDescription, _tokenManager))
            {
                var accessTokenResponse = twitter.ProcessUserAuthorization();
                if (accessTokenResponse != null)
                {
                    UserName = accessTokenResponse.ExtraData["screen_name"];
                    return true;
                }
            }

            return false;
        }
    }
}