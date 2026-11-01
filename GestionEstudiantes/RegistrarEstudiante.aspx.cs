using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace GestionEstudiantes
{
    public partial class RegistrarEstudiante : Page
    {
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            DateTime fechaNacimiento = DateTime.MinValue;

            if (!string.IsNullOrWhiteSpace(txtFechaNacimiento.Text) &&
                !DateTime.TryParse(txtFechaNacimiento.Text,
                    out fechaNacimiento))
            {
                MostrarMensaje("La fecha de nacimiento no es válida.", false);
                return;
            }

            string cadena = ConfigurationManager
                .ConnectionStrings["ConexionSQL"]
                .ConnectionString;

            string consulta = @"
                INSERT INTO dbo.Estudiantes
                (Cedula, Nombre, Apellido, Correo, Telefono, FechaNacimiento)
                VALUES
                (@Cedula, @Nombre, @Apellido, @Correo, @Telefono, @FechaNacimiento)";

            try
            {
                using (SqlConnection conexion = new SqlConnection(cadena))
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
                        .Value = string.IsNullOrWhiteSpace(txtFechaNacimiento.Text)
                            ? (object)DBNull.Value
                            : fechaNacimiento.Date;

                    conexion.Open();

                    comando.ExecuteNonQuery();
                }

                LimpiarFormulario();

                MostrarMensaje(
                    "El estudiante se registró correctamente.", true);
            }
            catch (Exception)
            {
                MostrarMensaje(
                    "No fue posible registrar el estudiante. Verifica la conexión y los datos ingresados.",
                    false);
            }
        }

        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = Server.HtmlEncode(mensaje);

            lblMensaje.CssClass = correcto
                ? "message"
                : "message";

            lblMensaje.Style["background-color"] = correcto
                ? "#e8f8ee"
                : "#feecec";

            lblMensaje.Style["color"] = correcto
                ? "#207443"
                : "#bd3030";
        }

        private void LimpiarFormulario()
        {
            txtCedula.Text = "";
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtCorreo.Text = "";
            txtTelefono.Text = "";
            txtFechaNacimiento.Text = "";
        }
    }
}
