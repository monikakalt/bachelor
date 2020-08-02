using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
namespace Tests
{
    /// <summary>
    /// Base class for API tests
    /// </summary>
    public class ApiBaseTest
    {

        /// <summary>
        /// URL the test API server is listening on
        /// </summary>
        protected string url { get; set; } = "https://localhost:44328/api/";
        /// <summary>
        /// The last authentication token we received
        /// </summary>
        protected string authToken
        {
            get; set;
        }

        /// <summary>
        /// Authorizes apiUser against the API
        /// </summary>
        /// <returns>Access token</returns>
        protected string Authorize()
        {
            var json = this.Post(this.url + "users/authenticate", new { email = "kaltenytmonika3@gmail.com", password = "123456789" });

            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            try
            {
                this.authToken = result.token;
                return this.authToken;
            }
            catch
            {
                return null;
            }
        }

        protected T Get<T>(string v, bool sendAuthToken = true)
        {
            try
            {
                using (var client = new WebClient())
                {
                    if (sendAuthToken)
                    {
                        client.Headers.Add("Authorization", "Token " + this.authToken);
                    }

                    var json = client.DownloadString(v);

                    T result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
                    return result;
                }
            }
            catch (WebException exc)
            {
                if (exc.Response != null && exc.Response.GetType() == typeof(HttpWebResponse) && ((HttpWebResponse)exc.Response).StatusCode == HttpStatusCode.NotFound)
                {
                    return default(T);
                }

                throw exc;
            }
        }

        /// <summary>
        /// Posts data to an url
        /// </summary>
        /// <param name="url">URL to post data to, usually url + prefix + "mycontroller/myfunction"</param>
        /// <param name="data">JSONized data to post</param>
        /// <returns>Unparsed API response</returns>
        protected string Post(string url, string data)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Encoding = Encoding.UTF8;

                client.Headers.Add("Authorization", "Token " + this.authToken);
                var json = client.UploadString(url, "POST", data);

                return json;
            }
        }

        /// <summary>
        /// Posts data to an url
        /// </summary>
        /// <param name="url">URL to post data to, usually url + prefix + "mycontroller/myfunction"</param>
        /// <param name="obj">data to post</param>
        /// <returns>Unparsed API response</returns>
        protected string Post(string url, object obj)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Encoding = Encoding.UTF8;

                client.Headers.Add("Authorization", "Token " + this.authToken);
                client.Encoding = Encoding.UTF8;
                var objJson = JsonConvert.SerializeObject(obj);
                var json = client.UploadString(url, "POST", objJson);

                return json;
            }
        }

        /// <summary>
        /// Issues a GET request towards the API
        /// </summary>
        /// <param name="url">URL to get data from</param>
        /// <param name="sendAuthToken">if true, the auth token will be sent (default)</param>
        /// <param name="additionalHeaders">Additional headers to send along with the request</param>
        /// <returns>Instance of T</returns>
        protected string Get(string url, bool sendAuthToken = true, Dictionary<string, string> additionalHeaders = null)
        {
            try
            {
                using (var client = new WebClient())
                {
                    if (sendAuthToken)
                    {
                        client.Headers.Add("Authorization", "Token " + this.authToken);
                    }

                    if (additionalHeaders != null && additionalHeaders.Any())
                        foreach (var kvp in additionalHeaders)
                            client.Headers.Add(kvp.Key, kvp.Value);

                    var data = client.DownloadString(url);
                    return data;
                }
            }
            catch (WebException exc)
            {
                if (exc.Response != null && exc.Response.GetType() == typeof(HttpWebResponse) && ((HttpWebResponse)exc.Response).StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw exc;
            }
        }

        /// <summary>
        /// Issues a PUT request towards the API
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="data">Data to send</param>
        /// <param name="sendAuthToken">if true, the auth token will be sent along with the request (default)</param>
        /// <returns>Unparsed API response</returns>
        protected string Put(string url,int id, object data, bool sendAuthToken = true)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Encoding = Encoding.UTF8;

                if (sendAuthToken)
                {
                    client.Headers.Add("Authorization", "Token " + this.authToken);
                }

                var json = client.UploadString(url, "PUT", Newtonsoft.Json.JsonConvert.SerializeObject(data));
                return json;
            }
        }

        /// <summary>
        /// Issues a DELETE
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="sendAuthToken">if true, the auth token will be sent along with the request (default)</param>
        protected string Delete(string url, bool sendAuthToken = true)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                client.Encoding = Encoding.UTF8;

                if (sendAuthToken)
                {
                    client.Headers.Add("Authorization", "Token " + this.authToken);
                }

                var data = client.UploadData(url, "DELETE", new byte[0]);
                return Encoding.Default.GetString(data);
            }
        }
    }

}
