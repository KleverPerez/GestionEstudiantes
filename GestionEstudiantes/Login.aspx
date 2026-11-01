<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="GestionEstudiantes.Login" %>

<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Iniciar sesión | Gestión de Estudiantes</title>
    <link href="Content/auth.css" rel="stylesheet" />
</head>

<body class="auth-body">
    <form id="form1" runat="server">
        <main class="auth-card">

            <div class="auth-brand">
                <div class="auth-logo">G</div>
                <h1>Gestión de Estudiantes</h1>
                <p>Acceso al sistema</p>
            </div>

            <div class="auth-field">
                <label for="txtUsuario">Usuario</label>
                <asp:TextBox ID="txtUsuario" runat="server"
                    CssClass="auth-input"
                    MaxLength="50"
                    placeholder="Usuario" />

                <asp:RequiredFieldValidator ID="valUsuario"
                    runat="server"
                    ControlToValidate="txtUsuario"
                    ErrorMessage="Campo obligatorio."
                    CssClass="auth-validator"
                    ForeColor="Red"
                    Display="Dynamic" />
            </div>

            <div class="auth-field">
                <label for="txtContrasena">Contraseña</label>
                <asp:TextBox ID="txtContrasena" runat="server"
                    CssClass="auth-input"
                    TextMode="Password"
                    MaxLength="100"
                    placeholder="Contraseña" />

                <asp:RequiredFieldValidator ID="valContrasena"
                    runat="server"
                    ControlToValidate="txtContrasena"
                    ErrorMessage="Campo obligatorio."
                    CssClass="auth-validator"
                    ForeColor="Red"
                    Display="Dynamic" />
            </div>

            <asp:Button ID="btnIngresar" runat="server"
                Text="Iniciar sesión"
                CssClass="auth-button"
                OnClick="btnIngresar_Click" />

            <asp:Label ID="lblMensaje" runat="server"
                CssClass="auth-message" />

            <asp:HyperLink ID="lnkRegistro" runat="server"
                NavigateUrl="~/RegistroUsuario.aspx"
                CssClass="auth-link">
                Crear una cuenta
            </asp:HyperLink>

        </main>
    </form>
</body>
</html>
