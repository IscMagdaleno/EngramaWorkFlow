-- Tabla: Usuarios
-- Propósito: Almacenar usuarios con roles para autenticación y permisos
CREATE TABLE UsuariosWF (
    iUsuarioID INT PRIMARY KEY IDENTITY(1,1),
    nvchNombre NVARCHAR(100) NOT NULL,
    nvchEmail NVARCHAR(100) UNIQUE NOT NULL,
    nvchContrasenaHash NVARCHAR(256) NOT NULL, -- Hash seguro de la contraseña
    nvchRol NVARCHAR(20) NOT NULL CHECK (nvchRol IN ('Administrador', 'Desarrollador')),
    dtFechaCreacion DATETIME DEFAULT GETDATE(),
    bActivo BIT DEFAULT 1
);
CREATE INDEX IX_UsuariosWF_nvchEmail ON UsuariosWF(nvchEmail);

-- Tabla: Planes
-- Propósito: Guardar planes de trabajo generados por el LLM
CREATE TABLE Planes (
    iPlanID INT PRIMARY KEY IDENTITY(1,1),
    iUsuarioID INT NOT NULL FOREIGN KEY REFERENCES UsuariosWF(iUsuarioID),
    nvchTitulo NVARCHAR(200) NOT NULL, -- Ej: "Plan para app de inventario"
    nvchDescripcion NVARCHAR(1000), -- Resumen del plan
    dtFechaCreacion DATETIME DEFAULT GETDATE(),
    nvchEstado NVARCHAR(20) DEFAULT 'Pendiente' CHECK (nvchEstado IN ('Pendiente', 'EnProgreso', 'Completado'))
);
CREATE INDEX IX_Planes_iUsuarioID ON Planes(iUsuarioID);
CREATE INDEX IX_Planes_nvchTitulo ON Planes(nvchTitulo);

-- Tabla: Respuestas
-- Propósito: Almacenar respuestas a las 10 preguntas iniciales
CREATE TABLE Respuestas (
    iRespuestaID INT PRIMARY KEY IDENTITY(1,1),
    iPlanID INT NOT NULL FOREIGN KEY REFERENCES Planes(iPlanID),
    nvchPregunta NVARCHAR(500) NOT NULL, -- Ej: "¿Cuál es la descripción general?"
    nvchRespuesta NVARCHAR(MAX) NOT NULL, -- Respuesta detallada
    dtFechaCreacion DATETIME DEFAULT GETDATE()
);
CREATE INDEX IX_Respuestas_iPlanID ON Respuestas(iPlanID);

-- Tabla: Fases
-- Propósito: Detallar fases de cada plan generadas por el LLM
CREATE TABLE Fases (
    iFaseID INT PRIMARY KEY IDENTITY(1,1),
    iPlanID INT NOT NULL FOREIGN KEY REFERENCES Planes(iPlanID),
    iNumeroFase INT NOT NULL, -- Orden de la fase (1, 2, 3...)
    nvchTitulo NVARCHAR(200) NOT NULL, -- Ej: "Diseñar base de datos"
    nvchDescripcion NVARCHAR(MAX) NOT NULL, -- Detalles de la fase
    nvchEstado NVARCHAR(20) DEFAULT 'Pendiente' CHECK (nvchEstado IN ('Pendiente', 'EnProgreso', 'Completada')),
    dtFechaCreacion DATETIME DEFAULT GETDATE(),
    dtFechaCompletada DATETIME
);
CREATE INDEX IX_Fases_iPlanID ON Fases(iPlanID);
CREATE INDEX IX_Fases_nvchEstado ON Fases(nvchEstado);

-- Tabla: MensajesChat
-- Propósito: Registrar interacciones con el LLM para cada fase
CREATE TABLE MensajesChat (
    iMensajeID INT PRIMARY KEY IDENTITY(1,1),
    iFaseID INT NOT NULL FOREIGN KEY REFERENCES Fases(iFaseID),
    nvchContenidoEnviado NVARCHAR(MAX) NOT NULL, -- Mensaje enviado al LLM
    nvchRespuestaLLM NVARCHAR(MAX), -- Respuesta del LLM (código/comentarios)
    dtFechaEnvio DATETIME DEFAULT GETDATE()
);
CREATE INDEX IX_MensajesChat_iFaseID ON MensajesChat(iFaseID);
