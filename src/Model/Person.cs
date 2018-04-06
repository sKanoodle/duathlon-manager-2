using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuathlonManager2.Model
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Club { get; set; }
        public int YearOfBirth { get; set; }
        public Sex Sex { get; set; }
    }
}
