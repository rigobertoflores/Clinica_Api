using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class Expediente
{
    public int Clave { get; set; }

    public string HistoriaId { get; set; } = null!;

    public string? Expediente1 { get; set; }

    public int Id { get; set; }
}
