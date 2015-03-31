using System.Collections.Generic;
using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.module.LoginAward;
using com.game.module.Mail;
using Com.Game.Module.Manager;
using Com.Game.Module.Role;
using com.game.module.test;
using Com.Game.Module.TopList;
using Com.Game.Module.VIP;
using com.game.vo;
using com.u3d.bases.debug;
using PCustomDataType;
using com.game.module.effect;
using UnityEngine;

namespace com.game.module.main
{
    public class MainTopLeftView : BaseView<MainTopLeftView>
    {
        private readonly List<Button> buffItemList = new List<Button>();
        
        public Button btn_hy; //好友按钮
        public Button btn_yj; //邮件按钮
        public Button btn_dljl; //登录奖励
        private UILabel newMailNumLabe;

        private Button btn_jiahao;
        private Button btn_vip;
        private List<PBuff> buffList;
        private PBuff currentBuff;

        private UISprite iconBkg;
        private UILabel labDiam;
        private UILabel labLevel;
        private UILabel labLijin;
        private UILabel labGold;
        private UILabel labTili;
        public UILabel labVip;
        private UISprite sprHead;
        private GameObject topright;
        private UILabel zhanli; //玩家战力
		private bool firstUpgrade = true;
		private int preLevel;

        public override ViewLayer layerType
        {
            get { return ViewLayer.NoneLayer; }
        }

        public override bool playClosedSound
        {
            get { return false; }
        }

        protected override void Init()
        {
            btn_hy = FindInChild<Button>("Buttons/btn_hy");
            btn_yj = FindInChild<Button>("Buttons/btn_yj");
            btn_dljl = FindInChild<Button>("Buttons/btn_dljl");
            btn_hy.onClick = HaoYouOnClick;
            btn_yj.onClick = MailButtonClick;
            btn_dljl.onClick = LoginAwardOnClick;
            newMailNumLabe = FindInChild<UILabel>("Buttons/btn_yj/NewMailNumLabel");
            topright = FindChild("topright");
            btn_jiahao = FindInChild<Button>("btn_jiahao");
            btn_vip = FindInChild<Button>("btn_vip");
            labVip = FindInChild<UILabel>("btn_vip/label");
            labDiam = FindInChild<UILabel>("jb/wenzi");
            labGold = FindInChild<UILabel>("yb/wenzi");
            labLijin = FindInChild<UILabel>("lijin/wenzi");
            labLevel = FindInChild<UILabel>("dengji/level");
            labTili = FindInChild<UILabel>("tili");
            sprHead = FindInChild<UISprite>("tou");
            iconBkg = FindInChild<UISprite>("dengji/background");
            iconBkg.atlas = Singleton<AtlasManager>.Instance.GetAtlas(AtlasManager.Header);
            iconBkg.spriteName = "djk";
            zhanli = FindInChild<UILabel>("zhanli");

            GetBuffItemList();
            //体力购买
            btn_jiahao.onClick += tiLiOnClick;
            //VIP按钮
            btn_vip.onClick = OpenVIPView;
        }

        //获取Item的GameObject
        private void GetBuffItemList()
        {
            Button temp;
            for (int i = 1; i < 3; i++)
            {
                temp = FindInChild<Button>("topleft/buff" + i);
                if (temp != null)
                {
                    temp.onClick += BuffOnClick;
                    //equipList.Add(temp);
                    temp.gameObject.SetActive(false);
                    buffItemList.Add(temp);
                }
            }
        }

        //更新主界面信息
        private void updateInfo()
        {
            labVip.text = MeVo.instance.vip.ToString();
            //updateWealth();
            updateGrade();
            //updateVigour();
            UpdateHeadSprite();
            UpdateRoleFight();
        }

        private void MailInfoUpdate(object sender, int code)
        {
			if (code == MailMode.UPDATE_ONEKEYATTACHE ||
			    code == MailMode.UPDATE_CURRENTMAIL ||
			    code == MailMode.UPDATE_ONEKEYDEL ||
			    code == MailMode.UPDATE_MAILLIST)
            {
                UpdateNewMailInfo();
            }
        }

        private void UpdateNewMailInfo()
        {
			if (Singleton<MailMode>.Instance.MailInfoVo.TotalUnReadMailNum == 0)
            {
				btn_yj.FindInChild<UISprite>("tips").SetActive(false);
            }
            else
            {
				btn_yj.FindInChild<UISprite>("tips").SetActive(true);
            }
        }

        //更新玩家战力
        private void UpdateRoleFight()
        {
            zhanli.text = LanguageManager.GetWord("RolePropView.zhanli") + MeVo.instance.fightPoint;
        }

        //更新头像
        private void UpdateHeadSprite()
        {
            sprHead.atlas = Singleton<AtlasManager>.Instance.GetAtlas(AtlasManager.Header);
            sprHead.spriteName = Singleton<RoleMode>.Instance.GetPlayerHeadSpriteName(MeVo.instance.job);
            sprHead.MakePixelPerfect();
        }

        //更新财富信息
        private void updateWealth()
        {
            labDiam.text = MeVo.instance.DiamondStr;
            labGold.text = MeVo.instance.DiamStr;
            labLijin.text = MeVo.instance.BindingDiamondStr;
        }

        //更新等级信息
        private void updateGrade()
        {
			if (AppMap.Instance.mapParser.MapVo.type == MapTypeConst.CITY_MAP)
			{
				if (firstUpgrade)
				{
					firstUpgrade = false;				
				}
				else if (MeVo.instance.Level > preLevel)
				{
					EffectMgr.Instance.CreateMainFollowEffect(EffectId.Main_RoleUpgrade, AppMap.Instance.me.Controller.gameObject, Vector3.zero);
				}
			}

			preLevel = MeVo.instance.Level;
            labLevel.text = MeVo.instance.Level.ToString();
        }

        //更新体力信息
        private void updateVigour()
        {
            labTili.text = LanguageManager.GetWord("RolePropView.Vigour") + MeVo.instance.vigour + "/" +
                           MeVo.instance.vigourFull;
            //暂时屏蔽体力更新显示
            //sld_tili.value = (float)MeVo.instance.vigour / MeVo.instance.vigourFull;
        }

        //打开 Buff 信息界面
        private void BuffOnClick(GameObject go)
        {
            SysBuffVo vo;
            Button buff;
            var temp = go.GetComponent<Button>();
            for (int i = 0, length = buffItemList.Count; i < length; i++)
            {
                buff = buffItemList[i];
                if (buff.Equals(temp))
                {
                    Log.info(this, "buffList[i].id: " + buffList[i].id);
                    vo = BaseDataMgr.instance.GetDataById<SysBuffVo>(buffList[i].id*100 + buffList[i].lvl);
                    //Singleton<TanTips>.Instance.OpenView(vo.id + " " + vo.name + " \n" + vo.desc);
                    Log.info(this, "Buff 信息： " + vo.id + "  " + vo.name + "i " + i);
                    break;
                }
            }
        }

        //点击了登录奖励
        private void LoginAwardOnClick(GameObject go)
        {
            //Singleton<LoginAwardView>.Instance.OpenView();
			Singleton<AwardView>.Instance.OpenView();
        }

        //好友
        private void HaoYouOnClick(GameObject go)
        {
            Singleton<TopListView>.Instance.OpenView();
        }

        //点击邮件按钮
        private void MailButtonClick(GameObject go)
        {
            Singleton<MailView>.Instance.OpenView();
        }

        private void tiLiOnClick(GameObject go)
        {
			Singleton<RoleControl>.Instance.OpenBuyVigourTips ();
        }

        //更新Buff
        private void UpdateBuff()
        {
            Log.info(this, "MainView : UpdateBuff");
            //this.buffList = Singleton<RoleMode>.Instance.buffList;
            //for(int i = 0,length =buffList.Count;i<length;i++)  //替换buff图标，暂时没有图标资源
            //{

            //}
            //
            //buffItemList[0].gameObject.SetActive(true);
        }

        //注册数据更新回调函数
        public override void RegisterUpdateHandler()
        {
            Singleton<RoleMode>.Instance.dataUpdated += updateDataHandler;
            Singleton<MailMode>.Instance.dataUpdated += MailInfoUpdate;
			Singleton<LoginAwardMode>.Instance.dataUpdated += ButtonTipsHandle;
            Singleton<FriendMode>.Instance.dataUpdated += ButtonTipsHandle;
        }

        public override void CancelUpdateHandler()
        {
            Singleton<RoleMode>.Instance.dataUpdated -= updateDataHandler;
           	Singleton<MailMode>.Instance.dataUpdated -= MailInfoUpdate;
			Singleton<LoginAwardMode>.Instance.dataUpdated -= ButtonTipsHandle;
            Singleton<FriendMode>.Instance.dataUpdated -= ButtonTipsHandle;
        }

        private void updateDataHandler(object sender, int code)
        {
            if (RoleMode.UPDATE_VIGOUR == code)
            {
                updateVigour();
            }
            else if (Singleton<RoleMode>.Instance.UPDATE_BUFF == code)
            {
                UpdateBuff();
            }
            else if (Singleton<RoleMode>.Instance.UPDATE_FORTUNE == code)
            {
                updateWealth();
            }
            else if (Singleton<RoleMode>.Instance.PLAYER_UPGRADE_INFO == code)
            {
                updateGrade();
            }
			else if (Singleton<RoleMode>.Instance.UPDATE_ROLE_ATTR == code)
			{
				UpdateRoleFight();
			}
        }

		private void ButtonTipsHandle(object sender, int code)
		{
			if (sender.Equals(LoginAwardMode.Instance) && code == LoginAwardMode.Instance.UPDATE_TIPS)
			{
				btn_dljl.FindInChild<UISprite>("tips").SetActive(LoginAwardMode.Instance.ShowTips);
			}

            if (sender.Equals(FriendMode.Instance) && code == FriendMode.ASKLIST)
            {
                btn_hy.FindInChild<UISprite>("tips").SetActive(FriendMode.Instance.ShowTips);
            }
		}

        protected override void HandleAfterOpenView()
        {
            updateInfo();
            updateWealth();
            updateVigour();
            UpdateNewMailInfo();
			UpdateGetAwardInfo();
            btn_hy.FindInChild<UISprite>("tips").SetActive(FriendMode.Instance.ShowTips);
        }

		//打开VIP界面
		private void OpenVIPView(GameObject go)
		{
			Singleton<VIPView>.Instance.OpenView();
		}

		//更新领取奖励按钮小红点状态
		private void UpdateGetAwardInfo()
		{
			/* grsyh
			int day    = Singleton<LoginAwardMode>.Instance.dayInfo.day;
			int status = Singleton<LoginAwardMode>.Instance.dayInfo.status;
			bool isShowTips = false;
			if(day <= LoginAwardConst.LoginAward_7 && status == (int)LoginAwardConst.GetStatus.Receive)
			{
				isShowTips = true;
			}
			btn_dljl.FindInChild<UISprite>("tips").SetActive(isShowTips);
			*/
		}
    }
}