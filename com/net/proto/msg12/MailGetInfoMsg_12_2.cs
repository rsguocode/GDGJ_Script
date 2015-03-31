/**
 * 邮件信息 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class MailGetInfoMsg_12_2
  	{

    public ushort code = 0;
    public uint mailId = 0;
    public string title = "";
    public uint sendTime = 0;
    public string content = "";
    public List<PMailAttach> mailAttachList = new List<PMailAttach>();
    public uint gold = 0;
    public uint diam = 0;
    public uint diamBind = 0;
    public List<PMailOther> mailOtherDataList = new List<PMailOther>();

    public static int getCode()
    {
        // (12, 2)
        return 3074;
    }

    public void read(MemoryStream msdata)
    {
        code = proto_util.readUShort(msdata);
        mailId = proto_util.readUInt(msdata);
        title = proto_util.readString(msdata);
        sendTime = proto_util.readUInt(msdata);
        content = proto_util.readString(msdata);
        PMailAttach.readLoop(msdata, mailAttachList);
        gold = proto_util.readUInt(msdata);
        diam = proto_util.readUInt(msdata);
        diamBind = proto_util.readUInt(msdata);
        PMailOther.readLoop(msdata, mailOtherDataList);
    }
   }
}