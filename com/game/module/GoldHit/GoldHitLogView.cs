using UnityEngine;
using System.Collections;
using com.game.module.test;
using Com.Game.Module.Waiting;
using com.u3d.bases.debug;
using Com.Game.Module.Arena;
using Com.Game.Module.Copy;
using com.game.module.map;
using System;
using com.game.vo;
using com.game.manager;
using com.game.module.Guide;

namespace Com.Game.Module.GoldHit
{
    //点击点石成金首先进入的界面
    public class GoldHitLogView : BaseView<GoldHitLogView>
    {
        private byte times;
        private Button close;
        public Button btn_enter;
        private UILabel shengyucishu;
        private UILabel lq_time;  //下次进入副本还需要的时间
        private Transform lengque;   //冷却加速的界面
        private Transform qclq;  //清除冷却界面
        private Button lq_jiasu;
        private Button btn_queding; //确定清除冷却时间
        private Button btn_quxiao; //取消清除冷却
        private int nCount = 0;
        private int nCdTime = 0;
        private bool strop = false;

        private UILabel titlelabel;
        private UILabel toptip;
        private UILabel cishulabel;
        private UILabel lijijinru;
        private UILabel lqlabel;
        private UILabel qclqlabel;
        private UILabel lqqd;
        private UILabel lqqx;

        public override ViewLayer layerType
        {
            get
            {
                return ViewLayer.HighLayer;
            }
        }
        public void Init()
        {
            //下面是找到各个组件
            close = FindInChild<Button>("btn_close");
            btn_enter = FindInChild<Button>("btn_enter");
            shengyucishu = FindInChild<UILabel>("btn_enter/wenzi/cishu");

            lengque = FindInChild<Transform>("lengque");
            lq_time = FindInChild<UILabel>("lengque/lefttime");
            lq_jiasu = FindInChild<Button>("lengque/btn_jiasu");

            qclq = FindInChild<Transform>("qclq");
            btn_queding = FindInChild<Button>("qclq/btn_queding");
            btn_quxiao = FindInChild<Button>("qclq/btn_quxiao");

            titlelabel = FindInChild<UILabel>("top/title_wenzi");
            toptip = FindInChild<UILabel>("top/tip");
            cishulabel = FindInChild<UILabel>("btn_enter/wenzi/cishu_wenzi");
            lijijinru = FindInChild<UILabel>("btn_enter/wenzi");
            lqlabel = FindInChild<UILabel>("lengque/jinru_wenzi");
            qclqlabel = FindInChild<UILabel>("qclq/wenzi");
            lqqd = FindInChild<UILabel>("qclq/btn_queding/wenzi");
            lqqx = FindInChild<UILabel>("qclq/btn_quxiao/wenzi");

            close.onClick = OnClickClose;
            btn_enter.onClick = OpenGoldHitMainView;
            lq_jiasu.onClick = OnClickJiasu;    //点击加速，清除冷却时间
            btn_queding.onClick = OnClickDueding;
            btn_quxiao.onClick = OnClickQuxiao;

            InitLabel();
        }

        private void InitLabel()
        {
            titlelabel.text = LanguageManager.GetWord("GoldHit.StrikeDiam");
            toptip.text = LanguageManager.GetWord("GoldHit.Tip");
            cishulabel.text = LanguageManager.GetWord("GoldHit.ChallengeTimes");
            lijijinru.text = LanguageManager.GetWord("GoldHit.EnterNow");
            lqlabel.text = LanguageManager.GetWord("GoldHit.PlayAgain");
            qclqlabel.text = LanguageManager.GetWord("GoldHit.ClearCD");
            lqqd.text = LanguageManager.GetWord("GoldHit.Comfirm");
            lqqx.text = LanguageManager.GetWord("GoldHit.Cancle");
        }
                

        //注册数据更新回调函数
        public override void RegisterUpdateHandler() 
        {
            Singleton<GoldHitMode>.Instance.dataUpdated += UpdateGoldHitView; //面板信息
            Singleton<GoldHitMode>.Instance.dataUpdated += UpdateClearCD;     //清除CD
        }

        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            Singleton<GoldHitMainView>.Instance.gameObject.SetActive(false);
            Singleton<GoldHitMode>.Instance.ApplyGoldHitInfo();
        }

		protected override void HandleBeforeCloseView()
		{
			base.HandleBeforeCloseView();
			Singleton<GoldHitMode>.Instance.StartTips();
		}
        

        //更新面板的冷却时间
        public void UpdateCDTime()
        {
            //if (Singleton<GoldHitMode>.Instance.mainOpen == false)
            {
                //Singleton<GoldHitMode>.Instance.ApplyGoldHitInfo();
				int temp = --nCdTime;
                if (nCdTime != 0)
                    lq_time.text = Singleton<GoldHitMainView>.Instance.FormatTime(temp);//(m < 10 ? ("0" + m.ToString()) : m.ToString()) + ":" + (s < 10 ? ("0" + s.ToString()) : s.ToString());
                if (temp > 1)
                {
                    temp--;
                }
                else if (temp <= 1)
                {
                    btn_enter.gameObject.SetActive(true);
                    lengque.gameObject.SetActive(false);
                }
            }
        }

        //数据更新函数
        private void UpdateGoldHitView(object sender, int code)
        {
            if(code == Singleton<GoldHitMode>.Instance.UPDATE_CHALLENGE_TIMES)
            {
                times = Singleton<GoldHitMode>.Instance.info.times;
                nCdTime = (int)Singleton<GoldHitMode>.Instance.info.remainTime;
                //Log.debug(this, "Remain time: " + nCdTime.ToString());
                vp_Timer.CancelAll("UpdateCDTime");
                if (nCdTime != 0)
                {
                    vp_Timer.CancelAll();
                    vp_Timer.In(0f, UpdateCDTime, nCdTime, 1f);
                }
                if (Singleton<GoldHitMode>.Instance.info.remainTime == 0)
                {
                    btn_enter.gameObject.SetActive(true);
                    lengque.gameObject.SetActive(false);
                }
                else
                {
                    lengque.gameObject.SetActive(true);
                    btn_enter.gameObject.SetActive(false);
                }
                shengyucishu.text = times.ToString() + "/6";    //打副本的次数
				qclqlabel.text = LanguageManager.GetWord("GoldHit.ClearCD", Singleton<GoldHitMode>.Instance.info.diam.ToString());
            }
        }

        //关闭该界面
        private void OnClickClose(GameObject go)
        {
            this.CloseView();
            Singleton<GoldHitMode>.Instance.mainOpen = true;
        }

        //进入点石成金的主界面
        private void OpenGoldHitMainView(GameObject go)
        {
            CloseView(); //关闭当前界面
            Singleton<GoldHitMode>.Instance.ApplyChangeSence();   //请求切场景
        }

        public void OpenMainView()
        {
            Singleton<GoldHitMainView>.Instance.OpenView();
            Singleton<GoldHitLogView>.Instance.CloseView();
        }

        //打开清除冷却的对话框
        private void OnClickJiasu(GameObject go)
        {
            //Log.debug(this, "打开清除冷却的对话框");
            qclq.gameObject.SetActive(true);   //弹出清除冷却的对话框， 注意，这个界面是不是会一直被打开？？
        }

        private void OnClickDueding(GameObject go)
        {
            //Log.debug(this, "确定消耗去清除冷却时间");
            Singleton<GoldHitMode>.Instance.ApplyClearCD();
            qclq.gameObject.SetActive(false); 
            //Singleton<GoldHitLogView>.Instance.OpenView();
        }

        //取消清除冷却时间，只需要disable这个界面
        private void OnClickQuxiao(GameObject go)
        {
            qclq.gameObject.SetActive(false); 
        }

        //清除Cd的更新
        private void UpdateClearCD(object sender, int code)
        {
            if(code == Singleton<GoldHitMode>.Instance.UPDATE_CLEAR_CD)   //表示清除CD成功
            {
                btn_enter.gameObject.SetActive(true);
                lengque.gameObject.SetActive(false);
            }
        }
    }
}