//////////////////////////////////////////////////////////////////////////////////////////////
//文件名称：LoginAwardConst
//文件描述：登陆奖励枚举
//创建者：张永明
//////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

namespace com.game.module.LoginAward
{
	public class LoginAwardConst : MonoBehaviour {
		
		public const int LoginAward_7 = 7;
		
		public const int LoginAwardItem_5 = 5;

		public const int SignDay_31 = 31;

		/// <summary>
		/// 领取状态
		/// </summary>
		public enum GetStatus
		{
			/// <summary>
			/// 未领取 = 0
			/// </summary>
			UuReceive = 0,
			/// <summary>
			/// 领取 = 1
			/// </summary>
			Receive = 1,
			/// <summary>
			/// 已领取 = 2
			/// </summary>
			HaveReveive = 2,
		};

		/// <summary>
		/// 礼包类型
		/// </summary>
		public enum GiftType
		{
			/// <summary>
			/// 激活码
			/// </summary>
			ActivationCode = 1,
		};
	}
}
