/**
 * 附近玩家 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RelationNearMsg_7_18
  	{

    public List<PRelationNear> near = new List<PRelationNear>();

    public static int getCode()
    {
        // (7, 18)
        return 1810;
    }

    public void read(MemoryStream msdata)
    {
        PRelationNear.readLoop(msdata, near);
    }
   }
}