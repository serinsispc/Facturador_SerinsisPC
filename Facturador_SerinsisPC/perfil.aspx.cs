using Facturador_SerinsisPC.Models.Controlers;
using Facturador_SerinsisPC.Models.ViewModels;
using System;
using System.Web.UI;

namespace Facturador_SerinsisPC
{
    public partial class perfil : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPerfil();
            }
        }

        protected void CargarPerfil()
        {
            if (Session["idUsuarioAdmin"] == null)
            {
                Response.Redirect("login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            int idUsuario = Convert.ToInt32(Session["idUsuarioAdmin"]);
            UsuarioAdmin usuario = control_UsuarioAdmin.ConsultarPorId(idUsuario);
            if (usuario == null)
            {
                return;
            }

            lblNombreUsuario.Text = usuario.nombreUsuario;
            lblLoginUsuario.Text = usuario.loginUsuario;
            lblWhatsAppUsuario.Text = string.IsNullOrWhiteSpace(usuario.whatsApp) ? "No definido" : usuario.whatsApp;
        }

        protected void btnCambiarClave_Click(object sender, EventArgs e)
        {
            if (Session["idUsuarioAdmin"] == null)
            {
                Response.Redirect("login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtClaveActual.Text) ||
                string.IsNullOrWhiteSpace(txtClavePerfilNueva.Text) ||
                string.IsNullOrWhiteSpace(txtClavePerfilNueva2.Text))
            {
                Mensage("Error", "Debes completar todos los campos.", "error");
                return;
            }

            if (txtClavePerfilNueva.Text != txtClavePerfilNueva2.Text)
            {
                Mensage("Error", "La confirmacion de la nueva clave no coincide.", "error");
                return;
            }

            string loginUsuario = Convert.ToString(Session["loginUsuario"]);
            UsuarioAdmin usuario = control_UsuarioAdmin.Validar(loginUsuario, txtClaveActual.Text);
            if (usuario == null)
            {
                control_UsuarioAdmin.RegistrarBitacora(Convert.ToInt32(Session["idUsuarioAdmin"]), loginUsuario, "CAMBIO_CLAVE_FAIL", "Intento con clave actual invalida");
                Mensage("Error", "La clave actual no es valida.", "error");
                return;
            }

            bool actualizada = control_UsuarioAdmin.ActualizarPasswordInterna(usuario.id, txtClavePerfilNueva.Text);
            if (!actualizada)
            {
                control_UsuarioAdmin.RegistrarBitacora(usuario.id, usuario.loginUsuario, "CAMBIO_CLAVE_FAIL", "No fue posible actualizar la clave desde el perfil");
                Mensage("Error", "No fue posible actualizar la clave.", "error");
                return;
            }

            control_UsuarioAdmin.RegistrarBitacora(usuario.id, usuario.loginUsuario, "CAMBIO_CLAVE_OK", "Clave actualizada desde el perfil");
            Mensage("Ok", "La clave fue actualizada correctamente.", "success");
        }

        protected void Mensage(string titulo, string mensage, string tipo)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "randomtext", "alerta2('" + titulo + "','" + mensage + "','" + tipo + "','" + "');", true);
        }
    }
}
