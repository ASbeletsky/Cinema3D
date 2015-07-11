using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoLib.Domian.Concrete.ConfigClasses
{
        public class TwitterServiceElement : ConfigurationElement
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
