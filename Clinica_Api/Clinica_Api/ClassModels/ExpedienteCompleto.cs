namespace Clinica_Api.ClassModels
{
    public class SeccionesExpedienteCompleto
    {
        public bool informacionGeneral { get; set; }
        public bool historia { get; set; }
        public bool imagenPerfil { get; set; }
        public bool imagenes { get; set; }
        public bool pendientes { get; set; }
        public bool recetas { get; set; }
        public bool complementarios { get; set; }
        public bool justificacionesInformes { get; set; } 
        public int idPaciente { get; set; }

    }
    public class ExpedienteCompleto
    {
        public bool? informacionGeneral { get; set; }
        public bool? historia { get; set; }
        public bool imagenPerfil { get; set; }
        public bool imagenes { get; set; }
        public bool? pendientes { get; set; }
        public bool? recetas { get; set; }
        public bool? complementarios { get; set; }
        public bool? justificacionesInformes { get; set; }

    }
}
