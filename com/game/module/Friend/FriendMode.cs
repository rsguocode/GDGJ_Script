using System.Linq;
using com.game.vo;
using com.game.module.test;
using com.game.data;
using com.game.manager;
using System.Collections.Generic;
using com.net.p8583;
using com.u3d.bases.debug;
using PCustomDataType;
using UnityEngine;
using Proto;

namespace Com.Game.Module.Role
{
    public class FriendMode : BaseMode<FriendMode>
    {
        public List<PRelationInfo> friendsList { get; private set; }//好友名单
        public List<PRelationInfo> blacksList { get; private set; } //黑名单
        public List<PRelationNear> nearByList { get; private set; } //附近玩家

        public ushort friendNum { get; private set; }
        public ushort maxNum { get; private set; }

        private List<RelationPushAcceptMsg_7_3>  _askList = new List<RelationPushAcceptMsg_7_3>();        
        public List<RelationPushAcceptMsg_7_3> askList { get {return _askList;} }//邀请信息


        public const int FRIEND = 1; //好友数据变化
        public const int BLACKLIST = 2;//黑名单数据变化
        public const int NEARBY = 3; //附近数据变化
        public const int ASKLIST = 4; //邀请数据变化
        public const int FRIENDNUM = 5; //好友数变化

        public override bool ShowTips
        {
            get { return askList.Count > 0; }
        }

        /// <summary>
        /// 设置好友信息
        /// </summary>
        public void SetFriendInfoList(List<PRelationInfo> friendsList, List<PRelationInfo> blacksList)
        {
            this.friendsList = friendsList;
            this.blacksList = blacksList;
            DataUpdate(FRIEND);
            DataUpdate(BLACKLIST);
        }
        /// <summary>
        /// 设置附近玩家信息
        /// </summary>
        public void SetNearbyPlayerInfo(List<PRelationNear> nearByList)
        {
            this.nearByList = nearByList;
            DataUpdate(NEARBY);
        }

        /// <summary>
        /// 好友数量信息
        /// </summary>
        /// <param name="friendNum">当前数量</param>
        /// <param name="maxNum">最大数量</param>
        public void SetFriendNumInfo(ushort friendNum, ushort maxNum)
        {
            this.friendNum = friendNum;
            this.maxNum = maxNum;
            DataUpdate(FRIENDNUM);
        }
        /// <summary>
        /// 添加好友邀请信息
        /// </summary>
        public bool AddFriendAskMsg(RelationPushAcceptMsg_7_3 ask)
        {
            //检查此roleId玩家对应的邀请信息是否已经存在，已存在则替换
            for (int i=0;i<askList.Count;i++)
            {
                RelationPushAcceptMsg_7_3 msg = askList[i];
                if (msg.roleId == ask.roleId)
                {
                    askList[i] = ask;
                    return false;
                }

            }
            askList.Add(ask);
            DataUpdate(ASKLIST);
            return true;
        }

        /// <summary>
        /// 接受好友邀请
        /// </summary>
        /// <param name="roleId">ID</param>
        /// <param name="accpet">接受</param>
        public void AcceptFriend(uint roleId,bool accept)
        {
            if (accept)
            {
                for (int i = 0; i < askList.Count; i++)
                {
                    if (askList[i].roleId == roleId)
                    {
                        RelationPushAcceptMsg_7_3 msg = askList[i];
                        PRelationInfo info = new PRelationInfo();
                        info.roleId = roleId;
                        info.sex = msg.sex;
                        info.name = msg.name;
                        info.job = msg.job;
                        info.lvl = msg.lvl;
                        info.vip = msg.vip;
                        info.isOnline = 1;
                        info.intimate = 0;
                        AddFriend(info);
                    }
                }
            }
            DeleteFriendAskInfo(roleId); 
        }

        /// <summary>
        /// 增加好友信息
        /// </summary>
        public void AddFriend(PRelationInfo info)
        {
            if (friendsList != null)
            {
                friendsList = new List<PRelationInfo>();

            }
            for (int i = 0; i < friendsList.Count; i++)
            {
                if (friendsList[i].roleId == info.roleId)
                {
                    friendsList[i] = info;
                    return;
                }
            }
            friendsList.Add(info);
            DataUpdate(FRIEND);

        }

        /// <summary>
        /// 删除好友信息
        /// </summary>
        /// <param name="roleId"></param>
        public void DeleteFriend(uint roleId)
        {
            int index = -1;
            if (friendsList != null)
            {
                for (int i = 0; i < friendsList.Count; i++)
                {
                    if (friendsList[i].roleId == roleId)
                    {
                        index = i;
                        break;
                    }
                }
                if (index > -1)
                {
                    friendsList.RemoveAt(index);
                    DataUpdate(FRIEND);
                }
            }
        }

        /// <summary>
        /// 删除好友邀请信息记录
        /// </summary>
        /// <param name="roleId">好友id</param>
        private void DeleteFriendAskInfo(uint roleId)
        {
            int index = -1;
            for (int i = 0; i < askList.Count; i++)
            {
                if (askList[i].roleId == roleId)
                {
                    index = i;
                    break;
                }
            }
            if (index > -1)
            {
                askList.RemoveAt(index);
                DataUpdate(ASKLIST);
            }
        }

        /// <summary>
        /// 移动好友到黑名单
        /// </summary>
        /// <param name="roleId"></param>
        public void MoveToBlackList(uint roleId)
        {
            //好友列表
            int index = -1;
            if (friendsList != null)
            {
                for (int i = 0; i < friendsList.Count; i++)
                {
                    if (friendsList[i].roleId == roleId)
                    {
                        index = i;
                        AddBlackList(friendsList[i]);
                        break;
                    }
                }
                if (index > -1)
                {
                    friendsList.RemoveAt(index);
                    DataUpdate(FRIEND);
                }
            }

            //附近玩家
            index = -1;
            if (nearByList != null)
            {
                for (int i = 0; i < nearByList.Count; i++)
                {
                    if (nearByList[i].roleId == roleId)
                    {
                        index = i;

                        PRelationInfo info = new PRelationInfo();
                        info.roleId = nearByList[i].roleId;
                        info.sex = nearByList[i].sex;
                        info.name = nearByList[i].name;
                        info.job = nearByList[i].job;
                        info.lvl = nearByList[i].lvl;
                        info.isOnline = 1;
                        info.intimate = 0;
                        AddBlackList(info);
                        break;
                    }
                }
                if (index > -1)
                {
                    nearByList.RemoveAt(index);
                    DataUpdate(NEARBY);
                }
            }

            //邀请列表
            index = -1;
            if (askList!=null)
            {
                for (int i = 0; i < askList.Count; i++)
                {
                    if (askList[i].roleId == roleId)
                    {
                        index = i;

                        PRelationInfo info = new PRelationInfo();
                        info.roleId = askList[i].roleId;
                        info.sex = askList[i].sex;
                        info.name = askList[i].name;
                        info.job = askList[i].job;
                        info.lvl = askList[i].lvl;
                        info.vip = askList[i].vip;
                        info.isOnline = 1;
                        info.intimate = 0;
                        AddBlackList(info);
                        break;
                    }
                }
                if (index > -1)
                {
                    askList.RemoveAt(index);
                    DataUpdate(ASKLIST);
                }
            }

        }

        /// <summary>
        /// 添加黑名单
        /// </summary>
        /// <param name="info"></param>
        private void AddBlackList(PRelationInfo info)
        {
            if (blacksList != null)
            {
                for (int i = 0; i < blacksList.Count; i++)
                {
                    if (blacksList[i].roleId == info.roleId)
                    {
                        blacksList[i] = info;
                        return;
                    }
                }
            }
            else
            {
                blacksList = new List<PRelationInfo>();
            }
            blacksList.Add(info);
            DataUpdate(BLACKLIST);
        }

        /// <summary>
        /// 设置玩家在线信息
        /// </summary>
        /// <param name="roleId">玩家Id</param>
        /// <param name="online">在线信息</param>
        public void SetFriendOlineInfo(uint roleId,byte online)
        {
            int index = -1;
            if (friendsList != null)
            {
                for (int i = 0; i < friendsList.Count; i++)
                {
                    if (friendsList[i].roleId == roleId)
                    {
                        index = i;
                        break;
                    }
                }
                if (index > -1)
                {
                    friendsList[index].isOnline = online;
                    DataUpdate(FRIEND);
                    return;
                }
            }
            if (blacksList != null)
            {
                for (int i = 0; i < blacksList.Count; i++)
                {
                    if (blacksList[i].roleId == roleId)
                    {
                        index = i;
                        break;
                    }
                }
                if (index > -1)
                {
                    blacksList[index].isOnline = online;
                    DataUpdate(BLACKLIST);
                    return;
                }
            }
        }

        /// <summary>
        /// 设置好友等级
        /// </summary>
        /// <param name="roleId">玩家Id</param>
        /// <param name="lvl">等级</param>
        public void SetFriendLvl(uint roleId, byte lvl)
        {
            int index = -1;
            if (friendsList != null)
            {
                for (int i = 0; i < friendsList.Count; i++)
                {
                    if (friendsList[i].roleId == roleId)
                    {
                        index = i;
                        break;
                    }
                }
                if (index > -1)
                {
                    friendsList[index].lvl = lvl;
                    DataUpdate(FRIEND);
                }
            }
        }


        /// <summary>
        /// 设置好友亲密度
        /// </summary>
        /// <param name="roleId">玩家Id</param>
        /// <param name="lvl">等级</param>
        public void SetFriendIntimate(uint roleId, uint itm)
        {
            int index = -1;
            if (friendsList != null)
            {
                for (int i = 0; i < friendsList.Count; i++)
                {
                    if (friendsList[i].roleId == roleId)
                    {
                        index = i;
                        break;
                    }
                }
                if (index > -1)
                {
                    friendsList[index].intimate = itm;
                    DataUpdate(FRIEND);
                }
            }
        }



        /// <summary>
        /// 删除黑名单
        /// </summary>
        /// <param name="roleId"></param>
        public void DeleteBlackList(uint roleId)
        {

            int index = -1;
            if (blacksList != null)
            {
                for (int i = 0; i < blacksList.Count; i++)
                {
                    if (blacksList[i].roleId == roleId)
                    {
                        index = i;
                        break;
                    }
                }
                if (index > -1)
                {
                    blacksList.RemoveAt(index);
                    DataUpdate(BLACKLIST);
                }
            }
        }

    }
}

