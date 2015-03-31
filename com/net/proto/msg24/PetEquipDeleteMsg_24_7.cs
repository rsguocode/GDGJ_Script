/**
 * 宠物身上删除一件装备 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetEquipDeleteMsg_24_7
  	{

    public uint petId = 0;
    public List<uint> equipId = new List<uint>();

    public static int getCode()
    {
        // (24, 7)
        return 6151;
    }

    public void read(MemoryStream msdata)
    {
        petId = proto_util.readUInt(msdata);
        proto_util.readLoopUInt(msdata, equipId);
    }
   }
}