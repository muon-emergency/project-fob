using project_fob.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace project_fob.DAL {
	public class FobContext : DbContext{



		public FobContext() : base("FobContext")
		{
			Database.CommandTimeout = 250;
		}
		public DbSet<User> User { get; set; }
		public DbSet<Attendee> Attendee { get; set; }
		public DbSet<Host> Host { get; set; }
		public DbSet<Meeting> Meeting { get; set; }
		public DbSet<Fob> Fob { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}



	}
}