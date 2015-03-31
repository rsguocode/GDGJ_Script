﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.u3d.bases.interfaces;
using com.bases.utils;
using com.u3d.bases.debug;

/**对象池管理**/
namespace com.u3d.bases.pool
{
    public class ObjectPoolMgr
    {
        private static Dictionary<string, ObjectPool> pool = new Dictionary<string, ObjectPool>();

		/**取出**/
		public static IPoolable get(string className)
		{
            IPoolable poolable = getPool(className).get();
            poolable.getBefore();
            //Log.info("ObjectPoolMgr", "-get() className:" + className + ",池中取出对象:" + poolable);
            return poolable;
		}
		
		/**归池**/
		public static void put(string className,IPoolable poolable)
		{
            if (poolable == null) return;
            if (StringUtils.isEmpty(className)) return;
            //Log.info("ObjectPoolMgr", "-put() className:" + className + ",返回池中对象:" + poolable);
			getPool(className).put(poolable);
		}
		
		private static ObjectPool getPool(string className){
            ObjectPool p = pool.ContainsKey(className) ? pool[className] : null;
            if (p == null) 
			{
                p = new ObjectPool(className);
                pool[className] = p;
			}
			return p;
		}
    }
}
