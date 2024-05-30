using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class Nota
{
    public int Id { get; set; }

    public string? Notas { get; set; }

    public int? Clave { get; set; }

    public string? Fecha { get; set; }
}
