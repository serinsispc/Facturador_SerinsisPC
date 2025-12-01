using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Microsoft.Win32;
using DevExpress.Utils.CommonDialogs.Internal;

namespace Facturador_SerinsisPC.Email
{
    public class Correo
    {
        public static string MyEmail = "info@serinsispcsas.com";
        public static string MyPassword = "zjmy ntqb jzfr zkjn";
        public static string[] MyAdjuntos;
        public static MailMessage mCorreo;
        public static void CrearCuerpoCorreo(string MyAlias, List<CorreosNotificaciones> destinatarios, string asunto, string textBody)
        {
            try
            {
                mCorreo = new MailMessage();
                mCorreo.From = new MailAddress(MyEmail, MyAlias, System.Text.Encoding.UTF8);
                foreach (CorreosNotificaciones lista in destinatarios)
                {
                    mCorreo.To.Add(lista.email.Trim());
                }
                mCorreo.Subject = asunto.Trim();
                mCorreo.Body = textBody.Trim();
                mCorreo.IsBodyHtml = true;
                mCorreo.Priority = MailPriority.High;

                //Adjuntos
                if (MyAdjuntos != null)
                {
                    for (int i = 0; i < MyAdjuntos.Length; i++)
                    {
                        mCorreo.Attachments.Add(new Attachment(MyAdjuntos[i]));
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }

        }
        public static void CrearCuerpoCorreo(string MyAlias, string destinatarios, string asunto, string textBody)
        {
            mCorreo = new MailMessage();
            mCorreo.From = new MailAddress(MyEmail, MyAlias, System.Text.Encoding.UTF8);
            mCorreo.To.Add(destinatarios.Trim());
            mCorreo.Subject = asunto.Trim();
            mCorreo.Body = textBody.Trim();
            mCorreo.IsBodyHtml = true;
            mCorreo.Priority = MailPriority.High;

            //Adjuntos
            if (MyAdjuntos != null)
            {
                for (int i = 0; i < MyAdjuntos.Length; i++)
                {
                    mCorreo.Attachments.Add(new Attachment(MyAdjuntos[i]));
                }
            }
        }
        public static bool Enviar()
        {
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.UseDefaultCredentials = false;
                smtp.Port = 25;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential(MyEmail, MyPassword);
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                { return true; };
                smtp.EnableSsl = true;
                smtp.Send(mCorreo);
                return true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return false;
            }

        }
    }
}