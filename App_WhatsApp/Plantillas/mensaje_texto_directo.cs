using System.Threading.Tasks;

namespace App_WhatsApp
{
    public class mensaje_texto_directo
    {
        public static async Task<WhatsAppResponse> enviar(
            string celular,
            string mensaje,
            string token,
            string phoneNumberId)
        {
            MetodosJSON metodosJSON = new MetodosJSON();

            WhatsAppRequest whatsApp = new WhatsAppRequest();
            whatsApp.token = token;
            whatsApp.phoneNumberId = phoneNumberId;
            whatsApp.messaging_product = "whatsapp";
            whatsApp.to = "57" + celular;
            whatsApp.type = "text";
            whatsApp.text = new TextContent
            {
                preview_url = false,
                body = mensaje
            };

            return await metodosJSON.WhatsApp_(whatsApp);
        }
    }
}
