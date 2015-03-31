﻿﻿﻿using System.Collections.Generic;
﻿﻿﻿using System.IO;
﻿﻿﻿using com.game;
using com.game.module.test;
/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/31 05:43:02 
 * function: 契约数据中心
 * *******************************************************/
using Proto;

namespace Com.Game.Module.Role.Deed
{
    public class DeedMode : BaseMode<DeedMode>
    {
        public const int TypeStrength = 1;
        public const int TypePhysique = 2;
        public const int TypeAgility  = 3;
        public const int TypeIntelligence = 4;
        public Dictionary<uint, uint> DeedCountDirectory = new Dictionary<uint, uint>();        //契约签署结果字典，便于查询
        public const int EventUpdateDeedInfo = 1;
        public const int EventUpdateSelectedDeedItem = 2;                                       //更新选中的契约
        public int CurrentDeedType;
        public Dictionary<uint, List<int>> DeedValueDictionary = new Dictionary<uint, List<int>>();  //契约签署属性效果值 
        private DeedItem _currentSelectedDeedItem;
        public DeedItem CurrentSelectedDeedItem
        {
            get { return  _currentSelectedDeedItem; }
            set
            {
                _currentSelectedDeedItem = value;
                DataUpdate(EventUpdateSelectedDeedItem);
            }
        }

        public static readonly uint[,] DeedTypeIds =
        {
            {1001,1002,1003,1004,1005,1006,1007},
            {2001,2002,2003,2004,2005,2006,2007},
            {3001,3002,3003,3004,3005,3006,3007},
            {4001,4002,4003,4004,4005,4006,4007}
        };

        public string[] TypeStrings;

        public void UpdateDeedInfo(List<uint> ids, List<uint> counts)
        {
            for (var i = 0; i < ids.Count; i++)
            {
                DeedCountDirectory[ids[i]] = counts[i];
            }
            DataUpdate(EventUpdateDeedInfo);
        }

        //--------------------------------------华丽的分割线，以下是协议请求操作---------------------------------
        public void RequestDeedInfo()
        {
            var msdata = new MemoryStream();
            Module_18.write_18_1(msdata);
            AppNet.gameNet.send(msdata, 18, 1);
        }

        public void RequestDeed(uint id, uint type)
        {
            var msdata = new MemoryStream();
            Module_18.write_18_2(msdata,id, type);
            AppNet.gameNet.send(msdata, 18, 2);
        }

    }
}