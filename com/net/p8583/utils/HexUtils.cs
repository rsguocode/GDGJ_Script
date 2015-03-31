﻿﻿using System;
using System.Globalization;
using System.Text;
namespace com.net.p8583.utils
{
	public class HexUtils
	{
		public static byte[] strToHexByte(string hexString)
		{
			hexString = hexString.Replace(" ", "");
			if (hexString.Length % 2 != 0)
			{
				hexString += " ";
			}
			byte[] array = new byte[hexString.Length / 2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
			}
			return array;
		}
		public static string byteToHexStr(byte[] bytes)
		{
			string text = "";
			if (bytes != null)
			{
				for (int i = 0; i < bytes.Length; i++)
				{
					text += bytes[i].ToString("X2");
				}
			}
			return text;
		}
		public static string ToHex(string s, string charset, bool fenge)
		{
			if (s.Length % 2 != 0)
			{
				s += " ";
			}
			Encoding encoding = Encoding.GetEncoding(charset);
			byte[] bytes = encoding.GetBytes(s);
			string text = "";
			for (int i = 0; i < bytes.Length; i++)
			{
				text += string.Format("{0:X}", bytes[i]);
				if (fenge && i != bytes.Length - 1)
				{
					text += string.Format("{0}", ",");
				}
			}
			return text.ToLower();
		}
		public static string UnHex(string hex, string charset)
		{
			if (hex == null)
			{
				throw new ArgumentNullException("hex");
			}
			hex = hex.Replace(",", "");
			hex = hex.Replace("/n", "");
			hex = hex.Replace("//", "");
			hex = hex.Replace(" ", "");
			if (hex.Length % 2 != 0)
			{
				hex += "20";
			}
			byte[] array = new byte[hex.Length / 2];
			for (int i = 0; i < array.Length; i++)
			{
				try
				{
					array[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
				}
				catch
				{
					throw new ArgumentException("hex is not a valid hex number!", "hex");
				}
			}
			Encoding encoding = Encoding.GetEncoding(charset);
			return encoding.GetString(array);
		}
	}
}
