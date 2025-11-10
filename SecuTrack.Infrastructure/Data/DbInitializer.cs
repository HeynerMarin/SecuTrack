using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SecuTrack.Core.Entities;
using SecuTrack.Core.Enums;

namespace SecuTrack.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            // Asegurar que la base de datos esté creada
            await context.Database.EnsureCreatedAsync();

            // Si ya hay datos, no hacer nada
            if (context.ISOControls.Any())
            {
                return;
            }

            // Seed de controles ISO 27002:2022
            await SeedISOControls(context);

            // Seed de proveedores de ejemplo
            await SeedProviders(context);

            await context.SaveChangesAsync();

            await SeedRolesAndUsers(context, serviceProvider);
        }

        private static async Task SeedISOControls(ApplicationDbContext context)
        {
            var controls = new List<ISOControl>
            {
                // ====================================
                // CONTROLES ORGANIZACIONALES (5.x)
                // ====================================
                new ISOControl
                {
                    ControlId = "5.1",
                    Category = ISOCategory.OrganizationalControls,
                    Title = "Políticas de seguridad de la información",
                    Description = "Políticas de seguridad de la información y políticas específicas por tema deben ser definidas, aprobadas por la dirección, publicadas, comunicadas y reconocidas por el personal pertinente y las partes interesadas relevantes, y revisadas a intervalos planificados y si ocurren cambios significativos.",
                    Purpose = "Proporcionar dirección y apoyo de la gestión para la seguridad de la información de acuerdo con los requisitos del negocio y las leyes y regulaciones pertinentes.",
                    Weight = 5,
                    IsCritical = true,
                    CloudAdaptation = "Verificar políticas específicas de seguridad en la nube: clasificación de datos, gestión de acceso cloud, cifrado, y cumplimiento regulatorio."
                },
                new ISOControl
                {
                    ControlId = "5.2",
                    Category = ISOCategory.OrganizationalControls,
                    Title = "Roles y responsabilidades de seguridad de la información",
                    Description = "Los roles y responsabilidades de seguridad de la información deben ser definidos y asignados de acuerdo con las necesidades de la organización.",
                    Purpose = "Asegurar que las responsabilidades de seguridad de la información estén definidas y asignadas.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Definir roles específicos para administración cloud: Cloud Security Architect, Cloud Operations, DevSecOps."
                },
                new ISOControl
                {
                    ControlId = "5.3",
                    Category = ISOCategory.OrganizationalControls,
                    Title = "Segregación de funciones",
                    Description = "Las funciones y áreas de responsabilidad en conflicto deben ser segregadas.",
                    Purpose = "Reducir oportunidades de modificación no autorizada o no intencional, o mal uso de los activos de la organización.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Implementar separación de roles en consolas cloud (IAM roles, grupos de permisos)."
                },
                new ISOControl
                {
                    ControlId = "5.4",
                    Category = ISOCategory.OrganizationalControls,
                    Title = "Responsabilidades de la dirección",
                    Description = "La dirección debe requerir que todo el personal aplique la seguridad de la información de acuerdo con las políticas, procedimientos y temas específicos establecidos de la organización.",
                    Purpose = "Asegurar que el personal conoce y cumple sus responsabilidades de seguridad de la información.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Dirección debe supervisar cumplimiento de políticas cloud y aprobar cambios en arquitectura."
                },
                new ISOControl
                {
                    ControlId = "5.5",
                    Category = ISOCategory.OrganizationalControls,
                    Title = "Contacto con las autoridades",
                    Description = "La organización debe establecer y mantener contacto con las autoridades pertinentes.",
                    Purpose = "Asegurar que la organización tiene un proceso establecido para contactar a las autoridades cuando sea necesario.",
                    Weight = 2,
                    IsCritical = false,
                    CloudAdaptation = "Mantener contacto con equipos de seguridad del proveedor cloud y autoridades de protección de datos."
                },
                new ISOControl
                {
                    ControlId = "5.6",
                    Category = ISOCategory.OrganizationalControls,
                    Title = "Contacto con grupos de interés especial",
                    Description = "La organización debe establecer y mantener contacto con grupos de interés especial u otros foros de seguridad especializados y asociaciones profesionales.",
                    Purpose = "Mantenerse actualizado sobre mejores prácticas y tendencias en seguridad de la información.",
                    Weight = 2,
                    IsCritical = false,
                    CloudAdaptation = "Participar en comunidades de seguridad cloud (AWS Security Hub, Azure Security Center community)."
                },
                new ISOControl
                {
                    ControlId = "5.7",
                    Category = ISOCategory.OrganizationalControls,
                    Title = "Inteligencia de amenazas",
                    Description = "La información relacionada con amenazas de seguridad de la información debe ser recopilada y analizada para producir inteligencia de amenazas.",
                    Purpose = "Proporcionar conciencia de amenazas actuales para apoyar la gestión de riesgos.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Suscribirse a feeds de inteligencia de amenazas específicos de cloud (AWS GuardDuty, Azure Sentinel)."
                },
                new ISOControl
                {
                    ControlId = "5.8",
                    Category = ISOCategory.OrganizationalControls,
                    Title = "Seguridad de la información en la gestión de proyectos",
                    Description = "La seguridad de la información debe ser integrada en la gestión de proyectos.",
                    Purpose = "Asegurar que la seguridad de la información sea parte de los proyectos.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Incluir requisitos de seguridad cloud en proyectos de migración y desarrollo cloud-native."
                },
                new ISOControl
                {
                    ControlId = "5.9",
                    Category = ISOCategory.OrganizationalControls,
                    Title = "Inventario de información y otros activos asociados",
                    Description = "Debe desarrollarse y mantenerse un inventario de información y otros activos asociados, incluyendo los propietarios.",
                    Purpose = "Identificar la información y otros activos asociados para asegurar que se les otorgue un nivel apropiado de protección.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Mantener inventario automatizado de recursos cloud (AWS Config, Azure Resource Graph)."
                },
                new ISOControl
                {
                    ControlId = "5.10",
                    Category = ISOCategory.OrganizationalControls,
                    Title = "Uso aceptable de la información y otros activos asociados",
                    Description = "Deben identificarse, documentarse e implementarse reglas para el uso aceptable de información y otros activos asociados.",
                    Purpose = "Asegurar que la información y otros activos asociados se utilicen solo para propósitos autorizados.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Definir políticas de uso aceptable para servicios cloud, instancias y almacenamiento."
                },
                new ISOControl
                {
                    ControlId = "5.11",
                    Category = ISOCategory.OrganizationalControls,
                    Title = "Devolución de activos",
                    Description = "El personal y las partes externas deben devolver todos los activos de la organización en su posesión al terminar su empleo, contrato o acuerdo.",
                    Purpose = "Proteger los activos de la organización cuando el personal deja la organización.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Revocar accesos cloud y recuperar credenciales al finalizar relaciones laborales."
                },
                new ISOControl
                {
                    ControlId = "5.12",
                    Category = ISOCategory.OrganizationalControls,
                    Title = "Clasificación de la información",
                    Description = "La información debe ser clasificada de acuerdo con las necesidades de seguridad de la información de la organización.",
                    Purpose = "Asegurar que la información recibe un nivel apropiado de protección.",
                    Weight = 5,
                    IsCritical = true,
                    CloudAdaptation = "Implementar etiquetas de clasificación en recursos cloud y buckets S3/Blob Storage."
                },
                new ISOControl
                {
                    ControlId = "5.13",
                    Category = ISOCategory.OrganizationalControls,
                    Title = "Etiquetado de la información",
                    Description = "Debe desarrollarse e implementarse un conjunto apropiado de procedimientos para el etiquetado de información.",
                    Purpose = "Asegurar que la información sea manejada de acuerdo con su clasificación.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Usar tags y metadata de cloud para etiquetar recursos según clasificación."
                },
                new ISOControl
                {
                    ControlId = "5.14",
                    Category = ISOCategory.OrganizationalControls,
                    Title = "Transferencia de información",
                    Description = "Deben establecerse, implementarse y mantenerse reglas, procedimientos o acuerdos de transferencia de información para todos los tipos de instalaciones de transferencia.",
                    Purpose = "Mantener la seguridad de la información transferida dentro de la organización y con cualquier entidad externa.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Cifrar datos en tránsito entre servicios cloud y hacia/desde on-premises (VPN, TLS)."
                },

                // ====================================
                // CONTROLES DE PERSONAS (6.x)
                // ====================================
                new ISOControl
                {
                    ControlId = "6.1",
                    Category = ISOCategory.PeopleControls,
                    Title = "Selección",
                    Description = "Las verificaciones de antecedentes de todos los candidatos a empleo deben llevarse a cabo de acuerdo con las leyes, regulaciones y ética pertinentes.",
                    Purpose = "Asegurar que el personal es confiable y adecuado para los roles que desempeñarán.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Verificar antecedentes de personal con acceso a consolas cloud y datos sensibles."
                },
                new ISOControl
                {
                    ControlId = "6.2",
                    Category = ISOCategory.PeopleControls,
                    Title = "Términos y condiciones de empleo",
                    Description = "Los acuerdos contractuales con personal y contratistas deben establecer sus responsabilidades y las de la organización en materia de seguridad de la información.",
                    Purpose = "Asegurar que el personal y los contratistas son conscientes de sus responsabilidades.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Incluir cláusulas sobre uso de servicios cloud y protección de credenciales."
                },
                new ISOControl
                {
                    ControlId = "6.3",
                    Category = ISOCategory.PeopleControls,
                    Title = "Concienciación, educación y capacitación en seguridad de la información",
                    Description = "El personal de la organización y las partes interesadas pertinentes deben recibir educación, capacitación y concienciación apropiadas sobre seguridad de la información.",
                    Purpose = "Asegurar que el personal es consciente de las amenazas y responsabilidades de seguridad.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Capacitar en seguridad cloud: IAM, cifrado, detección de amenazas, compliance."
                },
                new ISOControl
                {
                    ControlId = "6.4",
                    Category = ISOCategory.PeopleControls,
                    Title = "Proceso disciplinario",
                    Description = "Debe existir un proceso disciplinario formal para el personal que haya cometido una violación de seguridad de la información.",
                    Purpose = "Corregir violaciones de seguridad y prevenir su recurrencia.",
                    Weight = 2,
                    IsCritical = false,
                    CloudAdaptation = "Establecer proceso disciplinario para mal uso de recursos cloud."
                },
                new ISOControl
                {
                    ControlId = "6.5",
                    Category = ISOCategory.PeopleControls,
                    Title = "Responsabilidades después de la terminación o cambio de empleo",
                    Description = "Las responsabilidades y obligaciones de seguridad de la información que permanecen válidas después de la terminación o cambio de empleo deben ser definidas, comunicadas al personal y exigidas.",
                    Purpose = "Proteger los intereses de la organización como parte del proceso de cambio o terminación de empleo.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Deshabilitar inmediatamente accesos cloud al terminar empleo y auditar actividad previa."
                },
                new ISOControl
                {
                    ControlId = "6.6",
                    Category = ISOCategory.PeopleControls,
                    Title = "Acuerdos de confidencialidad o no divulgación",
                    Description = "Los acuerdos de confidencialidad o no divulgación que reflejen las necesidades de la organización para la protección de la información deben ser identificados, documentados, revisados regularmente y firmados por el personal y partes externas.",
                    Purpose = "Proteger la información confidencial.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Incluir protección de credenciales cloud y datos en acuerdos de confidencialidad."
                },
                new ISOControl
                {
                    ControlId = "6.7",
                    Category = ISOCategory.PeopleControls,
                    Title = "Trabajo remoto",
                    Description = "Se deben implementar medidas de seguridad cuando el personal trabaja remotamente para proteger la información.",
                    Purpose = "Asegurar la seguridad de la información cuando se trabaja fuera de las instalaciones de la organización.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Asegurar acceso remoto a recursos cloud mediante VPN, MFA y acceso condicional."
                },
                new ISOControl
                {
                    ControlId = "6.8",
                    Category = ISOCategory.PeopleControls,
                    Title = "Reporte de eventos de seguridad de la información",
                    Description = "La organización debe proporcionar un mecanismo para que el personal reporte eventos de seguridad de la información observados o sospechados.",
                    Purpose = "Asegurar que los eventos de seguridad de la información sean reportados rápidamente.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Implementar alertas automáticas de seguridad cloud y canal de reporte de incidentes."
                },

                // ====================================
                // CONTROLES FÍSICOS (7.x)
                // ====================================
                new ISOControl
                {
                    ControlId = "7.1",
                    Category = ISOCategory.PhysicalControls,
                    Title = "Perímetros de seguridad física",
                    Description = "Los perímetros de seguridad deben ser definidos y utilizados para proteger áreas que contienen información y otros activos asociados.",
                    Purpose = "Prevenir el acceso físico no autorizado, daño e interferencia a las instalaciones de la organización.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Verificar certificaciones de seguridad física de data centers del proveedor cloud (SOC 2, ISO 27001)."
                },
                new ISOControl
                {
                    ControlId = "7.2",
                    Category = ISOCategory.PhysicalControls,
                    Title = "Entrada física",
                    Description = "Las áreas seguras deben ser protegidas por controles de entrada apropiados.",
                    Purpose = "Asegurar que solo personal autorizado acceda a áreas seguras.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Revisar controles de acceso físico en data centers del proveedor (biometría, CCTV)."
                },
                new ISOControl
                {
                    ControlId = "7.3",
                    Category = ISOCategory.PhysicalControls,
                    Title = "Seguridad de oficinas, recintos e instalaciones",
                    Description = "Debe diseñarse e implementarse seguridad física para oficinas, recintos e instalaciones.",
                    Purpose = "Prevenir el acceso físico no autorizado, daño e interferencia.",
                    Weight = 2,
                    IsCritical = false,
                    CloudAdaptation = "Verificar políticas de seguridad física del proveedor para instalaciones críticas."
                },
                new ISOControl
                {
                    ControlId = "7.4",
                    Category = ISOCategory.PhysicalControls,
                    Title = "Monitoreo de seguridad física",
                    Description = "Las instalaciones deben ser monitoreadas continuamente contra acceso físico no autorizado.",
                    Purpose = "Detectar y responder a acceso físico no autorizado.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Confirmar que el proveedor cloud tiene monitoreo 24/7 en data centers."
                },
                new ISOControl
                {
                    ControlId = "7.5",
                    Category = ISOCategory.PhysicalControls,
                    Title = "Protección contra amenazas físicas y ambientales",
                    Description = "La protección contra amenazas físicas y ambientales debe ser diseñada e implementada.",
                    Purpose = "Prevenir daños a instalaciones e información.",
                    Weight = 3,
                    IsCritical = true,
                    CloudAdaptation = "Verificar redundancia geográfica, protección contra desastres naturales en regiones cloud."
                },
                new ISOControl
                {
                    ControlId = "7.6",
                    Category = ISOCategory.PhysicalControls,
                    Title = "Trabajo en áreas seguras",
                    Description = "Las medidas de seguridad para trabajar en áreas seguras deben ser diseñadas e implementadas.",
                    Purpose = "Prevenir acceso no autorizado y daño a información en áreas seguras.",
                    Weight = 2,
                    IsCritical = false,
                    CloudAdaptation = "No aplica directamente; verificar políticas del proveedor cloud."
                },
                new ISOControl
                {
                    ControlId = "7.7",
                    Category = ISOCategory.PhysicalControls,
                    Title = "Escritorio limpio y pantalla limpia",
                    Description = "Se deben definir e implementar apropiadamente reglas de escritorio limpio para papeles y medios de almacenamiento removibles y de pantalla limpia para instalaciones de procesamiento de información.",
                    Purpose = "Reducir el riesgo de acceso no autorizado, pérdida y daño de información.",
                    Weight = 2,
                    IsCritical = false,
                    CloudAdaptation = "Aplicar en oficinas corporativas; en cloud, asegurar que sesiones expiren automáticamente."
                },
                new ISOControl
                {
                    ControlId = "7.8",
                    Category = ISOCategory.PhysicalControls,
                    Title = "Ubicación y protección de equipos",
                    Description = "El equipo debe estar ubicado y protegido para reducir los riesgos de amenazas y peligros ambientales.",
                    Purpose = "Prevenir pérdida, daño, robo o compromiso de activos.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Verificar ubicación geográfica de data centers y certificaciones ambientales del proveedor."
                },
                new ISOControl
                {
                    ControlId = "7.9",
                    Category = ISOCategory.PhysicalControls,
                    Title = "Seguridad de activos fuera de las instalaciones",
                    Description = "Los activos fuera de las instalaciones deben ser protegidos.",
                    Purpose = "Prevenir pérdida, daño, robo o compromiso de activos fuera de las instalaciones.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Cifrar laptops y dispositivos con acceso a consolas cloud; usar VPN."
                },
                new ISOControl
                {
                    ControlId = "7.10",
                    Category = ISOCategory.PhysicalControls,
                    Title = "Medios de almacenamiento",
                    Description = "Los medios de almacenamiento deben ser gestionados durante todo su ciclo de vida.",
                    Purpose = "Prevenir divulgación, modificación, eliminación o destrucción no autorizadas de información.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Gestionar ciclo de vida de volúmenes, snapshots y backups en cloud."
                },
                new ISOControl
                {
                    ControlId = "7.11",
                    Category = ISOCategory.PhysicalControls,
                    Title = "Utilidades de soporte",
                    Description = "Las instalaciones de procesamiento de información deben ser protegidas de fallos de energía y otras interrupciones.",
                    Purpose = "Prevenir pérdida o compromiso de información debido a fallos de utilidades de soporte.",
                    Weight = 3,
                    IsCritical = true,
                    CloudAdaptation = "Verificar SLA de disponibilidad y redundancia de energía en data centers del proveedor."
                },
                new ISOControl
                {
                    ControlId = "7.12",
                    Category = ISOCategory.PhysicalControls,
                    Title = "Seguridad del cableado",
                    Description = "Los cables de energía y telecomunicaciones que transportan datos o soportan servicios de información deben ser protegidos.",
                    Purpose = "Prevenir interceptación, interferencia o daño.",
                    Weight = 2,
                    IsCritical = false,
                    CloudAdaptation = "No aplica directamente; es responsabilidad del proveedor cloud."
                },
                new ISOControl
                {
                    ControlId = "7.13",
                    Category = ISOCategory.PhysicalControls,
                    Title = "Mantenimiento de equipos",
                    Description = "El equipo debe ser mantenido correctamente para asegurar su disponibilidad, integridad y confidencialidad.",
                    Purpose = "Prevenir fallas del equipo y proteger contra modificación o mal uso.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Verificar políticas de mantenimiento y patching del proveedor cloud."
                },
                new ISOControl
                {
                    ControlId = "7.14",
                    Category = ISOCategory.PhysicalControls,
                    Title = "Disposición segura o reutilización de equipos",
                    Description = "Los elementos del equipo que contienen medios de almacenamiento deben ser verificados para asegurar que cualquier dato sensible y software con licencia haya sido eliminado o sobrescrito de forma segura.",
                    Purpose = "Prevenir divulgación de información cuando se dispone del equipo.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Verificar procedimientos de destrucción segura de hardware en data centers del proveedor."
                },

                // ====================================
                // CONTROLES TECNOLÓGICOS (8.x) - CRÍTICOS PARA CLOUD
                // ====================================
                new ISOControl
                {
                    ControlId = "8.1",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Dispositivos de punto final de usuario",
                    Description = "La información almacenada en, procesada por o accesible a través de dispositivos de punto final de usuario debe ser protegida.",
                    Purpose = "Asegurar la seguridad de información en dispositivos de punto final de usuario.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Implementar EDR y gestión de dispositivos con acceso a recursos cloud."
                },
                new ISOControl
                {
                    ControlId = "8.2",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Derechos de acceso privilegiado",
                    Description = "La asignación y uso de derechos de acceso privilegiado debe ser restringida y gestionada.",
                    Purpose = "Prevenir acceso no autorizado, modificación o eliminación de información.",
                    Weight = 5,
                    IsCritical = true,
                    CloudAdaptation = "Gestionar accesos root/admin en cloud con MFA obligatorio, JIT access, y auditoría."
                },
                new ISOControl
                {
                    ControlId = "8.3",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Restricción de acceso a la información",
                    Description = "El acceso a información y otros activos asociados debe ser restringido de acuerdo con la política de control de acceso establecida.",
                    Purpose = "Asegurar que el acceso a información es autorizado y restringido.",
                    Weight = 5,
                    IsCritical = true,
                    CloudAdaptation = "Implementar políticas IAM granulares, least privilege, y revisión periódica de permisos."
                },
                new ISOControl
                {
                    ControlId = "8.4",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Acceso al código fuente",
                    Description = "El acceso de lectura y escritura al código fuente, herramientas de desarrollo y bibliotecas de software debe ser gestionado apropiadamente.",
                    Purpose = "Prevenir la introducción de funcionalidad no autorizada y evitar cambios no intencionales.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Proteger repositorios en cloud (GitHub, CodeCommit) con acceso basado en roles."
                },
                new ISOControl
                {
                    ControlId = "8.5",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Autenticación segura",
                    Description = "Las tecnologías de autenticación segura deben ser implementadas de acuerdo con las restricciones de acceso y la política de control de acceso por tema.",
                    Purpose = "Limitar el acceso solo a usuarios autorizados.",
                    Weight = 5,
                    IsCritical = true,
                    CloudAdaptation = "Implementar MFA obligatorio, SSO, y autenticación federada para servicios cloud."
                },
                new ISOControl
                {
                    ControlId = "8.6",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Gestión de capacidad",
                    Description = "El uso de recursos debe ser monitoreado y ajustado en línea con los requisitos actuales y previstos de capacidad.",
                    Purpose = "Asegurar que el sistema funciona de manera efectiva.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Usar auto-scaling, monitoreo de CloudWatch/Azure Monitor, y alertas de capacidad."
                },
                new ISOControl
                {
                    ControlId = "8.7",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Protección contra malware",
                    Description = "La protección contra malware debe ser implementada y soportada por concienciación apropiada del usuario.",
                    Purpose = "Asegurar que la información y las instalaciones de procesamiento de información están protegidas contra malware.",
                    Weight = 5,
                    IsCritical = true,
                    CloudAdaptation = "Implementar antimalware en VMs cloud, escaneo de buckets S3, y protección de contenedores."
                },
                new ISOControl
                {
                    ControlId = "8.8",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Gestión de vulnerabilidades técnicas",
                    Description = "La información sobre vulnerabilidades técnicas de los sistemas de información en uso debe ser obtenida, la exposición de la organización a tales vulnerabilidades evaluada y medidas apropiadas tomadas.",
                    Purpose = "Prevenir la explotación de vulnerabilidades técnicas.",
                    Weight = 5,
                    IsCritical = true,
                    CloudAdaptation = "Usar servicios de escaneo de vulnerabilidades (AWS Inspector, Azure Security Center)."
                },
                new ISOControl
                {
                    ControlId = "8.9",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Gestión de configuración",
                    Description = "Las configuraciones, incluyendo las de seguridad, de hardware, software, servicios y redes deben ser establecidas, documentadas, implementadas, monitoreadas y revisadas.",
                    Purpose = "Asegurar que los sistemas están configurados correctamente y de manera segura.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Usar Infrastructure as Code (Terraform, CloudFormation) y gestión de configuración centralizada."
                },
                new ISOControl
                {
                    ControlId = "8.10",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Eliminación de información",
                    Description = "La información almacenada en sistemas de información, dispositivos o en cualquier otro medio de almacenamiento debe ser eliminada cuando ya no sea requerida.",
                    Purpose = "Prevenir divulgación no autorizada de información.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Implementar políticas de retención y eliminación automática en S3, Blob Storage, y bases de datos."
                },
                new ISOControl
                {
                    ControlId = "8.11",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Enmascaramiento de datos",
                    Description = "El enmascaramiento de datos debe ser usado de acuerdo con la política de control de acceso de la organización y los requisitos de negocio.",
                    Purpose = "Limitar la exposición de datos sensibles.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Usar servicios de enmascaramiento de datos en bases de datos cloud y data lakes."
                },
                new ISOControl
                {
                    ControlId = "8.12",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Prevención de fuga de datos",
                    Description = "Las medidas de prevención de fuga de datos deben ser aplicadas a sistemas, redes y cualquier otro dispositivo que procese, almacene o transmita información sensible.",
                    Purpose = "Detectar y prevenir la divulgación y extracción no autorizada de información.",
                    Weight = 5,
                    IsCritical = true,
                    CloudAdaptation = "Implementar DLP en cloud (AWS Macie, Microsoft Purview) para detectar datos sensibles."
                },
                new ISOControl
                {
                    ControlId = "8.13",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Respaldo de información",
                    Description = "Copias de respaldo de información, software e imágenes de sistema deben ser mantenidas y probadas regularmente.",
                    Purpose = "Proteger contra pérdida de datos.",
                    Weight = 5,
                    IsCritical = true,
                    CloudAdaptation = "Configurar backups automáticos, snapshots, y replicación entre regiones en cloud."
                },
                new ISOControl
                {
                    ControlId = "8.14",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Redundancia de instalaciones de procesamiento de información",
                    Description = "Las instalaciones de procesamiento de información deben ser implementadas con redundancia suficiente para cumplir los requisitos de disponibilidad.",
                    Purpose = "Asegurar la disponibilidad de instalaciones de procesamiento de información.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Usar multi-AZ, load balancers, y arquitecturas de alta disponibilidad en cloud."
                },
                new ISOControl
                {
                    ControlId = "8.15",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Registro (logging)",
                    Description = "Los registros que registran actividades, excepciones, fallos y otros eventos relevantes deben ser producidos, almacenados, protegidos y analizados.",
                    Purpose = "Registrar eventos y generar evidencia.",
                    Weight = 5,
                    IsCritical = true,
                    CloudAdaptation = "Habilitar CloudTrail, Azure Activity Log, y centralizar logs en SIEM cloud."
                },
                new ISOControl
                {
                    ControlId = "8.16",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Actividades de monitoreo",
                    Description = "Las redes, sistemas y aplicaciones deben ser monitoreados para comportamiento anómalo y se deben tomar acciones apropiadas para evaluar incidentes potenciales de seguridad de la información.",
                    Purpose = "Detectar comportamiento anómalo y fallas de controles de seguridad de la información.",
                    Weight = 5,
                    IsCritical = true,
                    CloudAdaptation = "Implementar monitoreo continuo con GuardDuty, Security Center, y alertas en tiempo real."
                },
                new ISOControl
                {
                    ControlId = "8.17",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Sincronización de reloj",
                    Description = "Los relojes de los sistemas de procesamiento de información usados por la organización deben ser sincronizados con fuentes de tiempo aprobadas.",
                    Purpose = "Asegurar la precisión de registros de auditoría.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Verificar sincronización NTP en instancias cloud y servicios."
                },
                new ISOControl
                {
                    ControlId = "8.18",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Uso de programas de utilidad privilegiados",
                    Description = "El uso de programas de utilidad que podrían ser capaces de anular los controles del sistema y de la aplicación debe ser restringido y estrechamente controlado.",
                    Purpose = "Prevenir el uso indebido de programas de utilidad privilegiados.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Restringir uso de herramientas administrativas cloud y auditar su uso."
                },
                new ISOControl
                {
                    ControlId = "8.19",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Instalación de software en sistemas operativos",
                    Description = "Deben ser implementados procedimientos y medidas para gestionar de manera segura la instalación de software en sistemas operativos.",
                    Purpose = "Prevenir la instalación de software no autorizado.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Usar AMIs/imágenes aprobadas y políticas de instalación de software en cloud."
                },
                new ISOControl
                {
                    ControlId = "8.20",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Seguridad de redes",
                    Description = "Las redes y los dispositivos de red deben ser asegurados, gestionados y controlados para proteger la información en sistemas y aplicaciones.",
                    Purpose = "Asegurar la protección de información en redes.",
                    Weight = 5,
                    IsCritical = true,
                    CloudAdaptation = "Configurar Security Groups, NACLs, NSGs, y firewalls de aplicación web en cloud."
                },
                new ISOControl
                {
                    ControlId = "8.21",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Seguridad de servicios de red",
                    Description = "Los mecanismos de seguridad, niveles de servicio y requisitos de servicio de los servicios de red deben ser identificados, implementados y monitoreados.",
                    Purpose = "Asegurar la seguridad de los servicios de red.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Revisar SLAs de seguridad y configuración de servicios de red cloud (VPC, VNet)."
                },
                new ISOControl
                {
                    ControlId = "8.22",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Segregación de redes",
                    Description = "Grupos de servicios de información, usuarios y sistemas de información deben ser segregados en redes.",
                    Purpose = "Prevenir acceso no autorizado a sistemas y servicios.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Implementar VPCs/VNets segregadas por ambiente (prod, dev) y sensibilidad de datos."
                },
                new ISOControl
                {
                    ControlId = "8.23",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Filtrado web",
                    Description = "El acceso a sitios web externos debe ser gestionado para reducir la exposición a contenido malicioso.",
                    Purpose = "Prevenir acceso a contenido potencialmente malicioso.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Implementar web filtering y protección contra amenazas en gateways cloud."
                },
                new ISOControl
                {
                    ControlId = "8.24",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Uso de criptografía",
                    Description = "Las reglas para el uso efectivo de criptografía, incluyendo gestión de claves criptográficas, deben ser definidas e implementadas.",
                    Purpose = "Asegurar el uso apropiado y efectivo de criptografía.",
                    Weight = 5,
                    IsCritical = true,
                    CloudAdaptation = "Usar KMS (AWS KMS, Azure Key Vault) para gestión de claves y cifrado en reposo/tránsito."
                },
                new ISOControl
                {
                    ControlId = "8.25",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Ciclo de vida de desarrollo seguro",
                    Description = "Las reglas para el desarrollo seguro de software y sistemas deben ser establecidas y aplicadas.",
                    Purpose = "Asegurar que la seguridad de la información está diseñada e implementada dentro del ciclo de vida de desarrollo.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Implementar DevSecOps con escaneo de seguridad en pipelines CI/CD cloud."
                },
                new ISOControl
                {
                    ControlId = "8.26",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Requisitos de seguridad de aplicaciones",
                    Description = "Los requisitos de seguridad de la información deben ser identificados, especificados y aprobados al desarrollar o adquirir aplicaciones.",
                    Purpose = "Asegurar que las aplicaciones están diseñadas e implementadas de manera segura.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Definir requisitos de seguridad cloud-native (autenticación, autorización, cifrado)."
                },
                new ISOControl
                {
                    ControlId = "8.27",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Arquitectura de sistemas seguros y principios de ingeniería",
                    Description = "Los principios para diseñar sistemas seguros deben ser establecidos, documentados, mantenidos y aplicados a cualquier actividad de desarrollo de sistemas de información.",
                    Purpose = "Asegurar que los sistemas de información están diseñados con seguridad incorporada.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Aplicar AWS Well-Architected Framework y Azure Security Benchmark en diseños cloud."
                },
                new ISOControl
                {
                    ControlId = "8.28",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Codificación segura",
                    Description = "Los principios de codificación segura deben ser aplicados al desarrollo de software.",
                    Purpose = "Prevenir vulnerabilidades de seguridad en aplicaciones.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Usar SAST/DAST en pipelines y seguir OWASP Top 10 para aplicaciones cloud."
                },
                new ISOControl
                {
                    ControlId = "8.29",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Pruebas de seguridad en desarrollo y aceptación",
                    Description = "Los procesos de prueba de seguridad deben ser definidos y implementados en el ciclo de vida de desarrollo.",
                    Purpose = "Identificar y corregir vulnerabilidades de seguridad antes de la producción.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Realizar pruebas de penetración y análisis de configuración en entornos cloud pre-prod."
                },
                new ISOControl
                {
                    ControlId = "8.30",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Desarrollo externalizado",
                    Description = "La organización debe dirigir, monitorear y revisar las actividades relacionadas con el desarrollo de sistemas externalizados.",
                    Purpose = "Mantener la seguridad del desarrollo de sistemas externalizado.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Asegurar que proveedores externos siguen políticas de seguridad cloud de la organización."
                },
                new ISOControl
                {
                    ControlId = "8.31",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Separación de ambientes de desarrollo, prueba y producción",
                    Description = "Los ambientes de desarrollo, prueba y producción deben ser separados y asegurados.",
                    Purpose = "Reducir los riesgos de acceso o cambios no autorizados al ambiente de producción.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Usar cuentas AWS/Azure separadas o VPCs distintas por ambiente."
                },
                new ISOControl
                {
                    ControlId = "8.32",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Gestión de cambios",
                    Description = "Los cambios a instalaciones y sistemas de procesamiento de información deben estar sujetos a procedimientos de gestión de cambios.",
                    Purpose = "Reducir el riesgo de fallas del sistema y seguridad.",
                    Weight = 4,
                    IsCritical = true,
                    CloudAdaptation = "Usar AWS Systems Manager Change Manager o Azure Automation para gestión de cambios."
                },
                new ISOControl
                {
                    ControlId = "8.33",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Información de prueba",
                    Description = "La información de prueba debe ser seleccionada, protegida y gestionada apropiadamente.",
                    Purpose = "Proteger la información operacional usada para pruebas.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Anonimizar datos de producción antes de usar en ambientes de prueba cloud."
                },
                new ISOControl
                {
                    ControlId = "8.34",
                    Category = ISOCategory.TechnologicalControls,
                    Title = "Protección de sistemas de información durante pruebas de auditoría",
                    Description = "Las pruebas de auditoría y otras actividades de aseguramiento que involucren evaluación de sistemas operacionales deben ser planificadas y acordadas.",
                    Purpose = "Minimizar la interrupción a procesos de negocio durante auditorías.",
                    Weight = 3,
                    IsCritical = false,
                    CloudAdaptation = "Coordinar auditorías de configuración cloud sin impactar producción."
                }
            };

            await context.ISOControls.AddRangeAsync(controls);
        }

        private static async Task SeedProviders(ApplicationDbContext context)
        {
            var providers = new List<Provider>
            {
                new Provider
                {
                    Name = "Amazon Web Services (AWS)",
                    Type = ProviderType.IaaS,
                    Region = "Global",
                    Description = "Plataforma de servicios en la nube más amplia y profundamente adoptada, ofreciendo más de 200 servicios completos desde centros de datos a nivel mundial.",
                    ContactEmail = "aws-security@amazon.com",
                    WebsiteUrl = "https://aws.amazon.com",
                    CreatedAt = DateTime.UtcNow
                },
                new Provider
                {
                    Name = "Microsoft Azure",
                    Type = ProviderType.Hybrid,
                    Region = "Global",
                    Description = "Plataforma de computación en la nube de Microsoft con servicios de infraestructura, plataforma y software como servicio.",
                    ContactEmail = "azure-security@microsoft.com",
                    WebsiteUrl = "https://azure.microsoft.com",
                    CreatedAt = DateTime.UtcNow
                },
                new Provider
                {
                    Name = "Google Cloud Platform (GCP)",
                    Type = ProviderType.IaaS,
                    Region = "Global",
                    Description = "Suite de servicios de computación en la nube que funciona en la misma infraestructura que Google utiliza internamente.",
                    ContactEmail = "cloud-security@google.com",
                    WebsiteUrl = "https://cloud.google.com",
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Providers.AddRangeAsync(providers);
        }
        private static async Task SeedRolesAndUsers(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Crear Roles
            string[] roles = { "Administrador", "Auditor", "Consultor" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Crear usuario Administrador por defecto
            var adminEmail = "admin@secutrck.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Administrador del Sistema",
                    Department = "TI - Seguridad",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Administrador");
                }
            }

            // Crear usuario Auditor por defecto
            var auditorEmail = "auditor@secutrck.com";
            var auditorUser = await userManager.FindByEmailAsync(auditorEmail);

            if (auditorUser == null)
            {
                var auditor = new ApplicationUser
                {
                    UserName = auditorEmail,
                    Email = auditorEmail,
                    FullName = "Auditor Principal",
                    Department = "Auditoría",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(auditor, "Auditor@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(auditor, "Auditor");
                }
            }

            // Crear usuario Consultor por defecto
            var consultorEmail = "consultor@secutrck.com";
            var consultorUser = await userManager.FindByEmailAsync(consultorEmail);

            if (consultorUser == null)
            {
                var consultor = new ApplicationUser
                {
                    UserName = consultorEmail,
                    Email = consultorEmail,
                    FullName = "Consultor de Seguridad",
                    Department = "Consultoría",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(consultor, "Consultor@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(consultor, "Consultor");
                }
            }
        }
    }
}