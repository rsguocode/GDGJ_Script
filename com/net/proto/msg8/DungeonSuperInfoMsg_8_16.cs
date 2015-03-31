/**
 * 精英副本信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class DungeonSuperInfoMsg_8_16
  	{

    public List<PSuperDungeonInfo> info = new List<PSuperDungeonInfo>();
    public uint maxid = 0;

    public static int getCode()
    {
        // (8, 16)
        return 2064;
    }

    public void read(MemoryStream msdata)
    {
        PSuperDungeonInfo.readLoop(msdata, info);
        maxid = proto_util.readUInt(msdata);
    }
   }
}