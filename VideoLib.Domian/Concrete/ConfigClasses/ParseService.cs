using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;


namespace VideoLib.Domian.Concrete.ConfigClasses
{
    public class ParseServiceElement : ConfigurationElement
    {
        [ConfigurationProperty("AppID", IsRequired = true)]
        public string AppID
        {
            get { return (string) this["AppID"]; }
            set { this["AppID"] = value; }
        }

        [ConfigurationProperty("DotNetKey", IsRequired = true)]
        public string DotNetKey
        {
            get { return (string)this["DotNetKey"]; }
            set { this["DotNetKey"] = value; }
        }
    }
}
