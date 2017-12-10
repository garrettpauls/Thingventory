using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Thingventory.Core.Data;

namespace Thingventory.Core.Migrations
{
    [DbContext(typeof(ThingDataContext))]
    [Migration("20171210004214_AddNotesToLocation")]
    partial class AddNotesToLocation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.4");

            modelBuilder.Entity("Thingventory.Core.Data.ItemEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<DateTimeOffset>("CreatedInstant")
                        .HasColumnName("CreatedInstant");

                    b.Property<int>("LocationId")
                        .HasColumnName("LocationId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name");

                    b.Property<DateTimeOffset>("UpdatedInstant")
                        .HasColumnName("UpdatedInstant");

                    b.HasKey("Id");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("Thingventory.Core.Data.LocationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnName("Notes");

                    b.HasKey("Id");

                    b.ToTable("Location");
                });
        }
    }
}
