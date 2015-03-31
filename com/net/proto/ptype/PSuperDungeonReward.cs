/**
 * 精英副本的奖励领取信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PSuperDungeonReward
  	{

    public uint cityid = 0;
    public byte status1 = 0;
    public byte status2 = 0;
    public byte status3 = 0;

    public void read(MemoryStream msdata)
    {
        
        cityid = proto_util.readUInt(msdata);
        status1 = proto_util.readUByte(msdata);
        status2 = proto_util.readUByte(msdata);
        status3 = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, cityid);
        proto_util.writeUByte(msdata, status1);
        proto_util.writeUByte(msdata, status2);
        proto_util.writeUByte(msdata, status3);
    }
    
    public static void readLoop(MemoryStream msdata, List<PSuperDungeonReward> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PSuperDungeonReward _pm = new PSuperDungeonReward();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PSuperDungeonReward> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PSuperDungeonReward ps in p) ps.write(msdata);
        }
    
    
   }
}