using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class Agendum
{
    public DateTime Dia { get; set; }

    public string Hora { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string? Procedimiento { get; set; }
}
