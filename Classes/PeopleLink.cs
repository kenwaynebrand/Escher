using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escher_Test.Classes
{
    public class PeopleLink
    {
        public PeopleLink() { }
        public Guid PersonID { get; set; } // Id to the Person linked to this Person
        public bool IsMain { get; set; }  // Flag denoting that this linked person is in the mainfile
    }
}
