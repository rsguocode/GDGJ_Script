using com.game.module.test;
using com.game.vo;
//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 yidao studio
//All rights reserved
//文件名称：BuffManager;
//文件描述：Buff统一管理器;
//创建者：潘振峰;
//创建日期：2014/6/6 18:30:36;
//////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using UnityEngine;
using com.u3d.bases.display.controler;
using com.game.data;
using com.u3d.bases.controller;
using PCustomDataType;
using com.game.consts;

namespace com.game.manager
{
    public class BuffManager : MonoBehaviour
    {

        protected void Update()
        {

        }

        /// <summary>
        /// 存储玩家身上buff列表信息;
        /// </summary>
        /// <param name="_hostId"></param>
        /// <param name="pbufflist">注意：这里的pbuff只取id, lv</param>
        /// <returns></returns>
        public bool AddBuff(string _hostId, List<PBuff> pbufflist, bool autoEndPlayPreBuff = false)
        {
            if (autoEndPlayPreBuff)
            {
                SetBuffEndPlay(_hostId);
            }
            BuffController bc = null;
            foreach (PBuff pb in pbufflist)
            {
                bc = new BuffController();
                bc.SetVo(_hostId, pb.id, pb.lvl);
                AddBuff(_hostId, bc);
            }
            return true;
        }


        /// <summary>
        /// 自动更加状态表判断是否能增加指定buff,不能增加的话返回false;
        /// </summary>
        /// <param name="_hostId"></param>
        /// <param name="_buffVo"></param>
        /// <returns></returns>
        public bool AddBuff(string _hostId, BuffController _buffController)
        {
            List<BuffController> _tmpBufList;
            if (false == mBuffList.ContainsKey(_hostId))
            {
                _tmpBufList = new List<BuffController>();
            }
            else
            {
                _tmpBufList = mBuffList[_hostId];
            }
            if (IsCanAddBuff(_hostId, _buffController))
            {
                _tmpBufList.Add(_buffController);
                mBuffList[_hostId] = _tmpBufList;
                return true;
            }
            return false;
        }

        public void RemoveBuff(string _hostId)
        {
            mBuffList.Remove(_hostId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_hostId"></param>
        /// <param name="_buffPosTag">参看BuffConst中的POS_XXXX</param>
        public void RemoveBuff(string _hostId, string _buffPosTag)
        {
            if (mBuffList.ContainsKey(_hostId))
            {
                List<BuffController> _tmpBufList = mBuffList[_hostId];
                foreach (BuffController tmpBuff in _tmpBufList)
                {
                    if (tmpBuff.PosTag == _buffPosTag)
                    {
                        tmpBuff.Dispose();
                        _tmpBufList.Remove(tmpBuff);
                    }
                }
            }
        }


        public void RemoveBuff(string _hostId, BuffController bc)
        {
            if (mBuffList.ContainsKey(_hostId))
            {
                List<BuffController> _tmpBufList = mBuffList[_hostId];
                foreach (BuffController tmpBuff in _tmpBufList)
                {
                    if (tmpBuff.isEqual(bc))
                    {
                        tmpBuff.Dispose();
                        _tmpBufList.Remove(tmpBuff);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 对指定玩家身上当前正在运行的Buff执行endPlay
        /// </summary>
        /// <param name="hostId"></param>
        public void SetBuffEndPlay(string hostId)
        {
            List<BuffController> buffControllerList = GetBuffsByHostId(hostId);
            foreach (BuffController bc in buffControllerList)
            {
                if(bc.CurState == PlayConst.BUFF_STATE.RUNNING){
                    bc.EndPlay();
                }
            }
        }

        public List<BuffController> GetBuffsByHostId(string _hostId)
        {
            List<BuffController> tmpBufList = null;
            if (mBuffList.ContainsKey(_hostId))
            {
                tmpBufList = mBuffList[_hostId];
            }
            return tmpBufList;
        }

        /// <summary>
        /// 用于检测是否能在当前宿主身上添加指定BUFF;
        /// </summary>
        /// <param name="_hostId"></param>
        /// <param name="_buffId"></param>
        /// <returns></returns>
        public bool IsCanAddBuff(string _hostId, BuffController bc)
        {
            return true;
        }

        private Dictionary<string, List<BuffController>> mBuffList = new Dictionary<string, List<BuffController>>();
        private static BuffManager instance = null;
        public static BuffManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BuffManager();
                }
                return instance;
            }
        }
    }
}
