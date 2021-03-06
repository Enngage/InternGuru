﻿using System;
using Entity.Base;

namespace Service.Events
{
    public class UpdateEventArgs<T> : EventArgs where T : IEntity
    {
        public T Obj { get; set; }
        public T OriginalObj { get; set; }
    }
}
