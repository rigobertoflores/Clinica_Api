using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class RelacionPlantilllaPaciente
{
    public int Id { get; set; }

    public int PlantillaId { get; set; }

    public int PacienteId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? FechaUltActualizacion { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? NombrePlantilla { get; set; }

    public string? NombrePaciente { get; set; }
}
