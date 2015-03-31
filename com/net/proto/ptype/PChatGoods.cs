/**
 * 角色物品信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PChatGoods
  	{

    public uint id = 0;
    public byte repos = 0;

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        repos = proto_util.readUByte(msdata);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUByte(msdata, repos);
    }
    
    public static void readLoop(MemoryStream msdata, List<PChatGoods> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PChatGoods _pm = new PChatGoods();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PChatGoods> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PChatGoods ps in p) ps.write(msdata);
        }
    
    
   }
}