using System.Collections;
using System.Collections.Generic;

namespace NServiceBusMail
{
    public class AddressList: IList<string>
    {
        List<string> innerList;

        public AddressList()
        {
             innerList = new List<string>();
        }

        public static implicit operator AddressList(string address)
        {
            return new AddressList {address};
        }
        public static implicit operator AddressList(List<string> address)
        {
            return new AddressList { innerList = new List<string>(address) };
        }
        public static implicit operator List<string>(AddressList address)
        {
            return new List<string>(address.innerList);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string item)
        {
            innerList.Add(item);
        }

        public void Clear()
        {
            innerList.Clear();
        }

        public bool Contains(string item)
        {
            return innerList.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
             innerList.CopyTo(array,arrayIndex);
        }

        public bool Remove(string item)
        {
            return innerList.Remove(item);
        }

        public int Count
        {
            get
            {
                return innerList.Count;
            }
        }

        bool ICollection<string>.IsReadOnly
        {
            get { return ((IList<string>) innerList).IsReadOnly; }
        }

        public int IndexOf(string item)
        {
            return innerList.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            innerList.Insert(index,item);
        }

        public void RemoveAt(int index)
        {
            innerList.RemoveAt(index);
        }

        public string this[int index]
        {
            get
            {
                return innerList[index];
            }
            set
            {
                innerList[index] = value;
            }
        }
    }
}