using com.game.module.battle;
using com.game.Public.Confirm;
using com.game.utils;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using System.Collections.Generic;
using com.game.manager;
using Proto;
using com.game.module.map;
using com.u3d.bases.debug;
using Com.Game.Module.Tips;
using Com.Game.Module.Role;

namespace Com.Game.Module.Boss
{
    public class BossView : BaseView<BossView>
    {

        public override string url { get { return "UI/Boss/WorldBossView.assetbundle"; } }
        public override ViewLayer layerType { get { return ViewLayer.BaseLayer; } }
        public override bool waiting { get { return false; } }

        private Button btn_quit;//退出
        private UILabel leftTime;//剩余时间
        private Button btn_gj;//攻击
        private Button btn_bj;//暴击

        private UILabel beginTime;//开始倒计时


        private Button btn_jiantou;//收缩伤害信息按钮
        private UILabel myHurt;//自己的伤害
        private UILabel myLabel;//自己伤害文本

        private GameObject container;//
        private TweenHeight heightTween;//宽度动画
        private List<UIWidgetContainer> hurtList = new List<UIWidgetContainer>();//伤害列表

		private GameObject tips;//Boss战结束，排名

		private uint restTime;
		private bool jiantouOpen;//伤害排行是否展开 是 展开

        protected override void Init()
        {
			tips = FindChild("anchor/tips");
			tips.SetActive(false);
            BossTips.Instance.gameObject = tips;
            leftTime = FindInChild<UILabel>("anchor/lefttime/label");
            btn_quit = FindInChild<Button>("anchor/btn_tc");
			btn_quit.onClick = QuitOnClick;

            btn_bj = FindInChild<Button>("anchor/btn_bj");

            btn_gj = FindInChild<Button>("anchor/btn_gj");
            btn_gj.FindInChild<UILabel>("name").text = LanguageManager.GetWord("Boss.Attack");
            btn_bj.FindInChild<UILabel>("name").text = LanguageManager.GetWord("Boss.BaoJi");
			
			btn_gj.FindInChild<UILabel>("percent").text = "0%";
            btn_bj.FindInChild<UILabel>("percent").text = "0%";
            btn_gj.onClick = GuWuTips;
            btn_bj.onClick = GuWuTips;

            btn_jiantou = FindInChild<Button>("anchor/hurtrank/btn_jiantou");
			btn_jiantou.onClick = JianTouOnClick;
            myHurt = FindInChild<UILabel>("anchor/hurtrank/value");
            heightTween = FindInChild<TweenHeight>("anchor/hurtrank/background");
			EventDelegate.Add(heightTween.onFinished,TweenForwardCallBack);
            container = FindChild("anchor/hurtrank/container");
            UIWidgetContainer temp;
            for (int i = 1; i < 11; i++)
            {
                temp = FindInChild<UIWidgetContainer>("anchor/hurtrank/container/item" + i);
				temp.SetActive(false);
                hurtList.Add(temp);
            }

        }
		protected override void HandleAfterOpenView()
		{
			jiantouOpen = true;
		    lastHp = 0;
			UpdateGuWuInfo();
			UpdateHurtRank();
			BossEnd();
			UpdateGuWuInfo();
			MyHurt();
			UpdateBossHp();
            tips.SetActive(false);
		    InitGuwuOpen();
		}
        /// <summary>
        /// vip开启鼓舞判断
        /// </summary>
        private void InitGuwuOpen()
        {
            bool isOpen = VipManager.IsVipDriotOpen(VipManager.WorldBossInspiation);
            btn_bj.SetActive(isOpen);
            btn_gj.SetActive(isOpen);
        }

		//更新Boss血条
		private uint lastHp = 0;
		public void UpdateBossBlood(uint current,uint total)
		{
		    if (lastHp == 0)
		    {
		        lastHp = current;
		    }
            BattleMode.Instance.SetCurrentHp("", total, lastHp, current, 1, 3, BossMode.Instance.BoosIcon);
			lastHp = current;
		}

		public override void RegisterUpdateHandler()
		{
			Singleton<BossMode>.Instance.dataUpdated += UpdateBossHandler;
			Singleton<RoleMode>.Instance.dataUpdated += UpdateRoleHandle;
		}
		public override void CancelUpdateHandler()
		{
			Singleton<BossMode>.Instance.dataUpdated -= UpdateBossHandler;
			Singleton<RoleMode>.Instance.dataUpdated -= UpdateRoleHandle;
		}
		private void UpdateRoleHandle(object sender ,int code)
		{
			if(code == Singleton<RoleMode>.Instance.UPDATE_ROLE_RELIFE)  //复活之后推出
			{
				if(Singleton<BossMode>.Instance.EndTimeLeft == 0)
					Singleton<BossControl>.Instance.QuitBossScene();
			}
		}
		private void UpdateBossHandler(object sender ,int code)
		{
			if(code == Singleton<BossMode>.Instance.UPDATE_ATTACK)
			{
				MyHurt();
			}
			else if(code == Singleton<BossMode>.Instance.UPDATE_GUWU_INFO)
			{
				UpdateGuWuInfo();
			}
			else if(code == Singleton<BossMode>.Instance.UPDATE_BOSS_DIE)
			{
				BossDie();
			}
			else if(code == Singleton<BossMode>.Instance.UPDATE_BOSS_HP)
			{
				UpdateBossHp();
			}
			else if(code == Singleton<BossMode>.Instance.UPDATE_END)
			{
				BossEnd();
			}
			else if(code == Singleton<BossMode>.Instance.UPDATE_AWARD_TOTAL)
			{
				AwardTotal();
			}
			else if(code == Singleton<BossMode>.Instance.UPDATE_HURT_RANK)
			{
				UpdateHurtRank();
			}
		}
        //更新鼓舞信息
		private void UpdateGuWuInfo()
		{
			btn_gj.FindInChild<UILabel>("percent").text = Singleton<BossMode>.Instance.AttackTime*10 + "%";
            btn_bj.FindInChild<UILabel>("percent").text = Singleton<BossMode>.Instance.CritTime * 10 + "%"; ;
		}
		//boss结束
		private void BossDie()
		{
			Log.info(this,"Boss Die:");
		}
		//更新Boss血量
		private void UpdateBossHp()
		{
			UpdateBossBlood(Singleton<BossMode>.Instance.currHp,Singleton<BossMode>.Instance.fullHp);
		}
		//世界Boss战结束
		private void BossEnd()
		{
			restTime = (uint)Singleton<BossMode>.Instance.EndTimeLeft;
			if(Singleton<BossMode>.Instance.isDie)  //Boss死亡
			{
				Singleton<BossTips>.Instance.OpenWNView(BossMode.Instance.BossName,4,Singleton<BossControl>.Instance.QuitBossScene);
			}
			else if(restTime ==0 && ! Singleton<BossMode>.Instance.isDie)  //如果时间到，Boss还没有死亡
				Singleton<BossControl>.Instance.QuitBossScene();//时间到了，直接退出世界Boss

            vp_TimeUtility.Units timeU = vp_TimeUtility.TimeToUnits(restTime);
            string timeStr = string.Format("{0:D2}:{1:D2}", timeU.minutes, timeU.seconds);
		    leftTime.text = timeStr;

		}
		//更新我的伤害信息
        private void MyHurt()
		{
			uint hur = Singleton<BossMode>.Instance.hurt;
			float tot = (float)Singleton<BossMode>.Instance.fullHp;
            if(tot != 0)
			    myHurt.text = VoUtils.LanguagerFormat("Boss.MyHurt",hur + "(" + string.Format("{0:P1}",hur/tot)+")");
            else
            {
                myHurt.text = VoUtils.LanguagerFormat("Boss.MyHurt", "(0.0 %)"); ;
            }
        }
		//累计奖励
		private void AwardTotal()
		{

		}
		//伤害排行榜
		private void UpdateHurtRank()
		{
			WorldBossRankMsg_22_3 hurtRank = Singleton<BossMode>.Instance.hurtRank;
			Log.info(this,hurtRank.hurt.Count+ ".....................cout:");
			List<uint> hurt = hurtRank.hurt;
			float hurtTotal = (float)Singleton<BossMode>.Instance.fullHp;
			List<string> name = hurtRank.name;
			UIWidgetContainer temp;
			for(int i=0,length = hurtList.Count;i<length;i++)
			{
				temp = hurtList[i];
				if(i<hurt.Count)
				{
					temp.FindInChild<UILabel>("name").text = name[i];
					temp.FindInChild<UILabel>("value").text = hurt[i]+" ("+string.Format("{0:P1}",hurt[i]/hurtTotal)+")";
					temp.FindInChild<UILabel>("index").text = (i+1) +".";
                    if (jiantouOpen)
                        temp.SetActive(true);
				}
				else 
				{
					temp.SetActive(false);

				}

			}
			//container.Reposition();
		}
		
		private int guWuType;
		private void GuWuTips(GameObject go)
		{
			if(go.Equals(btn_bj.gameObject))
			{
				guWuType = BossMode.CritGuwuId;
			}
			else
			{
				guWuType = BossMode.AttackGuwuId;
			}
            int cost = StringUtils.GetStringToInt(BossMode.Instance.GuWuCostList[guWuType-1].diam)[0];
            ConfirmMgr.Instance.ShowOkAlert(VoUtils.LanguagerFormat("Boss.GuWuCost",cost), string.Empty,
                GuWuOnClick, LanguageManager.GetWord("Boss.GuWu"));
		}
		private void GuWuOnClick()
		{
			Singleton<BossControl>.Instance.GuWu((byte)guWuType);
			
		}

		//伤害信息收缩
		private void JianTouOnClick(GameObject go)
		{
			if(jiantouOpen)
			{
                btn_jiantou.cachedTransform.localScale = new Vector3(1, -1, 1);
                jiantouOpen = false;
				container.SetActive(false);
                heightTween.Replay(false);
				
			}
			else{
                btn_jiantou.cachedTransform.localScale = Vector3.one;
				heightTween.PlayForward();
				jiantouOpen = true;
			}
		}
		private void TweenForwardCallBack()
		{
			if(jiantouOpen == true )
			{
				container.SetActive(true);
                UpdateHurtRank();
                
			}
		}
	    //退出世界Boss
		private void QuitOnClick(GameObject go)
		{
			Singleton<BossControl>.Instance.QuitBossScene();
		}

    }
}
