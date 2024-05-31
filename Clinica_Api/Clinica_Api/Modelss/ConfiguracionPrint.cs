using System;
using System.Collections.Generic;

namespace Clinica_Api.Modelss;

public partial class ConfiguracionPrint
{
    public int Id { get; set; }

    public decimal? Largo { get; set; }
    public decimal? Espacio { get; set; }

    public decimal? Ancho { get; set; }

    public decimal? MargenDerecho { get; set; }

    public decimal? MargenIzquierdo { get; set; }

    public decimal? MargenArriba { get; set; }

    public decimal? MargenAbajo { get; set; }

    public string Usuario { get; set; } = null!;

    public string? Encabezado { get; set; }

}
