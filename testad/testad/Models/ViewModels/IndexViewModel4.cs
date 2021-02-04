using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testad.Models;


namespace testad.Models.ViewModels
{
    public class IndexViewModel4 : DLP
    {
        public IEnumerable<DLP> DLPs { get; set; }

        public IEnumerable<DLPDark> DLPDarks { get; set; }
        public IEnumerable<PostList> PostLists { get; set; }


    }
}