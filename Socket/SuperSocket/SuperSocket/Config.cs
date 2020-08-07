using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace SuperSocket.Config
{
    ///自定义配置文件
    /// <summary>
    /// SubProtocol configuration
    /// </summary>
    [ConfigurationCollection(typeof(SubProtocol), AddItemName = "SubProtocol")]
    public class SubProtocolCollection : ConfigurationElementCollection
    {
        //Configuration attributes

        protected override ConfigurationElement CreateNewElement()
        {
            return new SubProtocol();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SubProtocol)element).Name;
        }

        public SubProtocol this[int i]
        {
            get
            {
                return (SubProtocol)base.BaseGet(i);
            }
        }

        public SubProtocol this[string key]
        {
            get
            {
                return (SubProtocol)base.BaseGet(key);
            }
        }
    }

    public class SubProtocol : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("uri", IsRequired = true)]
        public string Uri
        {
            get { return this["uri"].ToString(); }
            set { this["uri"] = value; }
        }

        [ConfigurationProperty("service", IsRequired = true)]
        public string Service
        {
            get { return this["service"].ToString(); }
            set { this["service"] = value; }
        }

        [ConfigurationProperty("contract", IsRequired = false)]
        public string Contract
        {
            get { return this["contract"].ToString(); }
            set { this["contract"] = value; }
        }
    }


    public class SubProtocolsConfig : ConfigurationSection
    {

        [ConfigurationProperty("SubProtocols")]
        public SubProtocolCollection SubProtocols
        {
            get
            {
                return (SubProtocolCollection)base["SubProtocols"];
            }
        }

    }

}
