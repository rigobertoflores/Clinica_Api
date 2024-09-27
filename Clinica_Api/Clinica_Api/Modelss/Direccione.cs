using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class Direccione
{
    public int Clave { get; set; }

    public string Dir { get; set; } = null!;

    public string? Direccion { get; set; }

    public string? Colonia { get; set; }

    public string? Ciudad { get; set; }

    public string? Estado { get; set; }

    public string? Cp { get; set; }

    public string? Telefonos { get; set; }

    public string? Celular { get; set; }

    public string? Lada { get; set; }

    public string? Tel2 { get; set; }

    public string? Tel3 { get; set; }

    public string? Ext1 { get; set; }

    public string? Ext2 { get; set; }

    public string? Ext3 { get; set; }

    public string? Fax { get; set; }
}
