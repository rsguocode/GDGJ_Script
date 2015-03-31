﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;
using com.u3d.bases.debug;
using com.bases.utils;

/**单条任务执行器
 * 添  加 人：陈开谦
   任务目标: [{索引 , 条件分类 , 副本ID, 场景ID, 怪物ID|npcId|技能ID|活动ID , 物品ID , 数量|等级 }] 
 * **/
namespace com.u3d.bases.task
{
    public class Task
    {
		private int count;                              //已接任务次数(环跑任务有效)  
		private IList<TaskCondition> vector;            //任务条件列表
		private IList<String> acceptBeforeSayList;      //领取前对话列表
		private IList<String> acceptAfterSayList;       //领取后对话列表
		private IList<String> finishSayList;            //完成对话列表 
        //private IList<TaskGoodsVo> rewardGoods;         //奖励物品列表
     

        /**任务构造器
         * @param taskId 任务ID
         * @param state  任务状态
         * @param target 任务目标[{索引,目标分类,场景ID,副本ID,关卡ID,怪物ID|NPCID|技能ID,物品ID|物品分类ID,击杀数量|技能等级|次数},{}....]
         * **/
        public Task(String taskId,int state,String target){
            this.taskId = taskId;
            this.state = state;
			this.vector=new List<TaskCondition>();
            this.parserTarget(target);
		}

        /**拆分任务目标**/
        private void parserTarget(String target){
            if (StringUtils.isEmpty(target) || target.Length <= 2) return;
            target = target.Substring(1, target.Length - 2);     //去掉 ‘[]’括号
			//Log.info(this,"target:"+target);

            int index = 0;
            TaskCondition vo;
			while(target.Length>0)
			{
				index=target.IndexOf("}");
				if(index==-1) break;
				vo=new TaskCondition(target.Substring(0,index+1) );
                target = target.Substring(target.Length >= index + 2 ? index + 2 : index + 1);
				vector.Add(vo);
			}
        }

         /**处理任务
		 * @param sort     任务条件分类值
		 * @param id       目标ID(NPC ID|怪物ID|使用技能ID)
		 * @param sum      完成数量|完成标识(1:完成,0:未完成)[例如:通关副本]
		 * @param MapId    所在场景ID
		 * @param backupId 所在副本ID
		 * @param goodsId  搜集物品ID|物品分类ID
		 * **/
		public void execute(String sort,
							String id,
							int sum=1,
							String mapId=null,
							String backupId=null,
							String goodsId=null){
			//仅对‘未完成’任务执行处理
			if(state!=TaskConst.STATE_DOING) return;                     
			if(vector.Count<1)
			{//无条件,直接改为'已完成'状态
                Log.info(this, "-execute() 任务ID:" + taskId + " 无条件直接完成！");
				taskFinishCallback();
				return;
			}  
			
			int count=0;
			foreach(TaskCondition vo in vector)
			{
				vo.match(sort,id,sum,mapId,backupId,goodsId);
                if (vo.isFinish()) count++;
			}
			
			//任务条件已全部达成
            Log.info(this, "taskId:" + taskId + ",isFinish:" + (count >= vector.Count ? true : false) + ",progress:" + progress());
			if(count>=vector.Count)
			{
                Log.info(this, "-execute() 任务ID:" + taskId + " 条件已完成！");
				taskFinishCallback();
			}
		}
		
		//任务完成回调
		private void taskFinishCallback(){
			this.state=TaskConst.STATE_FINISH;
            TaskCallback callback=TaskExecute.instance.callback;
            if (callback != null) callback(this);
		}
		
		/**更新任务
		 * @param state     任务状态
		 * @param count;    已领取次数
		 * @param c         已完成的条件,格式(索引#已完成数量@索引#已完成数量)
		 * **/
		public void update(int state,int count=-1,String c=null){
            this.state = state;
            this.count = count;
			if(state==TaskConst.STATE_DOING && vector.Count<1) 
			{//无条件,直接改为已完成状态
				taskFinishCallback();
				return;
			}

            if (StringUtils.isEmpty(c) == true) return;
            String[] cols;
            String[] array = c.Split('@');
            foreach (String str in array)
            {
                cols = str.Split('#');
                if (cols == null || cols.Length<1) continue;

                foreach (TaskCondition vo in vector)
                {
                    if (vo.index == int.Parse(cols[0]))
                    {
                        vo.value = int.Parse(cols[1]);
                        break;
                    }
                }
            }

			//检测任务是否完成 
			if(state==TaskConst.STATE_DOING)
            {
				bool isPass=true;
				foreach(TaskCondition vo in vector)
                {
					if(!vo.isFinish()) 
                    {
						isPass=false;
						break;
					}
				}
				if(isPass) taskFinishCallback();
			}
		}

        /**取得当前进度**/
        public string progress() { 
            if(vector==null || vector.Count<1) return string.Empty;
            TaskCondition item = vector[0];
            return item.count > 0 ? item.value + "/" + item.count : string.Empty;
        }

		/**指定NPC可否说话
		 * @param  npcId 
		 * @return [true:是,false:否]
		 **/
		public bool npcSayable(String npcId){
            if (StringUtils.isEmpty(npcId) || !curNpc().Equals(npcId)) return false;
            if (state == TaskConst.STATE_ACCEPT && !StringUtils.isEmpty(startTalk1))   return true;
			if(state==TaskConst.STATE_DOING)                                           return true;
            if (state == TaskConst.STATE_FINISH && !StringUtils.isEmpty(endTalk))      return true;
            if (state == TaskConst.STATE_UNACCEPT && !StringUtils.isEmpty(startTalk1)) return true;
			return false;
		}
		
		/**返回NPC任务状态
		 * @param  npcId
		 * @return [0:没有,1:可接,2:已接,3:完成]
		 **/
		public int npcTaskState(String npcId){
            if (StringUtils.isEmpty(npcId) || !curNpc().Equals(npcId)) return 0;
            if (state == TaskConst.STATE_ACCEPT)   return TaskConst.NPC_TASK_ACCPET;
			if(state==TaskConst.STATE_DOING)       return TaskConst.NPC_TASK_DOING;
			if(state==TaskConst.STATE_FINISH)      return TaskConst.NPC_TASK_FINISH;
            if (state == TaskConst.STATE_UNACCEPT) return TaskConst.NPC_TASK_ACCPET;
            return TaskConst.NPC_TASK_NO;
		}
		
		/**返回当前NPC**/
		public String curNpc(){
			if(state==TaskConst.STATE_ACCEPT) return startNpc;   //领取NPC
			if(state==TaskConst.STATE_DOING)  return startNpc;   //领取NPC
			if(state==TaskConst.STATE_FINISH) return endNpc;     //完成NPC
			if(state==TaskConst.STATE_UNACCEPT) return startNpc; 
			return "";
		}
		
		/**领取任务前对话**/
		public IList<String> acceptBeforeSay(){
            if (StringUtils.isEmpty(startTalk1)) return null;
			if(acceptBeforeSayList!=null) return acceptBeforeSayList;
			acceptBeforeSayList=str2vector(startTalk1);
			return acceptBeforeSayList;
		}
		
		/**领取任务后对话**/
		public IList<String> acceptAfterSay(){
            if (StringUtils.isEmpty(startTalk2)) return null;
			if(acceptAfterSayList!=null) return acceptAfterSayList;
			acceptAfterSayList=str2vector(startTalk2);
			return acceptAfterSayList;
		}

		/**交任务对话**/
		public IList<String> finishSay(){
            if (StringUtils.isEmpty(endTalk)) return null;
			if(finishSayList!=null) return finishSayList;
			finishSayList=str2vector(endTalk);
			return finishSayList;
		}

        //把任务对话内容处理,格式：[{1,还不去？},{2,马上就去}]
		private IList<String> str2vector(String str){
			int index=0;
			String value=null;
            IList<String> vector = new List<String>(); ;
			str=str.Substring(1,str.Length-2); 

			while(str.Length>0)
			{
				index=str.IndexOf("}");
				if(index==-1) break;

                //拆分单个 {1,还不去？}
				value=str.Substring(0,index+1);
                value = value.Substring(3, value.Length - 4);  //取出  还不去？ 部分
				vector.Add(value);

                //解析完，从长串中去掉  {1,还不去？}
                str = str.Substring(str.Length >= index + 2 ? index + 2 : index + 1);
			}
			return vector;
		}
		
		/**取得奖励描述**/
        private String formatReward="";
		public String rewardMoneyDesc{
            get
            {
                if (StringUtils.isEmpty(formatReward))
                {
                    if (copper > 0) formatReward += "[name]铜钱[-] [value]" + copper + "[-] ";
                    if (diam > 0)   formatReward += "[name]元宝[-] [value]" + diam + "[-] ";
                    if (exp > 0)    formatReward += "[name]经验[-] [value]" + exp + "[-] ";
                }
                return formatReward;
            }
		}
		
		/**物品列表**/
		/*public IList<TaskGoodsVo> goodsLists(){
			 //"[{2010302458,1,0},{2010402459,1,0}]"; 
            if(goods==null || goods.Length<=2) return null;
            if (rewardGoods != null) return rewardGoods;
            
            int index = 0;
            String value = null;
            TaskGoodsVo goodsVo;
            goods = goods.Substring(1, goods.Length - 2);
            rewardGoods = new List<TaskGoodsVo>();

            while (goods.Length > 0)
            {
                index = goods.IndexOf("}");
                if (index == -1) break;

                //拆分单个 {2010302458,1,0}
                value = goods.Substring(0, index + 1);
                value = value.Substring(1, value.Length - 2);  //取出  {2010302458,1,0} 部分

                //保存物品信息
                String[] array=value.Split(',');
                goodsVo = new TaskGoodsVo();
                goodsVo.goodsId = array[0];
                goodsVo.goodsSum = int.Parse(array[1]);
                goodsVo.carser = array[2];
                rewardGoods.Add(goodsVo);

                //解析完，从长串中去掉  {2010302458,1,0}
                goods = goods.Substring(goods.Length >= index + 2 ? index + 2 : index + 1);
            }
            return rewardGoods;
		}*/
		
		/**取得首个未达成条件
		 * @return TaskCondition
		 **/
		public TaskCondition curCondition(){
			foreach(TaskCondition vo in vector)
			{
                //条件已达成
                if (vo.isFinish() ) continue;
                //仅提取 采集|收集|杀怪|通关副本 目标类别的条件
                if (vo.sort == TaskConst.C_3 ||
                    vo.sort == TaskConst.C_4 ||
                    vo.sort == TaskConst.C_5 ||
                    vo.sort == TaskConst.C_6 ||
                    vo.sort == TaskConst.C_9 ) return vo;
			}
			return null;
		}

        /**获取指定索引条件**/
        public TaskCondition getCondition(int index)
        {
            if (index < 1) return null;
            foreach (TaskCondition vo in vector)
            {
                if (vo.index == index) return vo;
            }
            return null;
        }

		/**取得任务条件列表**/
		public IList<TaskCondition> conditionList(){
			return vector;
		}
		
		/**销毁任务**/
		public void dispose(){
            if (vector!=null) vector.Clear();
            if (acceptAfterSayList != null) acceptAfterSayList.Clear();
            if (acceptBeforeSayList != null) acceptBeforeSayList.Clear();
            if (finishSayList != null) finishSayList.Clear();
            //if (rewardGoods != null) rewardGoods.Clear();
			
			vector=null;
            //rewardGoods = null;
			formatReward=null;
			finishSayList=null;
			acceptAfterSayList=null;
			acceptBeforeSayList=null;
		}

        
        /**任务ID**/
        public String taskId { get; set; }
        /**任务类型**/
        public int taskType { get; set; }
        /**任务名称**/
        public String taskName { get; set; }
        /**任务状态**/
        public int state { get; set; }
        /**任务Tips**/
        public String tip { get; set; }
        /**发布任务NPC**/
        public String startNpc { get; set; }
        /**完成任务NPC**/
        public String endNpc { get; set; }
        /**领取任务前对话**/
        public String startTalk1 { get; set; }
        /**完成任务前对话**/
        public String startTalk2 { get; set; }
        /**完成任务对话**/
        public String endTalk { get; set; }
       
        /**奖励经验**/
        public int exp { get; set; }
        /**铜钱**/
        public int copper { get; set; }
        /**奖励物品描述**/
        public String rewardGoodsDesc { get; set; }
        /**声望**/
        public int prestige { get; set; }
        /**体力**/
        public int manual { get; set; }
        /**元宝**/
        public int diam { get; set; }

        /**说明**/
        public String desc { get; set; }
        /**循环次数**/
        public int cryle { get; set; }
    }
}
