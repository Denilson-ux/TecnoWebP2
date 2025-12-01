<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmListadoVentas.aspx.cs" Inherits="ProyectoVenta.PRESENTACION.frmListadoVentas" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Listado de Ventas - Pizzería</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        .estado-pendiente { background-color: #fff3cd; }
        .estado-proceso { background-color: #cfe2ff; }
        .estado-completado { background-color: #d1e7dd; }
        .estado-cancelado { background-color: #f8d7da; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid mt-4">
            <nav class="navbar navbar-expand-lg navbar-dark bg-danger mb-4">
                <div class="container-fluid">
                    <a class="navbar-brand" href="#">🍕 PIZZERÍA BAMBINO - VENTAS</a>
                    <div class="navbar-nav">
                        <asp:HyperLink runat="server" NavigateUrl="~/frmTipo.aspx" CssClass="nav-link" Text="Tipo" />
                        <asp:HyperLink runat="server" NavigateUrl="~/frmProducto.aspx" CssClass="nav-link" Text="Productos" />
                        <asp:HyperLink runat="server" NavigateUrl="~/frmCliente.aspx" CssClass="nav-link" Text="Clientes" />
                        <asp:HyperLink runat="server" NavigateUrl="~/frmVenta.aspx" CssClass="nav-link" Text="Pedidos" />
                        <asp:HyperLink runat="server" NavigateUrl="~/frmListadoVentas.aspx" CssClass="nav-link active" Text="Ver Ventas" />
                    </div>
                </div>
            </nav>

            <h2 class="text-center mb-4">📊 LISTADO DE VENTAS REALIZADAS</h2>

            <div class="card mb-4">
                <div class="card-header bg-primary text-white">
                    <h5>🔍 Filtros de Búsqueda</h5>
                </div>
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-md-3">
                            <label>Número de Pedido:</label>
                            <asp:TextBox ID="txtBuscarNumero" runat="server" CssClass="form-control" 
                                         placeholder="Ej: PZ-20251130123456"></asp:TextBox>
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
                            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" 
                                        OnClick="btnBuscar_Click" />
                            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-secondary" 
                                        OnClick="btnLimpiar_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-header bg-success text-white">
                    <h5>📋 Ventas Registradas</h5>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvVentas" runat="server" CssClass="table table-striped table-hover"
                                  AutoGenerateColumns="False" EmptyDataText="No hay ventas registradas" 
                                  OnRowDataBound="gvVentas_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="numero_venta" HeaderText="Nro. Pedido" />
                            <asp:BoundField DataField="fecha_venta" HeaderText="Fecha" 
                                            DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            <asp:BoundField DataField="nombre_cliente" HeaderText="Cliente" />
                            <asp:BoundField DataField="direccion_entrega" HeaderText="Dirección" />
                            <asp:BoundField DataField="total" HeaderText="Total" 
                                            DataFormatString="Bs. {0:N2}" HtmlEncode="False" />
                            <asp:BoundField DataField="metodo_pago" HeaderText="Pago" />
                            <asp:TemplateField HeaderText="Estado">
                                <ItemTemplate>
                                    <span class="badge bg-<%# GetEstadoColor(Eval("estado_venta").ToString()) %>">
                                        <%# Eval("estado_venta") %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones">
                                <ItemTemplate>
                                    <asp:HyperLink runat="server" 
                                        NavigateUrl='<%# "frmFactura.aspx?id=" + Eval("id_venta") %>'
                                        CssClass="btn btn-sm btn-info" Text="📄 Factura" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <div class="mt-3">
                        <asp:Label ID="lblTotal" runat="server" CssClass="fw-bold fs-5"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="mt-3">
                <asp:HyperLink runat="server" NavigateUrl="~/frmVenta.aspx" 
                               CssClass="btn btn-success btn-lg" Text="➕ Nuevo Pedido" />
            </div>
        </div>

        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>
