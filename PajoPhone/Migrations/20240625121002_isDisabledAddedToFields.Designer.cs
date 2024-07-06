﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PajoPhone.Models;

#nullable disable

namespace PajoPhone.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240625121002_isDisabledAddedToFields")]
    partial class isDisabledAddedToFields
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PajoPhone.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("PajoPhone.Models.FieldsKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("isDisabled")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CategoryId", "Key")
                        .IsUnique()
                        .HasFilter("[isDisabled] = 0");

                    b.ToTable("FieldsKeys");
                });

            modelBuilder.Entity("PajoPhone.Models.FieldsValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("FieldKeyId")
                        .HasColumnType("int");

                    b.Property<int>("IntValue")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("StringValue")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("FieldKeyId");

                    b.HasIndex("ProductId");

                    b.ToTable("FieldsValues");
                });

            modelBuilder.Entity("PajoPhone.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<double>("Price")
                        .HasColumnType("double");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PajoPhone.Models.Category", b =>
                {
                    b.HasOne("PajoPhone.Models.Category", "ParentCategory")
                        .WithMany("ChildCategories")
                        .HasForeignKey("ParentCategoryId");

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("PajoPhone.Models.FieldsKey", b =>
                {
                    b.HasOne("PajoPhone.Models.Category", "Category")
                        .WithMany("FieldsKeys")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("PajoPhone.Models.FieldsValue", b =>
                {
                    b.HasOne("PajoPhone.Models.FieldsKey", "FieldKey")
                        .WithMany()
                        .HasForeignKey("FieldKeyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PajoPhone.Models.Product", "Product")
                        .WithMany("FieldsValues")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FieldKey");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("PajoPhone.Models.Product", b =>
                {
                    b.HasOne("PajoPhone.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("PajoPhone.Models.Category", b =>
                {
                    b.Navigation("ChildCategories");

                    b.Navigation("FieldsKeys");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("PajoPhone.Models.Product", b =>
                {
                    b.Navigation("FieldsValues");
                });
#pragma warning restore 612, 618
        }
    }
}
