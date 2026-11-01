
<%@ Page Title="Panel principal" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="GestionEstudiantes._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-heading">
        <h2>Panel principal</h2>
        <p>Resumen general de la gestión de estudiantes.</p>
    </div>

    <!-- ESTADÍSTICAS -->
    <div class="stats-grid">

        <div class="stat-card">
            <div class="stat-header">
                <span>Total de estudiantes</span>
                <span class="stat-icon blue">♙</span>
            </div>
            <h3>
                <asp:Label ID="lblTotalEstudiantes" runat="server"
                    Text="0" />
            </h3>
            <p>Estudiantes registrados en el sistema.</p>
        </div>

        <div class="stat-card">
            <div class="stat-header">
                <span>Registros del mes</span>
                <span class="stat-icon green">＋</span>
            </div>
            <h3>
                <asp:Label ID="lblRegistrosMes" runat="server"
                    Text="0" />
            </h3>
            <p>Estudiantes incorporados este mes.</p>
        </div>

        <div class="stat-card">
            <div class="stat-header">
                <span>Últimos 7 días</span>
                <span class="stat-icon orange">▦</span>
            </div>
            <h3>
                <asp:Label ID="lblUltimosSieteDias" runat="server"
                    Text="0" />
            </h3>
            <p>Registros realizados recientemente.</p>
        </div>

    </div>

    <!-- GRÁFICO DE ACTIVIDAD -->
    <div class="card dashboard-card">

        <div class="section-heading">
            <div>
                <h3>Actividad de registros</h3>
                <p>Cantidad de estudiantes registrados por día.</p>
            </div>
            <span class="chart-legend">
                <span class="legend-dot"></span>
                Estudiantes
            </span>
        </div>

        <div class="activity-chart">

            <asp:Repeater ID="rptActividad" runat="server">
                <ItemTemplate>

                    <div class="activity-column">

                        <div class="activity-value">
                            <%# Eval("Cantidad") %>
                        </div>

                        <div class="activity-bar-container">
                            <div class="activity-bar"
                                style='height: <%# Eval("Altura") %>;'>
                            </div>
                        </div>

                        <div class="activity-label">
                            <%# Eval("Dia") %>
                        </div>

                    </div>

                </ItemTemplate>
            </asp:Repeater>

        </div>

    </div>

    <!-- ÚLTIMOS ESTUDIANTES -->
    <div class="card dashboard-card">

        <div class="section-heading">
            <div>
                <h3>Últimos estudiantes registrados</h3>
                <p>Los cinco registros más recientes de la base de datos.</p>
            </div>

            <a href="<%= ResolveUrl("~/Estudiantes.aspx") %>"
               class="btn btn-secondary">
                Ver todos
            </a>

        </div>

        <div class="table-container">

            <asp:GridView ID="gvUltimosEstudiantes" runat="server"
                AutoGenerateColumns="false"
                EmptyDataText="Todavía no existen estudiantes registrados."
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

                    <asp:BoundField DataField="FechaRegistro"
                        HeaderText="Fecha de registro"
                        DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                </Columns>

            </asp:GridView>

        </div>

    </div>

    <asp:Label ID="lblError" runat="server"
        CssClass="dashboard-error">
    </asp:Label>

</asp:Content>
