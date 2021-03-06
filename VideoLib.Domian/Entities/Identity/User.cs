﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace VideoLib.Domian.Entities.AuthEntities
{
    public class MyUser : IdentityUser<string,MyUserLogin,MyUserRole,MyUserClaim>
    {
        //Добавить дополнительные поля тут и таблице бд
        public string Name { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(MyUserManager manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }

    }
}
