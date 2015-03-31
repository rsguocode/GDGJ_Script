using UnityEngine;
using System.Collections;
namespace Com.Game.Module.Chat
{
	public class EmotionVo {
		public Vector3 loacalPos;
		public string emoName;

		public EmotionVo(Vector3 Pos, string Name)
		{
			this.loacalPos = Pos;
			this.emoName = Name;
		}

	}
}