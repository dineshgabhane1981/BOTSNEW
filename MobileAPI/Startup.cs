﻿using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;

[assembly: OwinStartup(typeof(MobileAPI.Startup))]

namespace MobileAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
          var url=  System.Configuration.ConfigurationManager.AppSettings["BaseURL"];
            app.UseJwtBearerAuthentication(
                 new JwtBearerAuthenticationOptions
                 {
                     AuthenticationMode = AuthenticationMode.Active,
                     TokenValidationParameters = new TokenValidationParameters()
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = url, //some string, normally web url,  
                        ValidAudience = url,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my_secret_key_98765"))
                     }
                 });
        }
    }
}