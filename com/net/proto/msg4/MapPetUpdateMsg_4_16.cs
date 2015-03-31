/**
 * 宠物更新 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapPetUpdateMsg_4_16
  	{

    public uint id = 0;
    public uint petId = 0;
    public uint lvl = 0;

    public static int getCode()
    {
        // (4, 16)
        return 1040;
    }

    public void read(MemoryStream msdata)
    {
        id = proto_util.readUInt(msdata);
        petId = proto_util.readUInt(msdata);
        lvl = proto_util.readUInt(msdata);
    }
   }
}