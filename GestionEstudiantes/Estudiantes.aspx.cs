
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace GestionEstudiantes
{
    public partial class Estudiantes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEstudiantes();
            }
        }

        private void CargarEstudiantes()
        {
            string cadena = ConfigurationManager
                .ConnectionStrings["ConexionSQL"]
                .ConnectionString;

            using (SqlConnection conexion =
                new SqlConnection(cadena))
            {
                try
                {
                    string consulta =
                        "SELECT * FROM dbo.Estudiantes";

                    SqlDataAdapter adaptador =
                        new SqlDataAdapter(consulta, conexion);

                    DataTable tabla = new DataTable();

                    adaptador.Fill(tabla);

                    gvEstudiantes.DataSource = tabla;
                    gvEstudiantes.DataBind();

                    lblEstado.Text =
                        "Conexión exitosa. Registros encontrados: "
                        + tabla.Rows.Count;
                }
                catch (Exception ex)
                {
                    lblEstado.Text =
                        "Error al consultar: "
                        + Server.HtmlEncode(ex.Message);
                }
            }
        }

        protected void gvEstudiantes_RowCommand(
            object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EliminarEstudiante")
            {
                try
                {
                    int indice = Convert.ToInt32(e.CommandArgument);

                    int idEstudiante = Convert.ToInt32(
                        gvEstudiantes.DataKeys[indice].Value);

                    string cadena = ConfigurationManager
                        .ConnectionStrings["ConexionSQL"]
                        .ConnectionString;

                    using (SqlConnection conexion =
                        new SqlConnection(cadena))
                    {
                        string consulta =
                            "DELETE FROM dbo.Estudiantes " +
                            "WHERE IdEstudiante = @IdEstudiante";

                        using (SqlCommand comando =
                            new SqlCommand(consulta, conexion))
                        {
                            comando.Parameters.Add(
                                "@IdEstudiante",
                                SqlDbType.Int
                            ).Value = idEstudiante;

                            conexion.Open();

                            int filasAfectadas =
                                comando.ExecuteNonQuery();

                            if (filasAfectadas > 0)
                            {
                                CargarEstudiantes();

                                lblEstado.Text =
                                    "El estudiante fue eliminado correctamente.";
                            }
                            else
                            {
                                lblEstado.Text =
                                    "No se encontró el estudiante seleccionado.";

                                CargarEstudiantes();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblEstado.Text =
                        "Error al eliminar el estudiante: "
                        + Server.HtmlEncode(ex.Message);
                }
            }
        }
    }
}
