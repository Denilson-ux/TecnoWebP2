<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmListadoVentas.aspx.cs" Inherits="ProyectoVenta.PRESENTACION.frmListadoVentas" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Listado de Ventas - Pizzería Bambino</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        .badge-pendiente { background-color: #ffc107; }
        .badge-completado { background-color: #28a745; }
        .badge-cancelado { background-color: #dc3545; }
        .badge-proceso { background-color: #17a2b8; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid mt-4">
            <nav class="navbar navbar-expand-lg navbar-dark bg-danger mb-4">
                <div class="container-fluid">
                    <a class="navbar-brand" href="#">PIZZERIA BAMBINO - VENTAS</a>
                    <div class="navbar-nav">
                        <asp:HyperLink runat="server" NavigateUrl="~/frmTipo.aspx" CssClass="nav-link" Text="Tipo" />
                        <asp:HyperLink runat="server" NavigateUrl="~/frmProducto.aspx" CssClass="nav-link" Text="Productos" />
                        <asp:HyperLink runat="server" NavigateUrl="~/frmCliente.aspx" CssClass="nav-link" Text="Clientes" />
                        <asp:HyperLink runat="server" NavigateUrl="~/frmVenta.aspx" CssClass="nav-link" Text="Pedidos" />
                        <asp:HyperLink runat="server" NavigateUrl="~/frmListadoVentas.aspx" CssClass="nav-link active" Text="Ver Ventas" />
                    </div>
                </div>
            </nav>

            <h2 class="text-center mb-4">LISTADO DE VENTAS REALIZADAS</h2>

            <div class="card mb-4">
                <div class="card-header bg-primary text-white">
                    <h5>Filtros de Busqueda</h5>
                </div>
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-3">
                            <label>Numero de Pedido:</label>
                            <asp:TextBox ID="txtBuscarNumero" runat="server" CssClass="form-control" placeholder="Ej: PZ-20251130123456"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label>Fecha Desde:</label>
                            <asp:TextBox ID="txtFechaDesde" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label>Fecha Hasta:</label>
                            <asp:TextBox ID="txtFechaHasta" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-3" style="padding-top: 32px;">
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
                            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" OnClick="btnLimpiar_Click" />
                        </div>
                    </div>
                    
                    <!-- Sección de Email para Reportes -->
                    <div class="row mt-3">
                        <div class="col-md-8">
                            <label>Email Destinatario (para enviar reporte):</label>
                            <asp:TextBox ID="txtEmailDestino" runat="server" CssClass="form-control" 
                                         placeholder="ejemplo@correo.com" TextMode="Email"></asp:TextBox>
                            <small class="text-muted">Se enviara un reporte en HTML con las ventas filtradas</small>
                        </div>
                        <div class="col-md-4" style="padding-top: 32px;">
                            <asp:Button ID="btnEnviarReporte" runat="server" Text="Enviar Reporte por Email" 
                                        CssClass="btn btn-warning w-100" OnClick="btnEnviarReporte_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-header bg-success text-white">
                    <h5>Ventas Registradas</h5>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvVentas" runat="server" CssClass="table table-bordered table-hover"
                                  AutoGenerateColumns="False" EmptyDataText="No hay ventas registradas">
                        <Columns>
                            <asp:BoundField DataField="numero_venta" HeaderText="Nro. Pedido" />
                            <asp:BoundField DataField="fecha_venta" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" HtmlEncode="False" />
                            <asp:BoundField DataField="nombre_cliente" HeaderText="Cliente" />
                            <asp:BoundField DataField="telefono" HeaderText="Telefono" />
                            <asp:BoundField DataField="total" HeaderText="Total" DataFormatString="Bs. {0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" />
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <span class='badge <%# GetEstadoBadgeClass(Eval("estado_venta").ToString()) %>'>
                                        <%# Eval("estado_venta") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="metodo_pago" HeaderText="Pago" />
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkFactura" runat="server"
                                                   NavigateUrl='<%# "frmFactura.aspx?id=" + Eval("id_venta") %>'
                                                   Text="Factura"
                                                   CssClass="btn btn-sm btn-info" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <div class="mt-3 text-end">
                        <h5>Total Acumulado: <asp:Label ID="lblTotalGeneral" runat="server" CssClass="text-success" Text="Bs. 0.00"></asp:Label></h5>
                    </div>
                </div>
            </div>

            <div class="text-center mt-3">
                <asp:HyperLink runat="server" NavigateUrl="~/frmVenta.aspx" CssClass="btn btn-success btn-lg" Text="Nuevo Pedido" />
            </div>
        </div>

        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>
