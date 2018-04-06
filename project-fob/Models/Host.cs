using System.ComponentModel.DataAnnotations;


namespace project_fob.Models
{
    public class Host
    {
        [Key]
        public int Id { get; set; }
        public Meeting Meeting { get; set; }

        public Host() { }
        

        public Host(string user, Meeting meeting)
        {
            Meeting = meeting;
        }
    }
}
