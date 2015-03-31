using com.game.consts;
using Com.Game.Module.Copy;
using com.u3d.bases.display.character;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game;
using com.game.cmd;
using com.net.interfaces;
using com.u3d.bases.debug;
using Proto;
using com.game.Public.Message;
using com.game.module.map;
using com.game.module.battle;
using com.u3d.bases.controller;
using com.game.vo;
using Com.Game.Module.Role;

namespace Com.Game.Module.Boss
{
	public class BossControl : BaseControl<BossControl> {

		protected override void NetListener()
		{
			AppNet.main.addCMD(CMD.CMD_22_1, Fun_22_1); //开启通知
			AppNet.main.addCMD(CMD.CMD_22_2, Fun_22_2); //累计奖励
			AppNet.main.addCMD(CMD.CMD_22_3, Fun_22_3); //伤害排行信息
			AppNet.main.addCMD(CMD.CMD_22_4, Fun_22_4); //鼓舞
			AppNet.main.addCMD(CMD.CMD_22_5, Fun_22_5); //boss死亡
			AppNet.main.addCMD(CMD.CMD_22_6, Fun_22_6); //boss血量
			AppNet.main.addCMD(CMD.CMD_22_7, Fun_22_7); //结束通知
			AppNet.main.addCMD(CMD.CMD_22_8, Fun_22_8); //玩家鼓舞情况（在登陆是下发）
			AppNet.main.addCMD(CMD.CMD_22_9, Fun_22_9); //战况（在登陆是下发）
			AppNet.main.addCMD(CMD.CMD_22_10, Fun_22_10); //战况（在登陆是下发）
			AppNet.main.addCMD(CMD.CMD_22_11, Fun_22_11); //战况（在登陆是下发）

			
		}
		public void QuitBossScene()
		{
			if(MeVo.instance.CurHp == 0)   //时间到了，还没复活就主动复活
			{
				Singleton<RoleMode>.Instance.ReLife();
				Singleton<BossTips>.Instance.ForceCloseView();
			}
			else
                CopyMode.Instance.ApplyQuitCopy();

            Log.info(this, "请求退出世界Boss");
		}
		//进入世界Boss副本,如果中途Boss死亡了，就还未开启
		public void EnterBossScene()
		{
			Log.info(this,"请求进入世界Boss");
			//if(Singleton<BossMode>.Instance.restTime == 0)  //已开启
			//{
				Singleton<BossMode>.Instance.isDie = false;
				Singleton<MapMode>.Instance.changeScene(40001, false, 5, 1.8f);
			//}
			//else{
				//MessageManager.Show("世界Boss还未开启!");
			//}
		}
		public void Attack(uint hurt)
		{
			Singleton<BossMode>.Instance.Attack(hurt);
		}
		public void GuWu( byte type)
		{
		    if ( !VipManager.IsVipDriotOpen(VipManager.WorldBossInspiation))
		        MessageManager.Show("Vip等级不够");
		    else
		    {
                if(type == BossMode.AttackGuwuId && BossMode.Instance.AttackTime == BossMode.GuwuMaxTime
                    || type == BossMode.CritGuwuId && BossMode.Instance.CritTime == BossMode.GuwuMaxTime)
                    MessageManager.Show("鼓舞属性已达上限");
                else
		            Singleton<BossMode>.Instance.GuWu(type);
		    }
		}
		//开启通知
		private void Fun_22_1(INetData data)
		{
			WorldBossStartNoticeMsg_22_1 message = new WorldBossStartNoticeMsg_22_1();
            message.read(data.GetMemoryStream());
			Singleton<BossMode>.Instance.BossStart(message.restTime);
            Log.info(this, "Fun_22_1 世界Boss开启 restTime:" + message.restTime);
		}
        //进入倒计时
		private void Fun_22_2(INetData data)
		{
			WorldBossReadyMsg_22_2 message =new  WorldBossReadyMsg_22_2();
			message.read(data.GetMemoryStream());
			//Singleton<BossMode>.Instance.BossComeTime(message.time);
			Log.info(this, "Fun_22_2 time 进入倒计时: " + message.time);
		}
		//伤害排行榜信息
		private void Fun_22_3(INetData data)
		{
			WorldBossRankMsg_22_3 message = new WorldBossRankMsg_22_3();
			message.read(data.GetMemoryStream());
			Singleton<BossMode>.Instance.HurtRank(message);
            Log.info(this, "Fun_22_3 伤害排行榜信息");
		}
		//鼓舞
		private void Fun_22_4(INetData data)
		{
			WorldBossGuwuMsg_22_4 message = new WorldBossGuwuMsg_22_4();
			message.read(data.GetMemoryStream());
			if(message.code == 0)
			{
				Singleton<BossMode>.Instance.GuWuInfo(message);
			}
			else{
				ErrorCodeManager.ShowError(message.code);
			}
            Log.info(this, "Fun_22_4 鼓舞");
		}
		//boss死亡
		private void Fun_22_5(INetData data)
		{
			WorldBossDieMsg_22_5 message = new WorldBossDieMsg_22_5();
			message.read(data.GetMemoryStream());
			Singleton<BossMode>.Instance.BossDie(message);
			//Singleton<BossTips>.Instance.OpenWNView("XXBoss",3,QuitBossScene);
			Log.info(this, "Fun_22_5 Boss死亡 gold : " + (message.gold + message.extGold)+ "  repu:  "+ (message.extRepu + message.repu));
		}
		//boss血量
		private void Fun_22_6(INetData data)
		{
			WorldBossHpMsg_22_6 message = new WorldBossHpMsg_22_6();
			message.read(data.GetMemoryStream());
			Singleton<BossMode>.Instance.BossHp(message);
			Log.info(this, "Fun_22_6 Boss血量 .........................." + "damage: " + message.damage + " curr : "+ message.currHp + " full: "+message.fullHp);
		}
		//结束通知
		private void Fun_22_7(INetData data)
		{
			WorldBossEndMsg_22_7 message = new WorldBossEndMsg_22_7();
			message.read(data.GetMemoryStream());
			Singleton<BossMode>.Instance.BossEnd(message.code,message.time);
			Log.info(this, "Fun_22_7 世界Boss结束通知 time: "+ message.time + " code : "  + message.code);
		}
		//玩家鼓舞情况（在登陆时下发）
		private void Fun_22_8(INetData data)
		{
			WorldBossSelfGuwuMsg_22_8 message = new WorldBossSelfGuwuMsg_22_8();
			message.read(data.GetMemoryStream());
			Singleton<BossMode>.Instance.GuWuBegin(message.attTime,message.dmageTime);
			Log.info(this,"count: "+message.attTime+"....count : " +message.dmageTime);
            Log.info(this, "Fun_22_8 玩家鼓舞情况（在登陆时下发）");
		}
		//战况（在登录时下发）
		private void Fun_22_9(INetData data)
		{
			WorldBossInfoMsg_22_9 message = new WorldBossInfoMsg_22_9();
			message.read(data.GetMemoryStream());
			Singleton<BossMode>.Instance.FightInfo(message);
			Log.info(this," .........hpful: " +message.hpfull + " damage: "+message.damage);
			Log.info(this, "Fun_22_9 状况登陆是下发： gold :" + message.gold + " repu : " + message.repu);
		}
		//角色攻击
		private void Fun_22_10(INetData data)
		{
 			WorldBossAttackMsg_22_10 message = new WorldBossAttackMsg_22_10();
			message.read(data.GetMemoryStream());
			if(message.code != 0)
				ErrorCodeManager.ShowError(message.code);
			else
			{
				//Singleton<BossMode>.Instance.RoleAttack(message.hurt);  //走22_9协议
			}
			Log.info(this, "Fun_22_10  角色攻击 hurt: " +message.hurt);
		}

		//boss攻击 
        /// <summary>
        /// type 0 表示预警 1 表示 释放
        /// </summary>
        /// <param name="data"></param>
		private void Fun_22_11(INetData data)
		{            
			WorldBossBossFightMsg_22_11 message = new WorldBossBossFightMsg_22_11();
			message.read(data.GetMemoryStream());
			if(AppMap.Instance.GetMonster().Count>0)
			{
			    MonsterDisplay monster = AppMap.Instance.GetMonster()[0];
                monster.StartShowWarningEffect(ColorConst.RedStart, ColorConst.RedEnd,2f);
			    if (message.type == 0)
			    {
			        monster.Controller.SkillController.RequestUseSkill(SkillController.Attack1);
			    }
			    else if (message.type == 1)
			    {
                    monster.Controller.SkillController.RequestUseSkill(SkillController.Attack2);
			    }
			}
            Debug.Log("****Fun_22_11 播放世界Boss技能 ttype 0 表示预警 1 表示 释放： " + message.type + " boss 技能Id： " + message.skillId);
            Log.info(this, "Fun_22_11 播放世界Boss技能 ttype 0 表示预警 1 表示 释放： " + message.type + " boss 技能Id： " + message.skillId);
		}



	}
}
