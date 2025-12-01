using Newtonsoft.Json;
using RFacturacionElectronicaDIAN.Entities.Request;
using RFacturacionElectronicaDIAN.Entities.Response;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace RFacturacionElectronicaDIAN.Factories
{
    public class FacturacionElectronicaDIANFactory
    {
        readonly string Api_Prod = "https://graph.facebook.com/v15.0/104132365893817/messages";

        //readonly string token;
        readonly string urlEndPoint;


        /// <summary>
        /// Factoria
        /// </summary>
        /// <param name="token"></param>
        /// <param name="fixed_hash"></param>
        /// <param name="usedTest"></param>
        public FacturacionElectronicaDIANFactory()
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

        /// <summary>
        /// Permite Enviar Facturas Nacionales a la DIAN
        /// </summary>
        /// <param name="facturaNacional"></param>
        /// <returns></returns>
        public async Task<FacturaNacionalResponse> FacturaNacional(FacturaNacionalRequest facturaNacional,int idVenta_frm)
        {
            try
            {
                string urlParameter = "invoice/" + facturaNacional.testSetID;

                string Json = JsonConvert.SerializeObject(facturaNacional);

                Json = Json.Replace("\"allowance_charges\":null,", "");

                string Response = await HttpWebRequestPost(urlParameter, Json, HttpMethod.Post, facturaNacional.token);

                if (!String.IsNullOrEmpty(Response))
                {
                    FacturaNacionalResponse facturaNacionalResponse = JsonConvert.DeserializeObject<FacturaNacionalResponse>(Response);

                    if (facturaNacionalResponse.zip_key != null)
                    {
                        facturaNacionalResponse = await FacturaValida(facturaNacional, facturaNacionalResponse.zip_key.ToString());
                    }

                    return facturaNacionalResponse;
                }

                return null;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }
        }
        public async Task<NotaCreditoResponse> NotaCredito(NotaCreditoRequest notaCredito)
        {
            try
            {
                string urlParameter = "credit-note/" + notaCredito.testSetID;

                string Json = JsonConvert.SerializeObject(notaCredito);

                Json = Json.Replace("\"allowance_charges\":null,", "");

                string Response = await HttpWebRequestPost(urlParameter, Json, HttpMethod.Post, notaCredito.token);

                if (!String.IsNullOrEmpty(Response))
                {
                    NotaCreditoResponse notaCreditoResponse = JsonConvert.DeserializeObject<NotaCreditoResponse>(Response);

                    return notaCreditoResponse;
                }

                return null;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }
        }
        public async Task<NotaDebitoResponse> NotaDebito(NotaDebitoRequest notaDebito)
        {
            try
            {
                string urlParameter = "debit-note/" + notaDebito.testSetID;

                string Json = JsonConvert.SerializeObject(notaDebito);

                Json = Json.Replace("\"allowance_charges\":null,", "");

                string Response = await HttpWebRequestPost(urlParameter, Json, HttpMethod.Post, notaDebito.token);

                if (!String.IsNullOrEmpty(Response))
                {
                    NotaDebitoResponse notaDebitoResponse = JsonConvert.DeserializeObject<NotaDebitoResponse>(Response);

                    return notaDebitoResponse;
                }

                return null;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }
        }
        /// <summary>
        /// Permite Enviar Facturas Nacionales a la DIAN
        /// </summary>
        /// <param name="facturaNacional"></param>
        /// <returns></returns>
        public async Task<FacturaNacionalResponse> FacturaValida(FacturaNacionalRequest facturaNacional, string zip_key)
        {
            try
            {
                string urlParameter = "invoice/" + zip_key;

                string Json = JsonConvert.SerializeObject(facturaNacional);

                Json = Json.Replace("\"allowance_charges\":null,", "");

                string Response = await HttpWebRequestPost(urlParameter, Json, HttpMethod.Post, facturaNacional.token);

                if (!String.IsNullOrEmpty(Response))
                {
                    return JsonConvert.DeserializeObject<FacturaNacionalResponse>(Response);
                }

                return null;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }
        }
        /// <summary>
        /// Permite enviar correo al cleinte
        /// </summary>
        /// <param name="facturaNacional"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        public async Task<CorreoResponse> FacturaMail(CorreoRequest facturaNacional, string uuid)
        {
            try
            {
                string urlParameter = "mail/send/" + uuid;

                string Json = JsonConvert.SerializeObject(facturaNacional);

                Json = Json.Replace("\"allowance_charges\":null,", "");

                string Response = await HttpWebRequestPost(urlParameter, Json, HttpMethod.Post, facturaNacional.token);

                if (!String.IsNullOrEmpty(Response))
                {
                    return JsonConvert.DeserializeObject<CorreoResponse>(Response);
                }

                return null;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }
        }
       /// <summary>
       /// Consultar el estado del documento.
       /// </summary>
       /// <param name="facturaNacional"></param>
       /// <param name="uuid"></param>
       /// <returns></returns>
        public async Task<FacturaNacionalResponse> ConsultaEstadoDocumento(Consulta_uuid_Request facturaNacional, string uuid)
        {
            try
            {
                string urlParameter = "status/document/" + uuid;

                string Json = JsonConvert.SerializeObject(facturaNacional);

                Json = Json.Replace("\"allowance_charges\":null,", "");

                string Response = await HttpWebRequestPost(urlParameter, Json, HttpMethod.Post, facturaNacional.token);

                if (!String.IsNullOrEmpty(Response))
                {
                    return JsonConvert.DeserializeObject<FacturaNacionalResponse>(Response);
                }

                return null;
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                throw;
            }
        }
    }
}
