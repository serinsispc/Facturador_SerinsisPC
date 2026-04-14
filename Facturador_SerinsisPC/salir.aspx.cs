using System;
using System.Web.UI;

namespace Facturador_SerinsisPC
{
    public partial class salir : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("login.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
    }
}
