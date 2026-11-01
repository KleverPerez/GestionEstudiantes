using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;

namespace GestionEstudiantes
{
    public partial class EditarEstudiante : Page
    {
        private string CadenaConexion
        {
            get
            {
                return ConfigurationManager
                    .ConnectionStrings["ConexionSQL"]
                    .ConnectionString;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int id;

                if (!int.TryParse(Request.QueryString["id"], out id) || id <= 0)
                {
                    MostrarMensaje("El identificador del estudiante no es válido.", false);
                    btnActualizar.Enabled = false;
                    return;
                }

                hfIdEstudiante.Value = id.ToString();
                CargarEstudiante(id);
            }
        }

        private void CargarEstudiante(int id)
        {
            string consulta = @"
                SELECT Cedula, Nombre, Apellido, Correo,
                       Telefono, FechaNacimiento
                FROM dbo.Estudiantes
                WHERE IdEstudiante = @Id";

            using (SqlConnection conexion = new SqlConnection(CadenaConexion))
            using (SqlCommand comando = new SqlCommand(consulta, conexion))
            {
                comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                try
                {
                    conexion.Open();

                    using (SqlDataReader lector = comando.ExecuteReader())
                    {
                        if (lector.Read())
                        {
                            txtCedula.Text = lector["Cedula"].ToString();
                            txtNombre.Text = lector["Nombre"].ToString();
                            txtApellido.Text = lector["Apellido"].ToString();
                            txtCorreo.Text = lector["Correo"].ToString();

                            txtTelefono.Text =
                                lector["Telefono"] == DBNull.Value
                                ? ""
                                : lector["Telefono"].ToString();

                            txtFechaNacimiento.Text =
                                lector["FechaNacimiento"] == DBNull.Value
                                ? ""
                                : Convert.ToDateTime(
                                    lector["FechaNacimiento"]
                                  ).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            MostrarMensaje("No se encontró el estudiante solicitado.", false);
                            btnActualizar.Enabled = false;
                        }
                    }
                }
                catch (Exception)
                {
                    MostrarMensaje("No fue posible cargar los datos del estudiante.", false);
                    btnActualizar.Enabled = false;
                }
            }
        }

        protected void btnActualizar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int id;

            if (!int.TryParse(hfIdEstudiante.Value, out id) || id <= 0)
            {
                MostrarMensaje("El identificador del estudiante no es válido.", false);
                return;
            }

            object fecha = DBNull.Value;

            if (!string.IsNullOrWhiteSpace(txtFechaNacimiento.Text))
            {
                DateTime fechaNacimiento;

                if (!DateTime.TryParseExact(
                    txtFechaNacimiento.Text,
                    "yyyy-MM-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out fechaNacimiento))
                {
                    MostrarMensaje("La fecha de nacimiento no es válida.", false);
                    return;
                }

                fecha = fechaNacimiento.Date;
            }

            string consulta = @"
                UPDATE dbo.Estudiantes
                SET Cedula = @Cedula,
                    Nombre = @Nombre,
                    Apellido = @Apellido,
                    Correo = @Correo,
                    Telefono = @Telefono,
                    FechaNacimiento = @FechaNacimiento
                WHERE IdEstudiante = @Id";

            try
            {
                using (SqlConnection conexion = new SqlConnection(CadenaConexion))
                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.Add("@Cedula", SqlDbType.NVarChar, 10)
                        .Value = txtCedula.Text.Trim();

                    comando.Parameters.Add("@Nombre", SqlDbType.NVarChar, 50)
                        .Value = txtNombre.Text.Trim();

                    comando.Parameters.Add("@Apellido", SqlDbType.NVarChar, 50)
                        .Value = txtApellido.Text.Trim();

                    comando.Parameters.Add("@Correo", SqlDbType.NVarChar, 100)
                        .Value = txtCorreo.Text.Trim();

                    comando.Parameters.Add("@Telefono", SqlDbType.NVarChar, 15)
                        .Value = string.IsNullOrWhiteSpace(txtTelefono.Text)
                            ? (object)DBNull.Value
                            : txtTelefono.Text.Trim();

                    comando.Parameters.Add("@FechaNacimiento", SqlDbType.Date)
                        .Value = fecha;

                    comando.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    conexion.Open();

                    int filas = comando.ExecuteNonQuery();

                    if (filas > 0)
                    {
                        Response.Redirect(
                            ResolveUrl("~/Estudiantes.aspx?actualizado=1"),
                            false);

                        Context.ApplicationInstance.CompleteRequest();
                    }
                    else
                    {
                        MostrarMensaje("No se encontró el estudiante para actualizar.", false);
                    }
                }
            }
            catch (Exception)
            {
                MostrarMensaje(
                    "No fue posible actualizar el estudiante. Verifica los datos y la conexión.",
                    false);
            }
        }

        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = Server.HtmlEncode(mensaje);
            lblMensaje.CssClass = "message";
            lblMensaje.Style["background-color"] =
                correcto ? "#e8f8ee" : "#feecec";
            lblMensaje.Style["color"] =
                correcto ? "#207443" : "#bd3030";
        }
    }
}
