﻿// <auto-generated />
using System;
using EFCoreWriting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace efcore_data_importer_JanStummer.Migrations
{
    [DbContext(typeof(ApplicationDataContext))]
    partial class ApplicationDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CountryIsoCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ParentCustomerID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("ParentCustomerID");

                    b.ToTable("Customers", t =>
                        {
                            t.HasCheckConstraint("CK_CountryIsoCode", "length(CountryIsoCode) = 2");
                        });
                });

            modelBuilder.Entity("OrderHeader", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CustomerID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DeliveryCountryIsoCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Incoterm")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateOnly>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("PaymentTerms")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.ToTable("OrderHeaders");
                });

            modelBuilder.Entity("OrderLine", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("OrderID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProductCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<double>("UnitPrice")
                        .HasColumnType("REAL");

                    b.HasKey("ID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderLines");
                });

            modelBuilder.Entity("Customer", b =>
                {
                    b.HasOne("Customer", "ParentCustomer")
                        .WithMany()
                        .HasForeignKey("ParentCustomerID");

                    b.Navigation("ParentCustomer");
                });

            modelBuilder.Entity("OrderHeader", b =>
                {
                    b.HasOne("Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("OrderLine", b =>
                {
                    b.HasOne("OrderHeader", "Order")
                        .WithMany("OrderLines")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Customer", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("OrderHeader", b =>
                {
                    b.Navigation("OrderLines");
                });
#pragma warning restore 612, 618
        }
    }
}
