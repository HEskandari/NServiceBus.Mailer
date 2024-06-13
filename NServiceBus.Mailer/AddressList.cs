namespace NServiceBus.Mailer
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Store e-mail addresses that are associated with an e-mail message.
    /// </summary>
    public class AddressList : IList<string>
    {
        List<string> innerList = new List<string>();

        /// <summary>
        /// A helper to convert a <see cref="string"/> to a <see cref="AddressList"/>.
        /// </summary>
        public static implicit operator AddressList(string address)
        {
            return new AddressList
            {
                address
            };
        }

        /// <summary>
        /// A helper to convert a <see cref="List{T}"/> to a <see cref="AddressList"/>.
        /// </summary>
        public static implicit operator AddressList(List<string> address)
        {
            return new AddressList
            {
                innerList = new List<string>(address)
            };
        }

        /// <summary>
        /// A helper to convert an <see cref="AddressList"/> to a <see cref="List{T}"/>.
        /// </summary>
        public static implicit operator List<string>(AddressList address)
        {
            return new List<string>(address.innerList);
        }

        /// <summary>
        /// <see cref="IEnumerable{T}.GetEnumerator"/>
        /// </summary>
        public IEnumerator<string> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        /// <summary>
        /// <see cref="ICollection{T}.Add"/>
        /// </summary>
        public void Add(string item)
        {
            innerList.Add(item);
        }

        /// <summary>
        /// <see cref="ICollection{T}.Clear"/>
        /// </summary>
        public void Clear()
        {
            innerList.Clear();
        }

        /// <summary>
        /// <see cref="ICollection{T}.Contains"/>
        /// </summary>
        public bool Contains(string item)
        {
            return innerList.Contains(item);
        }

        /// <summary>
        /// <see cref="ICollection{T}.CopyTo"/>
        /// </summary>
        public void CopyTo(string[] array, int arrayIndex)
        {
            innerList.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// <see cref="ICollection{T}.Remove"/>
        /// </summary>
        public bool Remove(string item)
        {
            return innerList.Remove(item);
        }

        /// <summary>
        /// <see cref="ICollection{T}.Count"/>
        /// </summary>
        public int Count => innerList.Count;

        bool ICollection<string>.IsReadOnly => ((IList<string>)innerList).IsReadOnly;

        /// <summary>
        /// <see cref="IList{T}.IndexOf"/>
        /// </summary>
        public int IndexOf(string item)
        {
            return innerList.IndexOf(item);
        }
        /// <summary>
        /// <see cref="IList{T}.Insert"/>
        /// </summary>
        public void Insert(int index, string item)
        {
            innerList.Insert(index, item);
        }

        /// <summary>
        /// <see cref="IList{T}.RemoveAt"/>
        /// </summary>
        public void RemoveAt(int index)
        {
            innerList.RemoveAt(index);
        }

        /// <summary>
        /// <see cref="IList{T}.this"/>
        /// </summary>
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