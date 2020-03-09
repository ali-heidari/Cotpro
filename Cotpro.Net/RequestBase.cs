using System;
using System.Collections.Generic;
using System.Text;

namespace Cotpro.Net
{
    public class RequestBase
    {
        int _port = 80;
        string _address = "127.0.0.1";
        string _contentType = "application/x-www-form-urlencoded";

        public RequestBase(string address,int port)
        {
            this._port = port;
            this._address = address;
        }

        public string RequestWebpage()
        {
            return this.RequestWebpage(this._address);
        }
        public string RequestWebpage(string Address)
        {
            return this.RequestWebpage(Address, this._port);
        }
        public string RequestWebpage(string Address, int Port)
        {
          return  this.RequestWebpage(Address, Port,null);
        }
        public string RequestWebpage(string Address,int Port,byte[] PostData)
        {
            string address=Address;
            if (address.Substring(address.Length - 1) == "/")
                address = address.Remove(address.Length - 1);
            // Create a request using a URL that can receive a post. 
            System.Net.WebRequest request = System.Net.WebRequest.Create(address + ":" + Port.ToString());
            // Set the Method property of the request to POST.
            request.Method = "POST";
            // Create POST data.            
            byte[] byteArray = PostData;
            // Set the ContentType property of the WebRequest.
            request.ContentType = _contentType;
            // Set the ContentLength property of the WebRequest.
            if (byteArray != null)
                request.ContentLength = byteArray.Length;
            // Get the request stream.
           System.IO.Stream dataStream ;
           try
           {
               dataStream = request.GetRequestStream();
               // Write the data to the request stream.
               if (byteArray != null)
                   dataStream.Write(byteArray, 0, byteArray.Length);
           }
           catch (Exception e)
           {
               throw e;
           }
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            System.Net.WebResponse response ;
            try
            {
                response = request.GetResponse();
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
            }
            catch (Exception e)
            {
                throw e;
            }
            // Open the stream using a StreamReader for easy access.
            System.IO.StreamReader reader = new System.IO.StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();            
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();

            return responseFromServer;
        }

    }
}
