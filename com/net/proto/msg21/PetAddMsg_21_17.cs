/**
 * 增加一个宠物 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetAddMsg_21_17
  	{

    public PPet pet = new PPet();

    public static int getCode()
    {
        // (21, 17)
        return 5393;
    }

    public void read(MemoryStream msdata)
    {
        pet.read(msdata);
    }
   }
}