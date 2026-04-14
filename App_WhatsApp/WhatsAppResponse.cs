using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_WhatsApp
{
    public class WhatsAppResponse
    {
        public string messaging_product { get; set; }
        public List<Contacts> contacts { get; set; }
        public List<Messages> messages { get; set; }
        public WhatsAppError error { get; set; }
        public string errorMessage { get; set; }
        public string rawResponse { get; set; }
    }

    public class Contacts
    {
        public string input { get; set; }
        public string wa_id { get; set; }
    }

    public class Messages
    {
        public string id { get; set; }
        public string message_status { get; set; }
    }

    public class WhatsAppError
    {
        public string message { get; set; }
        public string type { get; set; }
        public int code { get; set; }
        public int error_subcode { get; set; }
        public string fbtrace_id { get; set; }
    }
}
