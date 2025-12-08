<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCliente.aspx.cs" Inherits="ProyectoVenta.PRESENTACION.frmCliente" %>
 

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Gestión de Clientes - Pizzería</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <nav class="navbar navbar-expand-lg navbar-dark bg-danger mb-4">
                <div class="container-fluid">
                    <a class="navbar-brand" href="#">PIZZERÍA BAMBINO</a>
                    <div class="navbar-nav">
                        <asp:HyperLink runat="server" NavigateUrl="~/frmTipo.aspx" CssClass="nav-link" Text="Tipo"/>
                        <asp:HyperLink runat="server" NavigateUrl="~/frmProducto.aspx" CssClass="nav-link" Text="Productos"/>
                        <asp:HyperLink runat="server" NavigateUrl="~/frmCliente.aspx" CssClass="nav-link active" Text="Clientes"/>
                        <asp:HyperLink runat="server" NavigateUrl="~/frmVenta.aspx" CssClass="nav-link" Text="Ventas"/>
                    </div>
                </div>
            </nav>
            
            <h2 class="text-center mb-4">GESTIÓN DE CLIENTES</h2>
            
            <div class="card mb-4">
                <div class="card-header bg-info text-white">
                    <h5>Datos del Cliente</h5>
                </div>
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <label>Nombre:</label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <label>Apellidos:</label>
                            <asp:TextBox ID="txtApellidos" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    
                    <div class="row mb-3">
                        <div class="col-md-4">
                            <label>Teléfono:</label>
                            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="70123456"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label>Email:</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label>Zona:</label>
                            <asp:DropDownList ID="ddlZona" runat="server" CssClass="form-select">
                                <asp:ListItem Text="-- Seleccione --" Value=""></asp:ListItem>
                                <asp:ListItem Text="Centro" Value="Centro"></asp:ListItem>
                                <asp:ListItem Text="Norte" Value="Norte"></asp:ListItem>
                                <asp:ListItem Text="Sur" Value="Sur"></asp:ListItem>
                                <asp:ListItem Text="Este" Value="Este"></asp:ListItem>
                                <asp:ListItem Text="Oeste" Value="Oeste"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    
                    <div class="row mb-3">
                        <div class="col-md-12">
                            <label>Dirección:</label>
                            <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    
                    <div class="row mb-3">
                        <div class="col-md-12">
                            <label>Referencia:</label>
                            <asp:TextBox ID="txtReferencia" runat="server" CssClass="form-control" placeholder="Cerca de..."></asp:TextBox>
                        </div>
                    </div>
                    
                    <div class="text-center">
                        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-secondary" OnClick="btnNuevo_Click"/>
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success" OnClick="btnGuardar_Click"/>
                        <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-warning" OnClick="btnModificar_Click"/>
                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnEliminar_Click"/>
                        <asp:button runat="server" Text="Prueba" CssClass="btn btn-primary" Visible="false"/>
                    </div>
                </div>
            </div>

            <!-- este es un comentario -->
            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h5>Buscar Cliente</h5>
                </div>
                <div class="card-body">
                    <div class="input-group mb-3">
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar por nombre..."></asp:TextBox>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click"/>
                    </div>
                    
                    <asp:GridView ID="gvClientes" runat="server" CssClass="table table-striped table-hover" 
                        AutoGenerateColumns="False" 
                        DataKeyNames="id_cliente"
                        OnSelectedIndexChanged="gvClientes_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="id_cliente" HeaderText="ID" Visible="false" />
                        <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="apellido" HeaderText="Apellidos" />
                        <asp:BoundField DataField="telefono" HeaderText="Teléfono" />
                        <asp:BoundField DataField="email" HeaderText="Email" />
                        <asp:BoundField DataField="zona" HeaderText="Zona" visible="true"/>
                        <asp:CommandField ShowSelectButton="True" SelectText="Seleccionar" ButtonType="Button" />
                    </Columns>
                    </asp:GridView>

                </div>
            </div>
            
            <asp:Label ID="lblMensaje" runat="server" CssClass="alert mt-3" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>
