using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VideoLib.Entities.AuthEntities
{
    public class UserIntPK : IdentityUser<int, UserLoginIntPK, UserRoleIntPK, UserClaimIntPK>
    {
        //Добавить дополнительные поля тут и таблице бд
        public string Name { get; set; }
    }
}
