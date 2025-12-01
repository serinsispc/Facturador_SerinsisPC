using Facturador_SerinsisPC.Models;
using Facturador_SerinsisPC.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Facturador_SerinsisPC
{
    public partial class addDB : System.Web.UI.Page
    {
        public int idCliente = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            idCliente = (int)Session["idCliente"];
            if (!IsPostBack)
            {
                Cargar_DDL();
                Cargar_RP();
            }
        }
        protected void Cargar_RP()
        {
            List<DatBases> datBases = new List<DatBases>();
            string query = $"select *from DatBases where idCliente={idCliente}";
            datBases =JsonConvert.DeserializeObject<List<DatBases>>(DBEntities.ConsultaSQLServer(DBEntities.connectionString,query));
            rpDB_Cliente.DataSource = datBases;
            rpDB_Cliente.DataBind();
        }
        protected void Cargar_DDL()
        {
            try
            {
                List<ListaDB> listaDB = new List<ListaDB>();
                string query = $"select *from ListaDB";
                listaDB = JsonConvert.DeserializeObject<List<ListaDB>>(DBEntities.ConsultaSQLServer(DBEntities.connectionString, query));
                if (listaDB.Count > 0)
                {
                    ddl_db.DataValueField = "database_id";
                    ddl_db.DataTextField = "name";
                    ddl_db.DataSource = listaDB;
                    ddl_db.DataBind();
                }
            }
            catch (Exception ex) 
            {
                string e = ex.Message;
            }
        }

        protected void btn_Agregar_Click(object sender, EventArgs e)
        {
            DatBases datBases = new DatBases();
            datBases.idCliente = idCliente;
            datBases.nameDataBase=ddl_db.SelectedItem.Text;
            datBases.estado = 1;
            string query = $"exec InsertInto_DatBases {datBases.idCliente},'{datBases.nameDataBase}',{datBases.estado}";
            RespuestaSQL respuestaSQL = new RespuestaSQL();
            respuestaSQL = JsonConvert.DeserializeObject<List<RespuestaSQL>>(DBEntities.ConsultaSQLServer(DBEntities.connectionString,query)).FirstOrDefault();
            Cargar_RP();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as LinkButton).CommandArgument);
            try
            {
                DatBases datBases = new DatBases();
                string query = $"exec Delete_DatBases {id}";
                RespuestaSQL respuestaSQL = new RespuestaSQL();
                respuestaSQL = JsonConvert.DeserializeObject<List<RespuestaSQL>>(DBEntities.ConsultaSQLServer(DBEntities.connectionString, query)).FirstOrDefault();
                Cargar_RP();
            }
            catch(Exception ex)
            {
                string error=ex.Message;
            }
        }
    }
}