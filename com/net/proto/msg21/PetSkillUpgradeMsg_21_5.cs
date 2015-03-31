/**
 * 升级天赋技能 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class PetSkillUpgradeMsg_21_5
  	{

    public ushort code = 0;
    public uint id = 0;
    public byte pos = 0;
    public byte lvl = 0;

    public static int getCode()
    {
        // (21, 5)
        return 5381;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        id = proto_util.readUInt(msdata);
        pos = proto_util.readUByte(msdata);
        lvl = proto_util.readUByte(msdata);
    }
   }
}