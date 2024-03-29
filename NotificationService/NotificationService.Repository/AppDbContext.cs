﻿using Microsoft.EntityFrameworkCore;
using NotificationService.Model;
using NotificationService.Model.Sync;

namespace NotificationService.Repository
{
    public class AppDbContext : DbContext, IAppDbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

        public DbSet<NotificationConfig> NotificationConfigs { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Connection> Connections { get; set; }
    }
}
