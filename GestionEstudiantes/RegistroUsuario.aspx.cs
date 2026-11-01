using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Web.UI;

namespace GestionEstudiantes
{
    public partial class RegistroUsuario : Page
    {
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            string usuario = txtUsuario.Text.Trim();
            string contrasena = txtContrasena.Text;

            if (usuario.Length < 3)
            {
                MostrarMensaje("El usuario debe tener al menos 3 caracteres.", false);
                return;
            }

            if (contrasena.Length < 8)
            {
                MostrarMensaje("La contraseña debe tener al menos 8 caracteres.", false);
                return;
            }

            // Generar una sal aleatoria de 16 bytes.
            byte[] salt = new byte[16];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Generar el hash de la contraseña mediante PBKDF2-SHA256.
            byte[] hash;

            using (var pbkdf2 = new Rfc2898DeriveBytes(
                contrasena, salt, 150000, HashAlgorithmName.SHA256))
            {
                hash = pbkdf2.GetBytes(32);
            }

            string hashBase64 = Convert.ToBase64String(hash);
            string saltBase64 = Convert.ToBase64String(salt);

            string conexion = ConfigurationManager
                .ConnectionStrings["ConexionSQL"].ConnectionString;

            try
            {
                using (SqlConnection cn = new SqlConnection(conexion))
                {
                    string consulta = @"
                        INSERT INTO dbo.Usuarios
                        (Usuario, HashContrasena, Salt)
                        VALUES
                        (@Usuario, @Hash, @Salt)";

                    using (SqlCommand cmd = new SqlCommand(consulta, cn))
                    {
                        cmd.Parameters.AddWithValue("@Usuario", usuario);
                        cmd.Parameters.AddWithValue("@Hash", hashBase64);
                        cmd.Parameters.AddWithValue("@Salt", saltBase64);

                        cn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                MostrarMensaje("Cuenta creada correctamente. Ya puedes iniciar sesión.", true);

                txtUsuario.Text = "";
                txtContrasena.Text = "";
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    MostrarMensaje("Ese nombre de usuario ya está registrado.", false);
                }
                else
                {
                    MostrarMensaje("No se pudo crear la cuenta. Verifica la conexión con la base de datos.", false);
                }
            }
        }

        private void MostrarMensaje(string mensaje, bool correcto)
        {
            lblMensaje.Text = mensaje;
            lblMensaje.ForeColor = correcto
                ? System.Drawing.Color.Green
                : System.Drawing.Color.Red;
        }
    }
}
