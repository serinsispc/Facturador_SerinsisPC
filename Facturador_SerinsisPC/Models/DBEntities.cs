using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Facturador_SerinsisPC.Models
{
    public class DBEntities
    {
        public static string connectionString;
        public static void ConexionBase(string servidor, string db, string usuario, string clave)
        {
            connectionString = $"data source ={servidor}; initial catalog ={db}; user id = {usuario}; password = {clave}";
        }

        public static string ConsultaSQLServer(string connectionString, string query)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        dataTable.Load(reader); // Llena el DataTable con los datos del SqlDataReader
                    }
                }
            }
            string data = JsonConvert.SerializeObject(dataTable);
            return data;
        }

        public static int EjecutarSQLServer(string connectionString, string query)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
