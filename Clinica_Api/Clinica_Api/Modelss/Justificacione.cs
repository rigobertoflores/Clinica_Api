using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class Justificacione
{
    public int Id { get; set; }

    public string? Justificacion { get; set; }

    public string? Nombre { get; set; }

    public string? Usuario { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }
}
