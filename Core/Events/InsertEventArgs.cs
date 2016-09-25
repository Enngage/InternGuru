using Entity;
using System;

namespace Core.Events
{
    public class InsertEventArgs<T> : EventArgs where T : IEntity
    {
        public T Obj { get; set; }
    }
}
