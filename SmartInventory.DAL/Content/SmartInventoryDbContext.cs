using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Content
{ 
public class SmartInventoryDbContext : IdentityDbContext<ApplicationUser,IdentityRole,string>
    {
    public SmartInventoryDbContext(DbContextOptions<SmartInventoryDbContext> options) : base(options)
    {

    }
    DbSet<Product> Products { get; set; }
} }

