/**
 * 激活宠物 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetActivateMsg_21_8
  	{

    public ushort code = 0;
    public List<PPet> pet = new List<PPet>();

    public static int getCode()
    {
        // (21, 8)
        return 5384;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        PPet.readLoop(msdata, pet);
    }
   }
}