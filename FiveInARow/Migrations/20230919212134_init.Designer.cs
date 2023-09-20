﻿// <auto-generated />
using FiveInARow.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FiveInARow.Migrations
{
    [DbContext(typeof(FiveInARowDbContext))]
    [Migration("20230919212134_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FiveInARow.Models.GameRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("GameRecords");
                });

            modelBuilder.Entity("FiveInARow.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FiveInARow.Models.UserGameRecord", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("GameRecordId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "GameRecordId");

                    b.HasIndex("GameRecordId");

                    b.ToTable("UserGameRecords");
                });

            modelBuilder.Entity("FiveInARow.Models.UserGameRecord", b =>
                {
                    b.HasOne("FiveInARow.Models.GameRecord", "GameRecord")
                        .WithMany("UserGameRecords")
                        .HasForeignKey("GameRecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FiveInARow.Models.User", "User")
                        .WithMany("UserGameRecords")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameRecord");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FiveInARow.Models.GameRecord", b =>
                {
                    b.Navigation("UserGameRecords");
                });

            modelBuilder.Entity("FiveInARow.Models.User", b =>
                {
                    b.Navigation("UserGameRecords");
                });
#pragma warning restore 612, 618
        }
    }
}