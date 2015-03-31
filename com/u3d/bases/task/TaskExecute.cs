﻿﻿﻿﻿﻿using System;
using System.Collections.Generic;

/**任务总执行器
 * @author 陈开谦
 * @data   2013-06-16
 * **/
namespace com.u3d.bases.task
{
    //任务完成委托
    public delegate void TaskCallback(Task task);

    public class TaskExecute
    {
        internal TaskCallback callback;
        private IList<Task> npcTaskList;                          //临时变量,存储某个NPC的任务列表
        private Dictionary<String, Task> taskList;                //任务列表
        public static TaskExecute instance = new TaskExecute();


        public TaskExecute() {
            npcTaskList = new List<Task>();
            taskList = new Dictionary<String, Task>();
        }

        /**执行任务
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
			foreach(Task task in taskList.Values)
			{
				task.execute(sort,id,sum,mapId,backupId,goodsId);
			}
		}

        /**添加任务
		 * @param taskId 任务ID
		 * @param state  任务状态
		 * @param count  已领取次数
		 * @param target 条件目标,数据格式([{}]索引#已完成数量@索引#已完成数量)
		 **/
        public Task add(String taskId,int state,int count,String target){
            if (!valid(taskId)) return null;
            if(state<TaskConst.STATE_ACCEPT || state>TaskConst.STATE_UNACCEPT) return null;
			Task task=get(taskId);
            if (task != null) return task;

            task = new Task(taskId, state, target);
			taskList[taskId]=task;
			return task;
		}

        /**移除任务**/
        public bool remove(String taskId) {
            if (!valid(taskId)) return false;
            return taskList.Remove(taskId);
        }

        /**取得任务**/
        public Task get(String taskId) {
            if(!valid(taskId)) return null;
            return taskList.ContainsKey(taskId) ? taskList[taskId] : null;
        }

        /**修改任务
         * @param taskId 任务ID
         * @param state  任务状态
         * @param count  环跑次数,当任务类型为:环跑任务时有效
         * @param c      已完成的条件列表,数据格式(索引#已完成数量@索引#已完成数量)
         * **/
        public void update(String taskId,int state,int count=-1,String c=null) {
            if (state < TaskConst.STATE_ACCEPT || state > TaskConst.STATE_UNACCEPT) return;
            Task task = get(taskId);
            if(task!=null) task.update(state,count,c);
        }


        /**根据任务ID--取得其当前任务NPC ID**/
		public String getCurNpc(String taskId){
			Task task=get(taskId);
			return task!=null?task.curNpc():null; 
		}
		
		/**根据任务ID--取得其领取任务NPC ID**/
		public String getStartNpc(String taskId){
			Task task=get(taskId);
			return task!=null?task.startNpc:null; 
		}

        /**根据NPC ID--取得其任务状态
		 * @param npcId
		 * @return [0:没任务,1:可接,2:已接,3:可完成]
		 **/
		public int npcTaskState(String npcId){
			int s=0;
            int state = TaskConst.NPC_TASK_NO;
            foreach (Task task in taskList.Values)
			{
				s=task.npcTaskState(npcId);
				if(s==TaskConst.NPC_TASK_ACCPET) return s;                                                //可接
                else if (s == TaskConst.NPC_TASK_FINISH) state = s;                                       //可完成
                else if (s == TaskConst.NPC_TASK_DOING && state != TaskConst.NPC_TASK_FINISH) state = s;  //已接
			}
			return state;
		}

        /**根据NPC ID--取得其任务并按可完成，可接，已接排序
		 * @param  npcId
		 **/
		public IList<Task> npcTask(String npcId){
			npcTaskList.Clear();
            foreach (Task task in taskList.Values)
			{
                if (task.npcSayable(npcId)) npcTaskList.Add(task);
			}
            return npcTaskList;
		}

        /**取得主线任务**/
        public Task getMainTask() {
            foreach (Task task in taskList.Values)
            {
                if (task.taskType == TaskConst.MAIN_TASK) return task;
            }
            return null;
        }

        public Dictionary<String, Task> allTask {
            get { return taskList;}
        }

        /**注册任务完成回调**/
        public void addTaskFinishCallback(TaskCallback callback)
        {
            this.callback = callback;
        }

        /**初步校验任务ID值是否填写**/
        private bool valid(String str){
            return str != null && str.Trim().Length>0 ? true : false;
        }
    }
}
