# Curso: MongoDB Developer Fundamentals – Fullstack

## Objetivo del Repositorio
Diseñado para llevar el curso llamado "MongoDB Developer Fundamentals – Fullstack".

## Temario

### Módulo 1: MongoDB Developer Fundamentals
- Introducción a bases de datos MongoDB
- Cuándo usar MongoDB
- Accediendo a MongoDB
- Conceptos básicos del desarrollador
- Almacenamiento y recuperación

### Módulo 2: Optimizing Storage and Retrieval
- Introducción a índices y rendimiento
- Cómo se eligen los índices
- Perfil de base de datos
- Usando agregación

### Módulo 3: Design Skills and Advanced Features
- Funcionalidad más allá del almacenamiento
- Los desarrolladores internos deben saberlo
- Mejores prácticas para desarrolladores
- Diseño de esquemas

### Módulo 4: Production-Ready Development
- Replicación
- Sharding
- Seguridad

## Tecnologías Usadas
- Docker y Docker Compose
- MongoDB 7.2
- Kafka
- .NET Core 6

## Explicación del archivo `docker-compose.yml` de las prácticas
Este archivo `docker-compose.yml` configura un ambiente de desarrollo completo para la práctica del curso. Contiene los siguientes servicios:

1. **mongodb-users**:
   - **Imagen**: MongoDB latest
   - **Puertos**: 
     - `27017:27017`
   - **Volúmenes**: 
     - `mongo-data-users:/data/db`

2. **mongodb-movies**:
   - **Imagen**: MongoDB latest
   - **Puertos**: 
     - `27018:27017`
   - **Volúmenes**: 
     - `mongo-data-movies:/data/db`

3. **mongodb-logs**:
   - **Imagen**: MongoDB latest
   - **Puertos**: 
     - `27019:27017`
   - **Volúmenes**: 
     - `mongo-data-logs:/data/db`

4. **zookeeper**:
   - **Imagen**: confluentinc/cp-zookeeper:latest
   - **Puertos**: 
     - `2181:2181`
   - **Variables de entorno**:
     - `ZOOKEEPER_CLIENT_PORT=2181`
     - `ZOOKEEPER_TICK_TIME=2000`

5. **kafka**:
   - **Imagen**: confluentinc/cp-kafka:latest
   - **Puertos**: 
     - `9092:9092`
   - **Variables de entorno**:
     - `KAFKA_ZOOKEEPER_CONNECT=zookeeper:2181`
     - `KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://kafka:9092`
     - `KAFKA_LISTENERS=PLAINTEXT://0.0.0.0:9092`
     - `KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=1`
     - `KAFKA_TRANSACTION_STATE_LOG_REPLICATION_FACTOR=1`
     - `KAFKA_TRANSACTION_STATE_LOG_MIN_ISR=1`
   - **Volúmenes**: 
     - `/var/run/docker.sock:/var/run/docker.sock`
   - **Dependencias**: 
     - `zookeeper`

6. **msusers**:
   - **Imagen**: mcr.microsoft.com/dotnet/sdk:6.0
   - **Puertos**: 
     - `5002:5000`
   - **Volúmenes**: 
     - `./services/msusers:/app`
   - **Directorio de trabajo**: `/app`
   - **Comando**: 
     - `sh -c "while true; do sleep 1000; done"`
   - **Dependencias**: 
     - `kafka`
     - `mongodb-users`

7. **msmovies**:
   - **Imagen**: mcr.microsoft.com/dotnet/sdk:6.0
   - **Puertos**: 
     - `5003:5000`
   - **Volúmenes**: 
     - `./services/msmovies:/app`
   - **Directorio de trabajo**: `/app`
   - **Comando**: 
     - `sh -c "while true; do sleep 1000; done"`
   - **Dependencias**: 
     - `kafka`
     - `mongodb-movies`

8. **mslogs**:
   - **Imagen**: mcr.microsoft.com/dotnet/sdk:6.0
   - **Puertos**: 
     - `5004:5000`
   - **Volúmenes**: 
     - `./services/mslogs:/app`
   - **Directorio de trabajo**: `/app`
   - **Comando**: 
     - `sh -c "while true; do sleep 1000; done"`
   - **Dependencias**: 
     - `kafka`
     - `mongodb-logs`

Ambos servicios están conectados a una red llamada `movie-api-network`.

## Explicación del archivo `docker-compose.yml` de los ejemplos
Este archivo `docker-compose.yml` configura un ambiente de desarrollo completo para los ejemplos del curso. Contiene los siguientes servicios:

1. **mongodb-exercises**:
   - **Imagen**: MongoDB latest
   - **Contenedor**: mongodb-exercises
   - **Puertos**: `27017:27017`
   - **Volúmenes**: `mongo-data-exercises:/data/db`

2. **dotnet-exercises**:
   - **Imagen**: .NET SDK 6.0
   - **Contenedor**: dotnet-exercises
   - **Puertos**: `5000:5000`
   - **Volúmenes**: `./dotnet-exercises:/app`
   - **Directorio de trabajo**: `/app`
   - **Comando**: `sh -c "while true; do sleep 1000; done"`
   - **Dependencias**: `mongodb-exercises`

Ambos servicios están conectados a una red llamada `exercises-network`.

## Levantando los contenedores de las prácticas
Para levantar los contenedores con el entorno de desarrollo, se deben de seguir estos pasos:

1. Asegurarse de que el motor de Docker esté corriendo.
2. Acceder al directorio `practice`.
3. Ejecutar el siguiente comando para levantar los contenedores en segundo plano:
   ```sh
   docker-compose up -d
   ```
4. Lo anterior le indica a Docker que debe ejecutar el contenido del archivo `docker-compose.yml`.

## Levantando los contenedores de los ejemplos
Para levantar los contenedores con el entorno de desarrollo, se deben de seguir estos pasos:

1. Asegurarse de que el motor de Docker esté corriendo.
2. Acceder al directorio `examples`.
3. Ejecutar el siguiente comando para levantar los contenedores en segundo plano:
   ```sh
   docker-compose up -d
   ```
4. Lo anterior le indica a Docker que debe ejecutar el contenido del archivo `docker-compose.yml`.

## Todos los comandos para entrar a cada contenedor

### Comandos para contenedores de las prácticas
```sh
# Acceder al contenedor de Kafka
docker exec -it kafka bash

# Acceder al contenedor de MongoDB users
docker exec -it mongodb-users bash

# Acceder al contenedor de MongoDB movies
docker exec -it mongodb-movies bash

# Acceder al contenedor de MongoDB logs
docker exec -it mongodb-logs bash

# Acceder al contenedor de msusers
docker exec -it msusers bash

# Acceder al contenedor de msmovies
docker exec -it msmovies bash

# Acceder al contenedor de mslogs
docker exec -it mslogs bash
```

### Comandos para contenedores de los ejemplos
```sh
# Acceder al contenedor de MongoDB exercises
docker exec -it mongodb-exercises bash

# Acceder al contenedor de dotnet exercises
docker exec -it dotnet-exercises bash
```

### Comandos para levantar alguna de las aplicaciones anteriores de .net
```sh
#1. Acceder al contenedor a levantar

#2. Ejecutar los siguientes comandos
cd /app
dotnet restore
dotnet build
dotnet run

#3. Acceder a Swagger desde la máquina host (ver el docker-compose.yml para ver el puerto de cada uno)
http://localhost:5001/swagger/index.html
```
