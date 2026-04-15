using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace App_WhatsApp
{
    public class recordatorio_de_pago
    {
        public static async Task<WhatsAppResponse> plantilla(
            string celular,
            string nombre_cliente,
            string mes_pendiente,
            string meses_en_mora,
            int valor_total,
            string fecha_limite,
            string numero_cuenta,
            string nit_empresa,
            string titular_cuenta,
            string token,
            string phoneNumberId,
            string urlMeta = null)
        {
            MetodosJSON metodosJSON = new MetodosJSON();

            WhatsAppRequest whatsApp = new WhatsAppRequest();
            whatsApp.token = token;
            whatsApp.phoneNumberId = phoneNumberId;
            whatsApp.urlMeta = urlMeta;
            whatsApp.messaging_product = $"whatsapp";
            whatsApp.to = "57" + celular;
            whatsApp.type = $"template";

            whatsApp.template = new Template();

            whatsApp.template.name = "recordatorio_de_pago";

            whatsApp.template.language = new Language();

            whatsApp.template.language.code = $"es";

            whatsApp.template.components = new List<Components>();

            Components ItemComponents = new Components();

            ItemComponents.type = $"body";

            ItemComponents.parameters = new List<Parameters>();


            Parameters itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "nombre_cliente";
            itemParameters.text = $"{nombre_cliente}";
            ItemComponents.parameters.Add(itemParameters);

            itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "mes_pendiente";
            itemParameters.text = mes_pendiente;
            ItemComponents.parameters.Add(itemParameters);

            itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "meses_en_mora";
            itemParameters.text =meses_en_mora;
            ItemComponents.parameters.Add(itemParameters);

            itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "valor_total";
            itemParameters.text =$"{valor_total:N0}";
            ItemComponents.parameters.Add(itemParameters);

            itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "fecha_limite";
            itemParameters.text = fecha_limite;
            ItemComponents.parameters.Add(itemParameters);

            itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "numero_cuenta";
            itemParameters.text = numero_cuenta;
            ItemComponents.parameters.Add(itemParameters);

            itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "nit_empresa";
            itemParameters.text = nit_empresa;
            ItemComponents.parameters.Add(itemParameters);

            itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "titular_cuenta";
            itemParameters.text = titular_cuenta;
            ItemComponents.parameters.Add(itemParameters);

            whatsApp.template.components.Add(ItemComponents);

            return await metodosJSON.WhatsApp_(whatsApp);

        }
    }
}
