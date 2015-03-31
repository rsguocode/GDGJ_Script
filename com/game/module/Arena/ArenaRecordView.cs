using UnityEngine;
using System.Collections;
using com.game.module.test;
using System.Collections.Generic;
using com.u3d.bases.debug;
using com.game.vo;
using com.game.manager;
using System;

namespace Com.Game.Module.Arena
{
	public class ArenaRecordView : BaseView<ArenaRecordView>
	{
        public const int MaxSize = 15;
        public GameObject msg;
        private Vector3 v;
        public Transform panel;
        public UISprite shangla;
        public UISprite xiala;


        public  struct UIRec
        {
            public GameObject rec;
            public UILabel time;
            public UILabel fail;
            public UILabel content;
            public UIDragScrollView drag;
        }
        public List<UIRec> uiRecord = new List<UIRec>();

        public int index = 0;

		public override ViewLayer layerType {
			get {
				return ViewLayer.HighLayer;
			}
		}

		private Button btn_close;
		
		public void Init()
		{
			InitLabelLanguage ();
			btn_close = FindInChild<Button>("hylb/topright/btn_close");
            msg = FindInChild<Transform>("hylb/RecordPanel/rc").gameObject;
            shangla = FindInChild<UISprite>("hylb/shangla/by");
            xiala = FindInChild<UISprite>("hylb/xiala/by");

            v = msg.transform.localPosition;
            UIRec rec1 = new UIRec();
            rec1.rec = msg;
            rec1.time = msg.transform.FindChild("time").GetComponent<UILabel>();
            rec1.fail = msg.transform.FindChild("result").GetComponent<UILabel>();
            rec1.content = msg.transform.FindChild("content").GetComponent<UILabel>();
            rec1.drag = msg.transform.GetComponent<UIDragScrollView>();
            uiRecord.Add(rec1);

            for (int i = 0; i < 15; ++i)
            {
                GameObject record = GameObject.Instantiate(msg, msg.transform.position, msg.transform.rotation) as GameObject;
                record.transform.parent = msg.transform.parent;
                record.transform.localScale = new Vector3(1, 1, 1);
                v -= new Vector3(0, 50, 0);
                record.transform.localPosition = v;

                UIRec rec = new UIRec() ;
                rec.rec = record;
                rec.time = record.transform.FindChild("time").GetComponent<UILabel>();
                rec.fail = record.transform.FindChild("result").GetComponent<UILabel>();
                rec.content = record.transform.FindChild("content").GetComponent<UILabel>();
                rec.drag = record.transform.GetComponent<UIDragScrollView>();
                uiRecord.Add(rec);
                index ++;
            }   
            btn_close.onClick = BackOnClick;            
		}

		private void InitLabelLanguage()
		{
			FindInChild<UILabel>("hylb/mz/mz").text = LanguageManager.GetWord("ArenaRecordView.fightRecord");
		}

		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
//			Singleton<ArenaMode>.Instance.dataUpdated += UpdateArenaRankView;
            Singleton<ArenaMode>.Instance.dataUpdated += UpdateRecordView;
		}
		
		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
			NGUITools.SetLayer(gameObject, LayerMask.NameToLayer("TopUI"));
            //请求个人战斗记录
            Singleton<ArenaMode>.Instance.ApplyArenaRecord();
		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
//			Singleton<ArenaMode>.Instance.dataUpdated -= UpdateArenaRankView;
		}
		
		//在关闭数据更新回调函数之后执行
		protected override void HandleBeforeCloseView ()
		{
			base.HandleBeforeCloseView ();
 		}

        private Transform[] FindChild<T1>(string p)
        {
            throw new System.NotImplementedException();
        }

		//返回键被点击
		private void BackOnClick(GameObject go)
		{
            this.CloseView();
		}


        //得到个人战斗数据后调用
        private void UpdateRecordView(object sender, int code)
        {
            if (code == Singleton<ArenaMode>.Instance.UPDATE_ARENA_RECORD)
            {
                for (int j = 0; j < uiRecord.Count; j++)
                {
                    uiRecord[j].rec.SetActive(false);  //用不到的不显示
                }
                for (int j = 0; j < Singleton<ArenaMode>.Instance.record.Count; j++)
                {
                    uiRecord[j].time.text = Singleton<ArenaMode>.Instance.record[j].sTime;
					uiRecord[j].fail.text = (Singleton<ArenaMode>.Instance.record[j].nFail == 0)?
							"[4ad83b]" + LanguageManager.GetWord("ArenaRecordView.success") + "[-]" :
							"[ff0000]" + LanguageManager.GetWord("ArenaRecordView.fail") + "[-]";  //加入颜色区分，modify by lixi
                    uiRecord[j].content.text = Singleton<ArenaMode>.Instance.record[j].sContent;
                    uiRecord[j].rec.SetActive(true);
                    if (Singleton<ArenaMode>.Instance.record.Count < 10)   //6   否则，多余一屏的情况要滚动
                    {
                        uiRecord[j].drag.enabled = false;
                    }
                    else
                    {
                        uiRecord[j].drag.enabled = true;
                    }
                }
            }
        }

        public override void Update()
        {
            DisplayPageSprite();
        }


        public void DisplayPageSprite()
        {
            bool pageUp = false;
            bool pageDown = false;
                //还没有点击的时候最好也提示下面的翻页
                //if (rec.rec.name) 
            if (uiRecord.Count > 6 && (uiRecord[5].rec.transform.position.y < -0.4f))  //需要提示
            {
                xiala.gameObject.SetActive(true);
            }
         
            {
                foreach(UIRec rec in uiRecord)
                {
                    if (rec.rec.active == true)
                    {
                        //出现在界面之外的话，需要提示翻页
                        if (rec.rec.transform.position.y > 0.3f)
                        {
                            pageUp = true;
                        }
                        if (rec.rec.transform.position.y < -0.75f)
                        {
                            pageDown = true;
                        }
                    }
                }
                if (pageUp)
                {
                    shangla.gameObject.SetActive(true);
                }
                else {
                    shangla.gameObject.SetActive(false);
                }
                if (pageDown)
                {
                    xiala.gameObject.SetActive(true);
                }
                else {
                    xiala.gameObject.SetActive(false);
                }
        }
        }
	}
}