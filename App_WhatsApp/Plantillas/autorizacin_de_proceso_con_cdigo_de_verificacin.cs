using System.Collections.Generic;
using System.Threading.Tasks;

namespace App_WhatsApp
{
    public class autorizacin_de_proceso_con_cdigo_de_verificacin
    {
        public static async Task<WhatsAppResponse> plantilla(
            string celular,
            string nombreUsuario,
            string nombreEmpresa,
            string nombreProceso,
            string codigoVerificacion,
            string token,
            string phoneNumberId,
            string urlMeta = null)
        {
            MetodosJSON metodosJSON = new MetodosJSON();

            WhatsAppRequest whatsApp = new WhatsAppRequest();
            whatsApp.token = token;
            whatsApp.phoneNumberId = phoneNumberId;
            whatsApp.urlMeta = urlMeta;
            whatsApp.messaging_product = "whatsapp";
            whatsApp.to = "57" + celular;
            whatsApp.type = "template";
            whatsApp.template = new Template();
            whatsApp.template.name = "autorizacin_de_proceso_con_cdigo_de_verificacin";
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
            itemParameters.parameter_name = "nombre_empresa";
            itemParameters.text = nombreEmpresa;
            itemComponents.parameters.Add(itemParameters);

            itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "nombre_proceso";
            itemParameters.text = nombreProceso;
            itemComponents.parameters.Add(itemParameters);

            itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "codigo_verificacion";
            itemParameters.text = codigoVerificacion;
            itemComponents.parameters.Add(itemParameters);

            whatsApp.template.components.Add(itemComponents);

            return await metodosJSON.WhatsApp_(whatsApp);
        }
    }
}
