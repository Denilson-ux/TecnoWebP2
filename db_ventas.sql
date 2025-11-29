create database bdventa;
use bdventa;
create table categoria(
id_categoria int auto_increment primary key,
nombre varchar(40));

create table producto(
id_producto int AUTO_INCREMENT PRIMARY KEY,
descripcion varchar(30),
precio float not null,
stock int not null,
id_categoria int not null,
foreign key(id_categoria) references categoria(id_categoria));

create table cliente(
id_cliente int AUTO_INCREMENT PRIMARY KEY,
nombre varchar(30),
apellidos varchar(30),
empresa varchar(50),
telefono varchar(10),
direccion varchar(50));

create table venta(
id_venta int AUTO_INCREMENT PRIMARY KEY,
fecha date,
monto float,
id_cliente int,
foreign key(id_cliente) references cliente(id_cliente));

create table detalleVenta(
id_venta int,
id_producto int,
preciov float,
cantidad int,
primary key(id_venta,id_producto),
foreign key(id_venta) references venta(id_venta),
foreign key(id_producto) references producto(id_producto));


DELIMITER $
CREATE PROCEDURE guardarCategoria(in nomb varchar(40))
begin
	insert into categoria(nombre) values(nomb);
end $


DELIMITER $
CREATE PROCEDURE buscarCategoria(in buscar varchar(40))
begin
	select *from categoria where nombre like concat('%',buscar,'%');
end $


DELIMITER $
CREATE PROCEDURE selectCategoria()
begin
	select *from categoria;
end $



DELIMITER $
CREATE PROCEDURE guardarProducto(
    in descr varchar(30),
    in prec float,
    in stoc int,
    in id_cat int )
begin
	insert into producto(descripcion, precio, stock, id_categoria) values(descr, prec, stoc ,id_cat);
end $

DELIMITER $
CREATE PROCEDURE modificarProducto(
    in id_prod int,
    in descr varchar(30),
    in prec float,
    in stoc int,
    in id_cat int )
begin
	update producto set descripcion=descr, precio=prec, stock= stoc, id_categoria=id_cat
	where id_producto=id_prod;
end $

DELIMITER $
CREATE PROCEDURE eliminarProducto(in id_prod int)
begin
	DELETE from producto
	where id_producto=id_prod;
end $

DELIMITER $
CREATE PROCEDURE buscarProducto(in buscar varchar(30))
begin
	select p.id_producto, p.descripcion, p.precio, p.stock, c.nombre as nomb_categoria 
	from producto p inner join categoria c on c.id_categoria=p.id_categoria
	where p.descripcion like concat('%',buscar,'%');
end $


DELIMITER $
CREATE PROCEDURE guardarCliente(
    in nom varchar (30),
    in ape varchar(30),
    in emp varchar (50),
    in tel varchar (10),
    in dir varchar (50))
begin
	insert into cliente(nombre, apellidos, empresa, telefono, direccion) values(nom, ape, emp, tel, dir);
end $

DELIMITER $
CREATE PROCEDURE modificarCliente(
    in id_c int,
    in nom varchar(30),
    in ape varchar (30),
    in emp varchar (50),
    in tel varchar (10),
    in dir varchar (50))
begin
	update cliente set nombre=nom, apellidos=ape, empresa=emp, telefono=tel, direccion=dir
    where id_cliente=id_c;
end $

DELIMITER $
CREATE PROCEDURE eliminarCliente(in id_cli int)
begin
	delete from cliente 
    where id_cliente=id_cli;
end $
DELIMITER $
CREATE PROCEDURE buscarCliente(in buscar varchar(30))
begin
	select  *from cliente 
    where nombre like concat('%',buscar,'%') or apellidos like concat('%',buscar,'%');
end $

DELIMITER $
CREATE PROCEDURE guardarVenta(
    in fech date,
    in mont float,
    in id_cli int)
begin
    insert into venta(fecha, monto, id_cliente) values(fech, mont, id_cli);
end $
DELIMITER $
CREATE PROCEDURE modificarVenta(
    in id_v int,
    in fech date,
    in mont float,
    in id_cli int)
begin
	update venta set fecha=fech, monto=mont, id_cliente=id_cli
    where id_venta=id_v;
end $

DELIMITER $
CREATE PROCEDURE eliminarVenta(in id_v int)
begin
    delete from venta 
    where id_venta=id_v;
end $
DELIMITER $
CREATE PROCEDURE buscarVenta(in buscar varchar(30))
begin
	
	select v.id_venta, v.fecha, v.monto, concat_ws(' ',c.nombre, c.apellidos) as cliente
    from venta v inner join cliente c on c.id_cliente=v.id_cliente
    where concat_ws(' ',c.nombre, c.apellidos) like concat('%',buscar,'%');
end $

DELIMITER $
CREATE PROCEDURE guardarDetalleVenta(
	in id_p int, 
	in prec float,
	in cant int)
begin
 	declare id_v int;
    set id_v=(select MAX(venta.id_venta) from venta);
    insert into detalleVenta values(id_v, id_p, prec, cant);
    
    update producto set stock = stock - cant
    where producto.id_producto=id_p;
end $

DELIMITER $
CREATE PROCEDURE eliminarDetalleVenta(in id_v int)
begin
    delete from detalleVenta 
    where detalleVenta.id_venta=id_v;
end $

DELIMITER $
CREATE PROCEDURE buscarDetalleVenta(in id_v int)
begin
	select dv.id_producto, p.descripcion, p.precio, dv.cantidad, dv.preciov
    from detalleVenta dv inner join producto p on dv.id_producto = p.id_producto
    where dv.id_venta = id_v;
end $
DELIMITER $
CREATE PROCEDURE buscarClienteVenta(in id_v int)
begin
    select v.id_cliente from venta v
    where v.id_venta= id_v;
end $

DELIMITER $
CREATE TRIGGER modificarStock 
BEFORE DELETE ON detalleVenta 
FOR EACH ROW
BEGIN
 UPDATE producto p
    JOIN detalleVenta dv
    ON dv.id_producto = p.id_producto
    SET p.stock = p.stock + dv.cantidad
    WHERE dv.id_producto = OLD.id_producto
    and dv.id_venta=OLD.id_venta;
END $


call guardarCategoria ('Memoria RAM');
call guardarCategoria ('CPU');
call buscarCategoria('emoria');
call selectCategoria();
call guardarProducto ('Kingtons 8GB', 230, 15, 1);
call guardarProducto ('Kingtons 4GB', 230, 10, 1);
call modificarProducto (1, 'Kingtons 16GB', 450, 20, 1);
call buscarProducto ('K');
call eliminarProducto (2);
call guardarCliente('juan','perez añez','sofia srl','777778','barrio 3 demayo');
call modificarCliente('1','daisy','cossio quispe','sofia srl','777778','barrio 3 demayo');
call eliminarCliente (2);
call buscarCliente('quispe');
call guardarVenta('2020-06-04', 900,'1');
call modificarVenta('1','2020-06-05', 901,'1');
call eliminarVenta('1');
call buscarVenta('quispe');
call guardarDetalleVenta('1','900','2');
call eliminarDetalleVenta('3');
call buscarDetalleVenta(36);
call buscarClienteVenta(1);
select *from producto;
select *from venta;
select *from detalleVenta;