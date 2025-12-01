<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmVenta.aspx.cs" Inherits="ProyectoVenta.PRESENTACION.frmVenta" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Registro de Pedidos - Pizzeria</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <style>
        .orden-detalle { background-color: #fff3cd; padding: 15px; border-radius: 8px; }
        .totales { background-color: #d1ecf1; padding: 15px; border-radius: 8px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid mt-4">
            <nav class="navbar navbar-expand-lg navbar-dark bg-danger mb-4">
                <div class="container-fluid">
                    <a class="navbar-brand" href="#">PIZZERIA BAMBINO - PEDIDO</a>
                    <div class="navbar-nav">
                        <asp:HyperLink runat="server" NavigateUrl="~/frmTipo.aspx" CssClass="nav-link" Text="Tipo" />
                        <asp:HyperLink runat="server" NavigateUrl="~/frmProducto.aspx" CssClass="nav-link" Text="Productos" />
                        <asp:HyperLink runat="server" NavigateUrl="~/frmCliente.aspx" CssClass="nav-link" Text="Clientes" />
                        <asp:HyperLink runat="server" NavigateUrl="~/frmVenta.aspx" CssClass="nav-link active" Text="Pedidos" />
                        <asp:HyperLink runat="server" NavigateUrl="~/frmListadoVentas.aspx" CssClass="nav-link" Text="Ver Ventas" />
                    </div>
                </div>
            </nav>

            <h2 class="text-center mb-4">REGISTRO DE PEDIDO</h2>

            <div class="row mb-3">
                <div class="col-md-8">
                    <div class="card">
                        <div class="card-header bg-primary text-white">
                            <h5>Datos del Pedido</h5>
                        </div>
                        <div class="card-body">
                            <div class="row mb-3">
                                <div class="col-md-4">
                                    <label>Nro. Pedido:</label>
                                    <asp:TextBox ID="txtNumeroVenta" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label>Fecha:</label>
                                    <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row mb-3">
                                <div class="col-md-12">
                                    <label>Cliente:</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtCliente" runat="server" CssClass="form-control" ReadOnly="true" placeholder="Seleccione un cliente"></asp:TextBox>
                                        <asp:Button ID="btnBuscarCliente" runat="server" Text="Buscar" CssClass="btn btn-info" OnClick="btnBuscarCliente_Click" />
                                        <asp:HiddenField ID="hfIdCliente" runat="server" Value="0" />
                                    </div>
                                </div>
                            </div>

                            <div class="row mb-3">
                                <div class="col-md-12">
                                    <label>Ubicacion / Direccion entrega:</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtDireccionEntrega" runat="server"
                                                     CssClass="form-control" ReadOnly="true"
                                                     Placeholder="Seleccione en el mapa"></asp:TextBox>
                                        <button type="button" class="btn btn-outline-primary"
                                                onclick="mostrarMapaUbicacion();">
                                            Seleccionar ubicacion
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-4">
                    <div class="card totales">
                        <div class="card-body">
                            <h5 class="text-center mb-3">TOTALES</h5>
                            <table class="table table-sm">
                                <tr>
                                    <td><strong>Subtotal:</strong></td>
                                    <td class="text-end">
                                        <asp:Label ID="lblSubtotal" runat="server" Text="Bs. 0,00"></asp:Label>
                                    </td>
                                </tr>
                                <tr class="table-success">
                                    <td><strong>TOTAL:</strong></td>
                                    <td class="text-end">
                                        <strong>
                                            <asp:Label ID="lblTotal" runat="server" Text="Bs. 0,00"></asp:Label>
                                        </strong>
                                    </td>
                                </tr>
                            </table>

                            <div class="mb-2 text-center">
                                <label>Pago con PayPal:</label>
                                <div id="paypal-button-container"></div>
                                <asp:HiddenField ID="hfPagoAprobado" runat="server" Value="0" />
                                <asp:HiddenField ID="hfPayPalOrderID" runat="server" Value="" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="card mb-3">
                <div class="card-header bg-warning">
                    <h5>Agregar Productos al Pedido</h5>
                </div>
                <div class="card-body orden-detalle">
                    <div class="row mb-3">
                        <div class="col-md-5">
                            <label>Producto:</label>
                            <asp:DropDownList ID="ddlProducto" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlProducto_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-2" id="divTipo" runat="server" visible="false">
                            <label>Tipo:</label>
                            <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlTipo_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                        <div class="col-md-2">
                            <label>Cantidad:</label>
                            <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control" TextMode="Number" Text="1"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label>Precio Unit.:</label>
                            <asp:TextBox ID="txtPrecioUnit" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-1" style="padding-top: 32px;">
                            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="btn btn-success btn-lg" OnClick="btnAgregar_Click" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <label>Notas Especiales:</label>
                            <asp:TextBox ID="txtNotas" runat="server" CssClass="form-control" placeholder="Ej: Sin cebolla, extra queso..."></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="card mb-3">
                <div class="card-header bg-success text-white">
                    <h5>Detalle del Pedido</h5>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvDetalle" runat="server" CssClass="table table-bordered table-hover"
                                  AutoGenerateColumns="False" OnRowDeleting="gvDetalle_RowDeleting">
                        <Columns>
                            <asp:BoundField DataField="NombreProducto" HeaderText="Producto" />
                            <asp:BoundField DataField="Cantidad" HeaderText="Cant." />
                            <asp:BoundField DataField="PrecioUnitario" HeaderText="Precio Unit." DataFormatString="Bs. {0:N2}" HtmlEncode="False" />
                            <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="Bs. {0:N2}" HtmlEncode="False" />
                            <asp:BoundField DataField="NotasEspeciales" HeaderText="Notas" />
                            <asp:CommandField ShowDeleteButton="True" DeleteText="Quitar" ButtonType="Button" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="row mb-4">
                <div class="col-md-8">
                    <label>Observaciones:</label>
                    <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                </div>
                <div class="col-md-4 text-center" style="padding-top: 32px;">
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-secondary btn-lg" OnClick="btnNuevo_Click" />
                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Venta" CssClass="btn btn-success btn-lg" OnClick="btnGuardar_Click" />
                </div>
            </div>

            <asp:Label ID="lblMensaje" runat="server" CssClass="alert" Visible="false"></asp:Label>
        </div>

        <!-- MODAL CLIENTES -->
        <div class="modal fade" id="modalClientes" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-lg modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header bg-info text-white">
                        <h5 class="modal-title">Seleccionar Cliente</h5>
                        <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body">
                        <div class="row mb-3">
                            <div class="col-md-10">
                                <asp:TextBox ID="txtBuscarCliente" runat="server" CssClass="form-control"
                                             placeholder="Buscar por nombre o teléfono"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnFiltrarCliente" runat="server" Text="Buscar" CssClass="btn btn-primary w-100"
                                            OnClick="btnFiltrarCliente_Click" />
                            </div>
                        </div>

                        <asp:GridView ID="gvClientes" runat="server" CssClass="table table-striped table-hover"
                                      AutoGenerateColumns="False" OnRowCommand="gvClientes_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="id_cliente" HeaderText="ID" />
                                <asp:BoundField DataField="nombre_completo" HeaderText="Nombre" />
                                <asp:BoundField DataField="telefono" HeaderText="Telefono" />
                                <asp:BoundField DataField="direccion" HeaderText="Direccion" />
                                <asp:TemplateField HeaderText="Accion">
                                    <ItemTemplate>
                                        <asp:Button ID="btnSelCliente" runat="server"
                                            Text="Seleccionar"
                                            CommandName="SeleccionarCliente"
                                            CommandArgument='<%# Container.DataItemIndex %>'
                                            CssClass="btn btn-sm btn-success" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hfLatitud" runat="server" />
        <asp:HiddenField ID="hfLongitud" runat="server" />
        <asp:HiddenField ID="hfDireccionMapa" runat="server" />

        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"></script>

        <script>
            function mostrarModalClientes() {
                var myModal = new bootstrap.Modal(document.getElementById('modalClientes'));
                myModal.show();
            }
            function cerrarModalClientes() {
                var modalEl = document.getElementById('modalClientes');
                var modal = bootstrap.Modal.getInstance(modalEl);
                if (modal) modal.hide();
            }

            let map, marker, geocoder;

            function mostrarMapaUbicacion() {
                var modal = new bootstrap.Modal(document.getElementById('modalMapa'));
                modal.show();

                if (!map) {
                    initMap();
                }
            }

            function initMap() {
                const defaultPos = { lat: -16.5, lng: -68.15 };

                geocoder = new google.maps.Geocoder();

                map = new google.maps.Map(document.getElementById("map"), {
                    center: defaultPos,
                    zoom: 13
                });

                marker = new google.maps.Marker({
                    map: map,
                    position: defaultPos,
                    draggable: true
                });

                setPosition(defaultPos.lat, defaultPos.lng);

                if (navigator.geolocation) {
                    navigator.geolocation.getCurrentPosition(function (pos) {
                        const loc = {
                            lat: pos.coords.latitude,
                            lng: pos.coords.longitude
                        };
                        map.setCenter(loc);
                        marker.setPosition(loc);
                        setPosition(loc.lat, loc.lng);
                        reverseGeocode(loc);
                    });
                }

                map.addListener("click", function (e) {
                    marker.setPosition(e.latLng);
                    setPosition(e.latLng.lat(), e.latLng.lng());
                    reverseGeocode(e.latLng);
                });

                marker.addListener("dragend", function (e) {
                    setPosition(e.latLng.lat(), e.latLng.lng());
                    reverseGeocode(e.latLng);
                });
            }

            function buscarDireccion() {
                const dir = document.getElementById("txtBuscarDir").value;
                if (!dir) return;

                geocoder.geocode({ address: dir }, function (results, status) {
                    if (status === "OK" && results[0]) {
                        const loc = results[0].geometry.location;
                        map.setCenter(loc);
                        marker.setPosition(loc);
                        setPosition(loc.lat(), loc.lng());
                        actualizarLabelUbicacion(results[0].formatted_address);
                    }
                });
            }

            function reverseGeocode(latlng) {
                geocoder.geocode({ location: latlng }, function (results, status) {
                    if (status === "OK" && results[0]) {
                        actualizarLabelUbicacion(results[0].formatted_address);
                    }
                });
            }

            function actualizarLabelUbicacion(texto) {
                document.getElementById("lblUbicacionSel").innerText = texto;
                document.getElementById("<%= hfDireccionMapa.ClientID %>").value = texto;
            }

            function setPosition(lat, lng) {
                document.getElementById("<%= hfLatitud.ClientID %>").value = lat;
                document.getElementById("<%= hfLongitud.ClientID %>").value = lng;
            }

            function confirmarUbicacion() {
                const dir = document.getElementById("<%= hfDireccionMapa.ClientID %>").value;
                if (dir) {
                    document.getElementById("<%= txtDireccionEntrega.ClientID %>").value = dir;
                }
                var modal = bootstrap.Modal.getInstance(document.getElementById('modalMapa'));
                modal.hide();
            }
        </script>

        <!-- Modal del mapa -->
        <div class="modal fade" id="modalMapa" tabindex="-1" aria-hidden="true">
          <div class="modal-dialog modal-lg modal-dialog-centered">
            <div class="modal-content">
              <div class="modal-header bg-dark text-white">
                <h5 class="modal-title">Seleccionar Ubicacion de Entrega</h5>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
              </div>
              <div class="modal-body">
                <label>Buscar direccion:</label>
                <div class="input-group mb-2">
                  <input id="txtBuscarDir" type="text" class="form-control" placeholder="Ej: Warnes, Bolivia" />
                  <button type="button" class="btn btn-success" onclick="buscarDireccion();">
                    Buscar
                  </button>
                </div>

                <div id="map" style="width:100%;height:350px;"></div>

                <div class="mt-3">
                  <strong>Ubicacion seleccionada:</strong>
                  <div id="lblUbicacionSel" class="small text-muted">
                    Ninguna ubicacion seleccionada
                  </div>
                </div>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                <button type="button" class="btn btn-primary" onclick="confirmarUbicacion();">
                  Usar esta ubicacion
                </button>
              </div>
            </div>
          </div>
        </div>

        <script src="https://www.paypal.com/sdk/js?client-id=Aafq8M27njB8pt4MrjbFsGE74YukRHpMuhCuHMS8SA6x6Fwf7z9BDCnaBPeOFTSr19WXQEXtgNcd6RK_&currency=USD"></script>



        <script>
            function obtenerTotalNumerico() {
                var txt = document.getElementById("<%= lblTotal.ClientID %>").innerText || "0";
                txt = txt.replace("Bs.", "").replace("Bs", "").trim();
                txt = txt.replace(".", "").replace(",", ".");
                var valor = parseFloat(txt);
                if (isNaN(valor)) valor = 0;
                return valor;
            }

            if (typeof paypal === "undefined") {
                console.error("paypal no está definido; revisa si el SDK cargó correctamente.");
            } else {
                paypal.Buttons({
                    createOrder: function (data, actions) {
                        var totalBs = obtenerTotalNumerico();
                        var tipoCambio = 7;
                        var totalUsd = totalBs / tipoCambio;

                        if (totalUsd <= 0) {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: 'El total a pagar debe ser mayor a 0. Agregue productos al pedido.'
                            });
                            return actions.reject && actions.reject();
                        }

                        console.log("Total Bs:", totalBs, "Total USD:", totalUsd.toFixed(2));

                        return actions.order.create({
                            purchase_units: [{
                                amount: {
                                    value: totalUsd.toFixed(2)
                                },
                                description: 'Pedido Pizzería Bambino'
                            }]
                        });
                    },
                    onApprove: function (data, actions) {
                        console.log("onApprove data:", data);
                        return actions.order.capture().then(function (details) {
                            document.getElementById("<%= hfPagoAprobado.ClientID %>").value = "1";
                            document.getElementById("<%= hfPayPalOrderID.ClientID %>").value = data.orderID;
                            
                            Swal.fire({
                                icon: 'success',
                                title: '¡Pago Completado!',
                                html: `
                                    <strong>Pagado por:</strong> ${details.payer.name.given_name}<br>
                                    <strong>Email:</strong> ${details.payer.email_address}<br>
                                    <strong>ID Transacción:</strong> ${data.orderID}<br><br>
                                    <p class="text-success">Ahora puede presionar <strong>"Guardar Venta"</strong></p>
                                `,
                                confirmButtonText: 'Entendido'
                            });
                        });
                    },
                    onCancel: function (data) {
                        console.warn("Pago cancelado:", data);
                        Swal.fire({
                            icon: 'warning',
                            title: 'Pago Cancelado',
                            text: 'El pago fue cancelado. Puede intentarlo nuevamente.'
                        });
                    },
                    onError: function (err) {
                        console.error("PayPal error:", err);
                        Swal.fire({
                            icon: 'error',
                            title: 'Error en PayPal',
                            text: 'Ocurrió un error con el procesamiento del pago. Inténtelo nuevamente.'
                        });
                    }
                }).render('#paypal-button-container');
            }
        </script>

        <script async src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCsxWxTypWZuxQm7QFdzZjhQ3MLAsLVhM8"></script>
    </form>
</body>
</html>
