<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmTipo.aspx.cs" Inherits="ProyectoVenta.PRESENTACION.frmTipo" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Gestión de Tipos (Tamaños) - Pizzería</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
        <div class="container mt-4">
            <nav class="navbar navbar-expand-lg navbar-dark bg-danger mb-4">
                <div class="container-fluid">
                    <a class="navbar-brand" href="#">🍕 PIZZERÍA BAMBINO</a>
                    <div class="navbar-nav">
                        <asp:HyperLink runat="server" NavigateUrl="~/frmTipo.aspx" CssClass="nav-link active" Text="Tipo"/>
                        <asp:HyperLink runat="server" NavigateUrl="~/frmProducto.aspx" CssClass="nav-link" Text="Productos"/>
                        <asp:HyperLink runat="server" NavigateUrl="~/frmCliente.aspx" CssClass="nav-link" Text="Clientes"/>
                        <asp:HyperLink runat="server" NavigateUrl="~/frmVenta.aspx" CssClass="nav-link" Text="Ventas"/>
                    </div>
                </div>
            </nav>

            <h2 class="text-center mb-4">🍕 GESTIÓN DE TIPOS (Tamaños de Pizza)</h2>

            <div class="card mb-4">
                <div class="card-header bg-warning text-dark">
                    <h5>Datos del Tipo</h5>
                </div>
                <div class="card-body">
                    <div class="row mb-3">
                        <div class="col-md-8">
                            <label>Tipo o tamaño de pizza:</label>
                            <asp:TextBox ID="txtTipo" runat="server" CssClass="form-control" placeholder="Ej: Pequeño, Mediano, Grande"></asp:TextBox>
                        </div>
                        <div class="col-md-4 d-flex align-items-end">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success me-2" OnClick="btnGuardar_Click"/>
                            <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" CssClass="btn btn-secondary" OnClick="btnNuevo_Click"/>
                            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" CssClass="btn btn-danger" OnClick="btnEliminar_Click"/>
                        </div>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h5>Tipos registrados</h5>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvTipo" runat="server" CssClass="table table-striped table-hover"
                        AutoGenerateColumns="False" OnSelectedIndexChanged="gvTipo_SelectedIndexChanged">
                        <Columns>
                            <asp:BoundField DataField="id_tipo" HeaderText="ID" Visible="false" />
                            <asp:BoundField DataField="nombre" HeaderText="Tipo/Tamaño" />
                            <asp:CommandField ShowSelectButton="True" SelectText="Seleccionar" ButtonType="Button"/>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <asp:Label ID="lblMensaje" runat="server" CssClass="alert mt-3" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>
