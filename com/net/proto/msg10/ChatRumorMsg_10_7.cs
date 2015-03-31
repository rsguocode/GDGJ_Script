/**
 * 传闻 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class ChatRumorMsg_10_7
  	{

    public byte channel = 0;
    public uint typeId = 0;
    public byte must = 0;
    public List<PRumor> content = new List<PRumor>();

    public static int getCode()
    {
        // (10, 7)
        return 2567;
    }

    public void read(MemoryStream msdata)
    {
        channel = proto_util.readUByte(msdata);
        typeId = proto_util.readUInt(msdata);
        must = proto_util.readUByte(msdata);
        PRumor.readLoop(msdata, content);
    }
   }
}