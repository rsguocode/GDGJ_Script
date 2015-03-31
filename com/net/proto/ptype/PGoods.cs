/**
 * 物品信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;
		
using Proto;
    

namespace PCustomDataType
{
    public class PGoods
  	{

    public uint id = 0;
    public uint goodsId = 0;
    public ushort count = 0;
    public byte pos = 0;
    public ushort expire = 0;
    public ushort energy = 0;
    public List<PEquip> equip = new List<PEquip>();

    public void read(MemoryStream msdata)
    {
        
        id = proto_util.readUInt(msdata);
        goodsId = proto_util.readUInt(msdata);
        count = proto_util.readUShort(msdata);
        pos = proto_util.readUByte(msdata);
        expire = proto_util.readUShort(msdata);
        energy = proto_util.readUShort(msdata);
        PEquip.readLoop(msdata, equip);
    }

    public void write(MemoryStream msdata)
    {
        
        proto_util.writeUInt(msdata, id);
        proto_util.writeUInt(msdata, goodsId);
        proto_util.writeUShort(msdata, count);
        proto_util.writeUByte(msdata, pos);
        proto_util.writeUShort(msdata, expire);
        proto_util.writeUShort(msdata, energy);
        PEquip.writeLoop(msdata, equip);
    }
    
    public static void readLoop(MemoryStream msdata, List<PGoods> p)
        {
            int Len = proto_util.readShort(msdata);

            for (int i = 0; i < Len; i++)
            {
                PGoods _pm = new PGoods();
                _pm.read(msdata);
                p.Add(_pm);
            }
        }

        public static void writeLoop(MemoryStream msdata, List<PGoods> p)
        {
            proto_util.writeShort(msdata, (short)p.Count);

            foreach (PGoods ps in p) ps.write(msdata);
        }
    
    
   }
}