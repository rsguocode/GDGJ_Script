/**
 * gm和新手指导员列表 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RoleGmZhyListMsg_3_50
  	{

    public List<PGmZhy> zhyList = new List<PGmZhy>();

    public static int getCode()
    {
        // (3, 50)
        return 818;
    }

    public void read(MemoryStream msdata)
    {
        PGmZhy.readLoop(msdata, zhyList);
    }
   }
}