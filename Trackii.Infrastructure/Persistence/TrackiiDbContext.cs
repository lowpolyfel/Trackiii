using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Trackii.Domain.Entities;

namespace Trackii.Infrastructure.Persistence;

public sealed class TrackiiDbContext : DbContext
{
    public TrackiiDbContext(DbContextOptions<TrackiiDbContext> options)
        : base(options)
    {
    }

    public DbSet<Area> Areas => Set<Area>();
    public DbSet<Family> Families => Set<Family>();
    public DbSet<Subfamily> Subfamilies => Set<Subfamily>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<RouteStep> RouteSteps => Set<RouteStep>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<WipItem> WipItems => Set<WipItem>();
    public DbSet<WipStepExecution> WipStepExecutions => Set<WipStepExecution>();
    public DbSet<ScanEvent> ScanEvents => Set<ScanEvent>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Token> Tokens => Set<Token>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TrackiiDbContext).Assembly);
    }
}
