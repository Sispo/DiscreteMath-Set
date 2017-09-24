using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication51
{
    class Program
    {
        static void Main(string[] args)
        {
            Group fourGroup = new Group(4);

            fourGroup.analyse();

            Group sixGroup = new Group(6);

            sixGroup.analyse();
        }

       
    }
}
