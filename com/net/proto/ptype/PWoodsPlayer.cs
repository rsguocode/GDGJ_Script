/**
 * 金银岛每个冒险玩家的信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PWoodsPlayer
  	{

    public uint id = 0;
    public string name = "";
    public byte lvl = 0;
    public byte sex = 0;
    public byte job = 0;
    public uint fightPoint = 0;
    public uint remainTime = 0;
    public uint assistId = 0;
    public string assistName = "";
    public byte assistLvl = 0;
    public byte assistJob = 0;
    public uint assistFightPoint = 0;
    public byte robTimes = 0;
    public byte grade = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        lvl = proto_util.readUByte(msdata);
        sex = proto_util.readUByte(msdata);
        job = proto_util.readUByte(msdata);
        fightPoint = proto_util.readUInt(msdata);
        remainTime = proto_util.readUInt(msdata);
        assistId = proto_util.readUInt(msdata);
        assistName = proto_util.readString(msdata);
        assistLvl = proto_util.readUByte(msdata);
        assistJob = proto_util.readUByte(msdata);
        assistFightPoint = proto_util.readUInt(msdata);
        robTimes = proto_util.readUByte(msdata);
        grade = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeString(msdata, name);
        proto_util.writeUByte(msdata, lvl);
        proto_util.writeUByte(msdata, sex);
        proto_util.writeUByte(msdata, job);
        proto_util.writeUInt(msdata, fightPoint);
        proto_util.writeUInt(msdata, remainTime);
        proto_util.writeUInt(msdata, assistId);
        proto_util.writeString(msdata, assistName);
        proto_util.writeUByte(msdata, assistLvl);
        proto_util.writeUByte(msdata, assistJob);
        proto_util.writeUInt(msdata, assistFightPoint);
        proto_util.writeUByte(msdata, robTimes);
        proto_util.writeUByte(msdata, grade);
    }
    
    public static void readLoop(MemoryStream msdata, List<PWoodsPlayer> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PWoodsPlayer _pm = new PWoodsPlayer();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PWoodsPlayer> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PWoodsPlayer ps in p) ps.write(msdata);
        }
    
    
   }
}