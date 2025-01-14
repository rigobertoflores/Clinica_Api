using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class PlantillaJustificacion
{
    public int Id { get; set; }

    public string? Justificacion { get; set; }

    public string? Nombre { get; set; }

    public string? Usuario { get; set; }
}
