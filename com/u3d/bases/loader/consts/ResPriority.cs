﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**加载优先级常量**/
namespace com.u3d.bases.loader.consts
{
    public class ResPriority
    {
        public const int LOW      = 500;       //最低优先级 
        public const int SOUND    = 1000;      //声音优先级 
        public const int BINARY   = 1100;      //二进制流优先级
        public const int EFFECT   = 1200;      //普通特效优先级 
        public const int XML      = 1300;      //XML文件优先级 
 
        public const int IMGAE    = 1400;      //材质贴图优先级
        public const int NPC      = 2000;      //NPC优先级
        public const int WEAPON   = 3000;      //武器优先级  
        public const int MONSTER  = 4000;      //怪物优先级  
        public const int HORSE    = 5000;      //坐骑优先级  

        public const int ROLE     = 6000;      //角色优先级  
        public const int ME_ROLE  = 6100;      //本角色优先级  
        public const int SKILL    = 7000;      //技能优先级 
        public const int ME_SKILL = 7100;      //本角色技能优先级 
        public const int UI       = 8000;      //UI优先级 

        internal const int SYSTEM = 9000;      //最高优先级	
    }
}
