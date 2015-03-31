//using com.game.module.copy;
using com.game;
using com.game.cmd;
using com.game.consts;
using Com.Game.Module.Copy;
using Com.Game.Module.Guild;
using com.game.module.test;
using Com.Game.Module.Waiting;
using com.game.Public.Message;
using com.game.vo;
using com.net.interfaces;
using com.u3d.bases.debug;
using com.u3d.bases.display.character;
using com.u3d.bases.display.controler;
using PCustomDataType;
using Proto;
using com.game.module.main;
using com.game.Public.Confirm;
using com.game.manager;
using com.game.SDK;
using com.game.module.login;

namespace Com.Game.Module.Role
{
    public class RoleControl : BaseControl<RoleControl>
    {
        protected override void NetListener()
        {
            AppNet.main.addCMD(CMD.CMD_3_1, Fun_3_1);
            AppNet.main.addCMD(CMD.CMD_3_2, Fun_3_2);
            AppNet.main.addCMD(CMD.CMD_3_3, Fun_3_3);
            AppNet.main.addCMD(CMD.CMD_3_4, Fun_3_4);
            AppNet.main.addCMD(CMD.CMD_3_5, Fun_3_5);
//          AppNet.main.addCMD(CMD.CMD_3_20, Fun_3_20);
            AppNet.main.addCMD(CMD.CMD_3_40, Fun_3_40); //购买体力错误码返回
            AppNet.main.addCMD(CMD.CMD_3_41, Fun_3_41); //体力更新信息
            //AppNet.main.addCMD(CMD.CMD_3_2, Fun_3_2);		
            AppNet.main.addCMD(CMD.CMD_3_10, Fun_3_10); //获得经验
            AppNet.main.addCMD(CMD.CMD_3_11, Fun_3_11); //升级
            AppNet.main.addCMD(CMD.CMD_3_20, Fun_3_20); //玩家复活
            AppNet.main.addCMD(CMD.CMD_3_44, Fun_3_44); //玩家状态广播
        }

        /// <summary>
        ///     获取角色属性信息
        /// </summary>
        /// <param name="data">Data.</param>
        public void Fun_3_1(INetData data)
        {
            Log.info(this, "-Fun_3_1（）获取人物详细属性");
            var roleInfoMsg = new RoleInfoMsg_3_1();
            roleInfoMsg.read(data.GetMemoryStream());
            PRoleAttr roleAttr = roleInfoMsg.role;

            MeVo.instance.Id = roleAttr.id;
            MeVo.instance.Name = roleAttr.name;
            MeVo.instance.sex = roleAttr.sex;
            MeVo.instance.Level = roleAttr.level;
            MeVo.instance.job = roleAttr.job;

            //人物属性
            MeVo.instance.exp = roleAttr.exp;
            MeVo.instance.expFull = roleAttr.expFull;
            MeVo.instance.vip = roleAttr.vip;
            //Log.error(this, "Fun_3_1 has been called! " + MeVo.instance.vip.ToString());
            MeVo.instance.nation = roleAttr.nation;
			MeVo.instance.diam = roleAttr.gold;
			MeVo.instance.diamond = roleAttr.diam;
			MeVo.instance.bindingDiamond = roleAttr.diamBind;
            MeVo.instance.vigour = roleAttr.vigour;
            MeVo.instance.vigourFull = roleAttr.vigourFull;
            MeVo.instance.hasCombine = roleAttr.hasCombine;
            MeVo.instance.customFace = roleAttr.customFace;
            MeVo.instance.titleList = roleAttr.titleList;
            //MeVo.instance.fightPoint = roleAttr.fightpoint;
            MeVo.instance.repu = roleAttr.repu;

            if (MeVo.instance.guildId != roleAttr.guildId)
            {
                MeVo.instance.guildId = roleAttr.guildId;
                MeVo.instance.guildName = roleAttr.guildName;
                Singleton<GuildMode>.Instance.NotifyGuildIdChanged();
            }

            //人物基础信息
            PBaseAttr baseAttr = roleAttr.attr;
            MeVo.instance.Str = baseAttr.str;
            MeVo.instance.Agi = baseAttr.agi;
            MeVo.instance.Phy = baseAttr.phy;
            MeVo.instance.Wit = baseAttr.wit;
            MeVo.instance.CurHp = baseAttr.hpCur;
            MeVo.instance.Hp = baseAttr.hpFull;
            MeVo.instance.CurMp = baseAttr.mpCur;
            MeVo.instance.Mp = baseAttr.mpFull;
            MeVo.instance.AttPMin = baseAttr.attPMin;
            MeVo.instance.AttPMax = baseAttr.attPMax;
            MeVo.instance.AttMMin = baseAttr.attMMin;
            MeVo.instance.AttMMax = baseAttr.attMMax;
            MeVo.instance.DefP = baseAttr.defP;
            MeVo.instance.DefM = baseAttr.defM;
            MeVo.instance.Hit = baseAttr.hit;
            MeVo.instance.Dodge = (int)baseAttr.dodge;
            MeVo.instance.Crit = baseAttr.crit;
            MeVo.instance.CritRatio = baseAttr.critRatio;
            MeVo.instance.Flex = baseAttr.flex;
            MeVo.instance.HurtRe = baseAttr.hurtRe;
            MeVo.instance.Speed = baseAttr.speed;
            MeVo.instance.Luck = baseAttr.luck;

            MeVo.instance.DataUpdate(0);
            Log.info(this, "-Fun_3_1(),获取角色buff");
            //Singleton<RoleMode>.Instance.RequestBuffList();

            if(Singleton<MainTopLeftView>.Instance.labVip)
            {
                Singleton<MainTopLeftView>.Instance.labVip.text = MeVo.instance.vip.ToString();
            }
            //开始登陆流程
//			Singleton<LoginMode>.Instance.getRoleList(Singleton<LoginMode>.Instance.platformName, Singleton<LoginMode>.Instance.passwordmName);
        }


        //获取角色buff列表
        private void Fun_3_2(INetData data)
        {
            var roleListMsg = new RoleListMsg_3_2();
            roleListMsg.read(data.GetMemoryStream());
            Singleton<RoleMode>.Instance.UpdateBuffList(roleListMsg.list);
            Log.info(this, "-Fun_3_2()获取角色buff列表，Count : " + roleListMsg.list.Count);
            //MeVo.instance.bufferList = roleListMsg.list;
            BuffManager.Instance.AddBuff(MeVo.instance.Id.ToString(), roleListMsg.list, true);
        }

        public void RequestOtherInfo(uint roleId)
        {
            Singleton<RoleMode>.Instance.RequestOtherRoleInfo(roleId);
            Log.info(this, "请求他人角色信息，角色Id : " + roleId);
        }

        /// <summary>
        ///     请求他人信息
        /// </summary>
        /// <param name="data">Data.</param>
        private void Fun_3_3(INetData data)
        {
            var message = new RoleInfoOtherMsg_3_3();
            message.read(data.GetMemoryStream());
            if (message.code != 0)
            {
                ErrorCodeManager.ShowError(message.code);
                return;
            }
            Singleton<GoodsMode>.Instance.UpdateOtherEquips(message.goodsList);
            Singleton<RoleMode>.Instance.UpdateOtherInfo(message.role);
            Log.info(this, "goodsList Size: " + message.goodsList.Count + " role :   " + message.role.Count);
            //属性信息处理在下面
            Log.info(this, "Fun_3_3() 请求他人角色信息回调：" + message.code);
        }

        /// <summary>
        ///     玩家基础属性更新
        /// </summary>
        private void Fun_3_4(INetData data)
        {
            Log.info(this, "-Fun_3_4()玩家基础属性更新");
            var roleBaseAttrMsg = new RoleAttrMsg_3_4();
            roleBaseAttrMsg.read(data.GetMemoryStream());

            PBaseAttr baseAttr = roleBaseAttrMsg.attr;
            MeVo.instance.Str = baseAttr.str;
            MeVo.instance.Agi = baseAttr.agi;
            MeVo.instance.Phy = baseAttr.phy;
            MeVo.instance.Wit = baseAttr.wit;
            MeVo.instance.CurHp = baseAttr.hpCur;
            MeVo.instance.Hp = baseAttr.hpFull;
            MeVo.instance.CurMp = baseAttr.mpCur;
            MeVo.instance.Mp = baseAttr.mpFull;
            MeVo.instance.AttPMin = baseAttr.attPMin;
            MeVo.instance.AttPMax = baseAttr.attPMax;
            MeVo.instance.AttMMin = baseAttr.attMMin;
            MeVo.instance.AttMMax = baseAttr.attMMax;
            MeVo.instance.DefP = baseAttr.defP;
            MeVo.instance.DefM = baseAttr.defM;
            MeVo.instance.Hit = baseAttr.hit;
            MeVo.instance.Dodge = (int)baseAttr.dodge;
            MeVo.instance.Crit = baseAttr.crit;
            MeVo.instance.CritRatio = baseAttr.critRatio;
            MeVo.instance.Flex = baseAttr.flex;
            MeVo.instance.HurtRe = baseAttr.hurtRe;
            MeVo.instance.Speed = baseAttr.speed;
            MeVo.instance.Luck = baseAttr.luck;
			MeVo.instance.fightPoint = baseAttr.fightPoint;
            Singleton<RoleMode>.Instance.UpdateAttr();
        }

        /// <summary>
        ///     玩家财富值更新
        /// </summary>
        /// <param name="data">Data.</param>
        private void Fun_3_5(INetData data)
        {
            Log.info(this, "-Fun_3_5()玩家财富值更新");
            var roleFortuneMsg = new RoleFortuneMsg_3_5();
            roleFortuneMsg.read(data.GetMemoryStream());
			MeVo.instance.diam = roleFortuneMsg.gold;
			MeVo.instance.diamond = roleFortuneMsg.diam;
			MeVo.instance.bindingDiamond = roleFortuneMsg.diamBind;
            MeVo.instance.repu = roleFortuneMsg.repu;
            Singleton<RoleMode>.Instance.UpdateFortune();
            Singleton<WaitingView>.Instance.CloseView();

            Log.info(this,
			         "chafuxin.............................................................." + MeVo.instance.diam +
			         "................" + MeVo.instance.diamond + "................" +
			         MeVo.instance.bindingDiamond);
        }

        /// <summary>
        ///     角色经验值增加
        /// </summary>
        /// <param name="data">Data.</param>
        private void Fun_3_10(INetData data)
        {
            Log.info(this, "-Fun_3_10()角色经验值增加");
            var roleExpIncMsg = new RoleExpIncMsg_3_10();
            roleExpIncMsg.read(data.GetMemoryStream());
            MeVo.instance.exp += roleExpIncMsg.inc;
        }

        /// <summary>
        ///     玩家升级
        /// </summary>
        /// <param name="data">Data.</param>
        private void Fun_3_11(INetData data)
        {
            Log.info(this, "-Fun_3_11()玩家等级提升");
            var roleUpgradeMsg = new RoleUpgradeMsg_3_11();
            roleUpgradeMsg.read(data.GetMemoryStream());

            int oldLevel = MeVo.instance.Level;

            MeVo.instance.exp = roleUpgradeMsg.exp;
            MeVo.instance.expFull = roleUpgradeMsg.expFull;
            MeVo.instance.Level = roleUpgradeMsg.lvl;

			SDKManager.SDKRoleLevelLog (roleUpgradeMsg.lvl.ToString (), Singleton<LoginMode>.Instance.serverId.ToString ());
            Singleton<RoleMode>.Instance.PlayerUpgrade(oldLevel, roleUpgradeMsg.lvl);
        }

        // 原地满血复活
        private void Fun_3_20(INetData data)
        {
            var roleReviveMsg = new RoleReviveMsg_3_20();
            roleReviveMsg.read(data.GetMemoryStream());

            if (roleReviveMsg.code == 0)
            {
                MeVo.instance.CurHp = MeVo.instance.Hp;
                if (roleReviveMsg.type == MapTypeConst.ROLE_REVIVE_NO_MONEY)
                {
                    if (MeVo.instance.mapId == MapTypeConst.WORLD_BOSS)
                    {
                        AppMap.Instance.me.Pos(3,1.8f);
                    }
                }
                (AppMap.Instance.me.Controller as MeControler).Relive();
                Singleton<RoleMode>.Instance.UpdateReLife();
            }
            else
            {
                ErrorCodeManager.ShowError(roleReviveMsg.code);
            }
        }

        //购买体力错误码返回
        private void Fun_3_40(INetData data)
        {
            var roleVigourBuyMsg = new RoleVigourBuyMsg_3_40();
            roleVigourBuyMsg.read(data.GetMemoryStream());
            Log.info(this, "体力购买错误码: " + roleVigourBuyMsg.code + "购买方式：" + roleVigourBuyMsg.type);

            //显示错误信息
            if (roleVigourBuyMsg.code > 0)
            {
                ErrorCodeManager.ShowError(roleVigourBuyMsg.code);
            }
        }

        //获取体力更新信息
        private void Fun_3_41(INetData data)
        {
            var roleVigourInfo = new RoleVigourInfoMsg_3_41();
            roleVigourInfo.read(data.GetMemoryStream());
            MeVo.instance.vigour = roleVigourInfo.vigour;
            MeVo.instance.vigourFull = roleVigourInfo.vigourFull;
			MeVo.instance.DiamNeedForVigour = roleVigourInfo.diam;
            Log.info(this, "体力有更新，新的体力值: " + MeVo.instance.vigour);

            Singleton<RoleMode>.Instance.UpdateVigour();
        }

        //玩家状态同步
        private void Fun_3_44(INetData data)
        {
            var roleSyncMsg = new RoleSyncMsg_3_44();
            roleSyncMsg.read(data.GetMemoryStream());
            if (roleSyncMsg.id != MeVo.instance.Id)
            {
                PlayerDisplay play = AppMap.Instance.GetPlayer(roleSyncMsg.id + "");
                if (play == null || play.Controller == null) return;
                play.Pos(roleSyncMsg.x*0.001f, roleSyncMsg.y*0.001f);
                play.ChangeDire(roleSyncMsg.dir);
                play.Controller.StatuController.SetStatu(roleSyncMsg.state);
                play.SetSortingOrder(false);
            }
        }

        /// <summary>
        ///     通知服务端当前玩家的血和魔
        /// </summary>
        public void SendHpMpChange()
        {
            Singleton<RoleMode>.Instance.SendHeroHpAndMagic(MeVo.instance.CurHp, MeVo.instance.CurMp);
        }

		public void OpenBuyVigourTips()
		{
			string[] param = {MeVo.instance.DiamNeedForVigour.ToString(), "120"};
			ConfirmMgr.Instance.ShowCommonAlert(LanguageManager.GetWord("ConfirmView.BuyVigour", param),
			                                    ConfirmCommands.BUY_VIGOUR,
			                                    vigourBugCallback, LanguageManager.GetWord("ConfirmView.BuyVigourButton"),
			                                    null, LanguageManager.GetWord("ConfirmView.Cancel"));
		}
		
		private void vigourBugCallback()
		{
			Singleton<RoleMode>.Instance.BuyVigour(GameConst.BUY_VIGOUR_BY_MONEY);
			Log.info(this, "Buy vigour");
		}
    }
}