using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_WhatsApp.Plantillas
{
    public class confirmacin_de_pago
    {
        public static async Task<WhatsAppResponse> plantilla(string celular, 
            string nombre_cliente,
            string fecha_pago,
            string valor_pagado,
            string periodo_pagado,
            string token,
            string phoneNumberId)
        {
            MetodosJSON metodosJSON = new MetodosJSON();

            WhatsAppRequest whatsApp = new WhatsAppRequest();
            whatsApp.token = token;
            whatsApp.phoneNumberId = phoneNumberId;
            whatsApp.messaging_product = $"whatsapp";
            whatsApp.to = "57" + celular;
            whatsApp.type = $"template";

            whatsApp.template = new Template();

            whatsApp.template.name = "confirmacin_de_pago ";

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
            itemParameters.parameter_name = "fecha_pago";
            itemParameters.text = fecha_pago;
            ItemComponents.parameters.Add(itemParameters);

            itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "valor_pagado";
            itemParameters.text = valor_pagado;
            ItemComponents.parameters.Add(itemParameters);

            itemParameters = new Parameters();
            itemParameters.type = "text";
            itemParameters.parameter_name = "periodo_pagado";
            itemParameters.text = periodo_pagado;
            ItemComponents.parameters.Add(itemParameters);

            whatsApp.template.components.Add(ItemComponents);

            return await metodosJSON.WhatsApp_(whatsApp);

        }
    }
}
