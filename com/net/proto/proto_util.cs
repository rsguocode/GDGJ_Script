using System.IO;
using System;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace Proto
{
    public class proto_util
    {
        public static MemoryStream newStream = new MemoryStream();

        private static void readStream(MemoryStream oldStream, uint offset)
        {
            oldStream.Position = oldStream.Position + offset;
        }

        // ÓÐ·ûºÅ1×Ö½Ú
        public static sbyte readByte(MemoryStream data)
        {
            sbyte tmpvalue;
            tmpvalue = (sbyte)data.GetBuffer()[data.Position];
            readStream(data, sizeof(sbyte));
            return tmpvalue;
        }

        // ÎÞ·ûºÅ1×Ö½Ú
        public static byte readUByte(MemoryStream data)
        {
            byte tmpvalue;
            tmpvalue = (byte)data.GetBuffer()[data.Position];
            readStream(data, sizeof(sbyte));
            return tmpvalue;
        }

        // ÓÐ·ûºÅÁ½×Ö½Ú
        public static short readShort(MemoryStream data)
        {
            short tmpvalue;

            byte[] nowData = data.GetBuffer();
            tmpvalue = BitConverter.ToInt16(nowData, (int)data.Position);
            tmpvalue = IPAddress.NetworkToHostOrder(tmpvalue);
            readStream(data, sizeof(short));
            return tmpvalue;
        }

        // ÎÞ·ûºÅ2×Ö½Ú
        public static ushort readUShort(MemoryStream data)
        {
            ushort tmpvalue;

            byte[] nowData = data.GetBuffer();

            tmpvalue = BitConverter.ToUInt16(nowData, (int)data.Position);
            tmpvalue = (ushort)IPAddress.NetworkToHostOrder((short)tmpvalue);
            readStream(data, sizeof(short));

            return tmpvalue;
        }

        // ÓÐ·ûºÅ4×Ö½Ú
        public static int readInt(MemoryStream data)
        {
            int tmpvalue;

            byte[] nowData = data.GetBuffer();
            tmpvalue = BitConverter.ToInt32(nowData, (int)data.Position);
            tmpvalue = IPAddress.NetworkToHostOrder(tmpvalue);
            readStream(data, sizeof(int));

            return tmpvalue;
        }

        // ÎÞ·ûºÅ4×Ö½Ú
        public static uint readUInt(MemoryStream data)
        {
            uint tmpvalue;

            byte[] nowData = data.GetBuffer();
            tmpvalue = BitConverter.ToUInt32(nowData, (int)data.Position);
            tmpvalue = (uint)IPAddress.NetworkToHostOrder((int)tmpvalue);
            readStream(data, sizeof(int));

            return tmpvalue;
        }

        // ÎÞ·ûºÅ8×Ö½Ú
        public static UInt64 readULong(MemoryStream data)
        {
            UInt64 tmpvalue = 0;

            byte[] nowData = data.GetBuffer();

            int _begin = (int)data.Position;
            int _end = (int)data.Position + sizeof(Int64) + 1;

            for (int i = _begin, j = sizeof(Int64) - 1; i < _end; i++, j--)
                tmpvalue += (UInt64)(nowData[i] * Math.Pow(256, j));
            readStream(data, sizeof(Int64));

            return tmpvalue;
        }

        // ×Ö·û´®
        public static string readString(MemoryStream data)
        {
            int Len = readShort(data);

            string desc = System.Text.Encoding.UTF8.GetString(data.GetBuffer(), (int)data.Position, Len);
            readStream(data, (uint)Len);


            return desc;
        }

		public static bool readBool(MemoryStream data)
		{
			byte _value = readUByte(data);
			if (_value > 0)
								return true;
						else
								return false;
		}

		public static void readLoopBool(MemoryStream data, List<bool> _list)
		{
			int Len = readShort(data);
			for (int i = 0; i < Len; i++) _list.Add(readBool(data));
		}

        // ÆÕÍ¨1×Ö½ÚÕûÐÎÁÐ±í
        public static void readLoopUByte(MemoryStream data, List<byte> _list)
        {
            int Len = readShort(data);
            for (int i = 0; i < Len; i++) _list.Add(readUByte(data));
        }

		public static void readLoopByte(MemoryStream data, List<sbyte> _list)
		{
			int Len = readShort(data);
			for (int i = 0; i < Len; i++) _list.Add(readByte(data));
		}

        // ÆÕÍ¨2×Ö½ÚÕûÐÎÁÐ±í
        public static void readLoopUShort(MemoryStream data, List<ushort> _list)
        {
            int Len = readShort(data);
            for (int i = 0; i < Len; i++) _list.Add(readUShort(data));
        }

		public static void readLoopShort(MemoryStream data, List<short> _list)
		{
			int Len = readShort(data);
			for (int i = 0; i < Len; i++) _list.Add(readShort(data));
		}

        // ÆÕÍ¨4×Ö½ÚÕûÐÎÁÐ±í
        public static void readLoopUInt(MemoryStream data, List<uint> _list)
        {
            int Len = readShort(data);
            for (int i = 0; i < Len; i++) _list.Add(readUInt(data));
        }

		public static void readLoopInt(MemoryStream data, List<int> _list)
		{
			int Len = readShort(data);
			for (int i = 0; i < Len; i++) _list.Add(readInt(data));
		}

		public static void readLoopString(MemoryStream data, List<string> _list)
		{
			int Len = readShort(data);
			for (int i = 0; i < Len; i++) _list.Add(readString(data));
		}

        // ÓÐ·ûºÅ1×Ö½Ú
        public static void writeByte(MemoryStream data, sbyte tmpvalue)
        {
            byte[] tmp = new byte[1];
            tmp[0] = (byte)tmpvalue;
            data.Write(tmp, 0, 1);
        }
        
        // ÎÞ·ûºÅ1×Ö½Ú
        public static void writeUByte(MemoryStream data, byte tmpvalue)
        {
            byte[] nowData = BitConverter.GetBytes(tmpvalue);
            data.Write(nowData, 0, sizeof(byte));
        }

        // ÓÐ·ûºÅ2×Ö½Ú
        public static void writeShort(MemoryStream data, short tmpvalue)
        {
			tmpvalue = IPAddress.HostToNetworkOrder(tmpvalue);
            byte[] nowData = BitConverter.GetBytes(tmpvalue);
            data.Write(nowData, 0, sizeof(short));
        }

        // ÎÞ·ûºÅ2×Ö½Ú
        public static void writeUShort(MemoryStream data, ushort tmpvalue)
        {
            tmpvalue = (ushort)IPAddress.HostToNetworkOrder((short)tmpvalue);
            byte[] nowData = BitConverter.GetBytes(tmpvalue);
            data.Write(nowData, 0, sizeof(ushort));
        }

        // ÓÐ·ûºÅ4×Ö½Ú
        public static void writeInt(MemoryStream data, int tmpvalue)
        {
            tmpvalue = IPAddress.HostToNetworkOrder(tmpvalue);

            data.Write(BitConverter.GetBytes(tmpvalue), 0, sizeof(int));
        }

        // ÎÞ·ûºÅ4×Ö½Ú
        public static void writeUInt(MemoryStream data, uint tmpvalue)
        {
            tmpvalue = (uint)IPAddress.HostToNetworkOrder((int)tmpvalue);
            byte[] nowData = BitConverter.GetBytes(tmpvalue);
            data.Write(nowData, 0, sizeof(uint));
        }

        // ÎÞ·ûºÅ8×Ö½Ú
        public static void writeULong(MemoryStream data, UInt64 tmpvalue)
        {
            byte[] nowData = new byte[8];
            for (int i = 0; i < sizeof(UInt64); i++)
                nowData[i] = (byte)(tmpvalue / (Math.Pow(256, (sizeof(UInt64) - i - 1))));
            data.Write(nowData, 0, sizeof(UInt64));
        }


		public static void writeBool(MemoryStream data, bool tmpvalue)
		{
			byte _value = 0;
			if (tmpvalue) _value = 1;
			writeUByte(data, _value);
		}

		public static void writeLoopBool(MemoryStream data, List<bool> list)
		{
			proto_util.writeShort(data, (short)list.Count);
			foreach (bool ele in list) writeBool(data, ele);
		}

        public static void writeLoopUByte(MemoryStream data, List<byte> list)
        {
            proto_util.writeShort(data, (short)list.Count);
            foreach (byte ele in list) writeUByte(data, ele);
        }

		public static void writeLoopByte(MemoryStream data, List<sbyte> list)
		{
			proto_util.writeShort(data, (short)list.Count);
			foreach (sbyte ele in list) writeByte(data, ele);
		}

        public static void writeLoopUShort(MemoryStream data, List<ushort> list)
        {
            proto_util.writeShort(data, (short)list.Count);
            foreach (ushort ele in list) writeUShort(data, ele);
        }

		public static void writeLoopShort(MemoryStream data, List<short> list)
		{
			proto_util.writeShort(data, (short)list.Count);
			foreach (short ele in list) writeShort(data, ele);
		}

        public static void writeLoopUInt(MemoryStream data, List<uint> list)
        {
			if (list == null)
			{
				proto_util.writeShort(data, 0);
				return;
			}
            proto_util.writeShort(data, (short)list.Count);
            foreach (uint ele in list) writeUInt(data, ele);
        }

		public static void writeLoopInt(MemoryStream data, List<int> list)
		{
			proto_util.writeShort(data, (short)list.Count);
			foreach (int ele in list) writeInt(data, ele);
		}

		public static void writeLoopString(MemoryStream data, List<string> list)
		{
			proto_util.writeShort(data, (short)list.Count);
			foreach (string ele in list) writeString(data, ele);
		}

        // ×Ö·û´®
        public static void writeString(MemoryStream data, string _src)
        {
            MemoryStream byteString = new MemoryStream();
            byte[] nowData = Encoding.UTF8.GetBytes(_src);
            byteString.Write(nowData, 0, nowData.Length);

            writeShort(data, (short)nowData.Length);
            data.Write(byteString.GetBuffer(), 0, (int)byteString.Length);

            return;
        }
    }
}