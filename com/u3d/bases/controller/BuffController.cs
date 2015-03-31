using com.game;
using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.module.effect;
using com.game.module.SystemData;
using com.game.utils;
using Com.Game.Utils;
//////////////////////////////////////////////////////////////////////////////////////////////
//Copyright (C): 4399 yidao studio
//All rights reserved
//文件名称：BuffController;
//文件描述：;
//创建者：潘振峰;
//创建日期：2014/6/9 16:25:05;
//////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using com.u3d.bases.display;


namespace com.u3d.bases.controller
{
    public class BuffController
    {
        /// <summary>
        /// 存入BUFF实体
        /// </summary>
        /// <param name="_hostId"></param>
        /// <param name="_buffVo"></param>
        /// <param name="_isAutoStart"></param>
        public bool SetVo(string _hostId, uint _buffId, uint _buffLv, bool _isAutoStart = true, Action _onBuffCompleteFunc = null)
        {
            SysBuffVo tmpBuffVo = BaseDataMgr.instance.GetSysBuffVo(_buffId, _buffLv);
            this.onBuffCompleteFunc = _onBuffCompleteFunc;
            this.HostId = _hostId;
            this.BuffVo = tmpBuffVo;
            this.CurState = PlayConst.BUFF_STATE.NO_START;
            //获取对应HostId的对象;
            HostDisplay = AppMap.Instance.GetPlayer(HostId);
            if (HostDisplay == null)
            {
                HostDisplay = AppMap.Instance.GetMonster(HostId);
            }
            if (HostDisplay != null)
            {
                //加入Buff管理器中
                BuffManager.Instance.AddBuff(_hostId, this);
                if (_isAutoStart)
                {
                    StartPlay();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 剩余的BUFF时间(秒);
        /// </summary>
        /// <returns></returns>
        public int remainBuffTime()
        {
            if (PlayConst.BUFF_STATE.RUNNING == this.CurState)
            {
                return _durationTime - (ServerTime.Instance.Timestamp - _startPlayTime);
            }
            return 0;
        }


        /// <summary>
        /// BUFF开始起效果;
        /// </summary>
        public void StartPlay()
        {         
            this.CurState = PlayConst.BUFF_STATE.RUNNING;
            _startPlayTime = ServerTime.Instance.Timestamp;
            //播放特效;
            GameObject findHostObject = PlayUtils.GetPartBonesByHostAndTag(this.HostId, this.PosTag);
            if (findHostObject == null)
            {
                findHostObject = AppMap.Instance.GetPlayer(this.HostId.ToString()).Controller.gameObject;
            }
            if (findHostObject == null)
            {
                return;
            }
            //创建特效播放;
            Effect effectVo = new Effect();
                effectVo.URL = UrlUtils.GetBuffEffectUrl(this.BuffVo.effect.ToString());
                effectVo.Direction = HostDisplay.CurFaceDire;
                effectVo.BasePosition = findHostObject.transform.position;
                effectVo.Offset = Vector3.zero;
                effectVo.NeedCache = true;
                effectVo.Target = findHostObject.gameObject;
            EffectMgr.Instance.CreateBuffEffect(effectVo);
        }

        /// <summary>
        /// BUFF效果持续结束
        /// </summary>
        public void EndPlay()
        {
            this.CurState = PlayConst.BUFF_STATE.END;
            BuffManager.Instance.RemoveBuff(this.HostId, this);
            if (onBuffCompleteFunc != null)
            {
                onBuffCompleteFunc();
            }
        }

        /// <summary>
        /// 是否和对应的buff是一样的;
        /// </summary>
        /// <param name="bc"></param>
        /// <returns></returns>
        public bool isEqual(BuffController bc)
        {
            if (bc.BuffVo.id == this.BuffVo.id &&
                bc.BuffVo.lvl == this.BuffVo.lvl)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 做一些善后工作;
        /// </summary>
        public void Dispose()
        {

        }

        /**拥有buff的主人;*/
        public BaseDisplay HostDisplay;
        public SysBuffVo BuffVo;
        private Action onBuffCompleteFunc;
        /// <summary>
        /// 放置位置;
        /// </summary>
        private string _posTag;
        public string PosTag { get { return _posTag; } private set { _posTag = value; } }
        /// <summary>
        /// BUFF起效果的开始时间;
        /// </summary>
        private int _startPlayTime = 0;
        /// <summary>
        /// BUFF可以持续的时间;
        /// </summary>
        private int _durationTime = 0;
        /// <summary>
        /// 宿主ID;
        /// </summary>
        private string _hostId;
        public string HostId { get { return _hostId; } private set { _hostId = value; } }
        //是否正在运行，由buff管理器控制;
        public PlayConst.BUFF_STATE CurState;
    }
}
