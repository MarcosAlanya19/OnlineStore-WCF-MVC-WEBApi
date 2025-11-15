# Sistema de Tienda Online - Servicios Distribuidos

Sistema completo de gestión de productos implementado con arquitectura de servicios distribuidos utilizando **Web API REST** y **gRPC**, con cliente **ASP.NET Core MVC**.

---

## Tabla de Contenidos

- [Descripción](#descripción)
- [Arquitectura](#arquitectura)
- [Tecnologías](#tecnologías)
- [Requisitos Previos](#requisitos-previos)
- [Instalación](#instalación)
- [Configuración](#configuración)
- [Ejecución](#ejecución)
- [Endpoints](#endpoints)
- [Capturas de Pantalla](#capturas)
- [Estructura del Proyecto](#estructura)
- [Autores](#autores)

---

## Descripción

Este proyecto implementa un sistema completo de gestión de productos para una tienda online, demostrando el uso de dos tecnologías de servicios distribuidos:

- **Web API REST**: Servicio HTTP tradicional con JSON
- **gRPC**: Servicio de alto rendimiento con Protocol Buffers (alternativa moderna a WCF)
- **Cliente MVC**: Aplicación web que consume ambos servicios

El sistema permite realizar operaciones CRUD (Crear, Leer, Actualizar, Eliminar) sobre productos, almacenados en una base de datos SQL Server.

---

## Arquitectura

```
┌─────────────────────────────────────────┐
│      Cliente MVC (Puerto variable)      │
│  - Consume Web API REST (HttpClient)    │
│  - Consume gRPC Service (GrpcChannel)   │
└──────────┬──────────────┬───────────────┘
           │              │
           │              │
    ┌──────▼─────┐  ┌────▼──────┐
    │  Web API   │  │   gRPC    │
    │ REST:5000  │  │  :5123    │
    └──────┬─────┘  └────┬──────┘
           │              │
           └──────┬───────┘
                  │
         ┌────────▼─────────┐
         │   SQL Server     │
         │   (Docker:1433)  │
         │  DBTiendaOnline  │
         └──────────────────┘
```

### Flujo de Datos

1. **Usuario** interactúa con el navegador web
2. **Cliente MVC** procesa la petición
3. **Servicio** (REST o gRPC) recibe la solicitud
4. **Entity Framework Core** traduce a SQL
5. **SQL Server** ejecuta la consulta
6. Los datos retornan por el mismo camino

---

## Tecnologías

### Backend
- **.NET 9.0**: Framework principal
- **ASP.NET Core Web API**: Servicios REST
- **gRPC con Protocol Buffers**: Servicios de alto rendimiento
- **Entity Framework Core 9.0**: ORM

### Frontend
- **ASP.NET Core MVC**: Patrón Modelo-Vista-Controlador
- **Razor Pages**: Motor de vistas
- **Bootstrap 5**: Framework CSS
- **Bootstrap Icons**: Iconografía

### Base de Datos
- **SQL Server 2022**: Base de datos relacional
- **Docker**: Contenedor para SQL Server

### Herramientas
- **Rider / Visual Studio**: IDE
- **Swagger/OpenAPI**: Documentación de API
- **Azure Data Studio**: Gestión de base de datos

---

## ⚙️ Requisitos Previos

Antes de comenzar, asegúrate de tener instalado:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Rider](https://www.jetbrains.com/rider/) o [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [Azure Data Studio](https://azure.microsoft.com/products/data-studio/) (opcional)

### Verificar Instalaciones

```bash
# Verificar .NET
dotnet --version
# Debe mostrar: 9.0.x

# Verificar Docker
docker --version
# Debe mostrar: Docker version 20.x o superior
```

---

## Instalación

### 1. Clonar el Repositorio

```bash
git clone https://github.com/tu-usuario/tienda-online-system.git
cd tienda-online-system
```

### 2. Iniciar SQL Server en Docker

```bash
docker run -e "ACCEPT_EULA=Y" \
  -e "SA_PASSWORD=Root1234" \
  -e "MSSQL_PID=Developer" \
  -p 1433:1433 \
  --name sqlserver \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

**Verificar que el contenedor está corriendo:**

```bash
docker ps
```

### 3. Crear la Base de Datos

Ejecuta el siguiente script SQL en Azure Data Studio o desde línea de comandos:

```bash
# Conectar al contenedor
docker exec -it sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Root1234

# Ejecutar el script (copiar el contenido de Scripts SQL/01-CrearBaseDatos.sql)
```

O importa los archivos SQL desde la carpeta `Scripts SQL/` en orden:

1. `01-CrearBaseDatos.sql`
2. `02-CrearTablas.sql`
3. `03-InsertarDatos.sql`
4. `04-ProcedimientosAlmacenados.sql`
5. `05-Triggers.sql` (opcional)

### 4. Restaurar Paquetes NuGet

```bash
# Restaurar todos los proyectos
dotnet restore
```

### 5. Compilar la Solución

```bash
# Compilar todos los proyectos
dotnet build
```

---

## 🔧 Configuración

### Cadenas de Conexión

Verifica que los archivos `appsettings.json` de cada proyecto tengan la configuración correcta:

#### TiendaOnline.WebAPI/appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=DBTiendaOnline;User Id=sa;Password=Root1234;TrustServerCertificate=True;Encrypt=False;"
  }
}
```

#### TiendaOnline.gRPC/appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=DBTiendaOnline;User Id=sa;Password=Root1234;TrustServerCertificate=True;Encrypt=False;"
  }
}
```

#### TiendaOnline.MVC/appsettings.json

```json
{
  "ApiSettings": {
    "BaseUrl": "http://localhost:5000"
  },
  "GrpcSettings": {
    "BaseUrl": "http://localhost:5123"
  }
}
```

**⚠️ IMPORTANTE**: Ajusta los puertos según tu configuración local.

---

## ▶️ Ejecución

Necesitas ejecutar **3 proyectos simultáneamente**. Abre **3 terminales diferentes**:

### Terminal 1: Web API REST

```bash
cd TiendaOnline.WebAPI
dotnet run
```

Deberías ver:
```
info: Now listening on: http://localhost:5000
info: Now listening on: https://localhost:5001
```

### Terminal 2: Servicio gRPC

```bash
cd TiendaOnline.gRPC
dotnet run
```

Deberías ver:
```
info: Now listening on: http://localhost:5123
```

### Terminal 3: Cliente MVC

```bash
cd TiendaOnline.MVC
dotnet run
```

Deberías ver:
```
info: Now listening on: http://localhost:5XXXX
```

### Acceder a la Aplicación

1. **Cliente MVC**: Abre tu navegador en `http://localhost:XXXX` (el puerto que aparece en la terminal)
2. **Swagger (Web API)**: `http://localhost:5000/swagger`
3. **gRPC**: No tiene interfaz web, se consume desde el cliente MVC

---

## 📡 Endpoints

### Web API REST

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| `GET` | `/api/productos` | Obtiene todos los productos |
| `GET` | `/api/productos/{id}` | Obtiene un producto por ID |
| `POST` | `/api/productos` | Crea un nuevo producto |
| `PUT` | `/api/productos/{id}` | Actualiza un producto existente |
| `DELETE` | `/api/productos/{id}` | Elimina un producto |

### Ejemplo de Request

**POST /api/productos**

```json
{
  "nombre": "Teclado Mecánico",
  "descripcion": "Teclado RGB switches azules",
  "precio": 299.99,
  "stock": 30,
  "categoria": "Accesorios"
}
```

**Response: 201 Created**

```json
{
  "idProducto": 8,
  "nombre": "Teclado Mecánico",
  "descripcion": "Teclado RGB switches azules",
  "precio": 299.99,
  "stock": 30,
  "categoria": "Accesorios"
}
```

### gRPC Service

Definido en `Protos/productos.proto`:

```protobuf
service ProductosService {
  rpc GetProductos (Empty) returns (ProductosResponse);
  rpc GetProductoById (ProductoRequest) returns (ProductoResponse);
  rpc CreateProducto (CreateProductoRequest) returns (ProductoResponse);
  rpc UpdateProducto (UpdateProductoRequest) returns (ProductoResponse);
  rpc DeleteProducto (ProductoRequest) returns (DeleteResponse);
}
```

---

## 📸 Capturas de Pantalla {#capturas}

### 1. Swagger - Documentación del Web API
<img width="1910" height="1080" alt="image" src="https://github.com/user-attachments/assets/7603ffc0-3da9-41fc-9c12-c423b3d09ba1" />

### 2. Lista de Productos (Web API)
<img width="1135" height="638" alt="image" src="https://github.com/user-attachments/assets/74578d22-ca64-44e5-9e9e-e4ce19c540d5" />

### 4. Crear Producto
<img width="1118" height="637" alt="image" src="https://github.com/user-attachments/assets/f84f25a2-4391-4645-8f73-afe6d8a31fd8" />

### 5. Editar Producto
<img width="1125" height="633" alt="image" src="https://github.com/user-attachments/assets/d82b2570-9cb9-44e2-bd10-a79a5bda0461" />

### 6. Detalles del Producto
<img width="1133" height="641" alt="image" src="https://github.com/user-attachments/assets/ce32945a-8ba1-4ced-96fc-96ee83df5b03" />

---

## 📁 Estructura del Proyecto {#estructura}

```
TiendaOnlineSystem/
│
├── TiendaOnline.WebAPI/              # Servicio Web API REST
│   ├── Controllers/
│   │   └── ProductosController.cs    # CRUD endpoints REST
│   ├── Data/
│   │   └── TiendaContext.cs          # DbContext
│   ├── Models/
│   │   └── Producto.cs               # Modelo de datos
│   ├── Program.cs                    # Configuración del API
│   └── appsettings.json              # Configuración
│
├── TiendaOnline.gRPC/                # Servicio gRPC
│   ├── Protos/
│   │   └── productos.proto           # Definición del servicio
│   ├── Services/
│   │   └── ProductosGrpcService.cs   # Implementación gRPC
│   ├── Data/
│   │   └── TiendaContext.cs          # DbContext
│   ├── Models/
│   │   └── Producto.cs               # Modelo de datos
│   ├── Program.cs                    # Configuración gRPC
│   └── appsettings.json              # Configuración
│
├── TiendaOnline.MVC/                 # Cliente Web MVC
│   ├── Controllers/
│   │   ├── ProductosController.cs    # Consume Web API
│   │   └── ProductosGrpcController.cs # Consume gRPC
│   ├── Services/
│   │   ├── IProductoService.cs       # Interfaz REST
│   │   ├── ProductoService.cs        # Implementación REST
│   │   ├── IProductoGrpcService.cs   # Interfaz gRPC
│   │   └── ProductoGrpcService.cs    # Implementación gRPC
│   ├── Models/
│   │   └── Producto.cs               # Modelo de datos
│   ├── Views/
│   │   ├── Productos/
│   │   │   ├── Index.cshtml          # Lista de productos
│   │   │   ├── Create.cshtml         # Crear producto
│   │   │   ├── Edit.cshtml           # Editar producto
│   │   │   ├── Details.cshtml        # Detalles del producto
│   │   │   └── Delete.cshtml         # Confirmar eliminación
│   │   └── Shared/
│   │       └── _Layout.cshtml        # Layout principal
│   ├── Protos/
│   │   └── productos.proto           # Definición del servicio
│   ├── Program.cs                    # Configuración MVC
│   └── appsettings.json              # Configuración
│
├── Scripts SQL/
│   ├── 01-CrearBaseDatos.sql
│   ├── 02-CrearTablas.sql
│   ├── 03-InsertarDatos.sql
│   ├── 04-ProcedimientosAlmacenados.sql
│   └── 05-Triggers.sql
│
├── Documentacion/
│   ├── DocumentoTecnico.pdf
│   ├── AnalisisComparativo.pdf
│   └── Capturas/
│
├── README.md                         # Este archivo
└── TiendaOnlineSystem.sln            # Solución de Visual Studio/Rider
```
