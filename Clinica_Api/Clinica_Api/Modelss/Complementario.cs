using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class Complementario
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public byte[]? BlobData { get; set; }

    public string? Ext { get; set; }

    public int? Clave { get; set; }
}
