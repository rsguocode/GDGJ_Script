/**
 * 朋友信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RelationInfoMsg_7_1
  	{

    public List<PRelationInfo> friends = new List<PRelationInfo>();
    public List<PRelationInfo> blacks = new List<PRelationInfo>();

    public static int getCode()
    {
        // (7, 1)
        return 1793;
    }

    public void read(MemoryStream msdata)
    {
        PRelationInfo.readLoop(msdata, friends);
        PRelationInfo.readLoop(msdata, blacks);
    }
   }
}