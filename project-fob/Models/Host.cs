using System.ComponentModel.DataAnnotations;


namespace project_fob.Models 
{
    public class Host
    {
        [Key]
        public int Id { get; set; }

        public virtual User User { get; set; } 
        public virtual Meeting Meeting { get; set; }

        public Host() { }
        
        public Host(User user)
        {
            User = user;
        }

        public Host(User user, Meeting meeting)
        {
            User = user;
            Meeting = meeting;
        }
    }
}
