# Command Query Responsibility Segregation (CQRS): practical example [SQL + noSQL]

-----

**Nombre:** Jefersson Coronel Lavadenz

**Materia:** Escalabilidad de Sistemas

**Trainer:** Gustavo Rodriguez

-----

## Introducción a CQRS

---

### ¿Qué es CQRS?
El patrón CQRS (Command Query Responsibility Segregation) es un patrón de diseño arquitectónico que propone separar la 
responsabilidad de la lectura (query) de la responsabilidad de la escritura (command) en una aplicación. Esta separación 
permite optimizar cada una de estas operaciones de manera independiente, lo que puede conducir a un sistema más escalable, 
flexible y fácil de mantener.

### Componentes del Patrón CQRS
- **Command:** Representa una operación de escritura o modificación de datos en el sistema. Los comandos son responsables 
de realizar cambios en el estado del sistema.
- **Query:** Representa una operación de lectura o consulta de datos en el sistema. Las consultas son responsables de 
recuperar datos del sistema sin realizar modificaciones en su estado.
- **Modelo de Datos de Lectura (Read Model):** Es un modelo de datos optimizado para operaciones de lectura y consulta. 
Puede estar diseñado específicamente para satisfacer las necesidades de consulta de la aplicación y puede contener 
datos agregados o precalculados para mejorar el rendimiento de las consultas.
- **Modelo de Datos de Escritura (Write Model):** Es un modelo de datos optimizado para operaciones de escritura y 
modificación. Puede estar diseñado de manera diferente al modelo de datos de lectura y puede optimizarse para operaciones 
de escritura eficientes.

### ¿Para qué se usa?
El patrón CQRS se utiliza para mejorar la escalabilidad, el rendimiento y la flexibilidad de las aplicaciones al dividir el procesamiento de comandos (acciones que modifican el estado del sistema) y consultas (acciones que leen el estado del sistema).

Es particularmente útil en escenarios donde:

  1. Las cargas de lectura y escritura son significativamente diferentes.
  2. Se necesita un alto rendimiento en las consultas o en las escrituras.
  3. Se quiere adaptar la base de datos o los modelos de datos a las necesidades específicas de las operaciones de 
  lectura y escritura.
  4. Es necesario gestionar un sistema con reglas de negocio complejas o actualizaciones frecuentes.


### Características principales
1. **Separación de Concerns**:
    - Los comandos (operaciones de escritura) y las consultas (operaciones de lectura) se gestionan por separado.
    - Cada operación tiene su propio modelo de datos.
2. **Modelos de Lectura y Escritura Diferenciados**:
    - El modelo de escritura se optimiza para las operaciones que modifican el estado (crear, actualizar, eliminar).
    - El modelo de lectura está optimizado para consultas rápidas.
3. **Escalabilidad**:
    - Permite escalar de manera independiente las operaciones de lectura y escritura, lo que es útil cuando una de ellas tiene mayor demanda.
4. **Complejidad Adicional**:
    - Introduce más complejidad en el diseño de la aplicación, ya que requiere manejar dos modelos de datos y las reglas que los conectan.

### **Beneficios de aplicarlo**
1. **Mejora del rendimiento y la escalabilidad**:
    - Al separar las operaciones de lectura y escritura, puedes optimizar cada uno por separado, lo que mejora el rendimiento global y permite escalar ambos de manera independiente.
2. **Optimización de las consultas**:
    - El modelo de lectura se puede optimizar específicamente para consultas rápidas, utilizando técnicas como denormalización, índices personalizados o bases de datos especializadas.
3. **Flexibilidad**:
    - Permite que las consultas y los comandos evolucionen de manera independiente, lo que puede facilitar los cambios en la lógica de negocio sin afectar las operaciones de lectura.
4. **Mejor manejo de las reglas de negocio complejas**:
    - En sistemas donde las reglas de negocio son complejas y evolucionan rápidamente, separar la escritura de la lectura permite que el procesamiento de comandos sea más flexible sin complicar las consultas.
5. **Escalabilidad independiente**:
    - Al separar las consultas y los comandos, puedes escalar cada tipo de operación de manera independiente según sea necesario, lo que puede ser una ventaja en sistemas con demandas muy diferentes en lectura y escritura.
6. **Seguridad y control**:
    - Permite un control más detallado sobre el acceso a los datos. Las consultas pueden estar más restringidas y las escrituras pueden ser más controladas y validadas.

<br>

## Laboratorio

---

En este laboratorio, implementaremos un ejemplo práctico de cómo aplicar el patrón Command Query Responsibility 
Segregation (CQRS) utilizando una arquitectura que emplea SQL (PostgreSQL) para las operaciones de escritura y NoSQL (MongoDB) 
para las operaciones de lectura. A través de este ejercicio, comprenderemos cómo separar las responsabilidades de lectura 
y escritura en un sistema de gestión de productos.

### Caso de Uso: Mejora del Rendimiento en una API de Productos
En este laboratorio, vamos a construir un sistema que gestiona productos utilizando el patrón CQRS. Para ello:

- **Modelo de Lectura (MongoDB):**  `Product` 

    El **modelo de escritura** es utilizado para las operaciones de escritura en PostgreSQL. Este modelo refleja los datos que se almacenan y modifican en la base de datos.

- **Modelo de Escritura (PostgreSQL):** `ProductReadModel`

    El **modelo de lectura** se utiliza para optimizar las consultas en MongoDB. Este modelo hereda de `Product` y agrega un campo adicional `Category` para clasificar los productos como "Luxury" o "Economical" según su precio.
- **Sincronización de Datos:** `DataSyncBackgroundService`

    El servicio `DataSyncBackgroundService` se encarga de realizar la sincronización entre PostgreSQL y MongoDB. Realiza las siguientes acciones:

    1. **Insertar nuevos productos en MongoDB**.
    2. **Actualizar productos existentes en MongoDB**.
    3. **Eliminar productos que ya no existen en PostgreSQL**.

El servicio en segundo plano que sincroniza los productos entre PostgreSQL y MongoDB,
permitiendo que la base de datos de lectura (MongoDB) se mantenga actualizada con los cambios que ocurren en la base de datos de escritura (PostgreSQL).

La sincronización entre ambas bases de datos se realiza cada minuto, asegurando que cualquier cambio en PostgreSQL se 
refleje en MongoDB, y que los productos eliminados en SQL también sean eliminados en MongoDB.

### Requisitos Previos

Antes de comenzar, asegúrate de tener lo siguiente instalado:

- **Docker**: Para ejecutar Redis y pgAdmin en contenedores.
- **PostgreSQL**: Asegúrate de tener una instancia de PostgreSQL en funcionamiento con los datos de productos.
- **MongoDB**: Asegúrate de tener una instancia de MongoDB en funcionamiento para almacenar los productos y sus categorías.
- **.NET Core / ASP.NET Core**: Este servicio está diseñado para ser ejecutado en un entorno .NET Core.
- **Visual Studio / Rider**: Para editar y ejecutar el código.


---

### Guia de Instalacion

#### Paso 1: Clonar el Repositorio

Clona este repositorio en tu máquina local:

```bash
git clone git@github.com:JeferssonCL/EscalabilidadDeSistemas-JeferssonCoronel.git
cd EscalabilidadDeSistemas-JeferssonCoronel/CQRS_with_SQL-No_SQL
```

#### Paso 2: Instalar Paquete en .NET
Si aún no lo has hecho, instala las dependencias necesarias para trabajar con PostgreSQL y MongoDB en tu aplicación.
```bash
dotnet add package Npgsql
dotnet add package MongoDB.Driver
```

---

### Ejecucion
**1. Ejecutar el docker compose**
```bash
docker compose up
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
Este servicio puede ser probado para asegurarse de que la sincronización esté funcionando correctamente entre las bases de datos:

#### 1. **Prueba de Inserción en MongoDB**:

- **Acción**: Inserta un nuevo producto en PostgreSQL.
- **Verificación**: Verifica que el nuevo producto se haya insertado en MongoDB con la categoría correcta (basada en el precio).

#### 2. **Prueba de Actualización en MongoDB**:

- **Acción**: Actualiza un producto en PostgreSQL (por ejemplo, cambia su precio).
- **Verificación**: Verifica que el producto en MongoDB se haya actualizado correctamente.

#### 3. **Prueba de Eliminación en MongoDB**:

- **Acción**: Elimina un producto de PostgreSQL.
- **Verificación**: Verifica que el producto también se haya eliminado de MongoDB.

#### 4. **Prueba de Sincronización Completa**:

- **Acción**: Realiza una serie de inserciones, actualizaciones y eliminaciones en PostgreSQL.
- **Verificación**: Asegúrate de que todos los cambios se reflejan correctamente en MongoDB.

#### 5. **Prueba de Consulta (GET) en MongoDB**:

- **Acción**: Realiza una consulta para recuperar los productos de MongoDB.
- **Verificación**: Asegúrate de que los productos recuperados desde MongoDB son los mismos que los que se han sincronizado 
- desde PostgreSQL, con la categoría correcta asignada y la información actualizada.

Ejemplo:
- Realiza una consulta en MongoDB para obtener todos los productos con un precio superior a 100 (siendo la categoría "Luxury").
- Verifica que los productos recuperados tienen la categoría correcta y que las actualizaciones reflejan lo realizado en PostgreSQL.

<br>

## Conclusión

---

En este laboratorio, se ha implementado un servicio de sincronización entre dos bases de datos: PostgreSQL y MongoDB. 
El servicio tiene como objetivo garantizar que ambas bases de datos estén actualizadas y consistentes, sin importar en 
qué base de datos se realicen las operaciones (inserciones, actualizaciones o eliminaciones). Para lograr esto, se ha 
creado una sincronización periódica utilizando un servicio de fondo que se ejecuta cada minuto, sincronizando los datos entre ambas bases.
