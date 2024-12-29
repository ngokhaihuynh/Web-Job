using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebJob.Models.EF;

namespace WebJob.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Phone { get; set; }

        public bool IsVerified { get; set; }
        public DateTime? CreatedDate { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Adv> Ads { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyImage> CompanyImages { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Location> Locations { get; set; }  
        public DbSet<Level> Levels { get; set; }
        public DbSet<JobCategory> JobCategories { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobSkill> JobSkills { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<EmailSubscription> EmailSubscriptions { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<CategoryProduct> categoryProducts { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
        public DbSet<ProductImage> productImages { get; set; }
        public DbSet<ThongKe> thongKes { get; set; }
        public DbSet<Message> message { get; set; }
        public DbSet<Conversation> conversations { get; set; }
        public DbSet<EmployerVerification> employerVerifications { get; set; }

        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}