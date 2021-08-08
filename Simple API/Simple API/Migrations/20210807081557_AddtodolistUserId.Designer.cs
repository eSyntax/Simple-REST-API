﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Simple_API.Models;

namespace Simple_API.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20210807081557_AddtodolistUserId")]
    partial class AddtodolistUserId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("Simple_API.Models.ToDoList", b =>
                {
                    b.Property<int>("ToDoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("TaskCompleted")
                        .HasColumnType("int");

                    b.Property<int?>("UserInfoUserId")
                        .HasColumnType("int");

                    b.HasKey("ToDoId");

                    b.HasIndex("UserInfoUserId");

                    b.ToTable("ToDoList");
                });

            modelBuilder.Entity("Simple_API.Models.UserInfo", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Password")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<int>("Privileges")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("UserInfo");
                });

            modelBuilder.Entity("Simple_API.Models.ToDoList", b =>
                {
                    b.HasOne("Simple_API.Models.UserInfo", "UserInfo")
                        .WithMany("UserToDoList")
                        .HasForeignKey("UserInfoUserId");

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("Simple_API.Models.UserInfo", b =>
                {
                    b.Navigation("UserToDoList");
                });
#pragma warning restore 612, 618
        }
    }
}