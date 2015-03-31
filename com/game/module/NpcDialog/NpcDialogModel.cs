﻿﻿﻿using com.game.module.Task;
using com.game.module.test;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/07 09:42:12 
 * function: NPC对话框数据中心
 * *******************************************************/

namespace com.game.module.NpcDialog
{
    public class NpcDialogModel : BaseMode<NpcDialogModel>
    {
        public uint CurrentNpcId;    //当前对话NPCid
        public TaskVo CurrentTaskVo; //当前任务
    }
}