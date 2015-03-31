﻿﻿﻿using UnityEngine;
using System;
using com.game.module;

namespace com.game.dialog
{
    /// <summary>
    /// 死亡提示框
    /// </summary>
    //public class DeathPromptDialog : BaseView
    //{
    //    /// <summary>
    //    /// 单例
    //    /// </summary>
    //    public static DeathPromptDialog instance;

    //    /// <summary>
    //    /// 提示
    //    /// </summary>
    //    public UILabel prompt;

    //    /// <summary>
    //    /// 立即复活
    //    /// </summary>
    //    public GameObject nowGo;

    //    /// <summary>
    //    /// 回城复活
    //    /// </summary>
    //    public GameObject backGo;

    //    /// <summary>
    //    /// 倒计时
    //    /// </summary>
    //    public UILabel sec;

    //    protected uint tick = 60;

    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    private DeathPromptDialog() { }

    //    /// <summary>
    //    /// 初始化
    //    /// </summary>
    //    protected override void init()
    //    {
    //        //set instance
    //        instance = this;

    //        //register all events
    //        addOnClickEvent(backGo, onBackHome);
    //        addOnClickEvent(nowGo, onRightNow);

    //        //flag
    //        hasInited = true;

    //        close();
    //    }

    //    /// <summary>
    //    /// 显示视图
    //    /// </summary>
    //    public override void show()
    //    {
    //        tick = 60;
    //        InvokeRepeating("startCountDown", 0f, 1f);
    //        base.show();
    //    }

    //    /// <summary>
    //    /// 关闭视图
    //    /// </summary>
    //    public override void close()
    //    {
    //        stopCountDown();
    //        base.close();
    //    }

    //    /// <summary>
    //    /// 启动倒计时
    //    /// </summary>
    //    protected void startCountDown()
    //    {
    //        tick -= 1;
    //        sec.text = String.Format("（{0}秒）", tick);
    //        if (tick <= 0)
    //        {
    //            stopCountDown();
    //            onBackHome(null);
    //            close();
    //        }
    //    }

    //    /// <summary>
    //    /// 停止倒计时
    //    /// </summary>
    //    protected void stopCountDown()
    //    {
    //        CancelInvoke("startCountDown");
    //    }

    //    /// <summary>
    //    /// 回城复活
    //    /// </summary>
    //    /// <param name="go">游戏对象</param>
    //    protected void onBackHome(GameObject go)
    //    {
    //        //battleControl.aliveSend(1);
    //        close();
    //    }

    //    /// <summary>
    //    /// 立即复活
    //    /// </summary>
    //    /// <param name="go">游戏对象</param>
    //    protected void onRightNow(GameObject go)
    //    {
    //        //battleControl.aliveSend(2);
    //        close();
    //    }

    //    //private BattleControl battleControl { get { return (BattleControl)AppFacde.instance.getControl(BattleControl.NAME); } }

    //}
}
