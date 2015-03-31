
using System.Collections.Generic;
using Com.Game.Module.Role;
using com.game.module.test;
using com.game;
using com.game.cmd;
using System.IO;
using com.u3d.bases.debug;
using Proto;
using com.net.interfaces;
using com.game.Public.Message;
using com.game.manager;
using PCustomDataType;

namespace Com.Game.Module.Friend
{
    /// <summary>
    /// 好友业务控制类
    /// </summary>
    public class FriendControl : BaseControl<FriendControl>
    {
        private Dictionary<uint, bool> accepts = new Dictionary<uint, bool>();  

        protected override void NetListener()
        {
            //public const String CMD_7_1 = "1793"; // 朋友信息 : RelationInfoMsg_7_1
            //public const String CMD_7_2 = "1794"; // 邀请好友 : RelationAcceptMsg_7_2
            //public const String CMD_7_3 = "1795"; // push邀请好友请求 : RelationPushAcceptMsg_7_3
            //public const String CMD_7_4 = "1796"; // 回复好友 : RelationAnswerAcceptMsg_7_4
            //public const String CMD_7_5 = "1797"; // push回复 : RelationPushAnswerMsg_7_5
            //public const String CMD_7_6 = "1798"; // 删除好友 : RelationDeleteMsg_7_6
            //public const String CMD_7_7 = "1799"; // 移至黑名单 : RelationMoveMsg_7_7
            //public const String CMD_7_8 = "1800"; // 通知好友被删除 : RelationPushDelMsg_7_8
            //public const String CMD_7_9 = "1801"; // 上线通知 : RelationPushOnlineMsg_7_9
            //public const String CMD_7_10 = "1802"; // 根据角色名加好友 : RelationAcceptNameMsg_7_10
            //public const String CMD_7_11 = "1803"; // 删除黑名单 : RelationDeleteBlackMsg_7_11
            //public const String CMD_7_12 = "1804"; // 好友升级 : RelationFriendsLvlMsg_7_12
            //public const String CMD_7_13 = "1805"; // 好友数是否最大 : RelationIsFriendsMaxMsg_7_13
            //public const String CMD_7_14 = "1806"; // 更新亲密度 : RelationUpdateIntimateMsg_7_14
            //public const String CMD_7_16 = "1808"; // 批量删除 : RelationBatchDelMsg_7_16
            //public const String CMD_7_17 = "1809"; // 好友当前最大数量 : RelationFriendsCountMsg_7_17
            //public const String CMD_7_18 = "1810"; // 附近玩家 : RelationNearMsg_7_18

            AppNet.main.addCMD(CMD.CMD_7_1, ReceiveFriendsInfo_7_1);
            AppNet.main.addCMD(CMD.CMD_7_2, ReceiveAskInfo_7_2);
            AppNet.main.addCMD(CMD.CMD_7_3, ReceiveFriendAskInfo_7_3);
            AppNet.main.addCMD(CMD.CMD_7_4, ReceiveReplyInfo_7_4);
            AppNet.main.addCMD(CMD.CMD_7_5, ReceiveAskReplyInfo_7_5);
            AppNet.main.addCMD(CMD.CMD_7_6, ReceiveDeleteFriendInfo_7_6);
            AppNet.main.addCMD(CMD.CMD_7_7, ReceiveMoveToBlackList_7_7);
            AppNet.main.addCMD(CMD.CMD_7_8, ReceiveFriendDeleted_7_8);
            AppNet.main.addCMD(CMD.CMD_7_9, ReceiveFriendOnline_7_9);
            AppNet.main.addCMD(CMD.CMD_7_10, ReceiveFriendAskByName_7_10);
            AppNet.main.addCMD(CMD.CMD_7_11, ReceiveDeleteBlackList_7_11);
            AppNet.main.addCMD(CMD.CMD_7_12, ReceiveFriendLevelUp_7_12);
            AppNet.main.addCMD(CMD.CMD_7_14, ReceiveFriendIntimate_7_14);
            AppNet.main.addCMD(CMD.CMD_7_17, ReceiveFriendNumInfo_7_17);
            AppNet.main.addCMD(CMD.CMD_7_18, ReceiveNearByInfo_7_18);
            SendRequestForFriendInfo();
        }

        /// <summary>
        /// 发送请求获取好友信息
        /// </summary>
        public void SendRequestForFriendInfo()
        {
            MemoryStream mem = new MemoryStream();
            Module_7.write_7_1(mem);
            Log.info(this, "SendRequestForFriendInfo ");
            AppNet.gameNet.send(mem,7,1);
        }

        /// <summary>
        /// 处理用户好友黑名单信息
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveFriendsInfo_7_1(INetData data)
        {
            RelationInfoMsg_7_1 msg = new RelationInfoMsg_7_1();
            msg.read(data.GetMemoryStream());
            Log.info(this ,"friend list "+msg.friends.Count);
            Singleton<FriendMode>.Instance.SetFriendInfoList(msg.friends,msg.blacks);
        }

        /// <summary>
        /// 发送好友请求
        /// </summary>
        /// <param name="playerId">玩家Id</param>
        public void SendFriendAskInfo(uint roleId)
        {
            MemoryStream mem = new MemoryStream();
            Module_7.write_7_2(mem, roleId);
            AppNet.gameNet.send(mem, 7, 2);
        }

        /// <summary>
        /// 发送好友请求返回
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveAskInfo_7_2(INetData data)
        {
            RelationAcceptMsg_7_2 msg = new RelationAcceptMsg_7_2();
            
            msg.read(data.GetMemoryStream());
            if (msg.code != 0)
            {
                ErrorCodeManager.ShowError(msg.code);
            }
            else
            {
                MessageManager.Show(LanguageManager.GetWord("FriendControl.Suc"));
            }

        }


        /// <summary>
        /// 接收到好友请求信息
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveFriendAskInfo_7_3(INetData data)
        {
            RelationPushAcceptMsg_7_3 msg = new RelationPushAcceptMsg_7_3();
            msg.read(data.GetMemoryStream());
            if (Singleton<FriendMode>.Instance.AddFriendAskMsg(msg))
            {
                MessageManager.Show(LanguageManager.GetWord("FriendView.FriendAsk"));
            }
        }

        /// <summary>
        /// 回复好友邀请信息
        /// </summary>
        /// <param name="roleId">好友roleId</param>
        /// <param name="accetp">接受</param>
        public void ReplyFriendAskInfo(uint roleId,bool accetp)
        {
            MemoryStream mem = new MemoryStream();
            byte acceptB = accetp ? (byte)1 : (byte)0;
            Module_7.write_7_4(mem, roleId, acceptB);
            accepts[roleId] = accetp;
            AppNet.gameNet.send(mem, 7, 4);

        }

        /// <summary>
        /// 收到回复邀请返回信息
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveReplyInfo_7_4(INetData data)
        {
            RelationAnswerAcceptMsg_7_4 msg = new RelationAnswerAcceptMsg_7_4();
            msg.read(data.GetMemoryStream());

            bool accept = false;
            if (accepts.ContainsKey(msg.roleId))
            {
                accept = accepts[msg.roleId];
                accepts.Remove(msg.roleId);
            }
            if (msg.code != 0)
            {
                ErrorCodeManager.ShowError(msg.code);
                Singleton<FriendMode>.Instance.AcceptFriend(msg.roleId, false);
            }
            else
            {
                Singleton<FriendMode>.Instance.AcceptFriend(msg.roleId, accept);
            }

        }



        /// <summary>
        /// 收到邀请回复信息
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveAskReplyInfo_7_5(INetData data)
        {
            RelationPushAnswerMsg_7_5 msg = new RelationPushAnswerMsg_7_5();

            msg.read(data.GetMemoryStream());
            string action;
            if(msg.answer == 0)
            {
                action = LanguageManager.GetWord("FriendControl.Reject");
            }
            else
            {
                //对方同意加好友
                action = LanguageManager.GetWord("FriendControl.Accept");
                PRelationInfo info = new PRelationInfo();
                info.roleId = msg.roleId;
                info.sex = msg.sex;
                info.name = msg.name;
                info.job = msg.job;
                info.lvl = msg.lvl;
                info.vip = msg.vip;
                info.isOnline = 1;
                info.intimate = 0;
                Singleton<FriendMode>.Instance.AddFriend(info);
            }
            MessageManager.Show(msg.name + action + LanguageManager.GetWord("FriendControl.FriendAsk"));

        }

        /// <summary>
        /// 发送好友删除信息
        /// </summary>
        /// <param name="roleId"></param>
        public void SendDeleteFriendInfo(uint roleId)
       {     
            MemoryStream mem = new MemoryStream();
            Module_7.write_7_6(mem, roleId);
            AppNet.gameNet.send(mem, 7, 6);
        }

        /// <summary>
        /// 删除好友返回信息
        /// </summary>
        private void ReceiveDeleteFriendInfo_7_6(INetData data)
        {
            RelationDeleteMsg_7_6 msg = new RelationDeleteMsg_7_6();
            msg.read(data.GetMemoryStream());
            if (msg.code != 0)
            {
                ErrorCodeManager.ShowError(msg.code);
            }
            else
            {
                Singleton<FriendMode>.Instance.DeleteFriend(msg.roleId);
            }
        }

        /// <summary>
        /// 移动好友信息到黑名单
        /// </summary>
        /// <param name="roleId"></param>
        public void MoveToBlackList(uint roleId)
        {
            MemoryStream mem = new MemoryStream();
            Module_7.write_7_7(mem, roleId);
            AppNet.gameNet.send(mem, 7, 7);
        }

        /// <summary>
        /// 移除好友到黑名单
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveMoveToBlackList_7_7(INetData data)
        {
            RelationMoveMsg_7_7 msg = new RelationMoveMsg_7_7();
            msg.read(data.GetMemoryStream());
            if (msg.code != 0)
            {
                ErrorCodeManager.ShowError(msg.code);
            }
            else
            {
				MessageManager.Show(LanguageManager.GetWord("FriendControl.Suc"));
                Singleton<FriendMode>.Instance.MoveToBlackList(msg.roleId);
            }
        }

        /// <summary>
        /// 好友删除
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveFriendDeleted_7_8(INetData data)
        {
            RelationPushDelMsg_7_8 msg = new RelationPushDelMsg_7_8();
            msg.read(data.GetMemoryStream());
            Singleton<FriendMode>.Instance.DeleteFriend(msg.roleId);
        }

        /// <summary>
        /// 收到好友上下线信息
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveFriendOnline_7_9(INetData data)
        {
            RelationPushOnlineMsg_7_9 msg = new RelationPushOnlineMsg_7_9();
            msg.read(data.GetMemoryStream());
            Singleton<FriendMode>.Instance.SetFriendOlineInfo(msg.roleId,msg.isOnline);

        }

        /// <summary>
        /// 发送好友请求
        /// </summary>
        /// <param name="name">玩家名称</param>
        public void SendFriendAskByName(string name)
        {
            MemoryStream mem = new MemoryStream();
            Module_7.write_7_10(mem, name);
            AppNet.gameNet.send(mem, 7, 10);
        }

        /// <summary>
        /// 收到加好友返回信息
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveFriendAskByName_7_10(INetData data)
        {
            RelationAcceptNameMsg_7_10 msg = new RelationAcceptNameMsg_7_10();
            msg.read(data.GetMemoryStream());
            if (msg.code != 0)
            {
                ErrorCodeManager.ShowError(msg.code);
            }
            else
            {
                MessageManager.Show(LanguageManager.GetWord("FriendControl.Suc"));
            }

        }

        /// <summary>
        /// 移除黑名单
        /// </summary>
        /// <param name="roleId"></param>
        public void SendDeleteBlackList(uint roleId)
        {
            MemoryStream mem = new MemoryStream();
            Module_7.write_7_11(mem, roleId);
            AppNet.gameNet.send(mem, 7, 11);
        }

        /// <summary>
        /// 删除黑名单
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveDeleteBlackList_7_11(INetData data)
        {
            RelationDeleteBlackMsg_7_11 msg =  new RelationDeleteBlackMsg_7_11();
            msg.read(data.GetMemoryStream());

            if (msg.code != 0)
            {
                ErrorCodeManager.ShowError(msg.code);
            }
            else
            {
                Singleton<FriendMode>.Instance.DeleteBlackList(msg.roleId);
            }
        }

        /// <summary>
        /// 收到好友升级信息
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveFriendLevelUp_7_12(INetData data)
        {
            RelationFriendsLvlMsg_7_12 msg = new RelationFriendsLvlMsg_7_12();
            msg.read(data.GetMemoryStream());

            Singleton<FriendMode>.Instance.SetFriendIntimate(msg.roleId,msg.lvl);

        }

        /// <summary>
        /// 收到好友亲密度信息
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveFriendIntimate_7_14(INetData data)
        {
            RelationUpdateIntimateMsg_7_14 msg = new RelationUpdateIntimateMsg_7_14();
            msg.read(data.GetMemoryStream());

            Singleton<FriendMode>.Instance.SetFriendIntimate(msg.roleId, msg.intimate);

        }

        /// <summary>
        /// 发送附近玩家请求
        /// </summary>
        public void SendRequestForNearByInfo()
        {
            MemoryStream mem = new MemoryStream();
            Module_7.write_7_18(mem);
            AppNet.gameNet.send(mem, 7, 18);
        }

        /// <summary>
        /// 获取附近玩家信息
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveNearByInfo_7_18(INetData data)
        {
            RelationNearMsg_7_18 msg = new RelationNearMsg_7_18();
            msg.read(data.GetMemoryStream());
            Log.info(this,"ReceiveNearByInfo_7_18 "+ +msg.near.Count);
            Singleton<FriendMode>.Instance.SetNearbyPlayerInfo(msg.near);
        }

        /// <summary>
        /// 获取好友数量信息
        /// </summary>
        /// <param name="data"></param>
        private void ReceiveFriendNumInfo_7_17(INetData data)
        {
            RelationFriendsCountMsg_7_17 msg = new RelationFriendsCountMsg_7_17();
            msg.read(data.GetMemoryStream());
            Singleton<FriendMode>.Instance.SetFriendNumInfo(msg.nowCount, msg.maxCount);
        }

    }
}