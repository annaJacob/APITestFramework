using System;
using System.Configuration;

namespace ApiTesting.CSharp.Framework
{
    public class Settings
    {
        public static Uri Url => new Uri(ConfigurationManager.AppSettings["Url"]);
    }
}
