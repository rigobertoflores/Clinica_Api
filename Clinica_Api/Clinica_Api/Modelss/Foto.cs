using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class Foto
{
    public int Id { get; set; }

    public byte[]? BlobData { get; set; }
}
