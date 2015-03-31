using System;
using System.Linq;
using System.Text.RegularExpressions;
/**字符串工具类**/
using com.u3d.bases.debug;

namespace com.game.utils
{
    public class StringUtils
    {
		//string.Format("{0:D3}",23)   023 或者 string.Format("{0:000.00}",023.00);
		//string.Format("{0:N3}",14200)  14,200.00
		//string.Format("{0:P2}",0.2456)   24.56%
        /**判断传入串是否为空串
         * @return true:空串,false:非空串
         * **/
        public static bool isEmpty(String param)
        {
            return param != null && param.Trim().Length > 0 ? false : true;
        }

        /**比较两个字符串值是否相等
         * @return true:相等,false:不相等
         * **/
        public static bool isEquals(String param1, String param2)
        {
            if (param1 == null || param2 == null) return false;
            return param1.Equals(param2);
        }

        /// <summary>
        /// 格式化货币
        /// </summary>
        /// <param name="total">数量</param>
        /// <returns>返回格式化后的字符串</returns>
        public static string formatCurrency(int total)
        {
            string str = total.ToString();
			if(total >= 100000000)
			{
				int n = total / 10000;
				str = String.Format("{0}亿", n);
			}
            else if (total >= 10000)
            {
                int n = total / 10000;
                str = String.Format("{0}万", n);
            }
			//后续更加语言版本在做其他处理……
            return str;
        }
        /// <summary>  
        /// 判断一个字符串是否为合法数字(指定整数位数和小数位数)  
        /// </summary>  
        /// <param name="s">字符串</param>  
        /// <param name="precision">整数位数</param>  
        /// <param name="scale">小数位数</param>  
        /// <returns></returns>  
        public static bool IsNumber(string s, int precision, int scale)
        {
            if ((precision == 0) && (scale == 0))
            {
                return false;
            }
            string pattern = @"(^\d{1," + precision + "}";
            if (scale > 0)
            {
                pattern += @"\.\d{0," + scale + "}$)|" + pattern;
            }
            pattern += "$)";
            return Regex.IsMatch(s, pattern);
        }  

		//返回无后缀字符串
		public static string GetNoSuffixString(string str)
		{
			string result = str.Remove(str.LastIndexOf("."));
			return result;
		}

		//从[XXX,XXX,...]类型的字符串中解析出值列表
		public static string[] GetValueListFromString(string str, char separator = ',')
		{
			string[] resultArr;
			str = str.TrimStart('[');
            str = str.TrimEnd(']');
            str = str.Replace(" ", "");//去除字符串中的空格
            resultArr = str.Split(separator);
			return resultArr;
		}
		/// <summary>
		/// Splits the vo string.
		/// </summary>
		/// <returns>The vo string.</returns>
		/// <param name="str">String.</param>
		/// <param name="delimiter">Delimiter.</param>
		public static string[] SplitVoString(string str, string delimiter = ",")
		{
			string[] resultArr;
			str = str.Replace(" ", "");//去除字符串中的空格
			str = str.TrimStart('[');
			str = str.TrimEnd(']');

			str = str.Replace(delimiter,":");
			resultArr = str.Split(':');
			//resultArr = Regex.Split(str,delimiter);
			return resultArr;
		}

		/// <summary>
		/// 去掉字符串第一个'['和最后一个']'
		/// </summary>
		/// <returns>The vo string.</returns>
		/// <param name="str">String.</param>
		/// <param name="delimiter">Delimiter.</param>
		public static string GetValueString(string str)
		{
			str = str.Replace(" ", "");//去除字符串中的空格
			str = str.TrimStart('[');
			str = str.TrimEnd(']');

			//resultArr = Regex.Split(str,delimiter);
			return str;
		}

		//从[[[100,300],4000],[[301,500],4000],[[501,600],1500],[[601,700],500]]中解析出属性的最小值和最大值
		public static void GetAttrRange(string str, out int attrMin, out int attrMax)
		{
			str = StringUtils.GetValueString(str);  //解析后变成[[100,200],7000],[[201,300],3000],...
			string[] strDescribe1;
			strDescribe1 = StringUtils.SplitVoString (str, "],["); //解析后变成[100,200],7000 [201,300],3000 ...
			
			attrMin = 0;
			attrMax = 0;
			int attrLow, attrHigh;
			string[] strDescribe2;

			strDescribe2 = strDescribe1[0].Split(',');
			attrLow = int.Parse(strDescribe2[0].TrimStart('['));
			attrHigh = int.Parse(strDescribe2[1].TrimEnd(']'));
			attrMin = attrLow;
			attrMax = attrHigh;
			for(int i = 1; i < strDescribe1.Length; ++i)
			{
				strDescribe2 = strDescribe1[i].Split(',');
				attrLow = int.Parse(strDescribe2[0].TrimStart('['));
				attrHigh = int.Parse(strDescribe2[1].TrimEnd(']'));
				attrMin = attrMin < attrLow? attrMin: attrLow;
				attrMax = attrMax > attrHigh? attrMax: attrHigh;
			}
		}

		//配置字段是否有效
		public static bool IsValidConfigParam(string param)
		{
			return (null != param) && (param.Trim().Length > 0) && ("0" != param);
		}

		//将[[5,480,10],[22,96,90]]解析成(5,480,10) (22,96,10)
		//将[[5,480],[22,96]]解析成(5,480) (22,96)
		//将[[5,480]]解析成(5,480)
		public static string[] GetValueCost(string str)
		{
			string[] result;
			result = StringUtils.SplitVoString (str, "],");
			for (int i = 0; i < result.Length; ++i)
			{
				result[i] = result[i].TrimStart('[');
				result[i] = result[i].TrimEnd(']');
			}
			return result;
		}
		//将[{0, [100001], 1},  {5, [100002], 1}, {3, [100003], 2}, {4, [100004], 1}, {0, [100005], 3}]解析成数组
		public static string[] GetMonsterList(string str)
		{
			string[] resultArr; 
		
			
		
			str = str.Replace("[", "");//
			str = str.Replace("]", "");//

			str = Regex.Replace (str, @"\{[0-9]+,", "");
			str = Regex.Replace (str, @"[0-9]+\},", "");
			str = Regex.Replace (str, @"[0-9]+\}", "");
			//去掉最后一个逗号
			str = str.Substring (0, str.Length - 2);

			str = str.Replace(" ", "");

			resultArr = str.Split(',');
			return resultArr;
		}

        /// <summary>
        /// 解析[[1,2],[3,4]] 这类的字符串返回int[]数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] GetArrayStringToInt(string str)
        {
            string pStr = str.Substring(1, str.Length - 2);
            pStr = pStr.Replace("[", "");
            pStr = pStr.Replace("]", "");
            string[] proS = pStr.Split(',');
            if (proS.Count() > 0)
            {
                int[] result = new int[proS.Count()];
                for (int i=0;i<proS.Count();i++)
                {
                    result[i] = int.Parse(proS[i]);
                }
                return result;
            }
            return new int[0];
        }

        /// <summary>
        /// 解析[[1,2]|[3,4]] 这类的字符串返回int[][]数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[][] Get2DArrayStringToInt(string str)
        {
            if(str.Length > 0)
            {
                string pStr = str.Substring(1, str.Length - 2);
                if(pStr.Length > 0)
                {
                    string pStr2;
                    string[] proS1 = pStr.Split('|');
                    string[] proS2;
                    if (proS1.Count() > 0)
                    {
                        int[][] result = new int[proS1.Count()][];
                        for (int i = 0, length1 = proS1.Length; i < length1; i++)
                        {
                            pStr2 = proS1[i];
                            pStr2 = pStr2.Substring(1, pStr2.Length - 2);
                            proS2 = pStr2.Split(',');
                            result[i] = new int[proS2.Length];
                            for (int j = 0, length2 = proS2.Length; j < length2; j++)
                            {
                                result[i][j] = int.Parse(proS2[j]);
                            }
                        }
                        return result;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 解析[1,2,3] 这类的字符串返回int[]数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int[] GetStringToInt(string str)
        {
            if (str == null || str.Length<=2)
            {
                return new int[0];
            }

            string pStr = str.Substring(1, str.Length - 2);
            string[] proS = pStr.Split(',');
            int[] r = new int[proS.Length];

            for (int i = 0; i < proS.Length; i++)
            {
                r[i] = int.Parse(proS[i]);
            }
            return r;
        }

		//获得字符串字节长度
		public static int GetCharLength(string str)
		{
			int length = 0;

			for (int i=0; i<str.Length; i++)
			{
				if (Char.ConvertToUtf32(str, i) >= Convert.ToInt32("4e00", 16)
				    && Char.ConvertToUtf32(str, i) <= Convert.ToInt32("9fff", 16))
				{
					length += 2;
				}
				else
				{
					length += 1;
				}
			}

			return length;
		}
    }
}
