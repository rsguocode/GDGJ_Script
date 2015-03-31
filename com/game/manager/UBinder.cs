﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;

/**反序列化无法找到程序集 
 * http://blog.csdn.net/scgyyu/article/details/4232371
 * **/
namespace com.game.manager
{
    public class UBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            return ass.GetType(typeName);
        } 
    }
}
