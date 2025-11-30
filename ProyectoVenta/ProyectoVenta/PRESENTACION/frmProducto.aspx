<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmProducto.aspx.cs" Inherits="ProyectoVenta.PRESENTACION.frmProducto" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Gestion de Productos - Pizzeria</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <nav class="navbar navbar-expand-lg navbar-dark bg-danger mb-4">
                <div class="container-fluid">
                    <a class="navbar-brand" href="#">PIZZERIA BAMBINO</a>
                    <div class="navbar-nav">
                        <asp:HyperLink runat="server" NavigateUrl="~/frmTipo.aspx" CssClass="nav-link" Text="Tipo"/>
                        <asp:HyperLink runat="server" NavigateUrl="~/frmProducto.aspx" CssClass="nav-link active" Text="Productos"/>
                        <asp:HyperLink runat="server" NavigateUrl="~/frmCliente.aspx" CssClass="nav-link" Text="Clientes"/>
                        <asp:HyperLink runat="server" NavigateUrl="~/frmVenta.aspx" CssClass="nav-link" Text="Ventas"/>
                    </div>
                </div>
            </nav>
            
            <h2 class="text-center mb-4">GESTION DE PRODUCTOS</h2>
            
            <div class="card mb-4">
                <div class="card-header bg-warning">
                    <h5>Datos del Producto</h5>
                </div>
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-md-3">
                            <label>Codigo:</label>
                            <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" placeholder="PIZ001"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <label>Nombre del Producto:</label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Pizza Margarita"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label>Precio Base (Bs.):</label>
                            <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control" placeholder="0.00"></asp:TextBox>
                        </div>
                    </div>
                    
                    <div class="row mb-3">
                        <div class="col-md-4">
                            <label>Tipo:</label>
                            <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-select"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <label>Stock:</label>
                            <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" TextMode="Number" Text="0"></asp:TextBox>
                        </div>
                    </div>
                    
                    <div class="row mb-3">
                        <div class="col-md-12">
                            <label>Descripcion:</label>
                            <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Ingredientes..."></asp:TextBox>
                        </div>
                    </div>
                    
                    <div class="text-center">
                        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-secondary" OnClick="btnNuevo_Click"/>
                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success" OnClick="btnGuardar_Click"/>
                        <asp:Button ID="btnModificar" runat="server" Text="Modificar" CssClass="btn btn-warning" OnClick="btnModificar_Click"/>
                        <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnEliminar_Click"/>
                    </div>
                </div>
            </div>
            
            <div class="card">
                <div class="card-header bg-info text-white">
                    <h5>Buscar Producto</h5>
                </div>
                <div class="card-body">
                    <div class="input-group mb-3">
                        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Buscar por nombre..."></asp:TextBox>
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click"/>
                    </div>
                    
                    <asp:GridView ID="gvProductos" runat="server"
    CssClass="table table-striped table-hover"
    AutoGenerateColumns="False"
    DataKeyNames="id_producto"
    OnSelectedIndexChanged="gvProductos_SelectedIndexChanged">
    <Columns>
        <asp:BoundField DataField="codigo_producto" HeaderText="Codigo" />
        <asp:BoundField DataField="nombre_producto" HeaderText="Producto" />
        <asp:BoundField DataField="Nombre" HeaderText="Tipo" />
        <asp:BoundField DataField="precio_base" HeaderText="Precio Base"
                        DataFormatString="{0:N2}" HtmlEncode="False" />
        <asp:BoundField DataField="stock" HeaderText="Stock" />
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