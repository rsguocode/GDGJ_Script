﻿﻿﻿using System;
using UnityEngine;
using com.game.utils;
using com.u3d.bases.debug;
using com.u3d.bases.task;
using System.Collections.Generic;
using com.game.manager;
using com.game.data;

using com.game.module;
using com.game.ui;
using com.game.autoroad;
using com.u3d.bases.loader;
using System.Collections;
using com.game.consts;

namespace com.game.dialog
{
    /// <summary>
    /// NPC对话框。
    /// </summary>
    //public class NpcDialog : BaseView
    //{
    //    private int step = -1;                         //当前对话步长
    //    private Task task = null;                      //当前查看任务
    //    private IList<String> talkList;                //当前对话列表
    //    private IList<String> npcTask;                 //当前NPC的任务列表

    //    private String npcId;                          //当前Npc Id   
    //    public static NpcDialog instance;

    //    public static String NAME = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;

    //    //prefabs
    //    public const string URL = "prefab/ui/dialog/npc/npcdialog";
    //    public const string ITEM_URL = "prefab/ui/dialog/npc/widget/item";
    //    public const string ICON_URL = "texture/ui/npc/task";

    //    //layout
    //    public const float width = 527f;
    //    public const float height = 56f;
    //    public const float space = 5f;

    //    public GameObject closeGo;
    //    public GameObject itemsGo;
    //    public UISprite icon;           //半身像
    //    public UILabel talk;            //任务对话|NPC默认对话
    //    public UILabel reward;          //任务奖励 

    //    protected AtlasMgr atlasMgr;

    //    private GameObject groupGo;
    //    private GameObject itemGo;
    //    private Grid grid;

    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    private NpcDialog() { }

    //    /// <summary>
    //    /// 初始化
    //    /// </summary>
    //    protected override void init()
    //    {
    //        //variables
    //        instance = this;
    //        atlasMgr = new AtlasMgr(ICON_URL, 9, 10000);

    //        //load all widgets
    //        itemGo = (GameObject)ResMgr.instance.load(ITEM_URL);

    //        //components
    //        groupGo = NGUITools.AddChild(itemsGo);
    //        groupGo.name = "group";

    //        grid = groupGo.AddComponent<Grid>();
    //        grid.cellWidth = width;
    //        grid.cellHeight = height + space;
    //        grid.maxPerLine = 1;
    //        grid.sorted = true;
    //        grid.repositionNow = true;

    //        npcTask = new List<String>();

    //        //register all events
    //        addOnClickEvent(closeGo, onClose);

    //        //flag
    //        hasInited = true;

    //        close();
    //    }

    //    /// <summary>
    //    /// 添加项目
    //    /// </summary>
    //    /// <param name="objectName">游戏物体名称</param>
    //    /// <param name="name">项目名称</param>
    //    /// <returns>返回添加的项目</returns>
    //    public GameObject addItem(string objectName, string name)
    //    {
    //        GameObject child = NGUITools.AddChild(groupGo, itemGo);
    //        child.name = objectName;

    //        UILabel lab = child.GetComponentInChildren<UILabel>();
    //        lab.text = name;
    //        grid.repositionNow = true;

    //        return child;
    //    }

    //    /// <summary>
    //    /// 移除项目
    //    /// </summary>
    //    /// <param name="objectName">游戏物体名称</param>
    //    public void removeItem(string objectName)
    //    {
    //        GameObject child = Tools.find(groupGo, objectName);
    //        NGUITools.Destroy(child);

    //        grid.repositionNow = true;
    //    }

    //    /// <summary>
    //    /// 关闭对话框
    //    /// </summary>
    //    public override void close()
    //    {
    //        clearTask();
    //        base.close();
    //    }

    //    /// <summary>
    //    /// 显示对话框
    //    /// </summary>
    //    /// <param name="npcId">NPC编号</param>
    //    public void show(string npcId)
    //    {
    //        //NpcVo npcVo = BaseDataMgr.instance.getNpcVo(npcId);
    //        //icon.atlas = atlasMgr.getAtlas(Convert.ToInt32(npcVo.head));
    //        //icon.spriteName = npcVo.head;
    //        //icon.MakePixelPerfect();

    //        //base.show();
    //    }

    //    /// <summary>
    //    /// 弹出对话框
    //    /// </summary>
    //    /// <param name="npcId">与npc相对应的id</param>
    //    public void showDialog(String npcId)
    //    {
    //        //this.npcId = npcId;
    //        //NpcVo npcVo = BaseDataMgr.instance.getNpcVo(npcId);
    //        //if (npcVo == null) return;
    //        //talk.text = npcVo.name + "：" + npcVo.talk;
    //        //show(npcId);
    //        //showTask(npcId);
    //    }

    //    /**移除指定任务**/
    //    public void removeTask(String taskId)
    //    {
    //        removeItem(taskId);
    //        clearTask();
    //        if (gameObject.activeSelf) showDialog(npcId);
    //    }


    //    //================任务处理==================//
    //    /**显示NPC的任务**/
    //    private void showTask(String npcId)
    //    {
    //        IList<Task> list = TaskExecute.instance.npcTask(npcId);
    //        clearTask();

    //        GameObject go = null;
    //        foreach (Task item in list)
    //        {//列出NPC中任务
    //            string taskState="(" + TaskConst.getTaskState(item.state) + ")";
    //            taskState = "[" + (item.state == TaskConst.STATE_FINISH ? ColorConst.C_00FF00 : ColorConst.FFFF00) + "]" + taskState + "[-]";   //完成为绿色，其他状态为黄色 
    //            go = addItem(item.taskId, "[" + ColorConst.FFFF00 + "][" + item.taskId + "]" + item.taskName + "[-]" + taskState);
    //            go = Tools.find(go, "ibtn_item");
    //            addOnClickEvent(go, onClickItem);
    //            npcTask.Add(item.taskId);
    //        }
    //    }

    //    /**清除任务数据**/
    //    private void clearTask()
    //    {
    //        step = -1;
    //        task = null;
    //        if (reward != null) reward.text = "";
    //        if (itemsGo == null) return;
    //        if (npcTask != null) npcTask.Clear();
    //        List<GameObject> list = Tools.getChildren(groupGo);
    //        if (list == null || list.Count < 1) return;
    //        foreach (GameObject item in list) Destroy(item);
    //    }

    //    /**移除(除了当前查看的任务)**/
    //    private void removeNpcTaskExsc(String taskId)
    //    {
    //        foreach (String id in npcTask)
    //        {
    //            if (!StringUtils.isEquals(id, taskId)) removeItem(id);
    //        }
    //    }

    //    /**显示任务明细信息**/
    //    private void onClickItem(GameObject go)
    //    {
    //        String taskId = go.transform.parent.name;
    //        Task item = TaskExecute.instance.get(taskId);
    //        //Log.info(this, "-onClickBtnItem() 点击任务ID:" + taskId + ",rewardDesc:" + (item != null ? item.getRewardDesc() : null));

    //        if (item != null)
    //        {
    //            //第一次进入查看任务明细
    //            if (step == -1)
    //            {
    //                this.task = item;
    //                removeNpcTaskExsc(taskId);
    //                if (task.state == TaskConst.STATE_ACCEPT) talkList = task.acceptBeforeSay();
    //                if (task.state == TaskConst.STATE_DOING)  talkList = task.acceptAfterSay();
    //                if (task.state == TaskConst.STATE_FINISH) talkList = task.finishSay();
    //            }
    //            string moneyReward = task.rewardMoneyDesc.Replace("name", ColorConst.FFFF00);
    //            moneyReward = moneyReward.Replace("value", ColorConst.C_00FF00);
    //            reward.text = "奖励：" + moneyReward + task.rewardGoodsDesc;
    //            GameObject labelGo = Tools.find(Tools.getParent(go), "label");
    //            nextTalkForTask(labelGo.GetComponent<UILabel>());
    //        }
    //    }

    //    /**查看下一句对话**/
    //    private void nextTalkForTask(UILabel label)
    //    {
    //        ++step;

    //        if (talkList == null || talkList.Count < 1)
    //        {
    //            close();
    //            return;
    //        }
    //        if (step >= talkList.Count / 2)
    //        {//对话完成,提交任务 
    //            submitTask();
    //            close();
    //            return;
    //        }
    //        talk.text = talkList[step * 2];
    //        label.text = "[" + ColorConst.FFFF00 + "][" + TaskConst.getTaskType(task.taskType) + "][-]" + talkList[step * 2 + 1];
    //        //Log.info(this, "-nextTalkForTask() talk:" + talkList[step * 2] + ",label:" + talkList[step * 2 + 1]);
    //    }

    //    /**提交任务到后端**/
    //    private void submitTask()
    //    {
    //        switch (task.state)
    //        {
    //            //领取任务发送
    //            case TaskConst.STATE_ACCEPT:
    //                //removeItem(task.taskId);
    //                //taskMode.receiveTask(task.taskId);
    //                break;
    //            //任务进行中,追踪任务
    //            case TaskConst.STATE_DOING:
    //                AutoRoad.intance.createTaskPath(task.taskId);
    //                break;
    //            //完成任务发送
    //            case TaskConst.STATE_FINISH:
    //                //removeItem(task.taskId);
    //                //taskMode.submitTask(task.taskId);
    //                break;
    //        }
    //    }

    //    //private TaskMode taskMode { get { return (TaskMode)(AppFacde.instance.getControl(TaskControl.NAME) as TaskControl).mode; } }

    //}
}
