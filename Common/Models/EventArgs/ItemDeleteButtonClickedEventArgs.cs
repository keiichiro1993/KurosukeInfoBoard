using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.EventArgs
{
    public class ItemDeleteButtonClickedEventArgs<T> : System.EventArgs
    {
        private T item;

        public ItemDeleteButtonClickedEventArgs(T item)
        {
            this.item = item;
        }

        public T DeleteItem
        {
            get { return item; }
        }
    }
}
