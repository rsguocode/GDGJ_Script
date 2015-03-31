/**
 * 聊天内容推送 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class ChatContentPushMsg_10_3
  	{

    public byte chatType = 0;
    public uint senderId = 0;
    public ushort serverId = 0;
    public string senderName = "";
    public byte senderNation = 0;
    public byte senderSex = 0;
    public byte senderJob = 0;
    public byte senderLvl = 0;
    public byte senderVip = 0;
    public string content = "";
    public List<PChatPushGoods> goodsList = new List<PChatPushGoods>();

    public static int getCode()
    {
        // (10, 3)
        return 2563;
    }

    public void read(MemoryStream msdata)
    {
        chatType = proto_util.readUByte(msdata);
        senderId = proto_util.readUInt(msdata);
        serverId = proto_util.readUShort(msdata);
        senderName = proto_util.readString(msdata);
        senderNation = proto_util.readUByte(msdata);
        senderSex = proto_util.readUByte(msdata);
        senderJob = proto_util.readUByte(msdata);
        senderLvl = proto_util.readUByte(msdata);
        senderVip = proto_util.readUByte(msdata);
        content = proto_util.readString(msdata);
        PChatPushGoods.readLoop(msdata, goodsList);
    }
   }
}