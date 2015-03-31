/**
 * 宠物信息列表 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetListMsg_21_1
  	{

    public List<PPet> pet = new List<PPet>();

    public static int getCode()
    {
        // (21, 1)
        return 5377;
    }

    public void read(MemoryStream msdata)
    {
        PPet.readLoop(msdata, pet);
    }
   }
}