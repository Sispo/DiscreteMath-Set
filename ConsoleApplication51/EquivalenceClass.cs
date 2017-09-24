using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication51
{
    public class EquivalenceClass
    {
        public WeakReference groupRef;

        public int remainder;

        public List<int> list;

        public EquivalenceClass(WeakReference groupRef, int remainder)
        {
            this.groupRef = groupRef;
            this.remainder = remainder;

            this.generateList();
        }

        ~EquivalenceClass()
        {
            (groupRef.Target as Group).classesList.Remove(this);
        }

        void generateList()
        {
            this.list = new List<int>();

            for (int i = 1; i < 5; i++)
            {
                int m = (groupRef.Target as Group).m;
                int num = remainder + m * i;
                list.Add(num);
                list.Add(-num);
            }
            list.Sort();
        }

        public static void checkClasses(EquivalenceClass e1, EquivalenceClass e2)
        {
            if ((e1.groupRef.Target as Group).m != (e2.groupRef.Target as Group).m)
            {
                throw new Exception("You can't add two classes of different groups! Only same group classes are allowed for operations.");
            }
        }

        public static EquivalenceClass operator +(EquivalenceClass e1, EquivalenceClass e2)
        {
            checkClasses(e1, e2);

            int remainder = (e1.list.First() + e2.list.First()) % (e1.groupRef.Target as Group).m;
            remainder = remainder > 0 ? remainder : -remainder;

            return new EquivalenceClass(e1.groupRef, remainder);
        }

        public static EquivalenceClass operator *(EquivalenceClass e1, EquivalenceClass e2)
        {
            checkClasses(e1, e2);

            int remainder = (e1.list.First() * e2.list.First()) % (e1.groupRef.Target as Group).m;
            return new EquivalenceClass(e1.groupRef, remainder);
        }

        public static bool operator ==(EquivalenceClass e1, EquivalenceClass e2)
        {
            return e1.remainder == e2.remainder;
        }

        public static bool operator !=(EquivalenceClass e1, EquivalenceClass e2)
        {
            return e1.remainder != e2.remainder;
        }

        public override string ToString()
        {
            string desc = $"\nCLASS {remainder}:\n";

            foreach(int num in list)
            {
                desc += num + " ";
            }
            
            return desc + "\n";
        }
    }
}
