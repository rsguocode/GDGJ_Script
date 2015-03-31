/**
 * 任务表(自动生成，请勿编辑!)
 */
using System;
namespace com.game.data
{
    [Serializable]
    public class SysTask
    {
    public uint unikey; //唯一key
    public int taskId; //任务Id
    public string name; //任务名称
    public uint npcStart; //起始NPC
    public uint npcEnd; //结束NPC
    public int order; //主线任务顺序
    public string trace_accept; //可接任务任务追踪
    public string trace_uncom; //已接任务任务追踪
    public string trace_com; //完成任务追踪
    public int type; //任务类型
    public string kill_monster; //击败怪物需求
    public bool processShow; //是否需要计数
    public string talk_accept; //接受任务对话
    public uint give_goods; //对话后获得任务道具
    public string talk_com; //完成对话
    public string talk_uncom; //未完成对话
    public uint pre_task; //前置任务Id
    public uint next_task; //后置任务Id
    public string level; //等级限制
    public int accept_icon; //接受任务头像
    public int uncom_icon; //追踪任务头像
    public int com_icon; //完成任务头像
    public string sort; //任务栏排序
    public int times; //次数限制
    public uint talk; //对话NPC需求
    public string req; //需求
    public string ins_req; //副本通关需求
    public int exp; //经验奖励
    public int gold; //金币奖励
    public int diam; //钻石奖励
    public int spirit; //灵力奖励
    public int star; //魂气奖励
    public bool ir_pro_limit; //物品奖励是否有职业限制
    public string item_reward; //物品奖励
    public int guild_exp; //公会经验
    public int guild_con; //公会贡献值
    public int guild_mon_exp; //公会神兽经验
    public int repu; //声望值
    }
}
