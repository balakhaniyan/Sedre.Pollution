﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sedre.Pollution.Infrastructure;

namespace Sedre.Pollution.Infrastructure.Migrations
{
    [DbContext(typeof(PollutionDbContext))]
    partial class PollutionDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Sedre.Pollution.Domain.Models.DayIndicator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("ALatitude")
                        .HasColumnType("float");

                    b.Property<double>("ALongitude")
                        .HasColumnType("float");

                    b.Property<double>("BLatitude")
                        .HasColumnType("float");

                    b.Property<double>("BLongitude")
                        .HasColumnType("float");

                    b.Property<double>("CLatitude")
                        .HasColumnType("float");

                    b.Property<double>("CLongitude")
                        .HasColumnType("float");

                    b.Property<double>("Co")
                        .HasColumnType("float");

                    b.Property<double>("DLatitude")
                        .HasColumnType("float");

                    b.Property<double>("DLongitude")
                        .HasColumnType("float");

                    b.Property<int>("Date")
                        .HasColumnType("int");

                    b.Property<double>("No2")
                        .HasColumnType("float");

                    b.Property<double>("O3")
                        .HasColumnType("float");

                    b.Property<double>("Pm10")
                        .HasColumnType("float");

                    b.Property<double>("Pm25")
                        .HasColumnType("float");

                    b.Property<double>("So2")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("DayIndicators");
                });

            modelBuilder.Entity("Sedre.Pollution.Domain.Models.HourIndicator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("ALatitude")
                        .HasColumnType("float");

                    b.Property<double>("ALongitude")
                        .HasColumnType("float");

                    b.Property<double>("BLatitude")
                        .HasColumnType("float");

                    b.Property<double>("BLongitude")
                        .HasColumnType("float");

                    b.Property<double>("CLatitude")
                        .HasColumnType("float");

                    b.Property<double>("CLongitude")
                        .HasColumnType("float");

                    b.Property<double>("Co")
                        .HasColumnType("float");

                    b.Property<double>("DLatitude")
                        .HasColumnType("float");

                    b.Property<double>("DLongitude")
                        .HasColumnType("float");

                    b.Property<int>("Date")
                        .HasColumnType("int");

                    b.Property<double>("No2")
                        .HasColumnType("float");

                    b.Property<double>("O3")
                        .HasColumnType("float");

                    b.Property<double>("Pm10")
                        .HasColumnType("float");

                    b.Property<double>("Pm25")
                        .HasColumnType("float");

                    b.Property<double>("So2")
                        .HasColumnType("float");

                    b.Property<int>("Time")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("HourIndicators");
                });

            modelBuilder.Entity("Sedre.Pollution.Domain.Models.MonthIndicator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("ALatitude")
                        .HasColumnType("float");

                    b.Property<double>("ALongitude")
                        .HasColumnType("float");

                    b.Property<double>("BLatitude")
                        .HasColumnType("float");

                    b.Property<double>("BLongitude")
                        .HasColumnType("float");

                    b.Property<double>("CLatitude")
                        .HasColumnType("float");

                    b.Property<double>("CLongitude")
                        .HasColumnType("float");

                    b.Property<double>("Co")
                        .HasColumnType("float");

                    b.Property<double>("DLatitude")
                        .HasColumnType("float");

                    b.Property<double>("DLongitude")
                        .HasColumnType("float");

                    b.Property<int>("Date")
                        .HasColumnType("int");

                    b.Property<double>("No2")
                        .HasColumnType("float");

                    b.Property<double>("O3")
                        .HasColumnType("float");

                    b.Property<double>("Pm10")
                        .HasColumnType("float");

                    b.Property<double>("Pm25")
                        .HasColumnType("float");

                    b.Property<double>("So2")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("MonthIndicators");
                });
#pragma warning restore 612, 618
        }
    }
}
