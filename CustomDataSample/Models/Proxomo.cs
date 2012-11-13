using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proxomo;
using System.IO;
using System.Xml.Serialization;
using System.Web.Routing;

namespace CustomDataSample.Models
{
    public class Prox
    {
        private static ProxomoApi _proxomoApi;

        public static ProxomoApi api
        {
            get
            {
                if (_proxomoApi == null || _proxomoApi.IsAuthTokenExpired())
                {
                    System.Configuration.AppSettingsReader reader = new System.Configuration.AppSettingsReader();
                    
                    string applicationID = reader.GetValue("applicationID", typeof(string)).ToString();
                    string proxomoAPIKey = reader.GetValue("proxomoAPIKey", typeof(string)).ToString();
                    string tokenPath = reader.GetValue("tokenPath", typeof(string)).ToString();
                    string fullTokenPath = string.Format("{0}{1}", HttpRuntime.AppDomainAppPath, tokenPath);

                    if (!System.Text.RegularExpressions.Regex.IsMatch(applicationID, @"\d"))
                    { throw new Exception("Invalid ApplicationID, update the web.config with a valid value from the Proxomo website.");};

                    reader = null;

                    _proxomoApi = new Proxomo.ProxomoApi(applicationID, proxomoAPIKey, token: ReadToken(fullTokenPath));

                    WriteToken(_proxomoApi.AuthToken, fullTokenPath);
                }

                return _proxomoApi;
            }
        }

        private static bool WriteToken(Token token, string path)
        {
            try
            {
                XmlSerializer x = new XmlSerializer(typeof(Token));
                StreamWriter writer = new StreamWriter(path);
                x.Serialize(writer, token);
                writer.Close();
                writer.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static Token ReadToken(string path)
        {
            Token token = new Token();

            try
            {
                XmlSerializer x = new XmlSerializer(typeof(Token));
                StreamReader reader = new StreamReader(path);
                token = (Token)x.Deserialize(reader);
                reader.Close();
                reader.Dispose();

                if (string.IsNullOrWhiteSpace(token.AccessToken))
                {
                    return null;
                }
                else
                {
                    return token;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}