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
        readonly string Api_Prod = "https://graph.facebook.com/v22.0";


        /// <summary>
        /// Factoria
        /// </summary>
        /// <param name="token"></param>
        /// <param name="fixed_hash"></param>
        /// <param name="usedTest"></param>
        public MetodosJSON()
        {
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
                    string endpoint = ConstruirUrl(Url);
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(endpoint);

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
                    if (ex.Response != null)
                    {
                        using (StreamReader streamReader = new StreamReader(ex.Response.GetResponseStream()))
                        {
                            Response = streamReader.ReadToEnd();
                        }
                    }
                    else
                    {
                        Response = JsonConvert.SerializeObject(new WhatsAppResponse
                        {
                            errorMessage = ex.Message
                        });
                    }
                }
                catch (Exception ex)
                {
                    Response = JsonConvert.SerializeObject(new WhatsAppResponse
                    {
                        errorMessage = ex.Message
                    });
                }

                return Response;
            });
            return task;
        }

        private string ConstruirUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return Api_Prod;
            }

            if (Uri.TryCreate(url, UriKind.Absolute, out Uri absoluteUri))
            {
                return absoluteUri.ToString();
            }

            return Api_Prod.TrimEnd('/') + "/" + url.TrimStart('/');
        }

        public async Task<WhatsAppResponse> WhatsApp_(WhatsAppRequest whatsapp)
        {
            try
            {
                if (whatsapp == null ||
                    string.IsNullOrWhiteSpace(whatsapp.token) ||
                    (string.IsNullOrWhiteSpace(whatsapp.urlMeta) &&
                     string.IsNullOrWhiteSpace(whatsapp.phoneNumberId)))
                {
                    return null;
                }

                string urlParameter = !string.IsNullOrWhiteSpace(whatsapp.urlMeta)
                    ? whatsapp.urlMeta
                    : $"/{whatsapp.phoneNumberId}/messages";

                string Json = JsonConvert.SerializeObject(whatsapp);

                Json = Json.Replace("\"allowance_charges\":null,", "");
                Json = Json.Replace($"\"phoneNumberId\":\"{whatsapp.phoneNumberId}\",", "");
                Json = Json.Replace($"\"urlMeta\":\"{whatsapp.urlMeta}\",", "");

                string Response = await HttpWebRequestPost(urlParameter, Json, HttpMethod.Post, whatsapp.token);
                if (string.IsNullOrWhiteSpace(Response))
                {
                    return new WhatsAppResponse
                    {
                        errorMessage = "Meta no devolvio respuesta."
                    };
                }

                WhatsAppResponse resultado = JsonConvert.DeserializeObject<WhatsAppResponse>(Response);
                if (resultado != null)
                {
                    resultado.rawResponse = Response;

                    if (string.IsNullOrWhiteSpace(resultado.errorMessage) && resultado.error != null)
                    {
                        resultado.errorMessage = resultado.error.message;
                    }
                }

                return resultado ?? new WhatsAppResponse
                {
                    errorMessage = "No fue posible interpretar la respuesta de Meta.",
                    rawResponse = Response
                };
            }
            catch (Exception ex)
            {
                return new WhatsAppResponse
                {
                    errorMessage = ex.Message
                };
            }
        }
    }
}
