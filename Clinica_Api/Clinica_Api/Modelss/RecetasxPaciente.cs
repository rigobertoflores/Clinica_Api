using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clinica_Api.Modelss;

public partial class RecetasxPaciente
{
    [Key]
    public int Id { get; set; }

    public string? Receta { get; set; }

    public string? Fecha { get; set; }

    public int Clave { get; set; }
}
