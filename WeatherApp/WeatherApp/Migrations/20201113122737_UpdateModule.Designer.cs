﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WeatherApp.DB;

namespace WeatherApp.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20201113122737_UpdateModule")]
    partial class UpdateModule
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("WeatherApp.DB.Forecast", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CityName")
                        .HasColumnType("text");

                    b.Property<string>("CurrentForecast")
                        .HasColumnType("text");

                    b.Property<string>("DailyForecast")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("FaultUpdateLastDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Forecasts");
                });

            modelBuilder.Entity("WeatherApp.DB.Module", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Assembly")
                        .HasColumnType("bytea");

                    b.Property<string>("Author")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UploadDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Version")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Modules");
                });
#pragma warning restore 612, 618
        }
    }
}
