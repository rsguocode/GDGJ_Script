/**
 * 视野信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MapSightMsg_4_2
  	{

    public uint mapId = 0;
    public ushort phase = 0;
    public List<PMapRole> rolesEnter = new List<PMapRole>();
    public List<PMapMon> monsEnter = new List<PMapMon>();

    public static int getCode()
    {
        // (4, 2)
        return 1026;
    }

    public void read(MemoryStream msdata)
    {
        mapId = proto_util.readUInt(msdata);
        phase = proto_util.readUShort(msdata);
        PMapRole.readLoop(msdata, rolesEnter);
        PMapMon.readLoop(msdata, monsEnter);
    }
   }
}