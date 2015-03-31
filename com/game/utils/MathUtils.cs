using com.game.consts;
using UnityEngine;

/**
 * 数学相关的工具包
 */

namespace com.game.utils
{
    public class MathUtils
    {
        /** 返回一个0-10000随机数 */
        public static int Random()
        {
            return UnityEngine.Random.Range(0, 10000);
        }

        /** 返回一个0-某个指定数之间的随机数 */

        public static uint Random(uint n)
        {
            return (uint) Random((int) n);
        }

        public static int Random(int n)
        {
            return UnityEngine.Random.Range(0, n);
        }

        /** 返回一个指定区间的随机数 */

        public static uint Random(uint min, uint max)
        {
            return (uint) Random((int) min, (int) max);
        }

        public static int Random(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }

        /** 
		 * 百分比增加(10000为基数)
		 */

        public static int PercentAdd(int var, int percent)
        {
            var v = (float)(var + var*percent/GameConst.PROB_FULL_D);
            return (int)Mathf.Round(v);
        }

        /**
		 * 百分比相乘(10000为基数)
		 */

        public static int PercentMul(int var, int percent)
        {
            var v = (float)(var*percent/GameConst.PROB_FULL_D);
            return (int) Mathf.Round(v);
        }

        /**
		 * 百分比减少(10000为基数)
		 */

        public static int PercentSub(int var, int percent)
        {
            var v =(float)(var - var*percent/GameConst.PROB_FULL_D);
            return (int) Mathf.Max(0, Mathf.Round(v));
        }

        /// <summary>
        ///     返回从min到max之间的length个互异随机数序列
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int[] RandomSequence(int min, int max, int length)
        {
            var array = new int[length];
            int i = 0;
            int a;
            while (i < length)
            {
                a = Random(min, max);
                int j = 0;
                for (; j < i; j++)
                {
                    if (a == array[j])
                        break;
                }
                if (j == i)
                {
                    array[i] = a;
                    i++;
                }
            }
            return array;
        }

        /// <summary>
        ///     返回从min到max之间的互异随机数序列，最大长度为maxLength
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static int[] RandomRsequence(int min, int max, int maxLength)
        {
            int length = Random(0, maxLength);
            var array = new int[length];
            int i = 0;
            int a;
            while (i < length)
            {
                a = Random(min, max);
                int j = 0;
                for (; j < i; j++)
                {
                    if (a == array[j])
                        break;
                }
                if (j == i)
                {
                    array[i] = a;
                    i++;
                }
            }
            return array;
        }
    }
}