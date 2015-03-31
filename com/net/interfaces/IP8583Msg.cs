﻿﻿using System;
using System.Text;
namespace com.net.interfaces
{
	public interface IP8583Msg
	{
		int getMacIndex();
		void dispose();
		void clear(short index);
		void clear();
		byte[] getField(int index);
		int getFieldInt(int index);
		string getFieldTrim(int index);
		void setField(int index, string str);
		void setField(int index, int src);
		void setField(int index, StringBuilder builder);
		void setField(int index, byte[] bytes);
		bool isExisit(int index);
		byte[] getBitmap();
		string getHexBitmap();
		void setBitmap(byte[] bytes);
		string getCMD();
		void setCMD(string str);
		string getRespCode();
		void setRespCode(string str);
		string getHexMac();
		void setMac(string str);
		void setMac(byte[] bytes);
	}
}
