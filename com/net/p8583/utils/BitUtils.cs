﻿﻿using System;
using System.Text;
namespace com.net.p8583.utils
{
	public class BitUtils
	{
		public static byte[] convert(short[] indexs)
		{
			byte[] array = new byte[(int)NetParams.bitmapLen];
			short num = 0;
			while ((int)num < indexs.Length)
			{
				short num2 = indexs[(int)num];
				short num3 = (short)((num2 - 1) / 8);
				short num4 = (short)(7 - (num2 - 1) % 8);
				array[(int)num3] = (byte)((int)array[(int)num3] | 1 << (int)num4);
				num += 1;
			}
			return array;
		}
		public static void generateBitmap(byte[] bitmap, short index)
		{
			short num = (short)((index - 1) / 8);
			short num2 = (short)(7 - (index - 1) % 8);
			bitmap[(int)num] = (byte)((int)bitmap[(int)num] | 1 << (int)num2);
		}
		public static bool isExisit(byte[] b, short from, short index)
		{
			short num = (short)((index - 1) / 8 + from);
			short num2 = (short)(7 - (index - 1) % 8);
			return (b[(int)num] >> (int)num2 & 1) == 1;
		}
		public static void println(byte[] bytes)
		{
			if (bytes != null && bytes.Length == (int)NetParams.bitmapLen)
			{
				StringBuilder stringBuilder = new StringBuilder();
				short num = 0;
				while ((int)num < bytes.Length)
				{
					for (short num2 = 0; num2 < 8; num2 += 1)
					{
						if ((bytes[(int)num] >> (int)(7 - num2) & 1) == 1)
						{
							int num3 = (int)(num * 8 + num2 + 1);
							stringBuilder.Append((stringBuilder.Length > 0) ? ("," + num3) : string.Concat(num3));
						}
					}
					num += 1;
				}
				if (stringBuilder.Length > 0)
				{
					Console.WriteLine(stringBuilder);
				}
			}
		}
	}
}
