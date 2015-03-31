﻿﻿using com.net.interfaces;
using com.net.p8583.utils;
using System;
using System.Text;
namespace com.net.p8583.vo
{
	internal class P8583Msg : IP8583Msg
	{
		private int len = 0;
		private byte[][] field = null;
		public P8583Msg()
		{
			this.len = (int)(NetParams.bitmapLen * 8 + 1);
			this.field = new byte[this.len][];
			short num = 0;
			while ((int)num < this.field.Length)
			{
				this.field[(int)num] = new byte[0];
				num += 1;
			}
		}
		public int getMacIndex()
		{
			return this.len - 1;
		}
		public void dispose()
		{
			this.len = 0;
			short num = 0;
			while ((int)num < this.field.Length)
			{
				this.field[(int)num] = null;
				num += 1;
			}
			this.field = null;
		}
		public void clear(short index)
		{
			if (this.isExisit((int)index))
			{
				this.field[(int)index] = new byte[0];
			}
		}
		public void clear()
		{
			short num = 0;
			while ((int)num < this.field.Length)
			{
				this.clear(num);
				num += 1;
			}
		}
		public byte[] getField(int index)
		{
			byte[] result;
			if (this.isExisit(index))
			{
				result = this.field[index];
			}
			else
			{
				result = new byte[0];
			}
			return result;
		}
		public int getFieldInt(int index)
		{
			return int.Parse(this.getFieldTrim(index));
		}
		public string getFieldTrim(int index)
		{
			string result;
			if (this.isExisit(index))
			{
				string text = MiscUtils.bytes2String(this.field[index]);
				result = ((text != null) ? text.Trim() : string.Empty);
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
		public void setField(int index, string str)
		{
			if (this.isExisit(index) && str != null)
			{
				this.field[index] = MiscUtils.string2Bytes(str);
			}
		}
		public void setField(int index, int src)
		{
			this.setField(index, src.ToString());
		}
		public void setField(int index, StringBuilder builder)
		{
			if (builder != null)
			{
				this.setField(index, builder.ToString());
			}
		}
		public void setField(int index, byte[] bytes)
		{
			if (this.isExisit(index) && bytes != null)
			{
				this.field[index] = bytes;
			}
		}
		public bool isExisit(int index)
		{
			return index >= 0 && index < this.field.Length;
		}
		public byte[] getBitmap()
		{
			return this.field[0];
		}
		public string getHexBitmap()
		{
			return HexUtils.byteToHexStr(this.getBitmap());
		}
		public void setBitmap(byte[] bytes)
		{
			if (bytes != null)
			{
				this.field[0] = bytes;
			}
		}
		public string getCMD()
		{
			return this.getFieldTrim(1);
		}
		public void setCMD(string str)
		{
			this.setField(1, str);
		}
		public string getRespCode()
		{
			return this.getFieldTrim(2);
		}
		public void setRespCode(string str)
		{
			this.setField(2, str);
		}
		public string getHexMac()
		{
			return HexUtils.byteToHexStr(this.getField(this.getMacIndex()));
		}
		public void setMac(string str)
		{
			this.setField(this.getMacIndex(), str);
		}
		public void setMac(byte[] bytes)
		{
			if (bytes != null)
			{
				this.field[this.getMacIndex()] = bytes;
			}
		}
	}
}
