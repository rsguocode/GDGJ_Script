/**
 * 删除宠物 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetDeleteMsg_21_11
  	{

    public List<uint> ids = new List<uint>();

    public static int getCode()
    {
        // (21, 11)
        return 5387;
    }

    public void read(MemoryStream msdata)
    {
        proto_util.readLoopUInt(msdata, ids);
    }
   }
}