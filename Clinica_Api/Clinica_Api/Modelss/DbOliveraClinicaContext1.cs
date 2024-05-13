using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Clinica_Api.Modelss;

public partial class DbOliveraClinicaContext1 : DbContext
{
    public DbOliveraClinicaContext1()
    {
    }

    public DbOliveraClinicaContext1(DbContextOptions<DbOliveraClinicaContext1> options)
        : base(options)
    {
    }

    public virtual DbSet<Agendum> Agenda { get; set; }

    public virtual DbSet<Commaux> Commauxes { get; set; }

    public virtual DbSet<Dato> Datos { get; set; }

    public virtual DbSet<Direccione> Direcciones { get; set; }

    public virtual DbSet<Dx> Dxes { get; set; }

    public virtual DbSet<Expediente> Expedientes { get; set; }

    public virtual DbSet<ExpedienteX> ExpedienteXes { get; set; }

    public virtual DbSet<Foto> Fotos { get; set; }

    public virtual DbSet<Historia> Historias { get; set; }

    public virtual DbSet<Imagene> Imagenes { get; set; }

    public virtual DbSet<InformeExpediente> InformeExpedientes { get; set; }

    public virtual DbSet<InformeOperatorio> InformeOperatorios { get; set; }

    public virtual DbSet<Nota> Notas { get; set; }

    public virtual DbSet<Observacione> Observaciones { get; set; }

    public virtual DbSet<PacientesInformacionGeneral> PacientesInformacionGenerals { get; set; }

    public virtual DbSet<Receta> Recetas { get; set; }

    public virtual DbSet<RecetasX> RecetasXes { get; set; }

    public virtual DbSet<RecetasxPaciente> RecetasxPacientes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Database=db_olivera_clinica;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Modern_Spanish_CI_AS");

        modelBuilder.Entity<Agendum>(entity =>
        {
            entity.HasKey(e => new { e.Dia, e.Hora }).HasName("PK_Agenda_Agenda_Agenda");

            entity.Property(e => e.Dia).HasColumnType("datetime");
            entity.Property(e => e.Hora)
                .HasMaxLength(12)
                .IsFixedLength();
            entity.Property(e => e.Nombre)
                .HasMaxLength(40)
                .IsFixedLength();
            entity.Property(e => e.Procedimiento)
                .HasMaxLength(50)
                .IsFixedLength();
        });

        modelBuilder.Entity<Commaux>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("COMMAUX");

            entity.Property(e => e.Cabeza).HasColumnName("cabeza");
            entity.Property(e => e.ColorExp).HasMaxLength(40);
            entity.Property(e => e.ColorNota).HasMaxLength(40);
            entity.Property(e => e.ColorReceta).HasMaxLength(40);
            entity.Property(e => e.Data).HasMaxLength(50);
            entity.Property(e => e.Eam)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.Epm)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.Fecha).HasMaxLength(50);
            entity.Property(e => e.Md).HasColumnName("MD");
            entity.Property(e => e.Mi).HasColumnName("MI");
            entity.Property(e => e.Minf).HasColumnName("MInf");
            entity.Property(e => e.Ms).HasColumnName("MS");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.Nombre).HasMaxLength(40);
            entity.Property(e => e.Pie).HasColumnName("pie");
            entity.Property(e => e.Serie)
                .HasMaxLength(8)
                .IsFixedLength();
            entity.Property(e => e.Tam)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.TipoExp).HasMaxLength(80);
            entity.Property(e => e.TipoNota).HasMaxLength(80);
            entity.Property(e => e.TipoReceta).HasMaxLength(80);
            entity.Property(e => e.Tpm)
                .HasMaxLength(11)
                .IsFixedLength();
            entity.Property(e => e.Ultima).HasMaxLength(50);
        });

        modelBuilder.Entity<Dato>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.Letra, e.Ext }).HasName("PK_LosDatos_LosDatos");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Letra).HasMaxLength(40);
            entity.Property(e => e.Ext).HasMaxLength(10);
        });

        modelBuilder.Entity<Direccione>(entity =>
        {
            entity.HasKey(e => new { e.Clave, e.Dir }).HasName("PK_DIRECCIONES_DIRECCIONES_DIRECCIONES");

            entity.ToTable("DIRECCIONES");

            entity.Property(e => e.Dir).HasMaxLength(15);
            entity.Property(e => e.Celular).HasMaxLength(40);
            entity.Property(e => e.Ciudad).HasMaxLength(40);
            entity.Property(e => e.Colonia).HasMaxLength(40);
            entity.Property(e => e.Cp)
                .HasMaxLength(5)
                .HasColumnName("CP");
            entity.Property(e => e.Direccion).HasMaxLength(40);
            entity.Property(e => e.Estado).HasMaxLength(15);
            entity.Property(e => e.Ext1).HasMaxLength(8);
            entity.Property(e => e.Ext2).HasMaxLength(8);
            entity.Property(e => e.Ext3).HasMaxLength(8);
            entity.Property(e => e.Fax).HasMaxLength(12);
            entity.Property(e => e.Lada).HasMaxLength(5);
            entity.Property(e => e.Tel2).HasMaxLength(12);
            entity.Property(e => e.Tel3).HasMaxLength(12);
            entity.Property(e => e.Telefonos).HasMaxLength(40);
        });

        modelBuilder.Entity<Dx>(entity =>
        {
            entity.HasKey(e => new { e.Clave, e.Dx1 }).HasName("PK_Dx_1_Dx_Dx");

            entity.ToTable("Dx");

            entity.Property(e => e.Dx1)
                .HasMaxLength(20)
                .HasColumnName("Dx");
        });

        modelBuilder.Entity<Expediente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_pacientes_pacientes_pacientes");

            entity.Property(e => e.Expediente1).HasColumnName("Expediente");
            entity.Property(e => e.HistoriaId)
                .HasMaxLength(9)
                .HasDefaultValueSql("('Pacientes')");
        });

        modelBuilder.Entity<ExpedienteX>(entity =>
        {
            entity.HasKey(e => e.Clave).HasName("PK_ExpedienteX_ExpedienteX_ExpedienteX");

            entity.ToTable("ExpedienteX");

            entity.Property(e => e.Clave).ValueGeneratedNever();
            entity.Property(e => e.Expediente).HasColumnType("image");
        });

        modelBuilder.Entity<Foto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_LasFotos_LasFotos");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<Historia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Historia__3214EC27590B369D");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Hc)
                .IsUnicode(false)
                .HasColumnName("HC");
            entity.Property(e => e.Nombre).IsUnicode(false);
        });

        modelBuilder.Entity<Imagene>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Imagenes__3214EC27327DB5C4");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Ext)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Letra)
                .HasMaxLength(40)
                .IsUnicode(false);
        });

        modelBuilder.Entity<InformeExpediente>(entity =>
        {
            entity.ToTable("Informe_Expediente");

            entity.Property(e => e.InformeId).HasMaxLength(9);
        });

        modelBuilder.Entity<InformeOperatorio>(entity =>
        {
            entity.ToTable("Informe_Operatorio");

            entity.Property(e => e.Informe).IsUnicode(false);
            entity.Property(e => e.Nombre).IsUnicode(false);
        });

        modelBuilder.Entity<Nota>(entity =>
        {
            entity.Property(e => e.Fecha)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<Observacione>(entity =>
        {
            entity.HasKey(e => e.Clave).HasName("PK_OBSERVACIONES_OBSERVACIONES_OBSERVACIONES_OBSERVACIONES");

            entity.ToTable("OBSERVACIONES");

            entity.Property(e => e.Clave).ValueGeneratedNever();
            entity.Property(e => e.O1).HasMaxLength(21);
            entity.Property(e => e.O2).HasMaxLength(21);
            entity.Property(e => e.O3).HasMaxLength(21);
            entity.Property(e => e.O4).HasMaxLength(21);
        });

        modelBuilder.Entity<PacientesInformacionGeneral>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Paciente__3214EC271EBF4044");

            entity.ToTable("Pacientes_InformacionGeneral");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Alcoholismo).HasMaxLength(255);
            entity.Property(e => e.Alergia).HasMaxLength(255);
            entity.Property(e => e.Amigdalitis).HasMaxLength(255);
            entity.Property(e => e.Bronconeumonia).HasMaxLength(255);
            entity.Property(e => e.Bronquitis).HasMaxLength(255);
            entity.Property(e => e.Cancer).HasMaxLength(255);
            entity.Property(e => e.Cardiopatias).HasMaxLength(255);
            entity.Property(e => e.Citomegalovirus).HasMaxLength(255);
            entity.Property(e => e.Clamydiasis).HasMaxLength(255);
            entity.Property(e => e.Condilomatosis).HasMaxLength(255);
            entity.Property(e => e.Diabetes).HasMaxLength(255);
            entity.Property(e => e.DiabetesMellitus).HasMaxLength(255);
            entity.Property(e => e.Digestivas).HasMaxLength(255);
            entity.Property(e => e.Displasias).HasMaxLength(255);
            entity.Property(e => e.Domicilio).HasMaxLength(255);
            entity.Property(e => e.DrogasOmedicamentos)
                .HasMaxLength(255)
                .HasColumnName("DrogasOMedicamentos");
            entity.Property(e => e.EdadDelEsposo).HasMaxLength(255);
            entity.Property(e => e.Eip)
                .HasMaxLength(255)
                .HasColumnName("EIP");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.EnfermedadesGeneticas).HasMaxLength(255);
            entity.Property(e => e.EstadoCivil).HasMaxLength(255);
            entity.Property(e => e.FechaConsulta)
                .HasMaxLength(50)
                .HasColumnName("fecha_consulta");
            entity.Property(e => e.FechaDeNacimiento).HasMaxLength(255);
            entity.Property(e => e.FechaUltimaConsulta)
                .HasMaxLength(50)
                .HasColumnName("fecha_ultima_consulta");
            entity.Property(e => e.GrupoSanguineo).HasMaxLength(255);
            entity.Property(e => e.Hematologicas).HasMaxLength(255);
            entity.Property(e => e.HepatitisViralTipo).HasMaxLength(255);
            entity.Property(e => e.Herpes).HasMaxLength(255);
            entity.Property(e => e.Hipertension).HasMaxLength(255);
            entity.Property(e => e.Hiv)
                .HasMaxLength(255)
                .HasColumnName("HIV");
            entity.Property(e => e.Inmunizaciones).HasMaxLength(255);
            entity.Property(e => e.Micosis).HasMaxLength(255);
            entity.Property(e => e.Nefropatias).HasMaxLength(255);
            entity.Property(e => e.Neurologicas).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(255);
            entity.Property(e => e.NombreDelEsposo).HasMaxLength(255);
            entity.Property(e => e.Ocupacion).HasMaxLength(255);
            entity.Property(e => e.OcupacionEsposo).HasMaxLength(255);
            entity.Property(e => e.OtraEnfermedad).HasMaxLength(255);
            entity.Property(e => e.OtrasEndocrinas).HasMaxLength(255);
            entity.Property(e => e.Parasitosis).HasMaxLength(255);
            entity.Property(e => e.Poblacion).HasMaxLength(255);
            entity.Property(e => e.PropiasDeLaInfancia).HasMaxLength(255);
            entity.Property(e => e.Referencia).HasMaxLength(255);
            entity.Property(e => e.Rubeola).HasMaxLength(255);
            entity.Property(e => e.Sexo).HasMaxLength(100);
            entity.Property(e => e.Sifilis).HasMaxLength(255);
            entity.Property(e => e.Tabaquismo).HasMaxLength(255);
            entity.Property(e => e.TabaquismoPasivo).HasMaxLength(255);
            entity.Property(e => e.Telefono).HasMaxLength(255);
            entity.Property(e => e.Toxoplasmosis).HasMaxLength(255);
            entity.Property(e => e.Trombosis).HasMaxLength(255);
            entity.Property(e => e.Tumores).HasMaxLength(500);
        });

        modelBuilder.Entity<Receta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_LasRecetas_LasRecetas");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
        });

        modelBuilder.Entity<RecetasX>(entity =>
        {
            entity.HasKey(e => e.Clave).HasName("PK_RecetasX_RecetasX_RecetasX");

            entity.ToTable("RecetasX");

            entity.Property(e => e.Clave).ValueGeneratedNever();
            entity.Property(e => e.Expediente).HasColumnType("image");
        });

        modelBuilder.Entity<RecetasxPaciente>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("RecetasxPaciente");

            entity.Property(e => e.Fecha).HasMaxLength(50);
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
