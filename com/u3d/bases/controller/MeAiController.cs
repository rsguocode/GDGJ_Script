using com.game.module.battle;
﻿﻿﻿using com.game.module.test;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/28 04:54:12 
 * function: 主角的AI控制，继承自玩家的AI控制
 * *******************************************************/

namespace com.u3d.bases.controller
{
    public class MeAiController : JianShiAiController
    {

        /// <summary>
        /// 设置怪物Ai
        /// </summary>
        public override void SetAi(bool value)
        {
           base.SetAi(value);
           Singleton<BattleMode>.Instance.IsAutoSystem = value;
        }  
    }
}