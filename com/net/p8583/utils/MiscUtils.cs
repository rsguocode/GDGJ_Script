﻿﻿using System;
using System.Text;
namespace com.net.p8583.utils
{
	public class MiscUtils
	{
		private const int P256 = 256;
		public static byte[] number2Bytes(int src, int len)
		{
			byte[] array = new byte[len];
			short num = 0;
			while ((int)num < len)
			{
				int num2 = src % 256;
				array[(int)num] = (byte)(num2 & 255);
				src /= 256;
				num += 1;
			}
			return array;
		}
		public static int bytes2Int(byte[] bytes)
		{
			int num = 0;
			short num2 = 0;
			while ((int)num2 < bytes.Length)
			{
				int num3 = (int)Math.Pow(256.0, (double)num2);
				num += (int)(bytes[(int)num2] & 255) * num3;
				num2 += 1;
			}
			return num;
		}
		public static string bytes2String(byte[] bytes)
		{
			string result;
			try
			{
				string arg_1A_0 = Encoding.GetEncoding(NetParams.encode).GetString(bytes);
				char[] trimChars = new char[1];
				result = arg_1A_0.TrimEnd(trimChars);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				result = null;
			}
			return result;
		}
		public static byte[] string2Bytes(string str)
		{
			return Encoding.GetEncoding(NetParams.encode).GetBytes(str);
		}
		public static byte[] fill(byte[] src, int len, char fillChar)
		{
			byte[] result;
			if (src.Length == len)
			{
				result = src;
			}
			else
			{
				if (src.Length > len)
				{
					byte[] array = new byte[len];
					Array.Copy(src, 0, array, 0, len);
					result = array;
				}
				else
				{
					int num = len - src.Length;
					StringBuilder stringBuilder = new StringBuilder();
					short num2 = 0;
					while ((int)num2 < num)
					{
						stringBuilder.Append(fillChar);
						num2 += 1;
					}
					byte[] array2 = new byte[len];
					byte[] array3 = MiscUtils.string2Bytes(stringBuilder.ToString());
					Array.Copy(src, 0, array2, 0, src.Length);
					Array.Copy(array3, 0, array2, src.Length, array3.Length);
					result = array2;
				}
			}
			return result;
		}
	}
}
