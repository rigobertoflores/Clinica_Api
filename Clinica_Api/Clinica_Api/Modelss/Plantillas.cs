using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;
public partial class Plantillas
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Asunto { get; set; }
        public string? CuerpoEmail { get; set; }
        public DateTime FechaEnvio { get; set; }  
        public string? Adjunto { get; set; }


    }

