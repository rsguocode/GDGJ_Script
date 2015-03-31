using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using com.bases.utils;
using com.game.data;
using com.game.manager;
using com.game.module.map;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.debug;
using com.u3d.bases.display;
using com.u3d.bases.display.character;
using com.u3d.bases.display.effect;
using com.u3d.bases.display.vo;
using Object = UnityEngine.Object;

/**游戏场景管理
 *游戏中只有一个游戏场景的实例*/

namespace com.u3d.bases.map
{
    public class GameMap : BaseMap
    {
        public MapPointDisplay MapPointDisplay; //当前场景传送点

        /**创建本角色**/

        public void CreateMe(PlayerVo vo)
        {
            if (vo == null) return;
            if (_me != null) return;
            vo.Type = DisplayType.ROLE;
            _me = new MeDisplay();
            //vo.ClothUrl = "Model/Role/" + vo.SysRoleBaseInfo.Model + "/Model/" + vo.SysRoleBaseInfo.Model +
            //              ".assetbundle";
			vo.ClothUrl = "Model/Role/" + "100001/Model/" + "100001" +
				".assetbundle";
            _me.SetVo(vo);
            if (_playerList.IndexOf(_me) == -1) _playerList.Add(_me);
            Object.DontDestroyOnLoad(_me.GoBase);
            Object.DontDestroyOnLoad(_me.GoCloth);
        }

        /**创建角色**/

        public PlayerDisplay CreatePlayer(PlayerVo vo)
        {
            if (vo == null) return null;
            vo.Type = DisplayType.ROLE;
            var display = new PlayerDisplay();
            objectList.Add(display);
            _playerList.Add(display);
            vo.ClothUrl = "Model/Role/" + vo.SysRoleBaseInfo.Model + "/Model/" + vo.SysRoleBaseInfo.Model +
                          ".assetbundle";
            display.SetVo(vo);
            return display;
        }

        /**创建宠物**/
        public PetDisplay CreatePet(DisplayVo vo)
        {
            if (vo == null) return null;
            vo.Type = DisplayType.PET;
            var display = new PetDisplay();
            _petDisplayList.Add(display);
            objectList.Add(display);
            PetVo pet = (PetVo)vo;
            string res = pet.SysPet.res;
            if (res.Equals("0"))
            {
                res = "10004";
            }
            vo.ClothUrl = "Model/Pet/" + res + "/Model/" + res + ".assetbundle";
            display.SetVo(vo);
            return display;
        }

        /**创建NPC**/

        public NpcDisplay CreateNpc(DisplayVo vo)
        {
            if (vo == null) return null;
            vo.Type = DisplayType.NPC;
            var display = new NpcDisplay(); 
            NpcDisplayList.Add(display);
            objectList.Add(display);
            SysNpcVo sysNpcVo = BaseDataMgr.instance.GetNpcVoById(vo.Id);
            if (sysNpcVo == null)
            {
                Log.error(this,"NPC表不存在NPC,Id:"+vo.Id);
                return null;
            }
            vo.ClothUrl = "Model/Npc/" + sysNpcVo.model + "/Model/BIP.assetbundle";
            display.SetVo(vo);
            return display;
        }

        /**创建副本陷阱**/

        public TrapDisplay CreateTrap(DisplayVo vo)
        {
            if (vo == null) return null;
            vo.Type = DisplayType.Trap;
            var display = new TrapDisplay(); 
            objectList.Add(display);
            SysTrap sysTrapVo = BaseDataMgr.instance.GetTrapVoById(vo.Id);
            vo.ClothUrl = "Model/Trap/" + sysTrapVo.Model + "/Model/BIP.assetbundle";
            display.SetVo(vo);
            return display;
        }

        /**创建副本跳转点**/

        public CopyPointDisplay CreateCopyPoint(DisplayVo vo)
        {
            if (vo == null) return null;
            vo.Type = DisplayType.COPY_POINT;
            var display = new CopyPointDisplay();
            objectList.Add(display);
            display.SetVo(vo);
            return display;
        }

        /**创建场景跳转点**/

        public MapPointDisplay CreateMapPoint(DisplayVo vo)
        {
            if (vo == null) return null;
            vo.Type = DisplayType.MAP_POINT;
            var display = new MapPointDisplay(); 
            objectList.Add(display);
            display.SetVo(vo);
            MapPointDisplay = display;
            return display;
        }

		/**创建场景跳转点**/
		
		public MapPointDisplay CreateWorldMapPoint(DisplayVo vo)
		{
			if (vo == null) return null;
			vo.Type = DisplayType.MAP_POINT;
			var display = new MapPointDisplay(); 
			objectList.Add(display);
			display.SetVo(vo);
			return display;
		}

        /**创建怪物**/

        public MonsterDisplay CreateMonster(DisplayVo vo)
        {
            if (vo == null) return null;
            vo.Type = DisplayType.MONSTER;
            var display = new MonsterDisplay();
            objectList.Add(display);
            var monsterVo = vo as MonsterVo;
            if (monsterVo != null)
            {
                vo.ClothUrl = "Model/Monster/" + monsterVo.MonsterVO.res + "/Model/BIP.assetbundle";
                UnityEngine.Debug.Log("****加载怪物BIP, BIP.assetbundle, id = ,vo.ClothUrl = " + vo.Id + ", " + vo.ClothUrl);
            }
            display.SetVo(vo);
            return display;
        }

        public void AddMonster(MonsterDisplay display)
        {
            _monsterList.Add(display);
        }

        /**根据ID取得玩家**/

        public PlayerDisplay GetPlayer(string id)
        {
            if (me != null && StringUtils.isEqualsExcFree(me.GetVo().Id.ToString(CultureInfo.InvariantCulture), id))
                return me;
            return (PlayerDisplay) GetDisplay(id, DisplayType.ROLE);
        }

        /**根据ID取得NPC**/

        public NpcDisplay GetNpc(String id)
        {
            return (NpcDisplay) GetDisplay(id, DisplayType.NPC);
        }

        /// <summary>
        ///     根据NPCid获取NPC的显示对象
        /// </summary>
        /// <param name="npcId"></param>
        /// <returns></returns>
        public NpcDisplay GetNpcDisplay(uint npcId)
        {
            return NpcDisplayList.FirstOrDefault(display => display.GetVo().Id == npcId);
        }

        /**根据ID取得副本传送点**/

        public CopyPointDisplay GetCopyPoint(String id)
        {
            return (CopyPointDisplay) GetDisplay(id, DisplayType.COPY_POINT);
        }

        /**根据副本组ID-- 取得副本传送点**/

        public CopyPointDisplay GetCopyPoint(int copyGroupId)
        {
            if (copyGroupId < 1) return null;
            return
                objectList.OfType<CopyPointDisplay>().FirstOrDefault(copyPoint => copyPoint.copyGroupId == copyGroupId);
        }

        /**根据ID取得场景传送点**/

        public MapPointDisplay GetMapPoint(String id)
        {
            return (MapPointDisplay) GetDisplay(id, DisplayType.MAP_POINT);
        }

        /**根据ID取得怪物**/

        public MonsterDisplay GetMonster(String id)
        {
            return (MonsterDisplay) GetDisplay(id, DisplayType.MONSTER);
        }

        public List<MonsterDisplay> GetMonster()
        {
            return _monsterList;
        }

		public int MonsterNumber
		{
			get {return _monsterList.Count;}
		}

        /**根据ID和对象类型取得对象**/

        private BaseDisplay GetDisplay(string id, int type)
        {
            if (StringUtils.isEmpty(id)) return null;
            return GetDisplay(type + "_" + id);
        }

        /**根据对象唯一Key去的游戏对象**/

        internal BaseDisplay GetDisplay(string key)
        {
            if (StringUtils.isEmpty(key)) return null;
            return objectList.FirstOrDefault(display => display.Key.Equals(key));
        }

        /**根据对象Id+对象类型移除对象
         * @param id   对象ID
         * @param type 对象类型(DisplayType.cs中指定)
         * **/

        public bool Remove(string id, int type)
        {
            return remove(GetDisplay(id, type));
        }

        /// <summary>
        ///     隐藏其他玩家
        /// </summary>
        public void HideOtherPlayer()
        {
            foreach (PlayerDisplay playerDisplay in _playerList)
            {
                if (playerDisplay.GetVo().Id != me.GetVo().Id)
                {
                    playerDisplay.GoBase.SetActive(false);
                    playerDisplay.Controller.GoName.SetActive(false);
                }
            }
        }

        /// <summary>
        ///     显示其他玩家
        /// </summary>
        public void ShowOtherPlayer()
        {
            foreach (PlayerDisplay playerDisplay in _playerList)
            {
                if (playerDisplay.GetVo().Id == me.GetVo().Id) continue;
                playerDisplay.GoBase.SetActive(true);
                playerDisplay.Controller.GoName.SetActive(true);
            }
        }

        /// <summary>
        ///     停止所有怪物AI
        /// </summary>
        public void StopAllMonstersAi()
        {
            MonsterMgr.CanSetAi = false;
            /*foreach (MonsterDisplay monsterDisplay in _monsterList)
            {
                monsterDisplay.Controller.AiController.SetAi(false);
            }*/
            
        }

        /// <summary>
        ///     启用所有怪物Ai
        /// </summary>
        public void StartAllMonstersAi()
        {
            MonsterMgr.CanSetAi = true;

            /*foreach (MonsterDisplay monsterDisplay in _monsterList)
            {
                monsterDisplay.Controller.AiController.SetAi(true);
            }*/
        }

        public void SetMonsterAnimationSpeed(float speed)
        {
            foreach (MonsterDisplay monsterDisplay in _monsterList)
            {
                monsterDisplay.Animator.speed = speed;
            }
        }
    }
}