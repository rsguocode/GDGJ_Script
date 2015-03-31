﻿﻿﻿using com.game.data;
﻿﻿﻿using com.game.manager;
﻿﻿﻿using com.u3d.bases.display.vo;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/02/26 03:53:52 
 * function: 陷阱对应的VO
 * *******************************************************/

namespace com.game.vo
{
    public class TrapVo :BaseRoleVo
    {

        private SysTrap _sysTrapVo;
        public uint TrapId;    //陷阱类型ID

        /// <summary>
        /// 缓存SysTrapVo信息，避免使用到陷阱相关信息时多次查表
        /// </summary>
        public SysTrap SysTrapVo
        {
            get
            {
                if (_sysTrapVo == null)
                {
                    _sysTrapVo = BaseDataMgr.instance.GetTrapVoById(TrapId);
                }
                return _sysTrapVo;
            }
        }
    }
}