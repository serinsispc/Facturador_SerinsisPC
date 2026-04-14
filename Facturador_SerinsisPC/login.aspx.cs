using Facturador_SerinsisPC.Models.Controlers;
using Facturador_SerinsisPC.Models.ViewModels;
using Facturador_SerinsisPC.Servicios;
using System.Linq;
using System;
using System.Web.UI;
using App_WhatsApp;

namespace Facturador_SerinsisPC
{
    public partial class login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClassConexionPrincipal.Configurar();

            if (Session["idUsuarioAdmin"] != null)
            {
                Response.Redirect("index.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
        }

        protected void btnIngresar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text) || string.IsNullOrWhiteSpace(txtClave.Text))
            {
                Mensage("Error", "Debes ingresar usuario y clave.", "error");
                return;
            }

            UsuarioAdmin usuario = control_UsuarioAdmin.Validar(txtUsuario.Text.Trim(), txtClave.Text);
            if (usuario == null || !usuario.estado)
            {
                control_UsuarioAdmin.RegistrarBitacora(null, txtUsuario.Text.Trim(), "LOGIN_FAIL", "Intento fallido de acceso");
                Mensage("Error", "Usuario o clave invalidos.", "error");
                return;
            }

            Session["idUsuarioAdmin"] = usuario.id;
            Session["nombreUsuario"] = usuario.nombreUsuario;
            Session["loginUsuario"] = usuario.loginUsuario;
            control_UsuarioAdmin.ActualizarUltimoAcceso(usuario.id);
            control_UsuarioAdmin.RegistrarBitacora(usuario.id, usuario.loginUsuario, "LOGIN_OK", "Ingreso correcto al panel");

            Response.Redirect("index.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }

        protected void btnEnviarCodigo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuarioRecuperar.Text) || string.IsNullOrWhiteSpace(txtWhatsAppRecuperar.Text))
            {
                Mensage("Error", "Debes ingresar el usuario y el WhatsApp autorizado.", "error");
                return;
            }

            UsuarioAdmin usuario = control_UsuarioAdmin.ConsultarRecuperacion(txtUsuarioRecuperar.Text.Trim(), txtWhatsAppRecuperar.Text.Trim());
            if (usuario == null || !usuario.estado)
            {
                control_UsuarioAdmin.RegistrarBitacora(null, txtUsuarioRecuperar.Text.Trim(), "RECUPERACION_CODIGO_FAIL", "No se valido el usuario con el WhatsApp informado");
                Mensage("Error", "No fue posible validar el usuario con el WhatsApp registrado.", "error");
                return;
            }

            string codigo = ClassSeguridad.GenerarCodigoRecuperacion();
            const int minutosVigencia = 10;

            if (!control_UsuarioAdmin.GenerarCodigoRecuperacion(usuario.id, codigo, minutosVigencia))
            {
                control_UsuarioAdmin.RegistrarBitacora(usuario.id, usuario.loginUsuario, "RECUPERACION_CODIGO_FAIL", "No se pudo guardar el codigo temporal");
                Mensage("Error", "No fue posible generar el codigo de recuperacion.", "error");
                return;
            }

            ConfigWhatsAppMeta configMeta = control_ConfigWhatsAppMeta.Consultar();
            if (configMeta == null ||
                string.IsNullOrWhiteSpace(configMeta.accessToken) ||
                string.IsNullOrWhiteSpace(configMeta.phoneNumberId))
            {
                control_UsuarioAdmin.RegistrarBitacora(usuario.id, usuario.loginUsuario, "RECUPERACION_CODIGO_FAIL", "No existe configuracion activa de WhatsApp Meta");
                Mensage("Error", "No se encontro la configuracion de WhatsApp Meta para enviar el codigo.", "error");
                return;
            }

            string mensajeRecuperacion =
                "Hola " + usuario.nombreUsuario + "," + Environment.NewLine + Environment.NewLine +
                "Recibimos una solicitud para recuperar el acceso al panel administrativo de SERINSIS PC." + Environment.NewLine + Environment.NewLine +
                "Tu codigo de verificacion es: " + codigo + Environment.NewLine + Environment.NewLine +
                "Este codigo vence en " + minutosVigencia + " minutos." + Environment.NewLine +
                "Si no solicitaste este cambio, ignora este mensaje.";

            WhatsAppResponse response = mensaje_texto_directo
                .enviar(
                    usuario.whatsApp,
                    mensajeRecuperacion,
                    configMeta.accessToken,
                    configMeta.phoneNumberId)
                .GetAwaiter()
                .GetResult();

            App_WhatsApp.Messages message = response?.messages?.FirstOrDefault();
            if (message == null || message.message_status != "accepted")
            {
                string detalleMeta = ObtenerDetalleMeta(response);

                string detalleBitacora = string.IsNullOrWhiteSpace(detalleMeta)
                    ? "Meta no confirmo el envio del codigo por WhatsApp"
                    : "Meta rechazo el envio: " + detalleMeta;

                control_UsuarioAdmin.RegistrarBitacora(usuario.id, usuario.loginUsuario, "RECUPERACION_CODIGO_FAIL", detalleBitacora);
                Mensage("Error", string.IsNullOrWhiteSpace(detalleMeta)
                    ? "No fue posible enviar el codigo por WhatsApp. Verifica la plantilla de Meta y vuelve a intentar."
                    : detalleMeta, "error");
                return;
            }

            control_UsuarioAdmin.RegistrarBitacora(usuario.id, usuario.loginUsuario, "RECUPERACION_CODIGO_OK", "Codigo temporal enviado por WhatsApp");
            Mensage("Ok", "Te enviamos un codigo temporal a tu WhatsApp autorizado. Este codigo vence en 10 minutos.", "success");
        }

        protected string ObtenerDetalleMeta(WhatsAppResponse response)
        {
            if (response == null)
            {
                return "Meta no devolvio respuesta.";
            }

            if (!string.IsNullOrWhiteSpace(response.errorMessage))
            {
                if (response.error != null && response.error.code > 0)
                {
                    return $"Meta respondio codigo {response.error.code}: {response.errorMessage}";
                }

                return response.errorMessage;
            }

            if (response.error != null)
            {
                return response.error.code > 0
                    ? $"Meta respondio codigo {response.error.code}: {response.error.message}"
                    : response.error.message;
            }

            if (!string.IsNullOrWhiteSpace(response.rawResponse))
            {
                return response.rawResponse;
            }

            return "Meta no confirmo el envio del mensaje.";
        }

        protected void btnRecuperar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsuarioRecuperar.Text) ||
                string.IsNullOrWhiteSpace(txtWhatsAppRecuperar.Text) ||
                string.IsNullOrWhiteSpace(txtCodigoRecuperacion.Text) ||
                string.IsNullOrWhiteSpace(txtClaveNueva.Text) ||
                string.IsNullOrWhiteSpace(txtClaveNueva2.Text))
            {
                Mensage("Error", "Debes completar todos los campos de recuperacion.", "error");
                return;
            }

            if (txtClaveNueva.Text != txtClaveNueva2.Text)
            {
                Mensage("Error", "La confirmacion de la nueva clave no coincide.", "error");
                return;
            }

            UsuarioAdmin usuario = control_UsuarioAdmin.ConsultarRecuperacion(txtUsuarioRecuperar.Text.Trim(), txtWhatsAppRecuperar.Text.Trim());
            bool actualizada = control_UsuarioAdmin.ConfirmarRecuperacionPorCodigo(
                txtUsuarioRecuperar.Text.Trim(),
                txtWhatsAppRecuperar.Text.Trim(),
                txtCodigoRecuperacion.Text.Trim(),
                txtClaveNueva.Text);

            if (!actualizada)
            {
                control_UsuarioAdmin.RegistrarBitacora(usuario != null ? (int?)usuario.id : null, txtUsuarioRecuperar.Text.Trim(), "RECUPERACION_FAIL", "Codigo invalido, vencido o no fue posible actualizar la clave");
                Mensage("Error", "El codigo no es valido, ya vencio o no fue posible actualizar la clave.", "error");
                return;
            }

            control_UsuarioAdmin.RegistrarBitacora(usuario != null ? (int?)usuario.id : null, txtUsuarioRecuperar.Text.Trim(), "RECUPERACION_OK", "Cambio de clave por codigo temporal enviado a WhatsApp");
            Mensage("Ok", "La clave fue actualizada correctamente. Ahora puedes iniciar sesion.", "success");
        }

        protected void Mensage(string titulo, string mensage, string tipo)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "randomtext", "alerta2('" + titulo + "','" + mensage + "','" + tipo + "','" + "');", true);
        }
    }
}
