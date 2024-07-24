using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace MusicClubManager.Models
{
    public class Image
    {
        public int Id { get; set; }


        public required string Alt { get; set; }

        public required byte[] Content { get; set; }

        public required string ContentType { get; set; }

   
        public required DateTime Created { get; set; }

        public required DateTime Updated { get; set; }


        public IList<Artist> Artists { get; set; } = [];
    }
}
