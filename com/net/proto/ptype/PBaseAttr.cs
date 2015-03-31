/**
 * 基础属性信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PBaseAttr
  	{

    public uint str = 0;
    public uint agi = 0;
    public uint phy = 0;
    public uint wit = 0;
    public uint hpCur = 0;
    public uint hpFull = 0;
    public uint mpCur = 0;
    public uint mpFull = 0;
    public uint attPMin = 0;
    public uint attPMax = 0;
    public uint attMMin = 0;
    public uint attMMax = 0;
    public uint defP = 0;
    public uint defM = 0;
    public uint hit = 0;
    public uint dodge = 0;
    public uint crit = 0;
    public uint critRatio = 0;
    public uint flex = 0;
    public uint hurtRe = 0;
    public uint speed = 0;
    public uint luck = 0;
    public uint fightPoint = 0;

    public void read(MemoryStream msdata)
    {
        
        str = proto_util.readUInt(msdata);
        agi = proto_util.readUInt(msdata);
        phy = proto_util.readUInt(msdata);
        wit = proto_util.readUInt(msdata);
        hpCur = proto_util.readUInt(msdata);
        hpFull = proto_util.readUInt(msdata);
        mpCur = proto_util.readUInt(msdata);
        mpFull = proto_util.readUInt(msdata);
        attPMin = proto_util.readUInt(msdata);
        attPMax = proto_util.readUInt(msdata);
        attMMin = proto_util.readUInt(msdata);
        attMMax = proto_util.readUInt(msdata);
        defP = proto_util.readUInt(msdata);
        defM = proto_util.readUInt(msdata);
        hit = proto_util.readUInt(msdata);
        dodge = proto_util.readUInt(msdata);
        crit = proto_util.readUInt(msdata);
        critRatio = proto_util.readUInt(msdata);
        flex = proto_util.readUInt(msdata);
        hurtRe = proto_util.readUInt(msdata);
        speed = proto_util.readUInt(msdata);
        luck = proto_util.readUInt(msdata);
        fightPoint = proto_util.readUInt(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, str);
        proto_util.writeUInt(msdata, agi);
        proto_util.writeUInt(msdata, phy);
        proto_util.writeUInt(msdata, wit);
        proto_util.writeUInt(msdata, hpCur);
        proto_util.writeUInt(msdata, hpFull);
        proto_util.writeUInt(msdata, mpCur);
        proto_util.writeUInt(msdata, mpFull);
        proto_util.writeUInt(msdata, attPMin);
        proto_util.writeUInt(msdata, attPMax);
        proto_util.writeUInt(msdata, attMMin);
        proto_util.writeUInt(msdata, attMMax);
        proto_util.writeUInt(msdata, defP);
        proto_util.writeUInt(msdata, defM);
        proto_util.writeUInt(msdata, hit);
        proto_util.writeUInt(msdata, dodge);
        proto_util.writeUInt(msdata, crit);
        proto_util.writeUInt(msdata, critRatio);
        proto_util.writeUInt(msdata, flex);
        proto_util.writeUInt(msdata, hurtRe);
        proto_util.writeUInt(msdata, speed);
        proto_util.writeUInt(msdata, luck);
        proto_util.writeUInt(msdata, fightPoint);
    }
    
    public static void readLoop(MemoryStream msdata, List<PBaseAttr> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PBaseAttr _pm = new PBaseAttr();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PBaseAttr> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PBaseAttr ps in p) ps.write(msdata);
        }
    
    
   }
}