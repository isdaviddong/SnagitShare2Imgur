
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace SnagitShare2Imgur
{
    public class Utility
    {
        static internal string getImgurClientID()
        {
            var store = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();
            if (!store.FileExists("ImgurClientID"))
            {
                // Console.WriteLine($"ImgurClientID is null.");
                // Console.WriteLine($"\nplease use following command to setup Imgur ClientID");
                // Console.WriteLine("line config --ImgurClientID [ImgurClientID]");
                return null;
            }
            else
            {
                var file = store.OpenFile("ImgurClientID", FileMode.Open);
                using (StreamReader reader = new StreamReader(file))
                {
                    var words = reader.ReadToEnd();
                    return words.Trim();
                }
            }
        }

        static internal dynamic UploadImage2Imgur(string ImgurClientID, byte[] FileBody)
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
            //DeserializeObject with dynamic data type 
            return Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(JSON);
        }
    }
}