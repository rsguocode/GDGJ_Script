/**
 * 购买银币的等级信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PBuySilverType
  	{

    public uint silver = 0;
    public bool canBuy = false;

    public void read(MemoryStream msdata)
    {
        
        silver = proto_util.readUInt(msdata);
        canBuy = proto_util.readBool(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, silver);
        proto_util.writeBool(msdata, canBuy);
    }
    
    public static void readLoop(MemoryStream msdata, List<PBuySilverType> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PBuySilverType _pm = new PBuySilverType();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PBuySilverType> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PBuySilverType ps in p) ps.write(msdata);
        }
    
    
   }
}