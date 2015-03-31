/**
 * 宠物装备栏信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetEquipBagInfoMsg_24_3
  	{

    public uint petId = 0;
    public List<PGoods> equip = new List<PGoods>();

    public static int getCode()
    {
        // (24, 3)
        return 6147;
    }

    public void read(MemoryStream msdata)
    {
        petId = proto_util.readUInt(msdata);
        PGoods.readLoop(msdata, equip);
    }
   }
}