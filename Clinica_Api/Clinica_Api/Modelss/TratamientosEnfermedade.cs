using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class TratamientosEnfermedade
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? DescripcionEnfermedad { get; set; }

    public string? Tratamiento { get; set; }

    public string? PalabrasClaves { get; set; }
}
