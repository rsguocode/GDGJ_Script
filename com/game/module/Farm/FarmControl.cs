using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game;
using com.game.cmd;
using com.u3d.bases.debug;


/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/03/04 09:39:52 
 * function:  家园种植系统控制类
 * *******************************************************/
using com.net.interfaces;
using Proto;
using com.game.Public.Message;
using com.game.vo;


namespace Com.Game.Module.Farm
{
	public class FarmControl : BaseControl<FarmControl> {

		protected override void NetListener ()
		{
			AppNet.main.addCMD(CMD.CMD_30_1, Fun_30_1);				//农场信息返回
			AppNet.main.addCMD(CMD.CMD_30_2, Fun_30_2);				//种植日志信息返回
			AppNet.main.addCMD(CMD.CMD_30_3, Fun_30_3);				//好友农场简略信息返回
			AppNet.main.addCMD(CMD.CMD_30_4, Fun_30_4);				//好友农场信息返回
			AppNet.main.addCMD(CMD.CMD_30_5, Fun_30_5);				//种子背包信息返回
			AppNet.main.addCMD(CMD.CMD_30_6, Fun_30_6);				//种子背包信息更新
			AppNet.main.addCMD(CMD.CMD_30_7, Fun_30_7);				//农场基础属性更新
			AppNet.main.addCMD(CMD.CMD_30_8, Fun_30_8);				//土地加速返回
			AppNet.main.addCMD(CMD.CMD_30_9, Fun_30_9);				//扩展土地返回
			AppNet.main.addCMD(CMD.CMD_30_10, Fun_30_10);				//升级土地申请返回
			AppNet.main.addCMD(CMD.CMD_30_11, Fun_30_11);				//种子商店信息返回
			AppNet.main.addCMD(CMD.CMD_30_12, Fun_30_12);				//购买种子返回
			AppNet.main.addCMD(CMD.CMD_30_13, Fun_30_13);				//农场操作返回
			AppNet.main.addCMD(CMD.CMD_30_14, Fun_30_14);				//一键收获返回
			AppNet.main.addCMD(CMD.CMD_30_15, Fun_30_15);				//农场种植返回
		}
		
		//服务器返回农场信息
		private void Fun_30_1(INetData data)
		{
			Log.info(this, "-Fun_30_1()服务器返回农场信息");
			FarmInfoMsg_30_1 farmInfoMsg_30_1 = new FarmInfoMsg_30_1 ();
			farmInfoMsg_30_1.read (data.GetMemoryStream ());
			if (farmInfoMsg_30_1.code == 0)
			{
				Singleton<FarmMode>.Instance.UpdateFarmInfo(MeVo.instance.Id, farmInfoMsg_30_1.lvl, 
				                                            farmInfoMsg_30_1.exp, farmInfoMsg_30_1.fullExp, farmInfoMsg_30_1.land);

			}
			else
			{
				ErrorCodeManager.ShowError(farmInfoMsg_30_1.code);	
				return;
			}
		}

		//服务器返回农场信息
		private void Fun_30_2(INetData data)
		{
			Log.info(this, "-Fun_30_2()服务器返回农场日志信息");
			FarmLogMsg_30_2 farmLogMsg_30_2 = new FarmLogMsg_30_2 ();
			farmLogMsg_30_2.read (data.GetMemoryStream ());
			if (farmLogMsg_30_2.code == 0)
			{
				Singleton<FarmMode>.Instance.UpdateFarmLog(farmLogMsg_30_2.log);
				
			}
			else
			{
				ErrorCodeManager.ShowError(farmLogMsg_30_2.code);	
				return;
			}
		}

		//服务器返回好友农场的简略信息
		private void Fun_30_3(INetData data)
		{
			Log.info(this, "-Fun_30_3()服务器返回好友农场的简略信息");
			FarmAllFarmSimpleInfoMsg_30_3 farmAllFarmSimpleInfoMsg_30_3 = new FarmAllFarmSimpleInfoMsg_30_3 ();
			farmAllFarmSimpleInfoMsg_30_3.read (data.GetMemoryStream ());
			if (farmAllFarmSimpleInfoMsg_30_3.code == 0)
			{
				Singleton<FarmMode>.Instance.UpdateFriendsFarmInfo(farmAllFarmSimpleInfoMsg_30_3.info);
				
			}
			else
			{
				ErrorCodeManager.ShowError(farmAllFarmSimpleInfoMsg_30_3.code);	
				return;
			}
		}

		//服务器返回好友农场信息
		private void Fun_30_4(INetData data)
		{
			Log.info(this, "-Fun_30_4()服务器返回好友农场信息");
			FarmFriendInfoMsg_30_4 farmFriendInfoMsg_30_4 = new FarmFriendInfoMsg_30_4 ();
			farmFriendInfoMsg_30_4.read (data.GetMemoryStream ());
			if (farmFriendInfoMsg_30_4.code == 0)
			{
				Singleton<FarmMode>.Instance.UpdateFarmInfo(farmFriendInfoMsg_30_4.id, farmFriendInfoMsg_30_4.lvl, 
				                                            farmFriendInfoMsg_30_4.exp, farmFriendInfoMsg_30_4.fullExp, farmFriendInfoMsg_30_4.land);
				
			}
			else
			{
				ErrorCodeManager.ShowError(farmFriendInfoMsg_30_4.code);	
				return;
			}
		}

		//服务器返回种子信息
		private void Fun_30_5(INetData data)
		{
			Log.info(this, "-Fun_30_5()服务器返回种子信息");
			FarmSeedBagMsg_30_5 farmSeedBagMsg_30_5 = new FarmSeedBagMsg_30_5 ();
			farmSeedBagMsg_30_5.read (data.GetMemoryStream ());

			Log.info(this, "-Fun_30_5()服务器返回种子类型个数：" + farmSeedBagMsg_30_5.seed.Count);

			Singleton<FarmMode>.Instance.UpdateMySeeds (farmSeedBagMsg_30_5.seed);
		}

		//种子背包信息更新
		private void Fun_30_6(INetData data)
		{
			Log.info(this, "-Fun_30_6()种子背包信息更新");
			FarmBagUpdateMsg_30_6 farmBagUpdateMsg_30_6 = new FarmBagUpdateMsg_30_6 ();
			farmBagUpdateMsg_30_6.read (data.GetMemoryStream ());
			
//			Log.info(this, "-Fun_30_5()服务器返回种子个数：" + farmSeedBagMsg_30_5.seed.Count);
			
			Singleton<FarmMode>.Instance.UpdateMySeeds (farmBagUpdateMsg_30_6.seed);
		}

		//农场属性信息更新
		private void Fun_30_7(INetData data)
		{
			Log.info(this, "-Fun_30_7()农场信息更新");
			FarmUpdateMsg_30_7 farmUpdateMsg_30_7 = new FarmUpdateMsg_30_7 ();
			farmUpdateMsg_30_7.read (data.GetMemoryStream ());

			Singleton<FarmMode>.Instance.UpdateFarmAttr(farmUpdateMsg_30_7.lvl, farmUpdateMsg_30_7.exp, farmUpdateMsg_30_7.fullExp);

		}

		//加速收获申请返回
		private void Fun_30_8(INetData data)
		{
			Log.info(this, "-Fun_30_8()加速收获申请返回");
			FarmSpeedSeedUpMsg_30_8 farmSpeedSeedUpMsg_30_8 = new FarmSpeedSeedUpMsg_30_8 ();
			farmSpeedSeedUpMsg_30_8.read (data.GetMemoryStream ());
			if (farmSpeedSeedUpMsg_30_8.code == 0)
			{
				Singleton<FarmView>.Instance.UpdateFarmView (Singleton<FarmMode>.Instance.farmInfo.id);
			}
			else
			{
				ErrorCodeManager.ShowError(farmSpeedSeedUpMsg_30_8.code);	
				return;
			}
		}

		//扩展土地申请返回
		private void Fun_30_9(INetData data)
		{
			Log.info(this, "-Fun_30_9()扩展土地申请返回");
			FarmNewLandMsg_30_9 farmNewLandMsg_30_9 = new FarmNewLandMsg_30_9 ();
			farmNewLandMsg_30_9.read (data.GetMemoryStream ());
			if (farmNewLandMsg_30_9.code == 0)
			{
				Singleton<FarmMode>.Instance.newExpandLand = farmNewLandMsg_30_9.pos;
				
			}
			else
			{
				ErrorCodeManager.ShowError(farmNewLandMsg_30_9.code);	
				return;
			}
		}

		//升级土地申请返回
		private void Fun_30_10(INetData data)
		{
			Log.info(this, "-Fun_30_10()升级土地申请返回");
			FarmUpgradeLandMsg_30_10 farmUpgradeLandMsg_30_10 = new FarmUpgradeLandMsg_30_10 ();
			farmUpgradeLandMsg_30_10.read (data.GetMemoryStream ());
			if (farmUpgradeLandMsg_30_10.code == 0)
			{
//				Singleton<FarmMode>.Instance.newExpandLand = farmUpgradeLandMsg_30_10.pos;
				Singleton<FarmView>.Instance.UpdateFarmView (Singleton<FarmMode>.Instance.farmInfo.id);
				MessageManager.Show("升级土地成功");
			}
			else
			{
				ErrorCodeManager.ShowError(farmUpgradeLandMsg_30_10.code);	
				return;
			}
		}

		//种子商店信息更新
		private void Fun_30_11(INetData data)
		{
			Log.info(this, "-Fun_30_11()种子商店信息更新");
			FarmSeedShopMsg_30_11 farmSeedShopMsg_30_11 = new FarmSeedShopMsg_30_11 ();
			farmSeedShopMsg_30_11.read (data.GetMemoryStream ());
			
			//			Log.info(this, "-Fun_30_5()服务器返回种子个数：" + farmSeedBagMsg_30_5.seed.Count);
			
			Singleton<FarmMode>.Instance.UpdateSeedsStore (farmSeedShopMsg_30_11.seed);
		}

		//购买种子返回
		private void Fun_30_12(INetData data)
		{
			Log.info(this, "-Fun_30_12()购买种子返回");
			FarmBuySeedMsg_30_12 farmBuySeedMsg_30_12 = new FarmBuySeedMsg_30_12 ();
			farmBuySeedMsg_30_12.read (data.GetMemoryStream ());
			if (farmBuySeedMsg_30_12.code == 0)
			{
//				Singleton<FarmMode>.Instance.newExpandLand = farmActionMsg_30_13.pos;
				MessageManager.Show("购买种子成功");
				Singleton<FarmMode>.Instance.UpdateSeedGoodNum(farmBuySeedMsg_30_12.goodsTypeId, farmBuySeedMsg_30_12.num);
			}
			else
			{
				ErrorCodeManager.ShowError(farmBuySeedMsg_30_12.code);	
				return;
			}
		}

		//农场操作返回
		private void Fun_30_13(INetData data)
		{
			Log.info(this, "-Fun_30_13()农场操作返回");
			FarmActionMsg_30_13 farmActionMsg_30_13 = new FarmActionMsg_30_13 ();
			farmActionMsg_30_13.read (data.GetMemoryStream ());
			if (farmActionMsg_30_13.code == 0)
			{
//				Singleton<FarmMode>.Instance.newExpandLand = farmActionMsg_30_13.pos;
				Singleton<FarmView>.Instance.UpdateFarmView (Singleton<FarmMode>.Instance.farmInfo.id);
			}
			else
			{
				ErrorCodeManager.ShowError(farmActionMsg_30_13.code);	
				return;
			}
		}

		//一键收获操作返回
		private void Fun_30_14(INetData data)
		{
			Log.info(this, "-Fun_30_14()一键收获操作返回");
			FarmHarvestOnekeyMsg_30_14 farmHarvestOnekeyMsg_30_14 = new FarmHarvestOnekeyMsg_30_14 ();
			farmHarvestOnekeyMsg_30_14.read (data.GetMemoryStream ());
			if (farmHarvestOnekeyMsg_30_14.code == 0)
			{
	//				Singleton<FarmMode>.Instance.newExpandLand = farmActionMsg_30_13.pos;
				Singleton<FarmView>.Instance.UpdateFarmView (Singleton<FarmMode>.Instance.farmInfo.id);
			}
			else
			{
				ErrorCodeManager.ShowError(farmHarvestOnekeyMsg_30_14.code);	
				return;
			}
		}

		//农场种植返回
		private void Fun_30_15(INetData data)
		{
			Log.info(this, "-Fun_30_15()农场种植操作返回");
			FarmPlantMsg_30_15 farmPlantMsg_30_15 = new FarmPlantMsg_30_15 ();
			farmPlantMsg_30_15.read (data.GetMemoryStream ());
			if (farmPlantMsg_30_15.code == 0)
			{
//				Singleton<FarmMode>.Instance.newExpandLand = farmActionMsg_30_13.pos;
				Singleton<FarmView>.Instance.UpdateFarmView (Singleton<FarmMode>.Instance.farmInfo.id);
				Singleton<MySeedsView>.Instance.CloseMySeedsView();
			}
			else
			{
				ErrorCodeManager.ShowError(farmPlantMsg_30_15.code);	
				return;
			}
		}


	}
}
