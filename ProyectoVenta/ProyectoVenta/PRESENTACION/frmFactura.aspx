<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmFactura.aspx.cs" Inherits="ProyectoVenta.PRESENTACION.frmFactura" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Factura - Pizzería Bambino</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        @media print {
            .no-print { display: none; }
            body { background-color: white; }
        }
        .factura-container {
            max-width: 800px;
            margin: 0 auto;
            background: white;
            padding: 30px;
            border: 2px solid #dee2e6;
            border-radius: 8px;
        }
        .header-factura {
            border-bottom: 3px solid #dc3545;
            padding-bottom: 20px;
            margin-bottom: 30px;
        }
        .info-section {
            background-color: #f8f9fa;
            padding: 15px;
            border-radius: 5px;
            margin-bottom: 20px;
        }
    </style>
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container mt-5">
            <div class="factura-container">
                <!-- Header -->
                <div class="header-factura">
                    <div class="row">
                        <div class="col-md-8">
                            <h1 class="text-danger">🍕 PIZZERÍA BAMBINO</h1>
                            <p class="mb-0"><strong>Teléfono:</strong> (591) 123-45678</p>
                            <p class="mb-0"><strong>Dirección:</strong> Warnes, Santa Cruz - Bolivia</p>
                        </div>
                        <div class="col-md-4 text-end">
                            <h3>FACTURA</h3>
                            <p class="mb-0"><strong>Nro:</strong> <asp:Label ID="lblNumeroVenta" runat="server"></asp:Label></p>
                            <p class="mb-0"><strong>Fecha:</strong> <asp:Label ID="lblFecha" runat="server"></asp:Label></p>
                        </div>
                    </div>
                </div>

                <!-- Información del Cliente -->
                <div class="info-section">
                    <h5 class="mb-3">👤 Información del Cliente</h5>
                    <div class="row">
                        <div class="col-md-6">
                            <p class="mb-1"><strong>Cliente:</strong> <asp:Label ID="lblCliente" runat="server"></asp:Label></p>
                            <p class="mb-1"><strong>Teléfono:</strong> <asp:Label ID="lblTelefono" runat="server"></asp:Label></p>
                        </div>
                        <div class="col-md-6">
                            <p class="mb-1"><strong>Dirección de Entrega:</strong></p>
                            <p class="mb-1"><asp:Label ID="lblDireccion" runat="server"></asp:Label></p>
                        </div>
                    </div>
                </div>

                <!-- Detalles del Pedido -->
                <h5 class="mb-3">📋 Detalle del Pedido</h5>
                <asp:GridView ID="gvDetalle" runat="server" CssClass="table table-bordered"
                              AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="nombre_producto" HeaderText="Producto" />
                        <asp:BoundField DataField="cantidad" HeaderText="Cant." ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="precio_unitario" HeaderText="Precio Unit." 
                                        DataFormatString="Bs. {0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="subtotal" HeaderText="Subtotal" 
                                        DataFormatString="Bs. {0:N2}" HtmlEncode="False" ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="notas_especiales" HeaderText="Notas" />
                    </Columns>
                </asp:GridView>

                <!-- Totales -->
                <div class="row mt-4">
                    <div class="col-md-6">
                        <div class="info-section">
                            <p class="mb-1"><strong>Estado del Pedido:</strong> 
                                <span class="badge bg-<%= GetEstadoBadge() %>">
                                    <asp:Label ID="lblEstado" runat="server"></asp:Label>
                                </span>
                            </p>
                            <p class="mb-1"><strong>Método de Pago:</strong> <asp:Label ID="lblMetodoPago" runat="server"></asp:Label></p>
                            <asp:Panel ID="pnlPayPal" runat="server" Visible="false">
                                <p class="mb-0"><strong>ID PayPal:</strong> <small><asp:Label ID="lblPayPalID" runat="server"></asp:Label></small></p>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <table class="table table-sm">
                            <tr>
                                <td class="text-end"><strong>Subtotal:</strong></td>
                                <td class="text-end" style="width: 120px;"><asp:Label ID="lblSubtotal" runat="server"></asp:Label></td>
                            </tr>
                            <tr class="table-success">
                                <td class="text-end"><strong>TOTAL:</strong></td>
                                <td class="text-end"><h5><asp:Label ID="lblTotal" runat="server"></asp:Label></h5></td>
                            </tr>
                        </table>
                    </div>
                </div>

                <!-- Observaciones -->
                <asp:Panel ID="pnlObservaciones" runat="server" Visible="false" CssClass="mt-3">
                    <div class="info-section">
                        <p class="mb-1"><strong>Observaciones:</strong></p>
                        <p class="mb-0"><asp:Label ID="lblObservaciones" runat="server"></asp:Label></p>
                    </div>
                </asp:Panel>

                <!-- Mensaje de Agradecimiento -->
                <div class="text-center mt-4 pt-3 border-top">
                    <p class="mb-1"><strong>¡Gracias por su compra!</strong></p>
                    <p class="text-muted small">Este documento es un comprobante de su pedido</p>
                </div>

                <!-- Botones -->
                <div class="text-center mt-4 no-print">
                    <button type="button" class="btn btn-primary btn-lg" onclick="window.print();">
                        🖨️ Imprimir Factura
                    </button>
                    <asp:HyperLink runat="server" NavigateUrl="~/frmListadoVentas.aspx" 
                                   CssClass="btn btn-secondary btn-lg" Text="← Volver al Listado" />
                    <asp:HyperLink runat="server" NavigateUrl="~/frmVenta.aspx" 
                                   CssClass="btn btn-success btn-lg" Text="➕ Nuevo Pedido" />
                </div>
            </div>
        </div>

        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>
