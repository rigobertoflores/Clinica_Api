using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clinica_Api.Modelss;

public partial class PacientesInformacionGeneral
{
    [Key]
    public int Id { get; set; }
    public int Clave { get; set; }

    public string? FechaDeNacimiento { get; set; }

    public string? Sexo { get; set; }

    public string? EstadoCivil { get; set; }

    public string? Ocupacion { get; set; }

    public string? Domicilio { get; set; }

    public string? Poblacion { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? NombreDelEsposo { get; set; }

    public string? EdadDelEsposo { get; set; }

    public string? OcupacionEsposo { get; set; }

    public string? Referencia { get; set; }

    public string? Diabetes { get; set; }

    public string? Hipertension { get; set; }

    public string? Trombosis { get; set; }

    public string? Cardiopatias { get; set; }

    public string? Cancer { get; set; }

    public string? EnfermedadesGeneticas { get; set; }

    public string? OtraEnfermedad { get; set; }

    public string? Inmunizaciones { get; set; }

    public string? Alcoholismo { get; set; }

    public string? Tabaquismo { get; set; }

    public string? TabaquismoPasivo { get; set; }

    public string? DrogasOmedicamentos { get; set; }

    public string? GrupoSanguineo { get; set; }

    public string? PropiasDeLaInfancia { get; set; }

    public string? Rubeola { get; set; }

    public string? Amigdalitis { get; set; }

    public string? Bronquitis { get; set; }

    public string? Bronconeumonia { get; set; }

    public string? HepatitisViralTipo { get; set; }

    public string? Parasitosis { get; set; }

    public string? Toxoplasmosis { get; set; }

    public string? Citomegalovirus { get; set; }

    public string? Herpes { get; set; }

    public string? Clamydiasis { get; set; }

    public string? Hiv { get; set; }

    public string? Sifilis { get; set; }

    public string? Micosis { get; set; }

    public string? Eip { get; set; }

    public string? DiabetesMellitus { get; set; }

    public string? OtrasEndocrinas { get; set; }

    public string? Nefropatias { get; set; }

    public string? Digestivas { get; set; }

    public string? Neurologicas { get; set; }

    public string? Hematologicas { get; set; }

    public string? Tumores { get; set; }

    public string? Condilomatosis { get; set; }

    public string? Displasias { get; set; }

    public string? Alergia { get; set; }

    public string? Nombre { get; set; }

    public string? FechaConsulta { get; set; }

    public string? FechaUltimaConsulta { get; set; }
}
