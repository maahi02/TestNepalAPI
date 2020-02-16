using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNepal.Dtos
{
    public static class ApplicationSettingVariables
    {
        public static String ImageUploadPath
        {
            get
            {
                return ConfigurationManager.AppSettings["ImageUploadPath"];
            }
        }

        public static String WebsiteBaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["WebsiteBaseUrl"];
            }
        }

        public static String MYOBAPIURL
        {
            get
            {
                return ConfigurationManager.AppSettings["MYOBAPIURL"];
            }
        }
        public static String MYOBAuthKey
        {
            get
            {
                return ConfigurationManager.AppSettings["MYOBAuthKey"];
            }
        }
        public static String MYOBAPIKEY
        {
            get
            {
                return ConfigurationManager.AppSettings["MYOBAPIKEY"];
            }
        }
        public static String MYOBProxyURL
        {
            get
            {
                return ConfigurationManager.AppSettings["MYOBProxyURL"];
            }
        }
        public static bool UseMYOBProxy
        {
            get
            {
                return ConfigurationManager.AppSettings["UseMYOBProxy"] == "1";
            }
        }
        public static String FirebaseServerKey
        {
            get
            {
                return ConfigurationManager.AppSettings["FirebaseServerKey"];
            }
        }

        public static String FirebaseSenderId
        {
            get
            {
                return ConfigurationManager.AppSettings["FirebaseSenderId"];
            }
        }
    }
}
