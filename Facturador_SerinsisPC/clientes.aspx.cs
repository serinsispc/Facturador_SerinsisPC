using Facturador_SerinsisPC.Models;
using Facturador_SerinsisPC.Models.Controlers;
using Facturador_SerinsisPC.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Facturador_SerinsisPC
{
    public partial class clientes : System.Web.UI.Page
    {
        #region Eventos
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarListaClientes();
                CargarTipoPlan();
            }
        }
        protected void btnNuevoCliente_Click(object sender, EventArgs e)
        {
            ViewState["idCliente"] = 0;
            btnGuardar.Text = "Crear";
            panelModal.Visible = true;
        }
        protected void btnCerrarModal1_Click(object sender, EventArgs e)
        {
            panelModal.Visible = false;
        }
        protected void btnCerrarModal2_Click(object sender, EventArgs e)
        {
            panelModal.Visible = false;
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (VerificarCampos())
            {
                bool respuesta;
                if (btnGuardar.Text == "Crear")
                {
                   respuesta= GestionarCliente(0, (int)ViewState["idCliente"]);
                }
                else
                {
                   respuesta= GestionarCliente(1, (int)ViewState["idCliente"]);
                }
                if(respuesta)
                {
                    Mensage(1, "¡Ok!", "¡Cliente Gestionado correctamente...!", "success", "clientes.aspx");
                }
                else
                {
                    Mensage(1, "¡Error!", "¡proceso terminado con error...!", "error", "clientes.aspx");
                }
            }
            else
            {
                Mensage(2, "¡Error!", "¡Aun se encuentran capos vacíos...!", "error", "");
            }
            
        }
        protected void btnEditar_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            ViewState["idCliente"]= id;
            CargarCamposModals(id);
        }
        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            ViewState["idCliente"] = id;
            CargarCamposModals(id);
            panelModal.Visible = false;
            if (GestionarCliente(2, id))
            {
                Mensage(1, "¡Ok!", "¡Cliente Gestionado correctamente...!", "success", "clientes.aspx");
            }
            else
            {
                Mensage(1, "¡Error!", "¡proceso terminado con error...!", "error", "clientes.aspx");
            }
        }
        protected void btnEstado_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            ViewState["idCliente"] = id;
            Clientes clientes = new Clientes();
            clientes= control_Clientes.ConsultarIdCliente(id);
            if (clientes != null) 
            {
                if (clientes.estado == 0)
                {
                    clientes.estado = 1;
                }
                else
                {
                    clientes.estado = 0;
                }
            }
            control_Clientes.Crud(clientes,1);
            CargarListaClientes();
        }

        protected void btnAgregarDB_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            Session["idCliente"] = id;
            Server.Transfer("addDB.aspx");
        }

        #endregion Fin Eventos




        #region Funciones
        protected void CargarListaClientes()
        {
            rpClientes.DataSource = control_Clientes.ListaCompleta();
            rpClientes.DataBind();
        }
        protected void CargarTipoPlan()
        {
            ddl_TipoPlan.DataValueField = "id";
            ddl_TipoPlan.DataTextField = "nombrePlan";
            ddl_TipoPlan.DataSource=control_TipoPlan.ListaCompleta();
            ddl_TipoPlan.DataBind();
        }
        protected bool VerificarCampos()
        {
            if(ddl_TipoPlan.SelectedItem.Text!=""&&
               txtNit.Text!=""&&
               txtNombreComersial.Text!=""&&
               txtRepresentante.Text!=""&&
               txtWhatSapp.Text!=""&&
               txtValor.Text!=""&&
               txtCorreo.Text!=""&&
               txtSedes.Text != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected void CargarCamposModals(int idCliente)
        {
            V_Clientes vCliente = new V_Clientes();
            vCliente=control_Clientes.ConsultarIdCliente_vista(idCliente);
            if (vCliente != null)
            {
                ddl_TipoPlan.SelectedItem.Text =Convert.ToString(vCliente.nombrePlan);
                txtNit.Text = vCliente.nit;
                txtNombreComersial.Text = vCliente.nombreComercial;
                txtRepresentante.Text = vCliente.nombreRepresentate;
                txtWhatSapp.Text = vCliente.celular;
                txtValor.Text =Convert.ToString(Convert.ToInt32(vCliente.valorPlan));
                txtCorreo.Text= vCliente.correo;
                txtSedes.Text =Convert.ToString(vCliente.sedes);

                btnGuardar.Text = "Editar";

                panelModal.Visible = true;
            }
        }
        protected void Mensage(int estilo, string titulo, string mensage, string tipo, string aspx)
        {
            if (estilo == 1)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "randomtext", "alerta('" + titulo + "','" + mensage + "','" + tipo + "','" + aspx + "');", true);
            }
            if (estilo == 2)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "randomtext", "alerta2('" + titulo + "','" + mensage + "','" + tipo + "','" + "');", true);
            }
        }
        protected bool GestionarCliente(int Boton, int idCliente)
        {
            Clientes clientes = new Clientes();
            clientes = control_Clientes.ConsultarIdCliente(idCliente);
            if (clientes != null)
            {
                if (Boton == 0)
                {
                    return false;
                }
            }
            if (Boton == 0)
            {
                clientes = new Clientes();
                idCliente = 0;
            }
            clientes.id = idCliente;
            clientes.idTipoPlan = Convert.ToInt32(ddl_TipoPlan.SelectedItem.Value);
            clientes.nit = txtNit.Text;
            clientes.nombreComercial = txtNombreComersial.Text.ToUpper();
            clientes.nombreRepresentate = txtRepresentante.Text.ToUpper();
            clientes.celular = txtWhatSapp.Text;
            clientes.correo = txtCorreo.Text;
            clientes.sedes = Convert.ToInt32(txtSedes.Text);
            clientes.valorPlan = Convert.ToDecimal(txtValor.Text);
            clientes.estado = 1;
            return control_Clientes.Crud(clientes, Boton).respuesta;
        }


        #endregion Fin Funciones


    }
}