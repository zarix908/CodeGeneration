﻿using System.Collections.Generic;

namespace CodeGenerator
{
    public static class QueueExtension
    {
        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                queue.Enqueue(item);
            }
        } 
    }
}
