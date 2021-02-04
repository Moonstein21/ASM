using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testad.Models;


namespace testad.Models.ViewModels
{
    public class IndexViewModel3 : InformationSystem
    {
        public IEnumerable<Request> Requests { get; set; }

        public IEnumerable<Subdivition> Subdivitions { get; set; }

        public IEnumerable<PostList> PostLists { get; set; }

        public IEnumerable<InformationSystem> InformationSystems { get; set; }

        public IEnumerable<DocumentsIB> DocumentsIBs { get; set; }

        public IEnumerable<law> Laws { get; set; }
        public IEnumerable<Tablelaw> Tablelaws { get; set; }
        public IEnumerable<Rool> Rools { get; set; }
    }
}