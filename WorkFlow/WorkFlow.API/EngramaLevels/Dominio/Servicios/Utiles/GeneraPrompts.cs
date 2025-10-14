using System.Text;
using System.Text.Json;

using WorkFlow.Share.Objetos.Planes;
using WorkFlow.Share.PostClass.Proceso;

namespace WorkFlow.API.EngramaLevels.Dominio.Servicios.Utiles
{
	public static class GeneraPrompts
	{


		public static string FuncialidadPrompt(PostConversacion postModel, Proyecto proyecto, PlanTrabajo plan)
		{

			var prompt = new StringBuilder();

			prompt.AppendLine(@" Eres un arquitecto de software experto en aplicaciones web con .NET, Clean Architecture, SQL Server y Blazor/MudBlazor. 
				Tu tarea es ayudar en el desarrollo de la aplicación implementando las mejores practicas con patrones de diseño y principios SOLID.");


			prompt.AppendLine($"La aplicación tiene por nombre :  {proyecto.nvchNombre}");
			prompt.AppendLine($"La cual consiste en lo siguiente :  {proyecto.nvchDescripcion}");

			prompt.AppendLine($"En el siguiente Json te envió todos los módulos y funcionalidades de la aplicación");

			prompt.AppendLine($"");
			var json = JsonSerializer.Serialize(plan);
			prompt.AppendLine(json);

			prompt.AppendLine($"");

			prompt.AppendLine($"El proceso fue separado en Fases, y pasos.");

			var modulo = proyecto.fases.SingleOrDefault(e => e.iIdFase == postModel.iIdFase);
			prompt.AppendLine($"Justo se esta trabajando en el siguiente fase {modulo.nvchTitulo}, con propósito {modulo.nvchDescripcion}.");

			prompt.AppendLine($"Esta fase consta de varios pasos, los cuales se describen a continuación");

			foreach (var paso in modulo.pasos)
			{
				prompt.AppendLine($"Paso en secuencia numero: {paso.smNumeroSecuencia}.");
				prompt.AppendLine($"Descripción: {paso.nvchDescripcion}.");
				prompt.AppendLine($"Propósito: {paso.nvchProposito}.");
				prompt.AppendLine($"Características: {paso.nvchCaracteristicas}.");
				prompt.AppendLine($"Enfoque: {paso.nvchEnfoque}.");

			}




			return prompt.ToString();

		}


		public static string GeneraModulosPrompt()
		{

			// System Prompt simplificado para reducir riesgo de filtrado
			var systemPrompt = @"
				Eres un arquitecto de software experto en aplicaciones web con .NET, Clean Architecture, SQL Server y Blazor/MudBlazor.
				Tu tarea es desglosar aplicaciones en módulos funcionales según Domain-Driven Design (DDD) y principios SOLID.

				Debes de agregar 


				Reglas:
				- Identifica módulos lógicos basados en el propósito de la app.
				- Cada módulo debe tener:
				  - ""vchTitulo"": Nombre claro del módulo (en español).
				  - ""nvchProposito"": Propósito a detalle del módulo, explicando su rol en la app, describiendo a detalle el funcionamiento y el propósito de su creación.
				  - ""Funcionalidades"": Lista de funcionalidades, cada una con:
					- Descripción de la funcionalidad.
					- Entidades involucradas (e.g., User, Order).
					- Interacciones con otros módulos.
					- Detalles técnicos: Backend (.NET), DB (SQL Server), Frontend (MudBlazor).
					- Seguridad, escalabilidad, pruebas.
				- Genera salida en JSON válido con clave ""modules"".
				- Evita contenido sensible o complejo que pueda activar filtros de contenido.

				Ejemplo:
				{
				  ""LstModulos"": [
					{
					  ""vchTitulo"": ""Gestión de Usuarios"",
					  ""nvchProposito"": ""Administra usuarios según Clean Architecture. 
					  Este modulo mostrar una tabla con los usuario y un boton agregar nuevo, al agrega nuevo se visualizara el formulario... etc."",
					  ""LstFuncionalidades"": [
						{
						  ""nvchDescripcion"": ""Crear usuario."",
						  ""nvchEntidades"": ""User, Roles"",
						  ""nvchInteracciones"": ""Consulta Módulo de Autenticación."",
						  ""nvchTecnico"": ""Backend: Mediator; DB: Tabla Users; Frontend: MudForm."",
						  ""nvchConsideraciones"": ""Usar JWT; pruebas unitarias.""
						}
					  ]
					}
				  ]
				}";

			return systemPrompt.ToString();

		}

		public static string CreateFasesPrompt(string? json)
		{

			var promot = new StringBuilder();

			promot.AppendLine(@"
			Voy a crear un proyecto con las siguientes especificaciones, módulos y funcionalidades [aquí insertaré las specs detalladas del proyecto].
			Necesito que me des un plan de trabajo secuencial dividido en fases, donde se describa desde la creación inicial del proyecto hasta las pruebas finales.
			El plan debe avanzar fase por fase, idealmente asignando cada fase a un módulo o funcionalidad principal para construir la aplicación de forma incremental.
			En cada fase, describe secuencialmente la creación de entidades (como modelos de datos), clases (frontend y backend), diseños UX/UI (wireframes, mockups, flujos de usuario),
			clases de backend, servicios (APIs, lógica de negocio), interfaces (UI components, endpoints), y cualquier otro elemento relevante. Todo debe estar bien documentado en orden 
			secuencial para que pueda implementarse paso a paso.
			Para cada paso dentro de una fase, incluye:

			Descripción del paso: Qué se va a crear o hacer en este paso.
			Propósito: Para qué sirve este paso en el contexto del proyecto.
			Características clave: Las principales features o requisitos que debe cumplir.
			Cómo se realizará: Una explicación teórica de los enfoques, herramientas o mejores prácticas recomendadas (sin código, solo conceptos).
			Entidades/clases a crear: Lista específica de entidades de datos, clases, componentes UX/UI, servicios o interfaces que se generarán en este paso.

			El plan completo debe ser solo teórico y en texto plano, sin ningún código, diagramas o elementos visuales. 
			Proporciona todas las fases necesarias para completar el proyecto de principio a fin.
			Posteriormente, te enviaré cada paso individual del plan para que generes el código y el desarrollo correspondiente, 
			por lo que cada descripción debe ser lo suficientemente detallada y autónoma para guiar la implementación aislada.
			
			Dame la respueseta en formato json de la siguiete manera
			{{
  ""$schema"": ""http://json-schema.org/draft-07/schema#"",
  ""title"": ""Project Plan Schema"",
  ""type"": ""object"",
  ""properties"": {{
    ""proyectos"": {{
      ""type"": ""array"",
      ""items"": {{
        ""type"": ""object"",
        ""properties"": {{
          ""iIdProyecto"": {{ ""type"": ""integer"", ""minimum"": 1 }},
          ""nvchNombre"": {{ ""type"": ""string"", ""maxLength"": 255 }},
          ""nvchDescripcion"": {{ ""type"": ""string"" }},
          ""dtCreadoEn"": {{ ""type"": ""string"", ""format"": ""date-time"" }},
          ""dtActualizadoEn"": {{ ""type"": ""string"", ""format"": ""date-time"" }},
          ""fases"": {{
            ""type"": ""array"",
            ""items"": {{
              ""type"": ""object"",
              ""properties"": {{
                ""iIdFase"": {{ ""type"": ""integer"", ""minimum"": 1 }},
                ""iIdProyecto"": {{ ""type"": ""integer"", ""minimum"": 1 }},
                ""smNumeroSecuencia"": {{ ""type"": ""integer"", ""minimum"": 0, ""maximum"": 32767 }},
                ""nvchTitulo"": {{ ""type"": ""string"", ""maxLength"": 255 }},
                ""nvchDescripcion"": {{ ""type"": ""string"" }},
                ""dtCreadoEn"": {{ ""type"": ""string"", ""format"": ""date-time"" }},
                ""dtActualizadoEn"": {{ ""type"": ""string"", ""format"": ""date-time"" }},
                ""pasos"": {{
                  ""type"": ""array"",
                  ""items"": {{
                    ""type"": ""object"",
                    ""properties"": {{
                      ""iIdPaso"": {{ ""type"": ""integer"", ""minimum"": 1 }},
                      ""iIdFase"": {{ ""type"": ""integer"", ""minimum"": 1 }},
                      ""smNumeroSecuencia"": {{ ""type"": ""integer"", ""minimum"": 0, ""maximum"": 32767 }},
                      ""nvchDescripcion"": {{ ""type"": ""string"" }},
                      ""nvchProposito"": {{ ""type"": ""string"" }},
                      ""nvchCaracteristicas"": {{ ""type"": ""string"" }},
                      ""nvchEnfoque"": {{ ""type"": ""string"" }},
                      ""dtCreadoEn"": {{ ""type"": ""string"", ""format"": ""date-time"" }},
                      ""dtActualizadoEn"": {{ ""type"": ""string"", ""format"": ""date-time"" }}
                    }},
                    ""required"": [""iIdPaso"", ""iIdFase"", ""smNumeroSecuencia"", ""nvchDescripcion"", ""nvchProposito"", ""nvchCaracteristicas"", ""nvchEnfoque"", ""dtCreadoEn"", ""dtActualizadoEn""]
                  }}
                }}
              }},
              ""required"": [""iIdFase"", ""iIdProyecto"", ""smNumeroSecuencia"", ""nvchTitulo"", ""nvchDescripcion"", ""dtCreadoEn"", ""dtActualizadoEn""]
            }}
          }}
        }},
        ""required"": [""iIdProyecto"", ""nvchNombre"", ""nvchDescripcion"", ""dtCreadoEn"", ""dtActualizadoEn""]
      }}
    }}
  }},

  Json de Ejemplo:

  ""required"": [""proyectos""],
  ""example"": {{
    ""proyectos"": [
      {{
        ""iIdProyecto"": 1,
        ""nvchNombre"": ""Plataforma de Comercio Electrónico"",
        ""nvchDescripcion"": ""Sistema para gestionar productos, usuarios y pedidos en línea."",
        ""dtCreadoEn"": ""2025-10-03T13:15:00Z"",
        ""dtActualizadoEn"": ""2025-10-03T13:15:00Z"",
        ""fases"": [
          {{
            ""iIdFase"": 1,
            ""iIdProyecto"": 1,
            ""smNumeroSecuencia"": 1,
            ""nvchTitulo"": ""Configuración de la Base de Datos"",
            ""nvchDescripcion"": ""Diseñar e implementar el esquema de la base de datos para el sistema."",
            ""dtCreadoEn"": ""2025-10-03T13:15:00Z"",
            ""dtActualizadoEn"": ""2025-10-03T13:15:00Z"",
            ""pasos"": [
              {{
                ""iIdPaso"": 1,
                ""iIdFase"": 1,
                ""smNumeroSecuencia"": 1,
                ""nvchDescripcion"": ""Definir el modelo de datos para usuarios y productos."",
                ""nvchProposito"": ""Establecer las entidades principales para gestionar la información del sistema."",
                ""nvchCaracteristicas"": ""Debe incluir entidades para usuarios, productos y pedidos, con soporte para relaciones."",
                ""nvchEnfoque"": ""Utilizar un modelo relacional con normalización para evitar redundancia."",
                ""dtCreadoEn"": ""2025-10-03T13:15:00Z"",
                ""dtActualizadoEn"": ""2025-10-03T13:15:00Z""
              }},
              {{
                ""iIdPaso"": 2,
                ""iIdFase"": 1,
                ""smNumeroSecuencia"": 2,
                ""nvchDescripcion"": ""Crear las tablas en SQL Server."",
                ""nvchProposito"": ""Implementar el esquema de base de datos en SQL Server."",
                ""nvchCaracteristicas"": ""Tablas con claves primarias, foráneas y restricciones de integridad."",
                ""nvchEnfoque"": ""Usar scripts SQL para crear tablas con índices y restricciones."",
                ""dtCreadoEn"": ""2025-10-03T13:15:00Z"",
                ""dtActualizadoEn"": ""2025-10-03T13:15:00Z""
              }}
            ]
          }}
        ]
      }}
    ]
  }}
}}

			El proyecto esta representado en el siguiente Json: 
			" + json
			);

			return promot.ToString();

		}

		public static string GenericPromptSystem()
		{

			var systemPrompt = @"
				Eres un arquitecto de software experto en aplicaciones web con .NET, Clean Architecture, SQL Server y Blazor/MudBlazor.
				Tu tarea es desglosar aplicaciones en módulos funcionales según Domain-Driven Design (DDD) y principios SOLID.
				";

			return systemPrompt;
		}
	}
}
