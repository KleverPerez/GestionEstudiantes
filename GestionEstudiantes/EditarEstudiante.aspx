<%@ Page Title="Editar estudiante" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="EditarEstudiante.aspx.cs"
    Inherits="GestionEstudiantes.EditarEstudiante" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-heading">
        <h2>Editar estudiante</h2>
        <p>Modifica la información del estudiante seleccionado.</p>
    </div>

    <div class="card">

        <asp:Label ID="lblMensaje" runat="server"
            EnableViewState="false">
        </asp:Label>

        <asp:HiddenField ID="hfIdEstudiante" runat="server" />

        <div class="form-group">
            <label for="txtCedula">Cédula *</label>
            <asp:TextBox ID="txtCedula" runat="server"
                CssClass="form-control" MaxLength="10">
            </asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCedula" runat="server"
                ControlToValidate="txtCedula"
                ErrorMessage="La cédula es obligatoria."
                ValidationGroup="Edicion" Display="None" />
            <asp:RegularExpressionValidator ID="revCedula" runat="server"
                ControlToValidate="txtCedula"
                ValidationExpression="^[0-9]{10}$"
                ErrorMessage="La cédula debe contener exactamente 10 números."
                ValidationGroup="Edicion" Display="None" />
        </div>

        <div class="form-group">
            <label for="txtNombre">Nombre *</label>
            <asp:TextBox ID="txtNombre" runat="server"
                CssClass="form-control" MaxLength="50">
            </asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                ControlToValidate="txtNombre"
                ErrorMessage="El nombre es obligatorio."
                ValidationGroup="Edicion" Display="None" />
        </div>

        <div class="form-group">
            <label for="txtApellido">Apellido *</label>
            <asp:TextBox ID="txtApellido" runat="server"
                CssClass="form-control" MaxLength="50">
            </asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                ControlToValidate="txtApellido"
                ErrorMessage="El apellido es obligatorio."
                ValidationGroup="Edicion" Display="None" />
        </div>

        <div class="form-group">
            <label for="txtCorreo">Correo electrónico *</label>
            <asp:TextBox ID="txtCorreo" runat="server"
                CssClass="form-control" TextMode="Email"
                MaxLength="100">
            </asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCorreo" runat="server"
                ControlToValidate="txtCorreo"
                ErrorMessage="El correo es obligatorio."
                ValidationGroup="Edicion" Display="None" />
            <asp:RegularExpressionValidator ID="revCorreo" runat="server"
                ControlToValidate="txtCorreo"
                ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                ErrorMessage="Ingresa un correo electrónico válido."
                ValidationGroup="Edicion" Display="None" />
        </div>

        <div class="form-group">
            <label for="txtTelefono">Teléfono</label>
            <asp:TextBox ID="txtTelefono" runat="server"
                CssClass="form-control" MaxLength="15">
            </asp:TextBox>
        </div>

        <div class="form-group">
            <label for="txtFechaNacimiento">Fecha de nacimiento</label>
            <asp:TextBox ID="txtFechaNacimiento" runat="server"
                CssClass="form-control" TextMode="Date">
            </asp:TextBox>
        </div>

        <asp:ValidationSummary ID="vsErrores" runat="server"
            ValidationGroup="Edicion"
            CssClass="message"
            HeaderText="Revisa los siguientes campos:" />

        <div style="display:flex; gap:12px; flex-wrap:wrap;">

            <asp:Button ID="btnActualizar" runat="server"
                Text="Guardar cambios"
                CssClass="btn btn-primary"
                ValidationGroup="Edicion"
                OnClick="btnActualizar_Click" />

            <a href="<%= ResolveUrl("~/Estudiantes.aspx") %>"
               class="btn btn-secondary">
                Volver al listado
            </a>

        </div>

    </div>

</asp:Content>
