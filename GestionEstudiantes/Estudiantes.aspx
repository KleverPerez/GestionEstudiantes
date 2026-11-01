
<%@ Page Title="Estudiantes" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Estudiantes.aspx.cs"
    Inherits="GestionEstudiantes.Estudiantes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-heading">
        <h2>Listado de estudiantes</h2>
        <p>Consulta los estudiantes registrados en la base de datos.</p>
    </div>

    <div class="card">

        <div style="display:flex; justify-content:space-between;
                    align-items:center; flex-wrap:wrap; gap:15px;
                    margin-bottom:22px;">

            <div>
                <h3 style="font-size:17px; color:#1d2b42;">
                    Estudiantes registrados
                </h3>
                <p style="font-size:13px; color:#8794a7; margin-top:5px;">
                    Información almacenada en SQL Server.
                </p>
            </div>

        </div>

        <asp:Label ID="lblEstado" runat="server"
            CssClass="message">
        </asp:Label>

        <div class="table-container">

            <asp:GridView ID="gvEstudiantes" runat="server"
                AutoGenerateColumns="false"
                DataKeyNames="IdEstudiante"
                OnRowCommand="gvEstudiantes_RowCommand"
                EmptyDataText="No existen estudiantes registrados."
                CssClass="table"
                GridLines="None"
                CellPadding="8"
                Width="100%">

                <Columns>

                    <asp:BoundField DataField="IdEstudiante"
                        HeaderText="ID" />

                    <asp:BoundField DataField="Cedula"
                        HeaderText="Cédula" />

                    <asp:BoundField DataField="Nombre"
                        HeaderText="Nombre" />

                    <asp:BoundField DataField="Apellido"
                        HeaderText="Apellido" />

                    <asp:BoundField DataField="Correo"
                        HeaderText="Correo electrónico" />

                    <asp:BoundField DataField="Telefono"
                        HeaderText="Teléfono" />

                    <asp:BoundField DataField="FechaNacimiento"
                        HeaderText="Fecha de nacimiento"
                        DataFormatString="{0:dd/MM/yyyy}"
                        NullDisplayText="No registrada" />

                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>

                            <a class="btn btn-secondary"
                               href='<%# ResolveUrl("~/EditarEstudiante.aspx?id=" + Eval("IdEstudiante")) %>'>
                                Editar
                            </a>

                            <asp:LinkButton ID="btnEliminar"
                                runat="server"
                                Text="Eliminar"
                                CssClass="btn btn-secondary"
                                CommandName="EliminarEstudiante"
                                CommandArgument='<%# ((GridViewRow)Container).RowIndex %>'
                                OnClientClick="return confirm('¿Está seguro de que desea eliminar este estudiante? Esta acción no se puede deshacer.');">
                            </asp:LinkButton>

                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

        </div>

    </div>

</asp:Content>
