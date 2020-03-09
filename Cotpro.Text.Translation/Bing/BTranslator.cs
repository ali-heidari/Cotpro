using System;
using System.Net;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Web;

namespace Cotpro.Text.Translation.Bing
{
    public class BTranslator
    {
        WebRequest webRequest;
        WebResponse webResponse;
        AdmAccessToken token;

        private string _clientSecret = "";
        private string _clientId = "";
        /// <summary>
        /// cunstructor.
        /// </summary>
        public BTranslator(string ClientId, string ClientSecret)
        {
            _clientId = ClientId;
            _clientSecret = ClientSecret;
        }

        private AdmAccessToken getaccesstoken()
        {
            try
            {  //Prepare OAuth request 
                this.webRequest = WebRequest.Create("https://datamarket.accesscontrol.windows.net/v2/OAuth2-13");
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Method = "POST";
                byte[] bytes = Encoding.ASCII.GetBytes(
                    string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=http://api.microsofttranslator.com",
                    HttpUtility.UrlEncode(_clientId),
                    HttpUtility.UrlEncode(_clientSecret)));
                webRequest.ContentLength = bytes.Length;
                using (Stream outputStream = webRequest.GetRequestStream())
                {
                    outputStream.Write(bytes, 0, bytes.Length);
                }


                AsyncCallback async = new AsyncCallback(GetResponse);
                IAsyncResult iasync = webRequest.BeginGetResponse(async, null);

                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                //Get deserialized object from JSON stream              
                AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());

                return token;
            }
            catch (Exception bb)
            {
                throw bb;
                return null;
            }
            finally
            {
                if (webResponse != null)
                {
                    webResponse.Close();
                    webResponse = null;
                }
            }

        }

        private void GetResponse(IAsyncResult iasync)
        {
            webResponse = webRequest.EndGetResponse(iasync);
        }

        public string TranslateMethod(string text, string from, string to)
        {
            token = getaccesstoken();
            if (token == null)
                throw new NullReferenceException();
            //  string uri = new Microsoft.TranslatorContainer(new Uri("http://api.microsofttranslator.com/v2/Http.svc/Translate")).Translate("hi", "fa", "en").RequestUri.OriginalString;
            string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Web.HttpUtility.UrlEncode(text) + "&from=" + from + "&to=" + to;


            webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Headers.Add("Authorization", "Bearer " + token.access_token);
            webResponse = null;
            try
            {
                webResponse = webRequest.GetResponse();
                using (Stream stream = webResponse.GetResponseStream())
                {
                    System.Runtime.Serialization.DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(Type.GetType("System.String"));
                    string translation = (string)dcs.ReadObject(stream);
                    return translation;
                }
            }
            catch (Exception bb)
            {
                throw bb;
            }
            finally
            {
                if (webResponse != null)
                {
                    webResponse.Close();
                    webResponse = null;
                }
            }
        }
    [DataContract]
    private class AdmAccessToken
    {
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string token_type { get; set; }
        [DataMember]
        public string expires_in { get; set; }
        [DataMember]
        public string scope { get; set; }
    }
    }

}
