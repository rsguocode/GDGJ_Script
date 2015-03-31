/**
 * 礼包 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_29
  	{

    /**
     * 七天登陆礼包
     */
    public static void write_29_0(MemoryStream msdata)
    {
    }

    /**
     * 领取礼包
     */
    public static void write_29_1(MemoryStream msdata)
    {
    }

    /**
     * 领取福利礼包
     */
    public static void write_29_2(MemoryStream msdata, ushort type, string card)
    {
        proto_util.writeUShort(msdata, type);
        proto_util.writeString(msdata, card);
    }

    /**
     * 签到信息
     */
    public static void write_29_3(MemoryStream msdata)
    {
    }

    /**
     * 领取签到奖励
     */
    public static void write_29_4(MemoryStream msdata)
    {
    }
   }
}