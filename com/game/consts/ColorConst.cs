using System;
using UnityEngine;

/**颜色常量类**/
namespace com.game.consts
{
    public class ColorConst
    {
        /// <summary>
        ///     白色
        /// </summary>
        public const string WHITE = "[FFFFFF]"; //白色
        public const string WHITE_FORMAT = "[FFFFFF]{0}[-]"; //白色

        /// <summary>
        ///     黑色
        /// </summary>
        public const string BLACK = "[000000]"; //黑色
        public const string BLACK_FORMAT = "[000000]{0}[-]"; //黑色


        /// <summary>
        ///     绿色
        /// </summary>
        public const string GREEN = "[4AD83B]"; //绿色
        public const string GREEN_FORMAT = "[4AD83B]{0}[-]"; //绿色

        /// <summary>
        ///     蓝色
        /// </summary>
        public const string BLUE = "[3B8FED]"; //蓝色
        public const string BLUE_FORMAT = "[3B8FED]{0}[-]"; //蓝色

        /// <summary>
        ///     红色
        /// </summary>
        public const string RED = "[FF0000]"; //红色，帝
        public const string RED_FORMAT = "[FF0000]{0}[-]"; //红色，帝


        /// <summary>
        ///     橙色
        /// </summary>
        public const string ORANGE = "[FF7800]"; //橙色
        public const string ORANGE_FORMAT = "[FF7800]{0}[-]"; //橙色

        /// <summary>
        ///     紫色
        /// </summary>
        public const string PURPLE = "[B258F7]"; //紫色
        public const string PURPLE_FORMAT = "[B258F7]{0}[-]"; //紫色

        /// <summary>
        ///     黄色 醒目字颜色
        /// </summary>
        public const string YELLOW = "[FFE400]"; //黄色c
        public const string YELLOW_FORMAT = "[FFE400]{0}[-]"; //黄色c

        /// <summary>
        ///     褐色 页签字颜色
        /// </summary>
        public const string BROWN = "[5C240E]"; //褐色
        public const string BROWN_FORMAT = "[7E240E]{0}[-]"; //褐色

        /// <summary>
        /// 装备基础属性的蓝色 126 ，190 ，227
        /// </summary>
        public const string EQUIP_BLUE_FORMAT =  "[5CBEE3]{0}[-]";
        /// <summary>
        /// 
        /// </summary>
        public const string TAB_GRAY = "[A9A9A9]{0}[-]";//页签未选中的灰色
        /// <summary>
        /// 按钮的描边颜色
        /// </summary>
        public static readonly Color GreenOutline = new Color(65/255f,116/255f,35/255f);  //绿色按钮的描边
        public static readonly Color BlueOutline = new Color(65 / 255f, 116 / 255f, 35 / 255f);  //蓝色按钮的描边
        public static readonly Color YellowOutline = new Color(65 / 255f, 116 / 255f, 35 / 255f);  //黄色按钮的描边

		public static readonly Color GRAY = new Color(63f/255f, 63f/255f, 63f/255f); //灰色
        public static readonly Color Black = new Color(1f/255f, 1f/255f, 1f/255f); //黑色
        public static readonly Color Normal = new Color(1, 1, 1); //正常
        public static readonly Color Mask = new Color(73f/255f, 73f/255f, 73f/255f); //遮罩

        public static readonly Color GREEN_YES = new Color(127f / 255f, 1, 61f / 255f); //绿色提醒色
        public static readonly Color RED_NO = new Color(193f/255f, 37f/255f, 37f/255f); //红色提醒色

        public static readonly Color FONT_GRAY = new Color(169f / 255f, 169f / 255f, 169f / 255f); //字体 灰色
        public static readonly Color FONT_LIGHT = new Color(255f/255f, 203f/255f, 61f/255f); //字体主亮色
        public static readonly Color FONT_WHITE = new Color(255f/255f, 255f/255f, 255f/255f); //字体主亮色
        public static readonly Color FONT_BROWN = new Color(92f/255f, 36f/255f, 14f/255f); //字体褐色
        public static readonly Color FONT_BLACK = new Color(10f / 255f, 10f / 255f, 10f / 255f); //字体黑市

        public static readonly Color FONT_YELLOW = new Color(223f / 255f, 188f / 255f, 0f / 255f); //字体 黄色
        public static readonly Color FONT_RED = new Color(234f / 255f, 64f / 255f, 31f / 255f); //字体 红色
        public static readonly Color FONT_MILKWHITE = new Color(219f / 255f, 211f / 255f, 191f / 255f); //字体-乳白色
        public static readonly Color FONT_GREEN = new Color(127f / 255f, 223f / 255f, 64f / 255f); // 字体 绿色
        public static readonly Color FONT_BLUE = new Color(126f / 255f, 189 / 255f, 226f / 255f); // 字体 蓝色
        /**这里是特效的COLOR***/
        public static readonly Color GreenStart = new Color(90f/255f, 1, 60f/255f);
        public static readonly Color GreenEnd = new Color(38f/255f, 111f/255f, 24f/255f);
        public static readonly Color RedStart = new Color(255f/255f, 220f/255f, 220f/255f);
        public static readonly Color RedEnd = new Color(255f/255f, 0/255f, 0f/255f);
        public static readonly Color WhiteStart = new Color(38f/255f, 111/255f, 24f/255f);
        public static readonly Color WhiteEnd = new Color(38f/255f, 111/255f, 24f/255f);
        public static readonly Color BeAttackStep1Color = new Color(255f / 255f, 188 / 255f, 188 / 255f);
        public static readonly Color BeAttackStep2Color = new Color(255f / 255f, 80 / 255f, 80 / 255f);
        public static readonly Color BeAttackMaintainColor = new Color(255f / 255f, 20 / 255f, 10 / 255f);
        public static readonly Color Blood = new Color(1,0.078f,0.039f);

		public static readonly Color purple = new Color(223f/255f, 18f/255f, 82f/255f);
		public static readonly Color orange = new Color(255f/255f, 58f/255f, 0f/255f);
		public static readonly Color red = new Color(255f/255f, 0f/255f, 0f/255f);


        /**这里是掉血数字渐变色的Color**/
        public static readonly Color NormalGradientBottom = new Color(254f/255,255f/255,0f/255);
        public static readonly Color NormalGradientTop = new Color(254f/255, 43f/ 255, 0f/255);
        public static readonly Color CritGradientBottom = new Color(178f / 255, 0f / 255, 255f/255);
        public static readonly Color CritGradientTop = new Color(255f / 255, 197f / 255, 241f/255);
        public static readonly Color DodgeGradientBottom = new Color(240f / 255, 240f / 255, 240f/255);
        public static readonly Color DodgeGradientTop = new Color(111f / 255, 111f / 255, 111f/255);
        public static readonly Color AddHpGradientBottom = new Color(155f / 255, 255f / 255, 86f / 255);
        public static readonly Color AddHpGradientTop = new Color(0f / 255, 255f / 255, 0f / 255);
        public static readonly Color UnbeatableGradientBottom = new Color(255f / 255, 0f / 255, 0f / 255);
        public static readonly Color UnbeatableGradientTop = new Color(255f / 255, 180f / 255, 180f / 255);

    }
}