/**
 * 宠物身上装备更新 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetEquipUpdateMsg_24_6
  	{

    public uint petId = 0;
    public List<PGoods> equip = new List<PGoods>();

    public static int getCode()
    {
        // (24, 6)
        return 6150;
    }

    public void read(MemoryStream msdata)
    {
        petId = proto_util.readUInt(msdata);
        PGoods.readLoop(msdata, equip);
    }
   }
}