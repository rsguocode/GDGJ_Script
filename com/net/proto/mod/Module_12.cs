/**
 * 邮件 (自动生成，请勿编辑！)
 */

using System;
using System.IO;
using System.Collections.Generic;

using PCustomDataType;
    

namespace Proto
{
    public class Module_12
  	{

    /**
     * 收件箱邮件列表
     */
    public static void write_12_1(MemoryStream msdata)
    {
    }

    /**
     * 邮件信息
     */
    public static void write_12_2(MemoryStream msdata, uint mailId)
    {
        proto_util.writeUInt(msdata, mailId);
    }

    /**
     * 删除邮件
     */
    public static void write_12_3(MemoryStream msdata, List<uint> mailIds)
    {
        proto_util.writeLoopUInt(msdata, mailIds);
    }

    /**
     * 提取邮件附件
     */
    public static void write_12_4(MemoryStream msdata, uint mailId)
    {
        proto_util.writeUInt(msdata, mailId);
    }

    /**
     * 一键删除所有邮件
     */
    public static void write_12_7(MemoryStream msdata)
    {
    }

    /**
     * 一键提取附件
     */
    public static void write_12_8(MemoryStream msdata)
    {
    }
   }
}