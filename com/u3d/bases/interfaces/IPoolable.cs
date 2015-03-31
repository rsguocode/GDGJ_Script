﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**归池接口**/
namespace com.u3d.bases.interfaces
{
    public interface IPoolable
    {
        /**从池中取出前操作**/
		void getBefore();
		/**归池**/
		void Dispose();
    }
}
