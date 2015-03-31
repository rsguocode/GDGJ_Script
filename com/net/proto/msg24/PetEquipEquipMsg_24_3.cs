/**
 * 请求宠物的装备列表 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetEquipEquipMsg_24_3
  	{

    public uint id = 0;
    public List<PPetEquip> list = new List<PPetEquip>();

    public static int getCode()
    {
        // (24, 3)
        return 6147;
    }

    public void read(MemoryStream msdata)
    {
        id = proto_util.readUInt(msdata);
        PPetEquip.readLoop(msdata, list);
    }
   }
}