﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZiTechDev.Data.EntityFramework
{
    public class ZiTechDevDBContextFactory : IDesignTimeDbContextFactory<ZiTechDevDBContext>
    {
        public ZiTechDevDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("ZiTechDevDB");

            var optionsBuilder = new DbContextOptionsBuilder<ZiTechDevDBContext>();
            optionsBuilder.UseSqlServer(connectionString);
            return new ZiTechDevDBContext(optionsBuilder.Options);
        }
    }
}