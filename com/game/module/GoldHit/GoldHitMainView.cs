using com.game.Public.Hud;
using com.u3d.bases.display;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using System;
using Com.Game.Module.Copy;
using com.game.module.battle;
using com.game.module.map;
using Com.Game.Module.ContinueCut;
using com.game.module.Guide;
using com.game.module.hud;
using com.game;
using com.u3d.bases.controller;
using com.game.vo;
using com.u3d.bases.display.controler;
using com.u3d.bases.display.character;
using Com.Game.Module.Waiting;
using com.game.consts;
using com.game.module.effect;
using com.game.utils;
using System.Collections.Generic;
using com.game.sound;
using System.Collections.Generic;
using com.game.manager;

namespace Com.Game.Module.GoldHit
{
    public class GoldHitMainView : BaseView<GoldHitMainView>
    {
        public override string url { get { return "UI/GoldHit/GoldHit.assetbundle"; } }

        private UILabel lefttime_m;   //剩余时间
        //private UILabel lefttime_s; 
        private UILabel gold_count;  //当前金币
        private Button close;
        public int second = 400;
        public uint nGold = 0;
        public int nStart = 0;
        public UILabel title;

        private UILabel goldlab;
        private UILabel sysjlab;
        private UILabel titlelab;

        public void Init()
        {
            lefttime_m = FindInChild<UILabel>("Left/lefttime");
            gold_count = FindInChild<UILabel>("Left/gold_count");
            close = FindInChild<Button>("btn_close");
            title = FindInChild<UILabel>("Left/title");

            goldlab = FindInChild<UILabel>("Left/gold_lab");
            sysjlab = FindInChild<UILabel>("Left/sysj");
            titlelab = FindInChild<UILabel>("Left/title");

            title.fontSize += 2;
            close.onClick = OnClickBack;
            if (goldlab)
                InitLabel();
        }


        private void InitLabel()
        {
            goldlab.text = LanguageManager.GetWord("GoldHit.Gold");
            sysjlab.text = LanguageManager.GetWord("GoldHit.LeftTime");
            titlelab.text = LanguageManager.GetWord("GoldHit.StrikeDiam");
        }

        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();

            second = 400;
            nStart = 0;
            gold_count.text = "0";
            Singleton<GoldHitMode>.Instance.mainOpen = true;
        }

		protected override void HandleBeforeCloseView()
		{
			base.HandleBeforeCloseView();			
			Singleton<GoldHitJieSuan>.Instance.CloseView();
		}


        //注册数据更新回调函数
        public override void RegisterUpdateHandler()
        {
            Singleton<GoldHitMode>.Instance.dataUpdated += UpdateGold;
            Singleton<MapMode>.Instance.dataUpdated += UpdateLeftTimeHandler; //更新副本剩余时间
        }

        public override void CancelUpdateHandler()
        {
			Singleton<GoldHitMode>.Instance.dataUpdated -= UpdateGold;
            Singleton<MapMode>.Instance.dataUpdated -= UpdateLeftTimeHandler; //取消副本剩余时间
        }


        //退出电石成金的副本界面
        private void OnClickBack(GameObject go)
        {
            Singleton<GoldHitMode>.Instance.mainOpen = false;
            this.CloseView();
            Singleton<GoldHitMode>.Instance.AppyBakctoMain();    //回到主城
        }

        public override void Update()
        {
            base.Update();
            lefttime_m.text = FormatTime(Singleton<BattleView>.Instance.GetDugeonTime());
        }
 
        //时间格式化
        public string FormatTime(int time)
        {
            int min = time / 60;
            string minstr = min < 10 ? "0" + min : min + "";
            int sec = time % 60;
            string secstr = sec < 10 ? "0" + sec : sec + "";
            return (minstr + ":" + secstr);
        }

        public string Format(int time)
        {
            return time < 10 ? ("0" + time.ToString()) : time.ToString();
        }
        
        private void UpdateGold(object sender, int code)
        {
            if(code == Singleton<GoldHitMode>.Instance.UPDATE_GOLD_COUNT)
            {
                //播放吃钻石的声音
                int n = UnityEngine.Random.Range(0, 1);
                if (n == 0)
                {
					SoundMgr.Instance.PlayUIAudio(SoundId.Sound_EatDiam1);
                }
                else
                {
					SoundMgr.Instance.PlayUIAudio(SoundId.Sound_EatDiam2);
                }
                nStart = (int)nGold;
                nGold += Singleton<GoldHitMode>.Instance.goldNum.gold;
                Log.debug(this, "服务器返回的金币：" + nGold.ToString());

                var effectVo = new Effect
                {
					URL = UrlUtils.GetSkillEffectUrl(EffectId.Skill_StoneDiam),
                    BasePosition = Singleton<GoldHitMode>.Instance.Monster + new Vector3(0, -0.3f, 0),
                    NeedCache = true
                };
                EffectMgr.Instance.CreateSkillEffect(effectVo);
                vp_Timer.In(0.02f, AddGold, ((int)nGold - nStart), 0.02f);
                GoldFly((int)Singleton<GoldHitMode>.Instance.goldNum.gold);
            }


        }

        public void GoldFly(int count)    //金币飘字
        {
            MeDisplay me = AppMap.Instance.me;
            var actionControler = me.Controller;
            Vector3 pos = actionControler.transform.position;
            pos.y += actionControler.GetMeByType<ActionDisplay>().BoxCollider2D.size.y;
            HeadInfoItemManager.Instance.ShowHeadInfo("金币" + count, Color.green, pos, me.CurFaceDire, HeadInfoItemType.TypePetTalentHp);
        }

        public void AddGold()
        {
            if(nStart < nGold)
            {
                nStart++;
                gold_count.text = nStart.ToString();
            }
        }


        //更新副本剩余时间
        private void UpdateLeftTimeHandler(object sender, int code)
        {
            if (MapMode.EVENT_CODE_UPDATE_LEFTTIME == code)
            {
            }
        }
        
        public void OpenSuccessPannel()
        {
            //弹出副本通关的界面
            Singleton<GoldHitView>.Instance.InitMain();
            //Singleton<GoldHitMainView>.Instance.OpenView();
            Singleton<GoldHitMode>.Instance.useTime = 400 - Singleton<BattleView>.Instance.GetLeftTime();  //通关花费的时间
            Singleton<GoldHitMode>.Instance.nGold = (int)nGold;
            Singleton<WaitingView>.Instance.OpenView();
            Singleton<WaitingView>.Instance.CloseView();
            Singleton<GoldHitJieSuan>.Instance.OpenView();
            Singleton<GoldHitJieSuan>.Instance.successPannel.gameObject.SetActive(true);
            Singleton<GoldHitJieSuan>.Instance.fialPannel.gameObject.SetActive(false);
            Singleton<GoldHitMode>.Instance.mainOpen = false;
        }

        //弹出副本失败的界面
        public void OpenFailPannel(bool timeUp = false)
        {
            Log.debug(this, "will open fail pannel: gold: " + nGold.ToString());
            Singleton<GoldHitView>.Instance.InitMain();
            //Singleton<GoldHitMainView>.Instance.OpenView();
            if (timeUp == true)
            {
                Singleton<GoldHitMode>.Instance.useTime = 400;// -Singleton<BattleView>.Instance.GetDugeonTime();  //通关花费的时间
            }
            else
            {
                Singleton<GoldHitMode>.Instance.useTime = 400 - Singleton<BattleView>.Instance.GetLeftTime();
            }
            Singleton<GoldHitMode>.Instance.nGold = (int)nGold;
            Singleton<WaitingView>.Instance.OpenView();
            Singleton<WaitingView>.Instance.CloseView();
            Singleton<GoldHitJieSuan>.Instance.OpenView();
            Singleton<GoldHitJieSuan>.Instance.successPannel.gameObject.SetActive(false);
            Singleton<GoldHitJieSuan>.Instance.fialPannel.gameObject.SetActive(true);
            Singleton<GoldHitMode>.Instance.mainOpen = false;
        }

        public void CloseView()
        {
            Log.debug(this, "关闭界面！");
            nGold = 0;//金币数归零
        }
    }
}