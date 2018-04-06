using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuathlonManager2.Model
{
    public class Starter
    {
        public int StartNumber { get; set; }
        public Competition Competition { get; set; }
        public List<Person> Persons { get; set; }
        public string TeamName { get; set; }
        public Result Result { get; set; }
    }
}
