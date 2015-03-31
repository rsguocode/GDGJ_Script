using System.Collections.Generic;
using com.game.consts;
using com.game.module.hud;
using Com.Game.Module.SystemSetting;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.display;
using com.u3d.bases.display.character;
using com.u3d.bases.display.controler;
using UnityEngine;

/* *******************************************************
 * author :  xi li
 * email  :  504643437@qq.com  
 * history:  created by xi li   2014/03/01 10:07:52 
 * function:  玩家管理类
 * *******************************************************/

/**进入场景玩家--缓冲类**/

namespace com.game.module.map
{
    public class PlayerMgr
    {
        public static PlayerMgr instance = new PlayerMgr();
        private readonly List<PlayerVo> playerList; // 用于缓存所有场景玩家信息
        private readonly List<PlayerVo> waitCreatePlayerList; //用户缓存未实例化对象的玩家信息
        private float beginTime; //开始时间 
        private float delay = 0.05f; //间隔时间
        private bool isRuning; //运行状态
        //        private IDictionary<string,IList<PlayerVo>> playList;      //场景角色列表<MapId,角色列表>

        public PlayerMgr()
        {
            //            playList = new Dictionary<string, IList<PlayerVo>>();
            playerList = new List<PlayerVo>();
            waitCreatePlayerList = new List<PlayerVo>();
        }

        /**放入队列**/

        internal void addPlayer(PlayerVo playerVo)
        {
            if (playerVo == null) return;
//			if (StringUtils.isEmpty(mapId)) return;
//			IList<PlayerVo> list =null;
//			if(playList.ContainsKey(mapId)) 
//			{
//				list = playList[mapId];
//			}
//			else
//			{
//				list = new List<PlayerVo>();
//				playList.Add(mapId, list);
//			}
            foreach (PlayerVo item in playerList)
            {
                if (item.Id.Equals(playerVo.Id))
                {
                    removePlayer(item.Id); //避免因多次断线重连导致的重复创建问题.如果已有该玩家信息，先删除，再添加
//					playerList.Remove(item);
                    break;
                }
            }

            //上一个步骤已包含该次循环判断
//			foreach (PlayerVo item in waitCreatePlayerList) 
//			{
//				if (item.Id.Equals(playerVo.Id))
//				{
//					waitCreatePlayerList.Remove(item);
//					break;
//				}
//			}

            playerList.Add(playerVo);
            waitCreatePlayerList.Add(playerVo);
        }

        /**移除玩家**/

        internal void removePlayer(uint playerId)
        {
//			if (StringUtils.isEmpty(playerId)) return;
//			String mapId = AppMap.Instance.mapParser.MapId.ToString();
            AppMap.Instance.remove(AppMap.Instance.GetPlayer(playerId.ToString())); //移除玩家模型对象（playerdisplay）
//			IList<PlayerVo> list = playList.ContainsKey(mapId) ? playList[mapId] : null;
//			if (list == null) return;
            foreach (PlayerVo item in playerList)
            {
                if (item.Id.Equals(playerId))
                {
                    playerList.Remove(item);
                    break;
                }
            }

            foreach (PlayerVo item in waitCreatePlayerList)
            {
                if (item.Id.Equals(playerId))
                {
                    waitCreatePlayerList.Remove(item);
                    break;
                }
            }
        }

        /**获取缓冲区中玩家信息**/

        internal PlayerVo getPlayer(uint playerId)
        {
//			IList<PlayerVo> list = playList.ContainsKey(mapId) ? playList[mapId] : null;
//			if (list == null || list.Count < 1) return null;
            foreach (PlayerVo item in playerList)
            {
                if (item.Id.Equals(playerId))
                    return item;
            }
            return null;
        }

        //停止创建玩家
        public void stop()
        {
            isRuning = false;
        }

        //清除玩家缓存数据
        public void Clear()
        {
            playerList.Clear();
            waitCreatePlayerList.Clear();
        }

        //开始创建玩家
        public void start()
        {
            if (isRuning) return;
            isRuning = true;
            beginTime = Time.time;
        }

        /**添加角色到场景**/

        public void execute()
        {
            if (!isRuning) return;
            float time = Time.time;
            if (time - beginTime < delay) return;
//			try
//			{
            beginTime = time;
//				String mapId = AppMap.Instance.mapParser.MapId.ToString();
//				IList<PlayerVo> list = playList.ContainsKey(mapId) ? playList[mapId] : null;
            if (waitCreatePlayerList == null || waitCreatePlayerList.Count < 1)
                return;
            PlayerVo item = waitCreatePlayerList[0];
            waitCreatePlayerList.Remove(item);

            //创建角色 增加名字UI
            item.ModelLoadCallBack = LoadPlayerModelBack;
            AppMap.Instance.CreatePlayer(item);
//			}
//			catch (Exception ex)
//			{
//				Log.error(this, "-execute() 添加玩到场景出错,reason:"+ex.StackTrace);
//			}   
        }

        private void LoadPlayerModelBack(BaseDisplay display)
        {
            var player = display as PlayerDisplay;
            //名称
            GameObject name = NGUITools.AddChild(display.Controller.gameObject);
            player.ChangeDire(Directions.Right);
            BoxCollider2D boxCollider2D = player.BoxCollider2D;
            float y = (boxCollider2D.center.y + boxCollider2D.size.y*0.5f)*display.GoCloth.transform.localScale.y;
            ;
            name.name = "name";
            Transform mTrans = name.transform;
            mTrans.localPosition = new Vector3(0f, y + 0.2f, 0f);
            GameObject goName = HudView.Instance.AddItem(display.GetVo().Name, mTrans, HudView.Type.Player);
            (player.Controller as ActionControler).GoName = goName;

            if ((AppMap.Instance.mapParser.MapId == MapTypeConst.ARENA_MAP)
                || (AppMap.Instance.mapParser.MapId == MapTypeConst.GoldSilverIsland_MAP))
            {
                display.Controller.AiController.SetAi(true); //英雄榜刚开始启用AI
            }
            else
            {
                display.Controller.AiController.SetAi(false);
            }
            if (SystemSettingMode.Instance.HidePlayer && MeVo.instance.mapId!= MapTypeConst.ARENA_MAP&&MeVo.instance.mapId!=MapTypeConst.GoldSilverIsland_MAP)
            {  
                //如果系统设置有设置隐藏玩家，则不显示玩家
                display.GoBase.SetActive(false);
                display.Controller.GoName.SetActive(false);
            }
            if (display.GetMeVoByType<BaseRoleVo>().CurHp == 0)
            {
                display.Controller.StatuController.SetStatu(Status.DEATH);  //血量为0时设置为死亡状态
            }
            player.GetMeVoByType<PlayerVo>().UpdatePetDisplay();
        }
    }
}