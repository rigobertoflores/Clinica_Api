using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class Historia
{
    public int Id { get; set; }

    public string Hc { get; set; } = null!;

    public string Nombre { get; set; } = null!;
}
