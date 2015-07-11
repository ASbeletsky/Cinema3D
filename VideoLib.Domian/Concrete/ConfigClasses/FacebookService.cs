using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace VideoLib.Domian.Concrete.ConfigClasses
{
    public class FacebookServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("AppID", IsRequired = true)]
        public string AppID
        {
            get { return (string)this["AppID"]; }
            set { this["AppID"] = value; }
        }

        [ConfigurationProperty("AppSecret", IsRequired = true)]
        public string AppSecret
        {
            get { return (string)this["AppSecret"]; }
            set { this["AppSecret"] = value; }
        }
    }
}
