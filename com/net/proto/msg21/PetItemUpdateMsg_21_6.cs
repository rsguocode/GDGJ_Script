/**
 * 宠物属性更新 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetItemUpdateMsg_21_6
  	{

    public uint id = 0;
    public List<PItem> info = new List<PItem>();

    public static int getCode()
    {
        // (21, 6)
        return 5382;
    }

    public void read(MemoryStream msdata)
    {
        id = proto_util.readUInt(msdata);
        PItem.readLoop(msdata, info);
    }
   }
}