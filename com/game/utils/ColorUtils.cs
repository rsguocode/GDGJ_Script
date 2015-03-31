using UnityEngine;
using System.Collections;

using com.game.consts;
using com.u3d.bases.debug;

/**颜色工具类**/
namespace com.game.utils
{
	public class ColorUtils
	{
		
		//颜色值（白，绿，蓝，紫，橙，红）
		private static string[] ColorStrings = new string[]
		{
            ColorConst.WHITE,
			ColorConst.GREEN , 
			ColorConst.BLUE  ,
			ColorConst.PURPLE ,
            ColorConst.ORANGE,
			ColorConst.RED
		};

		//获取装备对应品质颜色
		public static string GetEquipColor(int type , string text)
		{
			return GetColorText(ColorStrings[type-1] , text);
		}

		//获取对应文本颜色
		public static string GetColorText(string color ,string text)
		{
			return color + text + "[-]";
		}

		//返回 0 -1 的RGB 的 Color 值
		public static Color GetColor(int r, int g, int b)
		{
			return new Color(r/255f, g/255f, b/255f);
		}

		//物品品质对应颜色
		public static Color[] colors = new Color[]
		{
			Color.white,
			NGUITools.ParseColor("06ff00" , 0),//绿色
			NGUITools.ParseColor("00c0ff" , 0),//蓝色
			NGUITools.ParseColor("d200ff" , 0),//紫色
			NGUITools.ParseColor("ff9000" , 0),//橙色
			Color.red,//红色
			NGUITools.ParseColor("fff000" , 0),//金色
		};

		/// <summary>
		/// 设置装备名称的颜色
		/// </summary>
		/// <param name="eqname">装备名称</param>
		/// <param name="rank">装备品质</param>
		public static void SetEqNameColor(UILabel eqname, int rank)
		{
			if (rank < 1 || rank >= colors.Length)
			{
				Log.info("Color Utils", "品质有问题");
				eqname.color = colors[0];   // 默认设置为白色
				return;
			}
			eqname.color = colors[rank-1];
		}
	}
}
