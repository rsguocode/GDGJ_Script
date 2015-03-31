﻿﻿using com.net.debug;
using com.net.interfaces;
using com.net.p8583.utils;
using com.net.p8583.vo;
using System;
namespace com.net.p8583
{
	internal class P8583Parser
	{
		internal bool isEncoderTrace = true;
		internal bool isDecoderTrace = true;
		internal static P8583Parser instance = new P8583Parser();
		public IP8583Msg decoder(byte[] bytes)
		{
			int num = 0;
			byte[] array = new byte[(int)NetParams.bitmapLen];
			P8583Msg p8583Msg = new P8583Msg();
			Array.Copy(bytes, 6, array, 0, array.Length);
			p8583Msg.setBitmap(array);
			if (this.isDecoderTrace)
			{
				BitUtils.println(array);
			}
			for (short num2 = 0; num2 < NetParams.bitmapLen; num2 += 1)
			{
				for (short num3 = 0; num3 < 8; num3 += 1)
				{
					if ((array[(int)num2] >> (int)(7 - num3) & 1) == 1)
					{
						short num4 = (short)(num2 * 8 + num3 + 1);
						P8583Tep p8583Tep = NetParams.tepMap[num4];
						byte[] array2;
						if (p8583Tep.maxLen > 0)
						{
							array2 = new byte[(int)p8583Tep.maxLen];
							Array.Copy(bytes, num, array2, 0, (int)p8583Tep.maxLen);
							num += (int)p8583Tep.maxLen;
							if (num4 == 1)
							{
								num += (int)NetParams.bitmapLen;
							}
							if (this.isDecoderTrace)
							{
								NetLog.info(this, string.Concat(new object[]
								{
									"-decoder() no=",
									num4,
									",maxLen=",
									p8583Tep.maxLen,
									",msg=",
									MiscUtils.bytes2String(array2)
								}));
							}
						}
						else
						{
							byte[] array3 = new byte[(int)p8583Tep.lenFlag];
							Array.Copy(bytes, num, array3, 0, (int)p8583Tep.lenFlag);
							num += (int)p8583Tep.lenFlag;
							array2 = new byte[MiscUtils.bytes2Int(array3)];
							Array.Copy(bytes, num, array2, 0, array2.Length);
							num += array2.Length;
							if (this.isDecoderTrace)
							{
								NetLog.info(this, string.Concat(new object[]
								{
									"-decoder() no=",
									num4,
									",lenFlag=",
									p8583Tep.lenFlag,
									",msg=",
									MiscUtils.bytes2String(array2)
								}));
							}
						}
						p8583Msg.setField((int)num4, array2);
					}
				}
			}
			return p8583Msg;
		}
		public byte[] encoder(IP8583Msg p8583Msg, bool isMac = false)
		{
			int macIndex = p8583Msg.getMacIndex();
			p8583Msg.setBitmap(this.createBitmap(p8583Msg));
			if (isMac)
			{
				BitUtils.generateBitmap(p8583Msg.getBitmap(), (short)macIndex);
			}
			byte[] array = new byte[0];
			byte[] bitmap = p8583Msg.getBitmap();
			if (this.isEncoderTrace)
			{
				BitUtils.println(bitmap);
			}
			for (short num = 0; num < NetParams.bitmapLen; num += 1)
			{
				for (short num2 = 0; num2 < 8; num2 += 1)
				{
					if ((bitmap[(int)num] >> (int)(7 - num2) & 1) == 1)
					{
						short num3 = (short)(num * 8 + num2 + 1);
						if ((int)num3 < macIndex)
						{
							P8583Tep p8583Tep = NetParams.tepMap[num3];
							byte[] array3;
							if (p8583Tep.maxLen > 0)
							{
								byte[] array2 = MiscUtils.fill(p8583Msg.getField((int)num3), (int)p8583Tep.maxLen, ' ');
								array3 = new byte[(int)array.Length + (int)p8583Tep.maxLen + (int)((num3 == 1) ? (int)NetParams.bitmapLen : 0)];
								Array.Copy(array, 0, array3, 0, array.Length);
								Array.Copy(array2, 0, array3, array.Length, (int)p8583Tep.maxLen);
								if (num3 == 1)
								{
									Array.Copy(bitmap, 0, array3, array.Length + (int)p8583Tep.maxLen, bitmap.Length);
								}
								if (this.isEncoderTrace)
								{
									NetLog.info(this, string.Concat(new object[]
									{
										"-encoder() no=",
										num3,
										",maxLen=",
										p8583Tep.maxLen,
										",msg=",
										MiscUtils.bytes2String(array2)
									}));
								}
							}
							else
							{
								int num4 = p8583Msg.getField((int)num3).Length;
								byte[] array2 = MiscUtils.number2Bytes(num4, (int)p8583Tep.lenFlag);
								array3 = new byte[array.Length + array2.Length + num4];
								Array.Copy(array, 0, array3, 0, array.Length);
								Array.Copy(array2, 0, array3, array.Length, array2.Length);
								Array.Copy(p8583Msg.getField((int)num3), 0, array3, array.Length + array2.Length, num4);
								if (this.isEncoderTrace)
								{
									NetLog.info(this, string.Concat(new object[]
									{
										"-encoder() no=",
										num3,
										",lenFlag=",
										p8583Tep.lenFlag,
										",msg len=",
										MiscUtils.bytes2Int(array2),
										",msg=",
										MiscUtils.bytes2String(p8583Msg.getField((int)num3))
									}));
								}
							}
							array = array3;
						}
					}
				}
			}
			byte[] result;
			if (isMac)
			{
				byte[] array4 = MiscUtils.string2Bytes("CMAC_000");
				byte[] array3 = new byte[array.Length + array4.Length];
				Array.Copy(array, 0, array3, 0, array.Length);
				Array.Copy(array4, 0, array3, array.Length, array4.Length);
				result = array3;
			}
			else
			{
				result = array;
			}
			return result;
		}
		public byte[] createBitmap(IP8583Msg p8583Msg)
		{
			int macIndex = p8583Msg.getMacIndex();
			byte[] array = new byte[(int)NetParams.bitmapLen];
			short num = 1;
			while ((int)num < macIndex)
			{
				if (p8583Msg.getField((int)num) != null && p8583Msg.getField((int)num).Length > 0)
				{
					BitUtils.generateBitmap(array, num);
				}
				num += 1;
			}
			return array;
		}
	}
}
