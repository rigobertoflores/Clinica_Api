using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class Imagene
{
    public int Id { get; set; }

    public string? Letra { get; set; }

    public byte[]? BlobData { get; set; }

    public string? Ext { get; set; }

    public int? Clave { get; set; }
}
