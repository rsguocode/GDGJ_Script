using com.game.manager;
using Com.Game.Module.Copy;
using com.game.module.Guide;
using Com.Game.Module.Manager;
using com.game.module.map;
using com.game.module.Task;
using com.game.module.test;
﻿﻿﻿using com.u3d.bases.display;
﻿﻿﻿using UnityEngine;
using com.u3d.bases.debug;
using com.game.module.effect;
using com.game.vo;
using com.game.utils;
using com.u3d.bases.controller;


/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/01/07 03:21:46 
 * function: 主UI的左侧功能，主线任务和支线任务指引
 * *******************************************************/
namespace com.game.module.main
{
    public class MainLeftView : BaseView<MainLeftView>
    {
		public override ViewLayer layerType {
			get {
				return ViewLayer.NoneLayer;
			}
		}

		public override bool playClosedSound { get { return false; } }

        
        //private TweenPlay showTween;//关闭打开动画

        protected override void Init()
        {
    
            //Singleton<MeStatuController>.Instance.AutoRoundEffect.SetActive(false);

            //Vector3 position = AppMap.Instance.me.GoBase.transform.position;
            //position.y = position.y + 2.0f;
            //EffectMgr.Instance.CreateMainEffect(EffectId.Main_AutoSerachRoad, position);
            //autoRoad = EffectMgr.Instance.GetMainEffectGameObject(EffectId.Main_AutoSerachRoad);
            //autoRoad.SetActive(false);
        }

    


        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
           
        }



    }
}