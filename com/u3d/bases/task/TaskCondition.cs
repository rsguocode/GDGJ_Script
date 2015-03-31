﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.u3d.bases.debug;
using com.bases.utils;

/**单条任务--条件处理器
 * @author 陈开谦
 * @date   2013-06-16
 *  条件分类以及填写模板：
    3--击杀怪物         [{索引 , 3 , 副本ID , 场景ID , 怪物ID , 0        , 击杀数量 }]
    4--采集物品         [{索引 , 4 , 0      , 场景ID , 物品ID , 采集数量 , 0        }]
    5--收集物品         [{索引 , 5 , 副本ID , 场景ID , 怪物ID , 物品ID   , 收集数量 }]
    6--检查背包物品     [{索引 , 6 , 0      , 0      , 0      , 物品ID   , 数量     }]
    7--主角升级         [{索引 , 7 , 0      , 0      , 0      , 0        , 等级     }]
    8--使用技能         [{索引 , 8 , 0      , 0      , 技能ID , 0        , 技能等级 }]
    9--通关副本         [{索引 , 9 , 副本ID , 场景ID , 0      , 0        , 0        }]
    10--活动类          [{索引 , 10, 0      , 场景ID , 活动ID , 0        , 0        }]
 * **/
namespace com.u3d.bases.task
{
    public class TaskCondition
    {
		public String sort;       //条件分类值
        public String targetId;   //目标ID(npcId、怪物ID、技能ID)
		public String mapId;      //场景ID
		public String backupId;   //副本ID
		public String goodsId;    //物品ID

        internal int index;       //索引号
        internal int count;       //杀怪数量、主角等级、技能等级
        internal int value;       //已完成数量|通过标识(1:已通关,0:未通关)


        /**构造条件
         *@param target 目标条件[{索引 , 条件分类 , 副本ID, 场景ID, 怪物ID|npcId|技能ID|活动ID , 物品ID , 数量|等级 }]
         ***/
        public TaskCondition(String target){
			target=target.Substring(1,target.Length-2);
            //Log.info(this, "condition:" + target);
            this.init(target.Split(','));
		}

		private void init(String[] split){
			this.index=int.Parse(split[0]);   //索引
			this.sort=split[1];               //条件分类值
            this.backupId=split[2];           //副本ID
            this.mapId = split[3];              //场景ID
            this.targetId = split[4];         //目标ID [怪物ID|NPCID|技能ID|活动ID ]
			this.goodsId=split[5];            //物品ID
			this.count=int.Parse(split[6]);   //数量|等级 
		}

		/**条件达成结果
		 * @return [true:已达成,false:未达成]
		 * **/
		public bool isFinish(){
            if (count < 1)
            {//使用bool比较 [0:未通过,>=1:已通过],像:通关副本
                if (value >= 1) return true;
            }
            else
            {//数值|数量比较 像:杀怪数量
                if (value >= count) return true;
            }
            return false;
		}

		/**匹配条件
		 * @param sort     条件分类值
		 * @param id       目标ID(NPC ID|怪物ID|技能ID|物品ID)
		 * @param sum      完成数量(NPC对话完成固定传1，表示完成)
		 * @param backupId 所在副本ID
		 * @param MapId    所在场景ID
		 * @param goodsId  物品ID
		 * **/
        internal void match(String sort,
                            String id,
                            int sum = 1,
                            String mapId = null,
                            String backupId = null, 
                            String cardId = null,
                            String goodsId = null) 
        {
            if (!StringUtils.isEquals(sort, this.sort)) return;

            //(主角升级|通关副本|参加活动) 不做目标ID判断
            if (!sort.Equals(TaskConst.C_7) && 
                !sort.Equals(TaskConst.C_9) && 
                !sort.Equals(TaskConst.C_10) )
            {
                if (!StringUtils.isEquals(id, this.targetId)) return;
            }

            //收集物品|检查容器内物品 判断物品ID
            if (sort.Equals(TaskConst.C_4) ||
                sort.Equals(TaskConst.C_5) ||
                sort.Equals(TaskConst.C_6) )
            {
                if (!StringUtils.isEquals(goodsId, this.goodsId)) return;
            }

            //杀怪|收集物品|通关副本 判断场景ID和副本ID
            if (sort.Equals(TaskConst.C_3) ||
                sort.Equals(TaskConst.C_5) ||
                sort.Equals(TaskConst.C_9) )
            {
                if (!StringUtils.isEquals(mapId, this.mapId))       return;
                //if (!StringUtils.isEquals(backupId, this.backupId)) return;
            }
            this.value += sum;
        }

    }
}
