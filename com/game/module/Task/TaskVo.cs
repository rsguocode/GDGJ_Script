using System.Collections.Generic;
using com.game.data;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/07 02:39:29 
 * function: 任务信息封装Vo
 * *******************************************************/

namespace com.game.module.Task
{
    public enum TaskStatu
    {
        StatuUnAccept = 1,
        StatuAccepted = 2,
        StatuFinished 
    }

    public class TaskVo
    {
        public uint TaskId;
        public TaskStatu Statu;
        public SysTask SysTaskVo;
        private int _targetNpcId;
        public bool CanCommit;

        public int TargetNpcId
        {
            get
            {
                switch (Statu)
                {
                     case TaskStatu.StatuUnAccept:
                        if (SysTaskVo.trace_accept.IndexOf("n", System.StringComparison.Ordinal) == 0)
                        {
                            return int.Parse(SysTaskVo.trace_accept.Split('.')[2]);
                        }
                     break;
                     case TaskStatu.StatuAccepted:
                        if (CanCommit)
                        {
                            if (SysTaskVo.trace_com.IndexOf("n", System.StringComparison.Ordinal) == 0)
                            {
                                return int.Parse(SysTaskVo.trace_com.Split('.')[2]);
                            }
                        }
                        else
                        {
                            if (SysTaskVo.trace_uncom.IndexOf("n", System.StringComparison.Ordinal) == 0)
                            {
                                return int.Parse(SysTaskVo.trace_uncom.Split('.')[2]);
                            }
                        }
                     break;
                     case TaskStatu.StatuFinished:
                        
                     break;
                }
                return 0;
            }
        }

        public int TaskIcon
        {
            get
            {
                switch (Statu)
                {
                    case TaskStatu.StatuUnAccept:
                        if (SysTaskVo.trace_accept.IndexOf("n", System.StringComparison.Ordinal) == 0)
                        {
                            return 1;
                        }
                        break;
                    case TaskStatu.StatuAccepted:
                        if (CanCommit)
                        {
                            if (SysTaskVo.trace_com.IndexOf("n", System.StringComparison.Ordinal) == 0)
                            {
                                return 1;
                            }
                        }
                        else
                        {
                            if (SysTaskVo.trace_uncom.IndexOf("n", System.StringComparison.Ordinal) == 0)
                            {
                                return 1;
                            }
                        }
                        break;
                    case TaskStatu.StatuFinished:

                        break;
                }
                return 0;
            }
        }

        public List<uint> RelatedNpcList
        {
            get
            {
                var npcList = new List<uint>();
                if (SysTaskVo.trace_accept.IndexOf("n", System.StringComparison.Ordinal) == 0)
                {
                    npcList.Add(uint.Parse(SysTaskVo.trace_accept.Split('.')[2]));
                }
                if (SysTaskVo.trace_uncom.IndexOf("n", System.StringComparison.Ordinal) == 0)
                {
                    npcList.Add(uint.Parse(SysTaskVo.trace_uncom.Split('.')[2]));
                }
                if (SysTaskVo.trace_com.IndexOf("n", System.StringComparison.Ordinal) == 0)
                {
                    npcList.Add(uint.Parse(SysTaskVo.trace_com.Split('.')[2]));
                }
                return npcList;
            }
        }
    }
}