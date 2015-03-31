using Com.Game.Module.Chat;
using Com.Game.Module.Friend;
using Com.Game.Module.GoldSilverIsland;
using Com.Game.Module.LuckWand;
using com.game.module.Mail;
﻿﻿﻿using com.game.module.map;
using com.game.module.login;
using Com.Game.Module.Pet;
using Com.Game.Module.Role.Deed;
using com.game.module.Task;
using com.game.module.test;
using Com.Game.Module.Role;
using Com.Game.Module.SystemSetting;
using com.game.module.Store;
using Com.Game.Module.Equip;
using Com.Game.Module.Lottery;
using Com.Game.Module.Copy;
using Com.Game.Module.Arena;
using Com.Game.Module.Boss;
using Com.Game.Module.GoldBox;
using Com.Game.Module.GoldHit;
using Com.Game.Module.Treasure;
using com.game.module.LoginAward;
using Com.Game.Module.Guild;
using Com.Game.Module.Farm;
using Com.Game.Module.VIP;
using Com.Game.Module.LuckDraw;
using Com.Game.Module.DaemonIsland;

namespace com.game
{
    public class AppFacde
    {

        public readonly static AppFacde instance = new AppFacde();


        public AppFacde()
        {
        }

        /**系统模块初始化**/
        public void Init()
        {
            Singleton<MapControl>.Instance.ToString();
            Singleton<LoginControl>.Instance.ToString();
            Singleton<GoodsControl>.Instance.ToString();
            Singleton<RoleControl>.Instance.ToString();
			Singleton<Equip1Control>.Instance.ToString();    //装备模块
			Singleton<AwardControl>.Instance.ToString ();           //注册副本奖励统计模块
			Singleton<ChatControl>.Instance.ToString ();            //注册聊天模块
            Singleton<MailControl>.Instance.ToString();             //注册邮件模块
			Singleton<StoreControl>.Instance.ToString ();           //注册商城模块
			Singleton<SystemSettingControl>.Instance.ToString();    //注册系统设置模块
			Singleton<LotteryControl>.Instance.ToString();          //注册抽奖模块
            Singleton<DeedControl>.Instance.ToString();             //注册契约模块
			Singleton<AwardControl>.Instance.ToString();            //注册副本奖励模块
			Singleton<ArenaControl>.Instance.ToString();            //注册竞技场模块
			Singleton<BossControl>.Instance.ToString();             //注册世界Boss模块
			Singleton<GoldBoxControl>.Instance.ToString();          //注册黄金宝箱模块
            Singleton<GoldHitControl>.Instance.ToString();          //注册击石成金模块
            Singleton<GoldSilverIslandControl>.Instance.ToString(); //注册金银岛模块
            Singleton<LuckWandControl>.Instance.ToString();         //注册幸运魔杖模块
			Singleton<TreasureControl>.Instance.ToString();         //注册地宫寻宝模块
            Singleton<LoginAwardControl>.Instance.ToString();       //注册登陆奖励模块
			Singleton<GuildControl>.Instance.ToString();            //注册公会模块
			Singleton<FarmControl>.Instance.ToString();             //注册家园种植模块
            Singleton<VIPControl>.Instance.ToString();              //注册VIP模块
            Singleton<GrowControl>.Instance.ToString();             //注册培养模块
			Singleton<LuckDrawControl>.Instance.ToString();         //注册萌宠献礼模块
			Singleton<DaemonIslandControl>.Instance.ToString();         //注册恶魔岛模块
        }

        /// <summary>
        /// 进入场景后执行的初始化，此时后端的初始化工作刚好完成
        /// </summary>
        public void InitAfterIntoScene()
        {
			Singleton<CopyControl>.Instance.ToString ();            //注册副本模块
            Singleton<TaskControl>.Instance.ToString();
            Singleton<PetControl>.Instance.ToString();
            Singleton<SkillControl>.Instance.ToString();            //注册技能模块
            Singleton<FriendControl>.Instance.ToString();            //注册技能模块

            VIPLogic.InitConfig(); //vip初始化配置
        }

    }
}
