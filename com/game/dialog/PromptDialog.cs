﻿﻿﻿using UnityEngine;
using System;
using com.game.module;
using com.u3d.bases.loader;
using com.game.data;
using com.game.manager;
using com.game.consts;
using com.game.module.map;
using com.game.vo;
using com.game.module.fight;

namespace com.game.dialog
{
    public delegate void CloseCallback(GameObject go);

    /// <summary>
    /// 提示框
    /// </summary>
    //public class PromptDialog : BaseView
    //{

    //    public CloseCallback callback;

    //    /// <summary>
    //    /// 单例
    //    /// </summary>
    //    public static PromptDialog instance;

    //    public const string url = "texture/ui/common/common";
    //    public const float duration = 3f;

    //    /// <summary>
    //    /// 类型
    //    /// </summary>
    //    public enum Type {
    //        Info,
    //        Warn,
    //    }

    //    /// <summary>
    //    /// 图标
    //    /// </summary>
    //    public UISprite icon;

    //    /// <summary>
    //    /// 提示
    //    /// </summary>
    //    public UILabel prompt;

    //    protected UIAtlas atlas;

    //    /// <summary>
    //    /// 构造函数
    //    /// </summary>
    //    private PromptDialog() {}

    //    /// <summary>
    //    /// 初始化
    //    /// </summary>
    //    protected override void init()
    //    {
    //        //set instance
    //        instance = this;

    //        //load all widgets
    //        atlas = ResMgr.instance.load(url, typeof(UIAtlas)) as UIAtlas;

    //        //register all events
    //        addOnClickEvent(gameObject, onClose);

    //        //flag
    //        hasInited = true;

    //        close();
    //    }

    //    /// <summary>
    //    /// 显示对话框
    //    /// </summary>
    //    public override void show()
    //    {
    //        base.show();

    //        Invoke("close", duration);
    //    }


    //    public override void close()
    //    {
    //        base.close();

    //        if (this.callback != null)
    //        {
    //            this.callback(gameObject);
    //            this.callback = null;
    //        }
    //    }

    //    /// <summary>
    //    /// 显示提示对话框
    //    /// </summary>
    //    /// <param name="message"></param>
    //    public void showInfoDialog(string message)
    //    {
    //        show();
    //        setIcon(Type.Info);
    //        prompt.text = message;
    //    }

    //    public void addCloseCallback(CloseCallback callback)
    //    {
    //        this.callback = callback;
    //    }

    //    /// <summary>
    //    /// 显示警告对话框
    //    /// </summary>
    //    /// <param name="message"></param>
    //    public void showWarnDialog(string message)
    //    {
    //        show();
    //        setIcon(Type.Warn);
    //        prompt.text = message;
    //    }

    //    /// <summary>
    //    /// 设置图标
    //    /// </summary>
    //    /// <param name="type">类型</param>
    //    protected void setIcon(Type type) {
    //        switch (type) { 
    //            case Type.Info:
    //                icon.spriteName = "icon_blue";
    //                break;
    //            case Type.Warn:
    //                icon.spriteName = "icon_red";
    //                break;
    //        } 
    //    }
    //}
}
