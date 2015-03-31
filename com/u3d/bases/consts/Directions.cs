﻿﻿﻿
/**方向常量类*/

using UnityEngine;

namespace com.u3d.bases.consts
{
    public class Directions
    {
	    //========  模型朝向  =========//
        public const int Top = 1;         //向上
        public const int Right = 2;       //面朝右
        public const int Down = 3;        //向下
        public const int Left  = 4;       //面朝左
        public const int TopRight = 5;    //右上
        public const int TopLeft = 6;     //左上
        public const int DownRight = 7;   //右下
        public const int DownLeft = 8;    //左下

        /// <summary>
        /// 获取对应的相反方向;
        /// </summary>
        /// <param name="dire"></param>
        /// <returns></returns>
        public static int GetOpposite(int dire)
        {
            switch (dire)
            {
                case Top:
                    return Down;
                case Down:
                    return Top;
                case Right:
                    return Left;
                case Left:
                    return Right;
                case TopRight:
                    return DownLeft;
                case DownLeft:
                    return TopRight;
                case TopLeft:
                    return DownRight;
                case DownRight:
                    return TopLeft;
                default:
                    return dire;
            }
        }

        /// <summary>
        /// 8向移动算法
        /// </summary>
        /// <param name="pos">摇杆位置</param>
        /// <returns>移动方向</returns>
        public static int GetDirByVector2(Vector2 pos)
        {
            int dir = 0;
            pos = pos.normalized;
            if (pos.x > 0)
            {
                float tag = pos.y/pos.x;
                if (tag > 5)
                {
                    dir = Top;
                }
                else if (tag > 0.5f)
                {
                    dir = TopRight;
                }
                else if (tag > -0.5f)
                {
                    dir = Right;
                }
                else if (tag > -5)
                {
                    dir = DownRight;
                }
                else
                {
                    dir = Down;
                }
            }
            else if (pos.x < 0)
            {
                float tag = pos.y / pos.x;
                if (tag < -5)
                {
                    dir = Top;
                }
                else if (tag < -0.5f)
                {
                    dir = TopLeft;
                }
                else if (tag < 0.5f)
                {
                    dir = Left;
                }
                else if (tag < 5)
                {
                    dir = DownLeft;
                }
                else
                {
                    dir = Down;
                }
            }
            else
            {
                if (pos.y > 0)
                {
                    dir = Top;
                }
                else if (pos.y < 0)
                {
                    dir = Down;
                }
            }
            return dir;
        }
    }
}
