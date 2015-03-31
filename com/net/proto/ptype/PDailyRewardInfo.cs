/**
 * 悬赏任务列表 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PDailyRewardInfo
  	{

    public ushort type = 0;
    public uint subtype = 0;
    public uint goodsId = 0;
    public byte count = 0;
    public byte times = 0;
    public byte maxTimes = 0;
    public byte status = 0;

    public void read(MemoryStream msdata)
    {
        
        type = proto_util.readUShort(msdata);
        subtype = proto_util.readUInt(msdata);
        goodsId = proto_util.readUInt(msdata);
        count = proto_util.readUByte(msdata);
        times = proto_util.readUByte(msdata);
        maxTimes = proto_util.readUByte(msdata);
        status = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUShort(msdata, type);
        proto_util.writeUInt(msdata, subtype);
        proto_util.writeUInt(msdata, goodsId);
        proto_util.writeUByte(msdata, count);
        proto_util.writeUByte(msdata, times);
        proto_util.writeUByte(msdata, maxTimes);
        proto_util.writeUByte(msdata, status);
    }
    
    public static void readLoop(MemoryStream msdata, List<PDailyRewardInfo> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PDailyRewardInfo _pm = new PDailyRewardInfo();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PDailyRewardInfo> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PDailyRewardInfo ps in p) ps.write(msdata);
        }
    
    
   }
}