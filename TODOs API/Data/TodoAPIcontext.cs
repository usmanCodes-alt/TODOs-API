using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TODOs_API.Models;

namespace TODOs_API.Data
{
    public class TodoAPIcontext : DbContext
    {
        public TodoAPIcontext(DbContextOptions<TodoAPIcontext> contextOptions) : base(contextOptions) { }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
