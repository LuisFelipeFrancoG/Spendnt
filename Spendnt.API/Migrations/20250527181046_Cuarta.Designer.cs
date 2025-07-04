﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Spendnt.API.Data;

#nullable disable

namespace Spendnt.API.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20250527181046_Cuarta")]
    partial class Cuarta
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Spendnt.Shared.Entities.Categoria", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EgresosId")
                        .HasColumnType("int");

                    b.Property<int?>("IngresosId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EgresosId");

                    b.HasIndex("IngresosId");

                    b.ToTable("Categorias");
                });

            modelBuilder.Entity("Spendnt.Shared.Entities.Egresos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<decimal>("Egreso")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("SaldoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SaldoId");

                    b.ToTable("Egresos");
                });

            modelBuilder.Entity("Spendnt.Shared.Entities.Historial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Monto")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Tipo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoriaId");

                    b.ToTable("Historiales");
                });

            modelBuilder.Entity("Spendnt.Shared.Entities.Ingresos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Ingreso")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("SaldoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SaldoId");

                    b.ToTable("Ingresos");
                });

            modelBuilder.Entity("Spendnt.Shared.Entities.RecordatorioGasto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("FechaProgramada")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("MontoEstimado")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Notas")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Titulo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RecordatoriosGasto");
                });

            modelBuilder.Entity("Spendnt.Shared.Entities.Saldo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("Saldo");
                });

            modelBuilder.Entity("Spendnt.Shared.Entities.Categoria", b =>
                {
                    b.HasOne("Spendnt.Shared.Entities.Egresos", null)
                        .WithMany("Categorias")
                        .HasForeignKey("EgresosId");

                    b.HasOne("Spendnt.Shared.Entities.Ingresos", null)
                        .WithMany("Categorias")
                        .HasForeignKey("IngresosId");
                });

            modelBuilder.Entity("Spendnt.Shared.Entities.Egresos", b =>
                {
                    b.HasOne("Spendnt.Shared.Entities.Saldo", "Saldo")
                        .WithMany("Egresos")
                        .HasForeignKey("SaldoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Saldo");
                });

            modelBuilder.Entity("Spendnt.Shared.Entities.Historial", b =>
                {
                    b.HasOne("Spendnt.Shared.Entities.Categoria", "Categoria")
                        .WithMany()
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categoria");
                });

            modelBuilder.Entity("Spendnt.Shared.Entities.Ingresos", b =>
                {
                    b.HasOne("Spendnt.Shared.Entities.Saldo", "Saldo")
                        .WithMany("Ingresos")
                        .HasForeignKey("SaldoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Saldo");
                });

            modelBuilder.Entity("Spendnt.Shared.Entities.Egresos", b =>
                {
                    b.Navigation("Categorias");
                });

            modelBuilder.Entity("Spendnt.Shared.Entities.Ingresos", b =>
                {
                    b.Navigation("Categorias");
                });

            modelBuilder.Entity("Spendnt.Shared.Entities.Saldo", b =>
                {
                    b.Navigation("Egresos");

                    b.Navigation("Ingresos");
                });
#pragma warning restore 612, 618
        }
    }
}
