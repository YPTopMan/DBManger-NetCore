using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DdManger.Web
{
    public class ConfigHelper
    {
        public T Get<T>(string key)
        {
            IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).Build();

            return config.GetSection(key).Get<T>();
        }


        public void Set<T>(T t) {
            IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", true, true).Build();
            config.Bind(t);
        }

        public void Set(string key,string value)
        {
            IConfiguration config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", true, true).Build();
            config.Bind(key,value);
        }
    }
}
