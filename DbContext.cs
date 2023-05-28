﻿using HCI.Models.Attractions.Model;
using HCI.Models.Locations.Model;
using HCI.Models.Restaurants.Model;
using HCI.Models.Users.Model;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Attraction> Attractions { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("HCI");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
        new User
        {
            Id = 1,
            Email = "vanja.client@example.com",
            Password = "password123",
            Name = "Vanja",
            Surname = "Client",
            Type = UserType.Client
        },
        new User
        {
            Id = 2,
            Email = "vanja.agent@example.com",
            Password = "password456",
            Name = "Vanja",
            Surname = "Agent",
            Type = UserType.Agent
        }
);
    }
}