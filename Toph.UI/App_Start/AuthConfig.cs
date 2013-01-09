﻿using System;
using DotNetOpenAuth.AspNet.Clients;
using Microsoft.Web.WebPages.OAuth;

namespace Toph.UI
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            //OAuthWebSecurity.RegisterFacebookClient(
            //    appId: "",
            //    appSecret: "");

            OAuthWebSecurity.RegisterGoogleClient();
            OAuthWebSecurity.RegisterClient(new OpenIdClient("myopenid", "https://myopenid.com/"), "MyOpenID", null);
            OAuthWebSecurity.RegisterYahooClient();
        }
    }
}
