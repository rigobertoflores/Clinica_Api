using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class Receta
{
    public int Id { get; set; }

    public byte[]? BlobData { get; set; }
}
