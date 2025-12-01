using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App_WhatsApp
{
    public class MetodosJSON
    {
        readonly string Api_Prod = "https://graph.facebook.com/v22.0/627745297095871/messages";

        //readonly string token;
        readonly string urlEndPoint;


        /// <summary>
        /// Factoria
        /// </summary>
        /// <param name="token"></param>
        /// <param name="fixed_hash"></param>
        /// <param name="usedTest"></param>
        public MetodosJSON()
        {
            //this.token = token;

            this.urlEndPoint = Api_Prod;
        }
        public Task<string> HttpWebRequestPost(string Url, string Json, HttpMethod httpMethod, [Optional] string token)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Task<string> task = Task.Run(() =>
            {
                string Response = null;

                try
                {
                    /// ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(urlEndPoint + Url);

                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Accept = "application/json";
                    httpWebRequest.Method = httpMethod.ToString();

                    if (!string.IsNullOrEmpty(token))
                    {
                        httpWebRequest.Headers.Add("Authorization", "Bearer " + token);
                    }

                    if ((httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put) && Json != null)
                    {
                        using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(Json);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                    }

                    using (HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            if (httpResponse.StatusCode == HttpStatusCode.OK || httpResponse.StatusCode == HttpStatusCode.Created)
                            {
                                Response = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
                catch (WebException ex)
                {
                    var resp = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();

                    dynamic obj = JsonConvert.DeserializeObject(resp);
                    var messageFromServer = obj.error.message;
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    Response = null;
                }

                return Response;
            });
            return task;
        }

        public async Task<WhatsAppResponse> WhatsApp_(WhatsAppRequest whatsapp)
        {
            try
            {
                string urlParameter = "";

                string Json = JsonConvert.SerializeObject(whatsapp);

                Json = Json.Replace("\"allowance_charges\":null,", "");

                string Response = await HttpWebRequestPost(urlParameter, Json, HttpMethod.Post, whatsapp.token);
                return JsonConvert.DeserializeObject<WhatsAppResponse>(Response);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                return null;
            }
        }
    }
}
