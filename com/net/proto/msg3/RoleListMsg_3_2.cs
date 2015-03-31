/**
 * buff列表 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleListMsg_3_2
  	{

    public List<PBuff> list = new List<PBuff>();

    public static int getCode()
    {
        // (3, 2)
        return 770;
    }

    public void read(MemoryStream msdata)
    {
        PBuff.readLoop(msdata, list);
    }
   }
}