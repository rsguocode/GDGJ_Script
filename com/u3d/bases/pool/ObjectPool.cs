﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using com.u3d.bases.interfaces;

/**对象缓存池**/
namespace com.u3d.bases.pool
{
    public class ObjectPool
    {
        public string className;
        public IList<IPoolable> list;

        public ObjectPool(string className)
        {
            this.className = className;
            list = new List<IPoolable>();
        }

        public IPoolable get()
        {
            if (list.Count < 1)
            {
                return (IPoolable)Activator.CreateInstance(Type.GetType(className));
            }
            IPoolable clz = list[0];
            list.RemoveAt(0);
            return clz;
        }

        public void put(IPoolable obj)
        {
            if (obj == null) return;
            list.Add(obj);
        }
    }
}
