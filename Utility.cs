  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace isRock
{
    public class Utility
    {
        static dynamic UploadImage2Imgur(string ImgurClientID, byte[] FileBody)
        {
            HttpClient client = new HttpClient();
            string uriBase = "https://api.imgur.com/3/upload";

            // Request headers.
            client.DefaultRequestHeaders.Add(
                "Authorization", $"Client-ID {ImgurClientID}");

            string uri = uriBase;

            HttpResponseMessage response;

            // Add the byte array as an octet stream to the request body.
            using (ByteArrayContent content = new ByteArrayContent(FileBody))
            {
                // and "multipart/form-data".
                content.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                // Asynchronously call the REST API method.
                response = client.PostAsync(uri, content).Result;
            }

            // Asynchronously get the JSON response.
            string JSON = response.Content.ReadAsStringAsync().Result;

            return Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(JSON);
        }
    }
}