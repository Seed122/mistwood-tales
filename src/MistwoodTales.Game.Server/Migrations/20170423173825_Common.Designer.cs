using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MistwoodTales.Game.Server.Context;

namespace MistwoodTales.Game.Server.Migrations
{
    [DbContext(typeof(ModelDbContext))]
    [Migration("20170423173825_Common")]
    partial class Common
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("MistwoodTales.Game.Server.Data.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationDate");

                    b.Property<string>("Login")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("MistwoodTales.Game.Server.Data.Models.Character", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<DateTime>("CreationDate");

                    b.Property<int>("Gold");

                    b.Property<int>("HP");

                    b.Property<int>("Level");

                    b.Property<int?>("MapId");

                    b.Property<int>("MaxHP");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("XP");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("MapId");

                    b.ToTable("Characters");
                });

            modelBuilder.Entity("MistwoodTales.Game.Server.Data.Models.Map", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Height");

                    b.Property<int>("Width");

                    b.HasKey("Id");

                    b.ToTable("Maps");
                });

            modelBuilder.Entity("MistwoodTales.Game.Server.Data.Models.Character", b =>
                {
                    b.HasOne("MistwoodTales.Game.Server.Data.Models.Account", "Account")
                        .WithMany("Characters")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MistwoodTales.Game.Server.Data.Models.Map", "Map")
                        .WithMany()
                        .HasForeignKey("MapId");
                });
        }
    }
}
