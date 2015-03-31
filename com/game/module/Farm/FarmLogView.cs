using System.Net.Mime;
using com.game.data;
using com.game.manager;
using com.game.vo;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using System.Collections.Generic;
using com.game.utils;
using com.game.consts;

namespace Com.Game.Module.Farm
{
	public class FarmLogView : BaseView<FarmLogView> {

		public override ViewLayer layerType {
			get {
				return ViewLayer.NoneLayer;
			}
		}

		private Button btn_close;
	    private UITextList textList;

		public void Init()
		{
			InitLabelLanguage ();
			btn_close = FindInChild<Button>("topright/btn_close");
		    textList = FindInChild<UITextList>("hylb/txt_lb");
			btn_close.onClick = CloseFarmLogView;
		}

		//多语言处理
		private void InitLabelLanguage()
		{
			//		FindInChild<UILabel>("btn_rank/label").text = LanguageManager.GetWord("ArenaMainView.rank");
		}

		//注册数据更新回调函数
		public override void RegisterUpdateHandler()
		{
			Singleton<FarmMode>.Instance.dataUpdated += UpdateFarmLogView;
		}
		
		//在注册数据更新回调函数之后执行
		protected override void HandleAfterOpenView ()
		{
			base.HandleAfterOpenView ();
			Singleton<FarmMode>.Instance.ApplyFarmLog ();
			
		}
		
		//关闭数据更新回调函数
		public override void CancelUpdateHandler()
		{
			Singleton<FarmMode>.Instance.dataUpdated -= UpdateFarmLogView;
			
		}

		//数据更新回调
		private void UpdateFarmLogView(object sender, int code)
		{
		    string opstring = string.Empty;
		    string dwstring = string.Empty;  //收获信息
		    string name = string.Empty;
			if (code == Singleton<FarmMode>.Instance.UPDATE_FARM_LOG)
			{
				List<FarmLog> logs = Singleton<FarmMode>.Instance.farmLog;
                string timeStr ;
			    foreach (FarmLog farmLog in logs)
			    {

                    //timeU = vp_TimeUtility.TimeToUnits(farmLog.time);
                    //timeStr = string.Format("{0:D2}:{1:D2}", timeU.hours, timeU.minutes);
					TimeProp prop = TimeUtil.GetElapsedTime(farmLog.time);
					if(prop.Days == 0 && prop.Hours == 0)
						timeStr = string.Format(LanguageManager.GetWord("FarmLog.Minute"),prop.Minutes);
					else if(prop.Days ==0)
                        timeStr = string.Format(LanguageManager.GetWord("FarmLog.Hour"), prop.Hours);
					else 
						timeStr = string.Format(LanguageManager.GetWord("FarmLog.Day"),prop.Days);
			        if (farmLog.id == MeVo.instance.Id)
			        {
                        name = LanguageManager.GetWord("FarmLog.You"); ;
                        opstring = LanguageManager.GetWord("FarmLog.Get");

			        }
			        else
			        {
						name = ColorConst.YELLOW + farmLog.name +"[-]";
                        opstring = LanguageManager.GetWord("FarmLog.OtherGet");
			        }
					if (farmLog.type == 2)
					{
					    name = string.Empty;
                        opstring = string.Empty;
                        dwstring = string.Format(LanguageManager.GetWord("FarmLog.UpdateLvl"), ColorConst.GREEN + farmLog.num + "[-]");
					}
					else if (farmLog.goodsId == 2) //金币
					{
                        dwstring = string.Format(ColorConst.GREEN + LanguageManager.GetWord("FarmLog.Gold") + "[-]", farmLog.num);
					}
					else if (farmLog.goodsId == 1) //经验
                        dwstring = string.Format(ColorConst.GREEN + LanguageManager.GetWord("FarmLog.Exp") + "[-]", farmLog.num);
					else //物品
					{
					    dwstring =string.Format( ColorConst.RED + BaseDataMgr.instance.GetDataById<SysItemVo>(farmLog.goodsId).name + " x {0}" +
					               "[-]",farmLog.num);
					}


			        textList.Add(timeStr + name + opstring +dwstring );
				}
				
			    
				//....
			}
		}



		//右上角关闭按钮被点击
		private void CloseFarmLogView(GameObject go)
		{
			this.CloseView ();
		}
	}
}