using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Funeral_Policy.Models;
using Microsoft.PowerBI.Api;
using Funeral_Policy.Models.CartModels;

namespace IdentitySample.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
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

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        //public DbSet<Admin> Admins { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        //public DbSet<IdentityRole> IdentityRoles { get; set; }
        public DbSet<Member> members { get; set; }
        public DbSet<AdditionalMemberView> AdditionalMembers { get; set; }
        public System.Data.Entity.DbSet<Funeral_Policy.Models.MemberApplication> MemberApplications { get; set; }

        public System.Data.Entity.DbSet<Funeral_Policy.Models.FuneralCoverPayout> funeralCoverPayouts { get; set; }

        public System.Data.Entity.DbSet<Funeral_Policy.Models.FuneralPlan> funeralPlans { get; set; }
        public System.Data.Entity.DbSet<Funeral_Policy.Models.Quote> quotes { get; set; }

        public System.Data.Entity.DbSet<Funeral_Policy.Models.Coffin> Coffins { get; set; }

        public System.Data.Entity.DbSet<Funeral_Policy.Models.FamilyMember> FamilyMembers { get; set; }

        public System.Data.Entity.DbSet<Funeral_Policy.Models.FuneralBooking> FuneralBookings { get; set; }

        public System.Data.Entity.DbSet<Funeral_Policy.Models.FuneralType> FuneralTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Cart_Item> Cart_Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_Item> Order_Items { get; set; }
        public DbSet<Order_Tracking> Order_Trackings { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Shipping_Address> Shipping_Addresses { get; set; }
       
        public System.Data.Entity.DbSet<Funeral_Policy.Models.Staff> Staffs { get; set; }

        public System.Data.Entity.DbSet<Funeral_Policy.Models.AssignStaff> AssignStaffs { get; set; }

        public System.Data.Entity.DbSet<Funeral_Policy.Models.CartModels.FuneralOrder> FuneralOrders { get; set; }
        public DbSet<TombstoneOrder> TombstoneOrders { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        public System.Data.Entity.DbSet<Funeral_Policy.Models.Beneficiary> Beneficiaries { get; set; }

        public System.Data.Entity.DbSet<Funeral_Policy.Models.Claim> Claims { get; set; }

        public System.Data.Entity.DbSet<Funeral_Policy.Models.MemberType> MemberTypes { get; set; }
    }
}