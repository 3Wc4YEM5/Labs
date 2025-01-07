﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockDBApp.Data;

#nullable disable

namespace StockDBApp.Migrations
{
    [DbContext(typeof(StockContext))]
    partial class StockContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StockDBApp.Models.Price", b =>
                {
                    b.Property<int>("PriceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PriceId"));

                    b.Property<double>("AveragePrice")
                        .HasColumnType("float");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PriceId");

                    b.HasIndex("Ticker");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("StockDBApp.Models.Stock", b =>
                {
                    b.Property<string>("Ticker")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Ticker");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("StockDBApp.Models.TodaysCondition", b =>
                {
                    b.Property<int>("ConditionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConditionId"));

                    b.Property<string>("Condition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Ticker")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ConditionId");

                    b.HasIndex("Ticker")
                        .IsUnique();

                    b.ToTable("TodaysConditions");
                });

            modelBuilder.Entity("StockDBApp.Models.Price", b =>
                {
                    b.HasOne("StockDBApp.Models.Stock", "Stock")
                        .WithMany("Prices")
                        .HasForeignKey("Ticker")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StockDBApp.Models.TodaysCondition", b =>
                {
                    b.HasOne("StockDBApp.Models.Stock", "Stock")
                        .WithOne("TodaysCondition")
                        .HasForeignKey("StockDBApp.Models.TodaysCondition", "Ticker")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StockDBApp.Models.Stock", b =>
                {
                    b.Navigation("Prices");

                    b.Navigation("TodaysCondition")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
