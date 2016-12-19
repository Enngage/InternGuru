﻿using Entity;
using System;

namespace Service.Events
{
    public class DeleteEventArgs<T> : EventArgs where T : IEntity
    {
        public T Obj { get; set; }
    }
}