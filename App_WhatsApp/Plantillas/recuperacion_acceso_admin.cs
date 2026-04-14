using System.Collections.Generic;
using System.Threading.Tasks;

namespace App_WhatsApp
{
    public class recuperacion_acceso_admin
    {
        public static async Task<WhatsAppResponse> plantilla(
            string celular,
            string nombreUsuario,
            string codigo,
            string minutosVigencia,
            string token,
            string phoneNumberId)
        {
            MetodosJSON metodosJSON = new MetodosJSON();

            WhatsAppRequest whatsApp = new WhatsAppRequest();
            whatsApp.token = token;
            whatsApp.phoneNumberId = phoneNumberId;
            whatsApp.messaging_product = "whatsapp";
            whatsApp.to = "57" + celular;
            whatsApp.type = "template";
            whatsApp.template = new Template();
            whatsApp.template.name = "recuperacion_acceso_admin";
            whatsApp.template.language = new Language();
            whatsApp.template.language.code = "es";
            whatsApp.template.components = new List<Components>();

            Components itemComponents = new Components();
            itemComponents.type = "body";
            itemComponents.parameters = new List<Parameters>();

            Parameters itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "nombre_usuario";
            itemParameters.text = nombreUsuario;
            itemComponents.parameters.Add(itemParameters);

            itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "codigo";
            itemParameters.text = codigo;
            itemComponents.parameters.Add(itemParameters);

            itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "minutos_vigencia";
            itemParameters.text = minutosVigencia;
            itemComponents.parameters.Add(itemParameters);

            whatsApp.template.components.Add(itemComponents);

            return await metodosJSON.WhatsApp_(whatsApp);
        }
    }
}
