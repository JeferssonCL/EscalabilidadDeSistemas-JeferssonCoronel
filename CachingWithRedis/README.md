# Caching con Redis: Configuración y Ejemplo Práctico

-----

**Nombre:** Jefersson Coronel Lavadenz

**Materia:** Escalabilidad de Sistemas

**Trainer:** Gustavo Rodriguez

-----

## Introducción a Redis y su Aplicación en Caching

---

### ¿Qué es Redis y para qué se usa?
El caching con Redis es una técnica utilizada para almacenar en memoria datos frecuentemente accedidos con el fin de 
mejorar la velocidad y eficiencia de las aplicaciones. Redis es una base de datos en memoria altamente rápida y escalable 
que se usa como caché para reducir la latencia de las consultas y mejorar el rendimiento de los sistemas.

La configuración del caché con Redis implica definir parámetros clave como la persistencia de los datos, 
la política de expiración, la cantidad de memoria utilizada, y la forma en que se invalidan o actualizan los datos almacenados.


### ¿Para qué se usa?
Se utiliza para:

- Reducir la carga en la base de datos al almacenar respuestas a consultas frecuentes.
- Mejorar la velocidad de respuesta de las aplicaciones, ya que los datos almacenados en memoria son más rápidos de acceder que en una base de datos tradicional.
- Optimizar la escalabilidad de aplicaciones web y APIs, evitando accesos innecesarios a la base de datos.
- Manejar sesiones de usuario y otros datos temporales de manera eficiente.


### Características principales

1. **Almacenamiento en memoria:** Redis guarda los datos en RAM, lo que permite acceso ultrarrápido.
2. **Soporte para estructuras de datos:** Permite almacenar strings, hashes, listas, sets y más.
3. **Persistencia opcional:** Se puede configurar para almacenar datos en disco si es necesario.
4. **Expiración de claves:** Se pueden establecer tiempos de vida (TTL) para que los datos expiren automáticamente.
5. **Estrategias de eliminación:** Redis permite definir políticas como LRU (Least Recently Used) para liberar espacio en memoria cuando es necesario.
6. **Alta disponibilidad:** Compatible con replicación y clustering para mejorar la tolerancia a fallos.
7. **Compatibilidad con múltiples lenguajes:** Se puede usar con Python, C#, Java, Node.js, entre otros.


### **Beneficios de aplicarlo**
✅ **Rendimiento mejorado:** Reduce los tiempos de respuesta al servir datos desde la memoria en lugar de la base de datos.  
✅ **Menos carga en la base de datos:** Reduce el número de consultas a bases de datos relacionales o NoSQL.  
✅ **Escalabilidad:** Permite manejar grandes volúmenes de tráfico sin afectar la eficiencia.  
✅ **Flexibilidad:** Puede ser configurado para distintas necesidades según el caso de uso.  
✅ **Optimización de costos:** Al reducir la cantidad de acceso a bases de datos en la nube, se pueden disminuir costos operativos.

<br>

## Laboratorio

---

En este laboratorio, hemos creado una solución para manejar productos utilizando un servicio de backend que interactúa con bases de datos PostgreSQL y Redis. Se desarrollaron varias partes del sistema, incluyendo controladores, servicios de caché y servicios de productos, todo bajo el marco de ASP.NET Core. Además, el sistema se optimizó para realizar operaciones de lectura y escritura de datos de manera eficiente, aprovechando la caché en memoria proporcionada por Redis.


### Caso de Uso: Mejora del Rendimiento en una API de Productos
Tenemos una API en .NET que maneja productos, esta api usa Arquitectura basada en 3 capas. Cada vez que un usuario solicita la lista de productos o un producto en 
especifico, la API consulta la base de datos, lo que puede ser **costoso en términos de tiempo y rendimiento**.
Para evitar llamadas repetitivas a la base de datos, usaremos Redis como **caché**. Redis almacenará la lista de productos 
en memoria y la API la servirá desde el caché si está disponible, en lugar de volver a consultar la base de datos.

#### 1. **Controladores de Producto (ProductsController)**

El **`ProductsController`** proporciona las rutas necesarias para manejar las operaciones CRUD sobre los productos. El controlador define los siguientes métodos:

#### 2. **Servicio de Caché con Redis (RedisCacheService)**

El **`RedisCacheService`** interactúa con **Redis**, proporcionando métodos para almacenar y recuperar productos de la caché. Los métodos importantes incluyen:

- **`SetAsync`**: Guarda un objeto en Redis, serializándolo como un JSON y estableciendo un tiempo de expiración.

- **`GetAsync`**: Recupera un objeto de Redis. Si el objeto no está presente, devuelve `null`.

- **`RemoveAsync`**: Elimina una clave específica de Redis, lo que es útil para mantener la consistencia entre la caché y la base de datos.

#### 3. **Servicios de Producto (ProductService)**

El **`ProductService`** proporciona la lógica de negocio para manejar las operaciones de los productos, interactuando con tanto la base de datos (a través de un repositorio) como la caché (a través de Redis).

#### 4. **Configuración de la Conexión a PostgreSQL y Redis**

El código de configuración en el **`Program.cs`** se encarga de configurar las conexiones a PostgreSQL y Redis:

- **PostgreSQL**: Se conecta a PostgreSQL utilizando las configuraciones proporcionadas desde las variables de entorno. La conexión se establece usando la cadena de conexión definida en la variable de entorno `POSTGRES_SQL_CONNECTION`.

- **Redis**: Se conecta a Redis mediante `localhost:6379`, lo que es ideal para entornos de desarrollo locales.

#### 5. **Flujo de trabajo y optimización de la caché**

La principal optimización en este sistema es el uso de **caché Redis** para almacenar productos y reducir la carga sobre la base de datos. La lógica sigue un patrón en el que:

1. **Primero se intenta obtener los datos de la caché.**
2. **Si los datos no están en caché**, se consulta la base de datos y los resultados se almacenan en la caché para futuras solicitudes.
3. En el caso de **actualizaciones o eliminaciones**, las entradas correspondientes en la caché se eliminan para evitar que los usuarios vean datos obsoletos.

Esto asegura que las lecturas de productos sean muy rápidas, utilizando la caché, y que las escrituras (agregados, actualizaciones y eliminaciones) se realicen en la base de datos, manteniendo la coherencia entre los datos de ambas fuentes.

### Requisitos Previos

Antes de comenzar, asegúrate de tener lo siguiente instalado:

- **Docker**: Para ejecutar Redis y pgAdmin en contenedores.
- **.NET SDK**: El proyecto está desarrollado usando C# y ASP.NET Core.
- **PostgreSQL**: Se utiliza como base de datos para almacenar los productos.
- **Redis**: Servirá para almacenar en caché los productos y mejorar el tiempo de respuesta de la aplicación.
- **Visual Studio / Rider**: Para editar y ejecutar el código.

---

### Guia de Instalacion

#### Paso 1: Instalar Redis

**En Windows**
Usa [Memurai](https://www.memurai.com/) o Docker:
```bash
docker run --name redis -d -p 6379:6379 redis
```

**En Linux/macOS**
```bash
sudo apt update && sudo apt install redis -y  # Ubuntu/Debian
brew install redis  # macOS
```

Ejecutar Redis:
```bash
redis-server
```

#### Paso 2: Clonar el Repositorio

Clona este repositorio en tu máquina local:

```bash
git clone git@github.com:JeferssonCL/EscalabilidadDeSistemas-JeferssonCoronel.git
cd EscalabilidadDeSistemas-JeferssonCoronel/CachingWithRedis
```

#### Paso 3: Instalar Paquete en .NET
Agrega el paquete de Redis si es que no se agrego:
```bash
dotnet add package StackExchange.Redis
```

---

### Ejecucion
**1. Ejecutar el docker compose y Redis**
```bash
docker compose up
docker run redis
```

**2. Agregar la migracion e inicializar las tablas en la bd**
```bash
cd Backend.Api
dotnet ef migrations add migration1
dotnet ef database update
```

**3. Ejecutar la API**
```bash
dotnet run
```
---

### Pruebas
Para probar la aplicación e interactuar con los endpoints, sigue estos pasos:

#### 1. Accede a los Endpoints de la API:
Puedes probar los endpoints de la API utilizando cualquier cliente REST como **Postman**, **Swagger** o **cURL**. 
A continuación se describen los detalles de cada endpoint:

**- GET Products (Obtener Todos los Productos):**
- **Endpoint:** `GET http://localhost:5287/api/Products`
- **Descripción:** Obtiene una lista de productos.
- **Respuesta:** Devuelve un objeto con 3 datos: Una lista de productos desde la caché si está disponible, de lo contrario, los obtiene desde la base de datos. La ubicación de donde saco los datos. Y por ultimo el tiempo que duro en sacar los datos.

**- GET Product by ID (Obtener un Producto por ID):**
- **Endpoint:** `GET http://localhost:5287/api/Products/{id}`
- **Descripción:** Obtiene un producto específico por su ID.
- **Ejemplo:** `GET http://localhost:5287/api/Products/1`
- **Respuesta:** Devuelve un objeto con 3 datos: el producto por su ID desde la caché si está disponible, de lo contrario, lo obtiene desde la base de datos. La ubicación de donde saco el dato. Y por ultimo el tiempo que duro en sacar el dato.

**- POST Product (Agregar un Nuevo Producto):**
- **Endpoint:** `POST http://localhost:5287/api/Products`
- **Descripción:** Agrega un nuevo producto a la base de datos.
- **Cuerpo de la Solicitud:**
  ```json
  {
    "name": "Nuevo Producto",
    "description": "Descripción del producto",
    "price": 20.5
  }
  ```
- **Respuesta:** Devuelve el producto recién agregado con el estado `201 Created`.

**- PUT Product (Actualizar un Producto Existente):**
- **Endpoint:** `PUT http://localhost:5287/api/Products/{id}`
- **Descripción:** Actualiza un producto con el ID especificado.
- **Ejemplo:** `PUT http://localhost:5287/api/Products/1`
- **Cuerpo de la Solicitud:**
  ```json
  {
    "id": 1,
    "name": "Producto Actualizado",
    "description": "Descripción actualizada",
    "price": 25.0
  }
  ```
- **Respuesta:** Devuelve el producto actualizado.

**- DELETE Product (Eliminar un Producto):**
- **Endpoint:** `DELETE http://localhost:5287/api/Products/{id}`
- **Descripción:** Elimina un producto por su ID.
- **Ejemplo:** `DELETE http://localhost:5287/api/Products/1`
- **Respuesta:** Devuelve el estado `204 No Content` si se elimina con éxito o `404 Not Found` si el producto no existe.

**- DELETE Clear Cache (Limpiar Caché de Productos):**
- **Endpoint:** `DELETE http://localhost:5287/api/dispose/clear-cache`
- **Descripción:** Limpia la caché de productos, forzando que la próxima solicitud obtenga los datos desde la base de datos.
- **Respuesta:** Devuelve un mensaje de confirmación: `"Cache removed"`.

#### 2. Verificar el Caché:
Puedes verificar el comportamiento del caché haciendo una secuencia de solicitudes:

1. **Haz una solicitud `GET` para obtener los productos.** La primera solicitud obtendrá los datos desde la base de datos.
2. **Haz la misma solicitud `GET` nuevamente.** Esta vez devolverá los datos desde la caché, y el tiempo de respuesta debería ser significativamente más rápido.
3. **Agrega un nuevo producto.** Realiza una solicitud `POST` para agregar un producto y luego limpia la caché llamando al endpoint `ClearCache`.
4. **Haz la solicitud `GET` nuevamente** para verificar que la caché se haya limpiado y la base de datos se consulte de nuevo.

#### 3. Monitorear el Rendimiento:
- **`TimeToGet`:** Para cada endpoint, verás cuánto tiempo tomó obtener los datos (`TimeToGet`) en la respuesta, lo que indica si los datos provienen de la caché o de la base de datos.

<br>

## Conclusión

---

Este laboratorio ha demostrado cómo integrar una arquitectura de caché con Redis en una aplicación backend que interactúa con una base de datos PostgreSQL. La implementación optimiza las operaciones de lectura y escritura, asegurando la consistencia y mejorando el rendimiento del sistema. La arquitectura también proporciona una base escalable que puede manejar un gran volumen de solicitudes de manera eficiente.
