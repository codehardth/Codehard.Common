﻿// <auto-generated />
using System;
using Infrastructure.Test;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Test.Migrations
{
    [DbContext(typeof(TestDbContext))]
    partial class TestDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Infrastructure.Test.Entities.ChildModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<Guid?>("MyModelId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("MyModelId");

                    b.ToTable("ChildModel");
                });

            modelBuilder.Entity("Infrastructure.Test.Entities.MyModel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("number")
                        .HasColumnType("integer");

                    b.Property<string>("text")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("Infrastructure.Test.Entities.ChildModel", b =>
                {
                    b.HasOne("Infrastructure.Test.Entities.MyModel", null)
                        .WithMany("Childs")
                        .HasForeignKey("MyModelId");
                });

            modelBuilder.Entity("Infrastructure.Test.Entities.MyModel", b =>
                {
                    b.Navigation("Childs");
                });
#pragma warning restore 612, 618
        }
    }
}
