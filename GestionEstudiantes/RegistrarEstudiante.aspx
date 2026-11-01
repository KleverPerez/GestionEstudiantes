<%@ Page Title="Registrar estudiante" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="RegistrarEstudiante.aspx.cs"
    Inherits="GestionEstudiantes.RegistrarEstudiante" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-heading">
        <h2>Registrar estudiante</h2>
        <p>Ingresa la información para incorporar un nuevo estudiante.</p>
    </div>

    <div class="card">

        <h3 style="margin-bottom:22px; color:#1d2b42;">
            Información personal
        </h3>

        <asp:ValidationSummary ID="vsErrores" runat="server"
            ValidationGroup="Registro"
            CssClass="message"
            HeaderText="Revisa los siguientes campos:" />

        <asp:Label ID="lblMensaje" runat="server"
            EnableViewState="false">
        </asp:Label>

        <div class="form-group">
            <label for="txtCedula">Cédula *</label>
            <asp:TextBox ID="txtCedula" runat="server"
                CssClass="form-control"
                MaxLength="10"
                placeholder="Ingrese la cédula">
            </asp:TextBox>

            <asp:RequiredFieldValidator
                ID="rfvCedula" runat="server"
                ControlToValidate="txtCedula"
                ErrorMessage="La cédula es obligatoria."
                ValidationGroup="Registro"
                Display="None" />

            <asp:RegularExpressionValidator
                ID="revCedula" runat="server"
                ControlToValidate="txtCedula"
                ValidationExpression="^[0-9]{10}$"
                ErrorMessage="La cédula debe contener exactamente 10 números."
                ValidationGroup="Registro"
                Display="None" />
        </div>

        <div class="form-group">
            <label for="txtNombre">Nombre *</label>
            <asp:TextBox ID="txtNombre" runat="server"
                CssClass="form-control"
                MaxLength="50"
                placeholder="Ingrese el nombre">
            </asp:TextBox>

            <asp:RequiredFieldValidator
                ID="rfvNombre" runat="server"
                ControlToValidate="txtNombre"
                ErrorMessage="El nombre es obligatorio."
                ValidationGroup="Registro"
                Display="None" />
        </div>

        <div class="form-group">
            <label for="txtApellido">Apellido *</label>
            <asp:TextBox ID="txtApellido" runat="server"
                CssClass="form-control"
                MaxLength="50"
                placeholder="Ingrese el apellido">
            </asp:TextBox>

            <asp:RequiredFieldValidator
                ID="rfvApellido" runat="server"
                ControlToValidate="txtApellido"
                ErrorMessage="El apellido es obligatorio."
                ValidationGroup="Registro"
                Display="None" />
        </div>

        <div class="form-group">
            <label for="txtCorreo">Correo electrónico *</label>
            <asp:TextBox ID="txtCorreo" runat="server"
                CssClass="form-control"
                TextMode="Email"
                MaxLength="100"
                placeholder="ejemplo@correo.com">
            </asp:TextBox>

            <asp:RequiredFieldValidator
                ID="rfvCorreo" runat="server"
                ControlToValidate="txtCorreo"
                ErrorMessage="El correo electrónico es obligatorio."
                ValidationGroup="Registro"
                Display="None" />

            <asp:RegularExpressionValidator
                ID="revCorreo" runat="server"
                ControlToValidate="txtCorreo"
                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                ErrorMessage="Ingresa un correo electrónico válido."
                ValidationGroup="Registro"
                Display="None" />
        </div>

        <div class="form-group">
            <label for="txtTelefono">Teléfono</label>
            <asp:TextBox ID="txtTelefono" runat="server"
                CssClass="form-control"
                MaxLength="10"
                placeholder="Ingrese el teléfono">
            </asp:TextBox>
        </div>

        <div class="form-group">
            <label for="txtFechaNacimiento">Fecha de nacimiento</label>
            <asp:TextBox ID="txtFechaNacimiento" runat="server"
                CssClass="form-control"
                TextMode="Date">
            </asp:TextBox>
        </div>

        <div style="display:flex; gap:12px; flex-wrap:wrap;">

            <asp:Button ID="btnGuardar" runat="server"
                Text="Guardar estudiante"
                CssClass="btn btn-primary"
                ValidationGroup="Registro"
                OnClick="btnGuardar_Click" />

            <a href="<%= ResolveUrl("~/Estudiantes.aspx") %>"
               class="btn btn-secondary">
                Cancelar
            </a>

        </div>

    </div>

</asp:Content>
