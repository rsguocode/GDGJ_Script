using com.game.vo;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.u3d.bases.debug;
using com.game.manager;
using Com.Game.Module.Copy;


namespace Com.Game.Module.GoldHit
{
    public class GoldHitJieSuan : BaseView<GoldHitJieSuan>
    {
        public UILabel time;   //通关时间
        public UILabel gold;  //总共获得的金币
        public Button tuichu;   //通关退出的按钮
        public Transform successPannel;  //副本成功的UI
        public Transform fialPannel;

        //翻译用
        private UILabel leaveLabel;
        private UILabel timeLabel;
        private UILabel yinbilabel;
        private UILabel faillabel;

        public void Init()
        {
            time = FindInChild<UILabel>("nr/shijian");
            gold = FindInChild<UILabel>("nr/shuzi");
            tuichu = FindInChild<Button>("btn_sy");
            successPannel = FindInChild<Transform>("GoldHitSuccess");
            fialPannel = FindInChild<Transform>("GoldHitFail");

            leaveLabel = FindInChild<UILabel>("btn_sy/label");
            timeLabel = FindInChild<UILabel>("nr/tg");
            yinbilabel = FindInChild<UILabel>("nr/yb");
            faillabel = FindInChild<UILabel>("GoldHitFail/xx");


            tuichu.onClick = OnClickClose;
            InitLabel();
        }

        private void InitLabel()
        {
            leaveLabel.text = LanguageManager.GetWord("GoldHit.Leave");
            timeLabel.text = LanguageManager.GetWord("GoldHit.CompleteTime");
            yinbilabel.text = LanguageManager.GetWord("GoldHit.GoldCount");
            faillabel.text = LanguageManager.GetWord("GoldHit.ChallengeFailure");
            timeLabel.text += ":";
            yinbilabel.text += ":";
        }

        protected override void HandleAfterOpenView()
        {
            //base.HandleAfterOpenView();
            //todo 
            //要GoldHitmode里取数据
           //同时判断副本成功还是失败，决定是否要显示
            //successPannel.gameObject.SetActive(true);
            //fialPannel.gameObject.SetActive(true);
            Log.debug(this, "GoldHitJiesuan Open" + Singleton<GoldHitMainView>.Instance.nGold.ToString());
            time.text = Singleton<GoldHitMainView>.Instance.FormatTime(Singleton<GoldHitMode>.Instance.useTime);
            gold.text = "+" + Singleton<GoldHitMainView>.Instance.nGold.ToString();
        }
        
        public override void RegisterUpdateHandler()
        {
            //Singleton<GoldHitMode>.Instance.dataUpdated += UpdateMonDeath;    //石像死亡
			Singleton<GoldHitMode>.Instance.dataUpdated += UpdateGold;
        }

		public override void CancelUpdateHandler()
		{
			Singleton<GoldHitMode>.Instance.dataUpdated -= UpdateGold;
		}

		private void UpdateGold(object sender, int code)
		{
			if(code == Singleton<GoldHitMode>.Instance.UPDATE_GOLD_COUNT)
			{
				gold.text = "+" + Singleton<GoldHitMainView>.Instance.nGold.ToString();
			}				
		}

        //关闭按钮，退出副本
        public void OnClickClose(GameObject go)
        {
            Singleton<GoldHitMainView>.Instance.CloseView();
            this.CloseView();
            if (MeVo.instance.CurHp == 0)
            {
                Singleton<CopyFailView>.Instance.ReviveReturnCity();  //死亡则请求回城复活
            }
            else
            {
                Singleton<CopyMode>.Instance.ApplyQuitCopy();//离开副本
            }
        }
    }
}
