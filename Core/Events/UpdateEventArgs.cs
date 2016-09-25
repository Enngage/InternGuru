using Entity;
using System;

namespace Core.Events
{
    public class UpdateEventArgs<T> : EventArgs where T : IEntity
    {
        public T Obj { get; set; }
    }
}
