using System.ComponentModel.DataAnnotations.Schema;

namespace FerryToHelsinkiWebsite.Data.Models
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserName { get; set; }
        public string MessageContents { get; set; }
    }
}
