using Entity;
using System;

namespace Service.Events
{
    public class InsertEventArgs<T> : EventArgs where T : IEntity
    {
        public T Obj { get; set; }
    }
}
