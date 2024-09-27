using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class Dato
{
    public int Id { get; set; }

    public string Letra { get; set; } = null!;

    public byte[] BlobData { get; set; } = null!;

    public string Ext { get; set; } = null!;
}
