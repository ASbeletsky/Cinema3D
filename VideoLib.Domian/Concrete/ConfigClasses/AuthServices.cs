using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace VideoLib.Domian.Concrete.ConfigClasses
{
    public class AuthServices : ConfigurationSection, IDisposable
    {
        [ConfigurationProperty("ParseService", IsRequired = true)]
        public ParseServiceElement ParseService
        {
            get { return (ParseServiceElement)this["ParseService"]; }
            set { this["ParseService"] = value; }
        }

        [ConfigurationProperty("FacebookService", IsRequired = true)]
        public FacebookServiceElement FacebookService
        {
            get { return (FacebookServiceElement)this["FacebookService"]; }
            set { this["FacebookService"] = value; }
        }

        [ConfigurationProperty("TwitterService", IsRequired = true)]
        public TwitterServiceElement TwitterService
        {
            get { return (TwitterServiceElement)this["TwitterService"]; }
            set { this["TwitterService"] = value; }
        }

        public static AuthServices Create()
        {
            return ConfigurationManager.GetSection("authorizationServices") as AuthServices; ;
        }

        public void Dispose()
        {           
        }
    }
}
