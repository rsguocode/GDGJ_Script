/**
 * 获得装备的失败加成成功率 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class SmeltGetEquipRateMsg_14_2
  	{

    public ushort code = 0;
    public uint id = 0;
    public byte repos = 0;
    public byte rate = 0;

    public static int getCode()
    {
        // (14, 2)
        return 3586;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        repos = proto_util.readUByte(msdata);
        rate = proto_util.readUByte(msdata);
    }
   }
}