/**
 * push回复 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class RelationPushAnswerMsg_7_5
  	{

    public byte answer = 0;
    public uint roleId = 0;
    public string name = "";
    public byte job = 0;
    public byte lvl = 0;
    public byte sex = 0;
    public byte vip = 0;

    public static int getCode()
    {
        // (7, 5)
        return 1797;
    }

    public void read(MemoryStream msdata)
    {
        answer = proto_util.readUByte(msdata);
        roleId = proto_util.readUInt(msdata);
        name = proto_util.readString(msdata);
        job = proto_util.readUByte(msdata);
        lvl = proto_util.readUByte(msdata);
        sex = proto_util.readUByte(msdata);
        vip = proto_util.readUByte(msdata);
    }
   }
}