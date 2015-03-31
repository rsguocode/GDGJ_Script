//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：EquipIdentificationConst
//文件描述：VIP枚举类
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

namespace Com.Game.Module.VIP
{
	public class VIPConsts 
	{
		//当前页码最小 = 0
		public const int PAGEMIN_0 = 0;
		
		//当前页码最大 = 12
		public const int PAGEMAX_12 = 12;
		
		//页码边界值 = 13
		public const int PAGEBOUNDARY_13 = 13;

		/// <summary>
		/// 领取状态
		/// </summary>
		public enum GetStatus
		{
			/// <summary>
			/// 不能领 = 0
			/// </summary>
			UuReceive = 0,
			/// <summary>
			/// 可领 = 1
			/// </summary>
			Receive = 1,
			/// <summary>
			/// 已领取 = 2
			/// </summary>
			HaveReveive = 2,
		};
	}
}