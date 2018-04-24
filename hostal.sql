--Creación de Usuario

CREATE USER HOSTAL IDENTIFIED BY 123;
GRANT DBA TO HOSTAL;

--Creación de Tablas

CREATE TABLE USUARIO(
ID_USUARIO NUMBER(5),
NOMBRE_USUARIO VARCHAR2(25) NOT NULL,
PASSWORD VARCHAR2(25) NOT NULL,
TIPO_USUARIO VARCHAR2(25) NOT NULL,
ESTADO VARCHAR2(15) NOT NULL,
CONSTRAINT PK_USUARIO PRIMARY KEY(ID_USUARIO)
);

CREATE TABLE PAIS(
ID_PAIS NUMBER(5),
NOMBRE_PAIS VARCHAR2(50) NOT NULL,
CONSTRAINT PK_PAIS PRIMARY KEY(ID_PAIS)
);

CREATE TABLE REGION(
ID_REGION NUMBER(5),
NOMBRE_REGION VARCHAR2(50) NOT NULL,
ID_PAIS NUMBER(5) NOT NULL,
CONSTRAINT PK_REGION PRIMARY KEY(ID_REGION),
CONSTRAINT FK_REGION_PAIS FOREIGN KEY(ID_PAIS) REFERENCES PAIS(ID_PAIS)
);

CREATE TABLE COMUNA(
ID_COMUNA NUMBER(5),
NOMBRE_COMUNA VARCHAR2(50) NOT NULL,
ID_REGION NUMBER(5) NOT NULL,
CONSTRAINT PK_COMUNA PRIMARY KEY(ID_COMUNA),
CONSTRAINT FK_COMUNA_REGION FOREIGN KEY(ID_REGION) REFERENCES REGION(ID_REGION)
);

CREATE TABLE CLIENTE(
RUT_CLIENTE NUMBER(8),
DV_CLIENTE VARCHAR2(1) NOT NULL,
DIRECCION_CLIENTE VARCHAR2(100) NOT NULL,
CORREO_CLIENTE VARCHAR2(50),
TELEFONO_CLIENTE NUMBER(12),
ID_COMUNA NUMBER(5) NOT NULL,
ID_USUARIO NUMBER(5) NOT NULL,
CONSTRAINT PK_CLIENTE PRIMARY KEY(RUT_CLIENTE),
CONSTRAINT FK_CLIENTE_COMUNA FOREIGN KEY(ID_COMUNA) REFERENCES COMUNA(ID_COMUNA),
CONSTRAINT FK_CLIENTE_USUARIO FOREIGN KEY(ID_USUARIO) REFERENCES USUARIO(ID_USUARIO)
);

CREATE TABLE TIPO_HABITACION(
ID_TIPO_HABITACION NUMBER(5),
NOMBRE_TIPO_HABITACION VARCHAR2(25) NOT NULL,
CANTIDAD_PASAJERO NUMBER(2) NOT NULL,
CONSTRAINT PK_TIPO_HABITACION PRIMARY KEY(ID_TIPO_HABITACION)
);

CREATE TABLE HABITACION(
NUMERO_HABITACION NUMBER(5),
PRECIO_HABITACION NUMBER(10) NOT NULL,
ESTADO_HABITACION VARCHAR2(50) NOT NULL,
ID_TIPO_HABITACION NUMBER(5) NOT NULL,
RUT_CLIENTE NUMBER(8),
CONSTRAINT PK_HABITACION PRIMARY KEY(NUMERO_HABITACION),
CONSTRAINT FK_HABITACION_TIPO_HABITACION FOREIGN KEY(ID_TIPO_HABITACION) REFERENCES TIPO_HABITACION(ID_TIPO_HABITACION),
CONSTRAINT FK_HABITACION_CLIENTE FOREIGN KEY(RUT_CLIENTE) REFERENCES CLIENTE(RUT_CLIENTE)
);

CREATE TABLE ACCESORIO(
ID_ACCESORIO NUMBER(5),
NOMBRE_ACCESORIO VARCHAR2(50),
PRECIO NUMBER(10),
CANTIDAD NUMBER(5),
CONSTRAINT PK_ACCESORIO PRIMARY KEY(ID_ACCESORIO)
);

CREATE TABLE DETALLE_ACCESORIOS(
ID_DETALLE_ACCESORIO NUMBER(5),
ID_ACCESORIO NUMBER(5) NOT NULL,
NUMERO_HABITACION NUMBER(5),
CANTIDAD NUMBER(5) NOT NULL,
CONSTRAINT PK_DETALLE_ACCESORIOS PRIMARY KEY(ID_DETALLE_ACCESORIO, NUMERO_HABITACION),
CONSTRAINT FK_DETALLE_ACCESORIOS_ACC FOREIGN KEY(ID_ACCESORIO) REFERENCES ACCESORIO(ID_ACCESORIO),
CONSTRAINT FK_DETALLE_ACCESORIOS_HAB FOREIGN KEY(NUMERO_HABITACION) REFERENCES HABITACION(NUMERO_HABITACION) 
);

CREATE TABLE PENSION(
ID_PENSION NUMBER(5),
NOMBRE_PENSION VARCHAR2(25) NOT NULL,
VALOR_PENSION NUMBER(10) NOT NULL,
NUMERO_HABITACION NUMBER(5) NOT NULL,
CONSTRAINT PK_PENSION PRIMARY KEY(ID_PENSION),
CONSTRAINT FK_PENSION_HABITACION FOREIGN KEY(NUMERO_HABITACION) REFERENCES HABITACION(NUMERO_HABITACION)
);

CREATE TABLE TIPO_PLATO(
ID_TIPO_PLATO NUMBER(5),
NOMBRE_TIPO_PLATO VARCHAR2(50) NOT NULL,
CONSTRAINT PK_TIPO_PLATO PRIMARY KEY(ID_TIPO_PLATO)
);

CREATE TABLE CATEGORIA(
ID_CATEGORIA NUMBER(5),
NOMBRE_CATEGORIA VARCHAR2(50) NOT NULL,
CONSTRAINT PK_CATEGORIA PRIMARY KEY(ID_CATEGORIA)
);

CREATE TABLE PLATO(
ID_PLATO NUMBER (5), 
NOMBRE_PLATO VARCHAR2(50) NOT NULL,
PRECIO_PLATO NUMBER(10) NOT NULL,
ID_CATEGORIA NUMBER(5) NOT NULL,
ID_TIPO_PLATO NUMBER(5) NOT NULL,
CONSTRAINT PK_PLATO PRIMARY KEY(ID_PLATO),
CONSTRAINT FK_PLATO_CATEGORIA FOREIGN KEY(ID_CATEGORIA) REFERENCES CATEGORIA(ID_CATEGORIA),
CONSTRAINT FK_PLATO_TIPO_PLATO FOREIGN KEY(ID_TIPO_PLATO) REFERENCES TIPO_PLATO(ID_TIPO_PLATO)
);

CREATE TABLE DETALLE_PLATOS(
ID_DETALLE_PLATOS NUMBER(5),
CANTIDAD NUMBER(5) NOT NULL,
ID_PLATO NUMBER(5) NOT NULL,
ID_PENSION NUMBER(5) NOT NULL,
CONSTRAINT PK_DETALLE_PLATOS PRIMARY KEY(ID_DETALLE_PLATOS),
CONSTRAINT FK_DETALLE_PLATOS_PLATO FOREIGN KEY(ID_PLATO) REFERENCES PLATO(ID_PLATO),
CONSTRAINT FK_DETALLE_PLATOS_PENSION FOREIGN KEY(ID_PENSION) REFERENCES PENSION(ID_PENSION)
);

CREATE TABLE HUESPED(
RUT_HUESPED NUMBER(8),
DV_HUESPED VARCHAR2(1) NOT NULL,
PNOMBRE_HUESPED VARCHAR2(50) NOT NULL,
SNOMBRE_HUESPED VARCHAR2(50),
APP_PATERNO_HUESPED VARCHAR2(50) NOT NULL,
APP_MATERNO_HUESPED VARCHAR2(50) NOT NULL,
TELEFONO_HUESPED NUMBER(12),
REGISTRADO VARCHAR2(1) NOT NULL,
NUMERO_HABITACION NUMBER(5),
RUT_CLIENTE NUMBER(8) NOT NULL,
CONSTRAINT PK_HUESPED PRIMARY KEY(RUT_HUESPED),
CONSTRAINT FK_HUESPED_HABITACION FOREIGN KEY(NUMERO_HABITACION) REFERENCES HABITACION(NUMERO_HABITACION),
CONSTRAINT FK_HUESPED_CLIENTE FOREIGN KEY(RUT_CLIENTE) REFERENCES CLIENTE(RUT_CLIENTE)
);

CREATE TABLE EMPLEADO(
RUT_EMPLEADO NUMBER(8),
DV_EMPLEADO VARCHAR2(1) NOT NULL,
PNOMBRE_EMPLEADO VARCHAR2(50) NOT NULL,
SNOMBRE_EMPLEADO VARCHAR2(50),
APP_PATERNO_EMPLEADO VARCHAR2(50) NOT NULL,
APP_MATERNO_EMPLEADO VARCHAR2(50) NOT NULL,
ID_USUARIO NUMBER(5) NOT NULL,
CONSTRAINT PK_EMPLEADO PRIMARY KEY(RUT_EMPLEADO),
CONSTRAINT FK_EMPLEADO_USUARIO FOREIGN KEY(ID_USUARIO) REFERENCES USUARIO(ID_USUARIO)
);

CREATE TABLE ORDEN_COMPRA(
NUMERO_ORDEN NUMBER(5),
CANTIDAD_HUESPEDES NUMBER(10) NOT NULL,
FECHA_LLEGADA DATE NOT NULL, 
FECHA_SALIDA DATE,
RUT_EMPLEADO NUMBER(8),
RUT_CLIENTE NUMBER(8),
CONSTRAINT PK_ORDEN_COMPRA PRIMARY KEY(NUMERO_ORDEN),
CONSTRAINT FK_ORDEN_COMPRA_EMPLEADO FOREIGN KEY(RUT_EMPLEADO) REFERENCES EMPLEADO(RUT_EMPLEADO),
CONSTRAINT FK_ORDEN_COMPRA_CLIENTE FOREIGN KEY(RUT_CLIENTE) REFERENCES CLIENTE(RUT_CLIENTE)
);

CREATE TABLE BOLETA(
ID_BOLETA NUMBER(10),
VALOR_DESC_BOLETA NUMBER,
VALOR_TOTAL_BOLETA NUMBER(15) NOT NULL,
FECHA_EMISION_BOLETA DATE NOT NULL,
RUT_EMPLEADO NUMBER(8) NOT NULL,
RUT_CLIENTE NUMBER(8),
CONSTRAINT PK_BOLETA PRIMARY KEY(ID_BOLETA),
CONSTRAINT FK_BOLETA_EMPLEADO FOREIGN KEY(RUT_EMPLEADO) REFERENCES EMPLEADO(RUT_EMPLEADO),
CONSTRAINT FK_BOLETA_CLIENTE FOREIGN KEY(RUT_CLIENTE) REFERENCES CLIENTE(RUT_CLIENTE)
);

CREATE TABLE DETALLE_BOLETA(
ID_DETALLE_BOLETA NUMBER(10),
DESCRIPCION_DETALLE VARCHAR2(250) NOT NULL,
CANTIDAD NUMBER(10) NOT NULL,
VALOR_TOTAL NUMBER(10) NOT NULL,
ID_BOLETA NUMBER(10),
CONSTRAINT PK_DETALLE_BOLETA PRIMARY KEY(ID_DETALLE_BOLETA, ID_BOLETA),
CONSTRAINT FK_DETALLE_BOLETA_BOLETA FOREIGN KEY(ID_BOLETA) REFERENCES BOLETA(ID_BOLETA)
);

CREATE TABLE TIPO_PROVEEDOR(
ID_TIPO_PROVEEDOR NUMBER(5),
NOMBRE_TIPO VARCHAR2(50) NOT NULL,
CONSTRAINT PK_TIPO_PROVEEDOR PRIMARY KEY(ID_TIPO_PROVEEDOR)
);

CREATE TABLE PROVEEDOR(
RUT_PROVEEDOR NUMBER(8),
DV_PROVEEDOR VARCHAR2(1) NOT NULL,
PNOMBRE_PROVEEDOR VARCHAR2(50) NOT NULL,
SNOMBRE_PROVEEDOR VARCHAR2(50),
APP_PATERNO_PROVEEDOR VARCHAR2(50) NOT NULL,
APP_MATERNO_PROVEEDOR VARCHAR2(50) NOT NULL,
ID_TIPO_PROVEEDOR NUMBER(5) NOT NULL,
ID_USUARIO NUMBER(5) NOT NULL,
CONSTRAINT PK_PROVEEDOR PRIMARY KEY(RUT_PROVEEDOR),
CONSTRAINT FK_PROVEEDOR_TIPO_PROVEEDOR FOREIGN KEY(ID_TIPO_PROVEEDOR) REFERENCES TIPO_PROVEEDOR(ID_TIPO_PROVEEDOR),
CONSTRAINT FK_PROVEEDOR_USUARIO FOREIGN KEY(ID_USUARIO) REFERENCES USUARIO(ID_USUARIO)
);

CREATE TABLE FAMILIA(
ID_FAMILIA NUMBER(5),
NOMBRE_FAMILIA VARCHAR2(250) NOT NULL,
CONSTRAINT PK_FAMILIA PRIMARY KEY(ID_FAMILIA)
);

CREATE TABLE PRODUCTO(
ID_PRODUCTO NUMBER(5),
NOMBRE_PRODUCTO VARCHAR2(50) NOT NULL,
FECHA_VENCIMIENTO_PRODUCTO DATE NOT NULL,
STOCK_PRODUCTO NUMBER(5) NOT NULL,
STOCK_CRITICO_PRODUCTO NUMBER(5) NOT NULL,
DESCRIPCION_PRODUCTO VARCHAR2(250) NOT NULL,
PRECIO_PRODUCTO NUMBER(10) NOT NULL,
ID_FAMILIA NUMBER(5) NOT NULL,
CONSTRAINT PK_PRODUCTO PRIMARY KEY(ID_PRODUCTO),
CONSTRAINT FK_PRODUCTO_FAMILIA FOREIGN KEY(ID_FAMILIA) REFERENCES FAMILIA(ID_FAMILIA)
);

CREATE TABLE RECEPCION(
NUMERO_RECEPCION NUMBER(5),
FECHA_RECEPCION DATE NOT NULL,
RUT_PROVEEDOR NUMBER(8) NOT NULL,
RUT_EMPLEADO NUMBER(8) NOT NULL,
CONSTRAINT PK_RECEPCION PRIMARY KEY(NUMERO_RECEPCION),
CONSTRAINT FK_RECEPCION_PROVEEDOR FOREIGN KEY(RUT_PROVEEDOR) REFERENCES PROVEEDOR(RUT_PROVEEDOR),
CONSTRAINT FK_RECEPCION_EMPLEADO FOREIGN KEY(RUT_EMPLEADO) REFERENCES EMPLEADO(RUT_EMPLEADO) 
);

CREATE TABLE DETALLE_RECEPCION(
ID_DETALLE_RECEPCION NUMBER(5),
CANTIDAD NUMBER(10) NOT NULL,
ID_PRODUCTO NUMBER(5) NOT NULL,
NUMERO_RECEPCION NUMBER(5),
CONSTRAINT PK_DETALLE_RECEPCION PRIMARY KEY(ID_DETALLE_RECEPCION, NUMERO_RECEPCION),
CONSTRAINT FK_DETALLE_RECEPCION_PRODUCTO FOREIGN KEY(ID_PRODUCTO) REFERENCES PRODUCTO(ID_PRODUCTO),
CONSTRAINT FK_DETALLE_RECEPCION_RECEPCION FOREIGN KEY(NUMERO_RECEPCION) REFERENCES RECEPCION(NUMERO_RECEPCION)
);


CREATE TABLE PEDIDO(
NUMERO_PEDIDO NUMBER(5),
FECHA_PEDIDO DATE NOT NULL, 
ESTADO_PEDIDO VARCHAR2(25) NOT NULL,
RUT_EMPLEADO NUMBER(8) NOT NULL,
NUMERO_RECEPCION NUMBER(5),
RUT_PROVEEDOR NUMBER(8) NOT NULL,
CONSTRAINT PK_PEDIDO PRIMARY KEY(NUMERO_PEDIDO),
CONSTRAINT FK_PEDIDO_EMPLEADO FOREIGN KEY(RUT_EMPLEADO) REFERENCES EMPLEADO(RUT_EMPLEADO),
CONSTRAINT FK_PEDIDO_PROVEEDOR FOREIGN KEY(RUT_PROVEEDOR) REFERENCES PROVEEDOR(RUT_PROVEEDOR),
CONSTRAINT FK_PEDIDO_RECEPCION FOREIGN KEY(NUMERO_RECEPCION) REFERENCES RECEPCION(NUMERO_RECEPCION)
);

CREATE TABLE DETALLE_PEDIDO(
ID_DETALLE_PEDIDO NUMBER(5),
CANTIDAD NUMBER(10) NOT NULL,
NUMERO_PEDIDO NUMBER(5),
ID_PRODUCTO NUMBER(5) NOT NULL,
CONSTRAINT PK_DETALLE_PEDIDO PRIMARY KEY(ID_DETALLE_PEDIDO, ID_PRODUCTO),
CONSTRAINT FK_DETALLE_PEDIDO_PEDIDO FOREIGN KEY(NUMERO_PEDIDO) REFERENCES PEDIDO(NUMERO_PEDIDO),
CONSTRAINT FK_DETALLE_PEDIDO_PRODUCTO FOREIGN KEY(ID_PRODUCTO) REFERENCES PRODUCTO(ID_PRODUCTO)
);


CREATE TABLE FACTURA(
ID_FACTURA NUMBER(10),
VALOR_NETO_FACTURA NUMBER(15) NOT NULL,
VALOR_IVA_FACTURA NUMBER(15) NOT NULL,
VALOR_DESC_FACTURA NUMBER,
VALOR_TOTAL_FACTURA NUMBER(15) NOT NULL,
FECHA_EMISION_FACTURA DATE NOT NULL,
RUT_CLIENTE NUMBER(8) NOT NULL,
RUT_EMPLEADO NUMBER(8) NOT NULL,
CONSTRAINT PK_FACTURA PRIMARY KEY(ID_FACTURA),
CONSTRAINT FK_FACTURA_CLIENTE FOREIGN KEY(RUT_CLIENTE) REFERENCES CLIENTE(RUT_CLIENTE),
CONSTRAINT FK_FACTURA_EMPLEADO FOREIGN KEY(RUT_EMPLEADO) REFERENCES EMPLEADO(RUT_EMPLEADO)
);

CREATE TABLE DETALLE_FACTURA(
ID_DETALLE_FACTURA NUMBER(10),
DESCRIPCION_DETALLE VARCHAR2(250) NOT NULL,
CANTIDAD NUMBER(10) NOT NULL,
VALOR_TOTAL NUMBER(15) NOT NULL,
ID_FACTURA NUMBER(10) NOT NULL,
RUT_CLIENTE NUMBER(8) NOT NULL,
CONSTRAINT PK_DETALLE_FACTURA PRIMARY KEY(ID_DETALLE_FACTURA),
CONSTRAINT FK_DETALLE_FACTURA_FACTURA FOREIGN KEY(ID_FACTURA) REFERENCES FACTURA(ID_FACTURA),
CONSTRAINT FK_DETALLE_FACTURA_CLIENTE FOREIGN KEY(RUT_CLIENTE) REFERENCES CLIENTE(RUT_CLIENTE)
);

-- Columna Nombre a la tabla cliente
ALTER TABLE CLIENTE ADD (NOMBRE_CLIENTE VARCHAR2(100) DEFAULT 'NO AGREGADO' NOT NULL);

--Constraint para agregar UNIQUE a las tablas
ALTER TABLE USUARIO
ADD UNIQUE (NOMBRE_USUARIO); 

--Creación de secuencias

CREATE SEQUENCE seq_usuario
MINVALUE 1
START WITH 2
INCREMENT BY 1;

--Inserción de Usuario Administrador

INSERT INTO USUARIO values (1, 'Admin', 'admin', 'Administrador', 'Habilitado');
INSERT INTO USUARIO values (null, 'Cliente', 'cliente', 'Cliente', 'Habilitado');
INSERT INTO USUARIO values (null, 'Proveedor', 'proveedor', 'Proveedor', 'Habilitado');
INSERT INTO USUARIO values (null, 'Empleado', 'empleado', 'Empleado', 'Habilitado');
INSERT INTO USUARIO values (null, 'AdminD', 'admin', 'Administrador', 'Deshabilitado');
INSERT INTO USUARIO values (null, 'Prueba', 'prueba', 'Prueba', 'Habilitado');

--Inserción de datos dirección

INSERT INTO PAIS VALUES (1, 'Chile');

INSERT INTO REGION VALUES(1, 'Región Metropolitana', 1);

INSERT INTO COMUNA VALUES(1, 'San Miguel',1);
INSERT INTO COMUNA VALUES(2, 'San Joaquín',1);
INSERT INTO COMUNA VALUES(3, 'Macul',1);
INSERT INTO COMUNA VALUES(4, 'Peñalolén',1);

--Inserción de datos Tipo PROVEEDOR

INSERT INTO TIPO_PROVEEDOR VALUES(1, 'Bebestibles');
INSERT INTO TIPO_PROVEEDOR VALUES(2, 'Verduras');
INSERT INTO TIPO_PROVEEDOR VALUES(3, 'Dulces');
INSERT INTO TIPO_PROVEEDOR VALUES(4, 'Alimentos');

--Inserción de datos Tipo Habitación
INSERT INTO TIPO_HABITACION VALUES(1, 'Simple Común', 1);
INSERT INTO TIPO_HABITACION VALUES(2, 'Doble Común', 2);
INSERT INTO TIPO_HABITACION VALUES(3, 'Triple Común', 3);
INSERT INTO TIPO_HABITACION VALUES(4, 'Simple Ejecutivo', 1);
INSERT INTO TIPO_HABITACION VALUES(5, 'Doble Ejecutivo', 2);
INSERT INTO TIPO_HABITACION VALUES(6, 'Triple Ejecutivo', 3);
INSERT INTO TIPO_HABITACION VALUES(7, 'Simple VIP', 1);
INSERT INTO TIPO_HABITACION VALUES(8, 'Doble VIP', 2);
INSERT INTO TIPO_HABITACION VALUES(9, 'Triple VIP', 3);

--Inserción de datos Tipo Plato
INSERT INTO TIPO_PLATO VALUES (1, 'Hot Dog');
INSERT INTO TIPO_PLATO VALUES (2, 'Empanada');
INSERT INTO TIPO_PLATO VALUES (3, 'Lasagna');

--Creación de TRIGGER

create or replace TRIGGER TGR_USUARIO
BEFORE INSERT ON USUARIO
FOR EACH ROW
 WHEN (new.ID_USUARIO IS NULL or new.ID_USUARIO = 0)
BEGIN
  SELECT SEQ_USUARIO.NEXTVAL 
  INTO :new.ID_USUARIO
  FROM dual;
END;

CREATE OR REPLACE TRIGGER TGR_CLIENTE
BEFORE INSERT ON CLIENTE
FOR EACH ROW
 WHEN (new.ID_USUARIO IS NULL or new.ID_USUARIO = 0) 
BEGIN
  SELECT SEQ_USUARIO.CURRVAL 
  INTO :new.ID_USUARIO
  FROM dual;
END;

CREATE OR REPLACE TRIGGER TGR_EMPLEADO
BEFORE INSERT ON EMPLEADO
FOR EACH ROW
 WHEN (new.ID_USUARIO IS NULL or new.ID_USUARIO = 0) 
BEGIN
  SELECT SEQ_USUARIO.CURRVAL 
  INTO :new.ID_USUARIO
  FROM dual;
END;

CREATE OR REPLACE TRIGGER TGR_PROVEEDOR
BEFORE INSERT ON PROVEEDOR
FOR EACH ROW
 WHEN (new.ID_USUARIO IS NULL or new.ID_USUARIO = 0) 
BEGIN
  SELECT SEQ_USUARIO.CURRVAL 
  INTO :new.ID_USUARIO
  FROM dual;
END;

--Alterando Contraseña Usuario
ALTER TABLE USUARIO
MODIFY PASSWORD VARCHAR2(255);

--Encriptando Contraseñas de Usuarios básicos
--Contraseña: admin
UPDATE USUARIO
SET PASSWORD='$2a$12$i4fY7wI7DtcJRVeHOitdn.0nuEebwCfoqNtx49sBIxuzXNYQUujIS'
WHERE nombre_usuario = 'Admin';

--Contraseña: empleado
UPDATE USUARIO
SET PASSWORD='$2a$12$7RNSh5xuIFf6z1ansi6aTeoYQQJXuO.2mg7zQrzWDYvdu.OH2lyd2'
WHERE nombre_usuario = 'Empleado';

--Contraseña: cliente 
UPDATE USUARIO
SET PASSWORD='$2a$12$iJ28fJuzmeSvTcLG2sJ1WOrSYogWPQF1yw5x6xgJnnJ.DukHZUhpi'
WHERE nombre_usuario = 'Cliente';

--Contraseña: Proveedor
UPDATE USUARIO
SET PASSWORD='$2a$12$gwfSuMQjh6onOVXyH7qjsuDAjpCXt527EI.EwbetNnSt4.Ey6safu'
WHERE nombre_usuario = 'Proveedor';

--Creacion tabla Detalle Orden
CREATE TABLE DETALLE_ORDEN (
ID_DETALLE NUMBER(10) CONSTRAINT PK_DETALLE_ORDEN PRIMARY KEY,
NUMERO_ORDEN NUMBER(5) NOT NULL, 
RUT_HUESPED NUMBER(8) NOT NULL,
CONSTRAINT FK_ORDEN_DETALLE_ORDEN FOREIGN KEY(NUMERO_ORDEN) REFERENCES ORDEN_COMPRA(NUMERO_ORDEN),
CONSTRAINT FK_HUESPED_DETALLE_ORDEN FOREIGN KEY(RUT_HUESPED) REFERENCES HUESPED(RUT_HUESPED)
);