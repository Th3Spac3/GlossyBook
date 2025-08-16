using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gloss
{
    public class UniqueList<T>
    {
        List<T> list;
        List<int> counter;

        public UniqueList()
        {
            counter = new List<int>();
            list = new List<T>();
        }

        private bool HasObject(T obj)
        {
            return list.Exists(c => c.Equals(obj));
        }

        public void Add(T obj)
        {
            if (HasObject(obj)) counter[list.FindIndex(c => c.Equals(obj))]++;
            else
            {
                list.Add(obj);
                counter.Add(1);
            }
        }

        public T Get(int index)
        {
            return list[index];
        }

        public int GetCount(int index)
        {
            return counter[index];
        }

        public void Append(T[] obj)
        {
            foreach(var item in obj) Add(item);
        }

        public int Count()
        {
            return list.Count;
        }
    }
}
