using System;
using System.Collections.Generic;
using System.Linq;
using com.game.data;
using com.game.consts;
using com.game.utils;
/**基础数据管理类**/
using PCustomDataType;
using com.game.vo;
using com.u3d.bases.debug;
using com.game.module.map;

namespace com.game.manager
{
    public class BaseDataMgr
    {
        //临时存储变量，避免多次创建对象，带来内存消耗
		private IList<SysStoryTalkListVo> storyTalkList;
        private Dictionary<string, Type> clzTypeData;
        private Dictionary<String, Dictionary<uint, object>> data;
		//private IList<string> preLoadMonList;
        public static BaseDataMgr instance = new BaseDataMgr();


        public BaseDataMgr()
        {
            storyTalkList = new List<SysStoryTalkListVo>();
            clzTypeData = BaseDataConst.registerClzType();
			//preLoadMonList = new List<string>();
        }

        /**根据类名取得Type**/
        public Type getClzType(string clzType)
        {
            return clzTypeData.ContainsKey(clzType) ? clzTypeData[clzType] : null;
        }

        /**初始化数据**/
        public void init(object data)
        {
            this.data = (Dictionary<String, Dictionary<uint, object>>)data; 
            // keys = SysBuffVo,SysConfigVo,SysDaemonislandVo,SysDailyBuffVo,SysDeedVo,SysMapVo.......
        }

        /// <summary>
        /// 获取物品信息
        /// </summary>
        /// <param name="id">物品编号</param>
        /// <returns>物品信息</returns>
        public SysItemVo getGoodsVo(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_ITEM_VO];
            return dict.ContainsKey(id) ? dict[id] as SysItemVo : null;
        }

        //判断id是否是一个普通物品
        public bool IsItem(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_ITEM_VO];
            return dict.ContainsKey(id) ? true : false;
        }

        /**根据场景ID--取得场景信息**/
        public SysMapVo GetMapVo(uint id)
        {
            //UnityEngine.Debug.Log("****BaseDataMgr::GetMapVo, id = " + id);

            Dictionary<uint, object> dict = data[BaseDataConst.SYS_MAP_VO];
            return dict.ContainsKey(id) ? (SysMapVo)dict[id] : null;
        }

        public SysErrorCodeVo GetErrorCodeVo(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_ERROR_CODE_VO];
            return dict.ContainsKey(id) ? (SysErrorCodeVo)dict[id] : null;
        }

        public SysBuffVo GetSysBuffVo(uint id, uint level)
        {
            uint uniKey = id*100 + level;
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_BUFF_VO];
            return dict.ContainsKey(uniKey) ? (SysBuffVo)dict[uniKey] : null;
        }

        //得到勋章信息
        public SysMedalVo GetSysMedalVo(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_MEDAL_VO];
            return dict.ContainsKey(id) ? (SysMedalVo)dict[id] : null;
        }

        /**根据怪物ID--取得怪物信息**/
        public SysMonsterVo getSysMonsterVo(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_MONSTER_VO];
            return dict.ContainsKey(id) ? (SysMonsterVo)dict[id] : null;
        }

        /**根据怪物Ai ID--取得怪物AI信息**/
        public SysMonsterAiVo getMonsterAiVo(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_MONSTER_AI_VO];
            return dict.ContainsKey(id) ? (SysMonsterAiVo)dict[id] : null;
        }

		/** 获取职业伤害系数 */
		public SysJobDamageRatioVo GetJobDamageRatio(uint job1, uint job2)
		{
			uint unikey = job1 * 100 + job2;
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_JOB_DAMAGE_RATIO];
			return dict.ContainsKey(unikey) ? (SysJobDamageRatioVo)dict[unikey] : null;
		}

		/** 获取副本地图表 */
		public SysDungeonTreeVo GetSysDungeonTree(uint id)
		{
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_DUNGEON_TREE_VO];
            return dict.ContainsKey(id) ? (SysDungeonTreeVo)dict[id] : null;
		}

		/** 获取副本奖励 */
		public SysDungeonRewardVo GetSysDungeonReward(uint id)
		{
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_DUNGEON_REWARD_VO];
			return dict.ContainsKey(id) ? (SysDungeonRewardVo)dict[id] : null;
		}

		/** 获取恶魔岛结构表 */
		public SysDaemonislandVo GetSysDaemonIslandTree(uint id)
		{
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_DAEMONISLAND_VO];
			return dict.ContainsKey(id) ? (SysDaemonislandVo)dict[id] : null;
		}

		/** 获取传闻表 */
		public SysRumor GetSysRumor(uint id)
		{
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_RUMOR_VO];
			return dict.ContainsKey(id) ? (SysRumor)dict[id] : null;
		}


        /**获取登录礼包*/
        public SysGiftVo GetGiftPack(int id, int type)
        {
            uint unikey =(uint)( id * 100 + type);
            Dictionary<uint,object> dict = data[BaseDataConst.SYS_GIFT_VO];
            return dict.ContainsKey(unikey) ? (SysGiftVo)dict[unikey] : null;
        }

        /**获取VIP描述信息**/
        public SysVipInfoVo GetVIPDescribe(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_VIP_INFO];
            return dict.ContainsKey(id) ? (SysVipInfoVo)dict[id] : null;
        }


		/** 获取地宫寻宝表 */
		public SysTreasure GetSysTreasure(uint id)
		{
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_TREASURE_VO];
			return dict.ContainsKey(id) ? (SysTreasure)dict[id] : null;
		}
		public Dictionary<uint, object> GetSysTreasureDic()
		{
			return data[BaseDataConst.SYS_TREASURE_VO];
		}


		/** 获取宠物表 */
		public SysPet GetSysPet(uint id)
		{
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_PET_VO];
			return dict.ContainsKey(id) ? (SysPet)dict[id] : null;
		}

		/** 获取宠物图鉴表 */
		public SysPetBook GetSysPetBook(uint id)
		{
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_PET_BOOK_VO];
			return dict.ContainsKey(id) ? (SysPetBook)dict[id] : null;
		}
		public Dictionary<uint, object> GetSysPetBookDic()
		{
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_PET_BOOK_VO];
			return data[BaseDataConst.SYS_PET_BOOK_VO];
		}

		/** 获取技能图鉴表 */
        public SysPetBookSkill GetSysPetBookSkill(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_PET_BOOK_SKILL_VO];
            return dict.ContainsKey(id) ? (SysPetBookSkill)dict[id] : null;
        }
		public Dictionary<uint, object> GetSysPetBookSkillDic()
		{
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_PET_BOOK_SKILL_VO];
			return data[BaseDataConst.SYS_PET_BOOK_SKILL_VO];
		}

		/** 获取商城数据表 */
		public SysVipMallVo GetSysVipMallVo(uint id, uint type)
		{
			uint unikey = id * 100 + type;
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_VIP_MALL_VO];
			return dict.ContainsKey(unikey) ? (SysVipMallVo)dict[unikey] : null;
		}

		/** 根据商品类别获取商城数据List */
		public List<SysVipMallVo> GetSysVipMallVoListByType(int type)
		{
			List<SysVipMallVo> VipMallList = new List<SysVipMallVo> ();
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_VIP_MALL_VO];
			foreach (Object keyvalue in dict.Values) {
				if( (keyvalue as SysVipMallVo).type == type)
				{
					VipMallList.Add(keyvalue as SysVipMallVo);
				}
			}
			return VipMallList;
		}

		/** 获取种子商店数据表 */
		public SysSeedMallVo GetSysSeedMallVo(uint id, uint type)
		{
			uint unikey = id * 100 + type;
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_SEED_MALL_VO];
			return dict.ContainsKey(unikey) ? (SysSeedMallVo)dict[unikey] : null;
		}
		
		/** 根据种子类别获取种子商店数据List */
		public List<SysSeedMallVo> GetSysSeedMallVoListByType(int type)
		{
			List<SysSeedMallVo> SeedMallList = new List<SysSeedMallVo> ();
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_SEED_MALL_VO];
			foreach (Object keyvalue in dict.Values) {
				if( (keyvalue as SysSeedMallVo).type == type)
				{
					SeedMallList.Add(keyvalue as SysSeedMallVo);
				}
			}
			return SeedMallList;
		}

		/** 获取种子信息 */
		public SysSeedVo GetSysSeedVo(uint id)
		{
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_SEED_VO];
			return dict.ContainsKey(id) ? (SysSeedVo)dict[id] : null;
		}

        public SysPriceVo GetSysPriceVos(uint type)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_PRICE_VO];
            return dict.ContainsKey(type) ? (SysPriceVo)dict[type] : null;
        }

        //根据特效ID从特效表中读取数据
        public SysEffectVo GetSysEffectVoByID(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_EFFECT_VO];
            return dict.ContainsKey(id) ? (SysEffectVo)dict[id] : null;
        }

        /**根据场景ID--取得场景传送点列表**/
        public IList<SysTransPosVo> getTeleportList(uint mapId)
        {
            SysTransPosVo vo = null;
            IList<SysTransPosVo> transPosList = new List<SysTransPosVo>();
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_TRANS_POS_VO];
            foreach (object item in dict.Values)
            {
                vo = (SysTransPosVo)item;
                if (mapId == vo.mapid)
                {
                    transPosList.Add(vo);
                }
            }
            return transPosList;
        }

		//获得场景预加载资源列表
		public IList<SysReadyLoadVo> GetScenePreLoadList(uint mapId, int subtype)
		{
            SysReadyLoadVo vo;
            IList<SysReadyLoadVo> preLoadList = new List<SysReadyLoadVo>();
            Dictionary<uint, object> dict = data[BaseDataConst.Sys_READY_LOAD_VO]; // 实际上是从 BinData.xml读取的

			//vo.mainid为0为所有场景公用资源
			//vo.job为0为所有职业公用资源
			foreach (object item in dict.Values)
			{
				vo = (SysReadyLoadVo)item;

				if ((PRTypeConst.MT_SCENE == vo.maintype)
				    && (subtype == vo.subtype)
				    && (mapId == Convert.ToUInt32(vo.mainid) || 0 == Convert.ToUInt32(vo.mainid))
				    && (MeVo.instance.job == (uint)vo.job || 0 == vo.job))
				{
					preLoadList.Add(vo);
				}
			}

			return preLoadList;
		}

		//获得场景怪物预加载怪物列表
		public IList<string> GetMonPreLoadList(uint mapId)
		{
            SysDungeonMon vo;
            SysDungeonMon MonData;
            string[] monsterlist;
            int length;
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_DUNGEON_MON];
            IList<string> preLoadMonList = new List<string>();

			uint[] mapKeys = {mapId*100+1,mapId*100+2,mapId*100+3};
			for (int i=0; i<mapKeys.Length; i++)
			{
                UnityEngine.Debug.Log("-----------------------mapkey = " + mapKeys[i]);

				if(dict.ContainsKey(mapKeys[i]))
				{
					MonData = (SysDungeonMon)dict[mapKeys[i]];
                    int phase = MonData.phase;
                    Log.debug(this,"ditujieduan_______________=" + phase);
					monsterlist = StringUtils.GetMonsterList(MonData.list);
					length = monsterlist.Length/3;
					for(int j=0;j<length;j++)
					{
						if(!preLoadMonList.Contains(monsterlist[1+j*3]))
						preLoadMonList.Add(monsterlist[1+j*3]);
					}
				}
			}
			
			return preLoadMonList;
		}

        /// <summary>
        ///  获取怪物刷新数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysDungeonMon getSysDungeonVo(uint id)
        {
            uint phase = MapMode.CUR_MAP_PHASE;
            uint key = id * 100 + phase;
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_DUNGEON_MON];
            return dict.ContainsKey(key) ? (SysDungeonMon)dict[key] : null;
        }

		private bool ContainsType(string vipType, string type)
		{
			string[] typeArr = StringUtils.GetValueListFromString(vipType);

			foreach (string item in typeArr)
			{
				if (item.Equals(type))
				{
					return true;
				}
			}

			return false;
		}


        /// <summary>
        /// 根据VIP等级和特权ID取出免费次数
        /// </summary>
        /// <param name="vipLevel"></param>
        /// <param name="RightType"></param>
        /// <returns></returns>
        public SysVipDroitVo GetGrowFreeTime(uint vipLevel, uint RightType)
        {
            uint unikey = vipLevel * 100 + RightType;
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_VIP_DROIT_VO];
            return dict.ContainsKey(unikey) ?(SysVipDroitVo)dict[unikey] : null;
            //return 0; //为0 的时候表示该特权还不开放
        }

		//根据特权ID获得VIP最低等级
		public int GetVIPLowestGrade(int type)
		{
			SysVipDroitVo vo;
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_VIP_DROIT_VO];	
			int result = 10000;
			foreach (object item in dict.Values)
			{
				vo = (SysVipDroitVo)item;				
				if (ContainsType(vo.type.ToString(), type.ToString()))
				{
					if (vo.vip < result)
					{
						result = vo.vip;
					}
				}
			}
			return result;
		}


		//获得剧情对话列表
		public IList<SysStoryTalkListVo> GetStoryTalkList(uint chapterId)
		{
		    storyTalkList.Clear();
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_STORY_TALKLIST_VO];

			foreach (object item in dict.Values)
			{
			    SysStoryTalkListVo vo = (SysStoryTalkListVo)item;
			    if (chapterId == vo.chapter_id)
				{
					storyTalkList.Add(vo);
				}
			}

		    return storyTalkList;
		}

		//获得NPC对象
		public SysNpcVo GetNpcVoById(uint id)
		{
            //UnityEngine.Debug.Log("****BaseDataMgr::GetNpcVoById, id = " + id);

			Dictionary<uint, object> dict = data[BaseDataConst.SYS_NPC_VO];
			return dict.ContainsKey(id) ? (SysNpcVo)dict[id] : null;
		}

        //获得陷阱对象
        public SysTrap GetTrapVoById(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_TRAP_VO];
            return dict.ContainsKey(id) ? (SysTrap)dict[id] : null;
        }

		//获得剧情章节对象
		public SysStoryChapterVo GetStoryChapterVoById(uint id)
		{
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_STORY_CHAPTER_VO];
			return dict.ContainsKey(id) ? (SysStoryChapterVo)dict[id] : null;
		}

		//获得剧情背景对象
		public SysStoryBgVo GetStoryBgVoById(uint id)
		{
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_STORY_BG_VO];
			return dict.ContainsKey(id) ? (SysStoryBgVo)dict[id] : null;
		}

        //根据技能ID从技能表中读取数据
        public SysSkillBaseVo GetSysSkillBaseVo(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_SKILL_BASE_VO];
            return dict.ContainsKey(id) ? (SysSkillBaseVo)dict[id] : null;
        }

        //根据技能组ID获取技能组信息
        public SysSkillActionVo GetSysSkillActionVo(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_SKILL_ACTION_VO];
            return dict.ContainsKey(id) ? (SysSkillActionVo)dict[id] : null;
        }

		//根据抽奖id获得抽奖信息
		public SysExchangeVo GetSysExchangeVo(uint id) 
		{
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_EXCHANGE_VO];
			return dict.ContainsKey(id) ? (SysExchangeVo)dict[id] : null;
		}

        /// <summary>
        /// 获取任务数据
        /// </summary>
        /// <param name="id">任务id</param>
        /// <returns></returns>
        public SysTask GetSysTaskVo(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_TASK_VO];
            return dict.ContainsKey(id) ? (SysTask)dict[id] : null;
        }

        /// <summary>
        /// 获取契约数据
        /// </summary>
        /// <param name="id">契约Id</param>
        /// <returns></returns>
        public SysDeedVo GetSysDeedVo(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_DEED_VO];
            return dict.ContainsKey(id) ? (SysDeedVo)dict[id] : null;
        }

        public SysRoleBaseInfoVo GetSysRoleBaseInfo(uint jobId)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_ROLE_BASE_INFO_VO];
            return dict.ContainsKey(jobId) ? (SysRoleBaseInfoVo)dict[jobId] : null;
        }

        //获得冒险森林数据
		public SysForest GetSysForestVo(uint id)
		{
			Dictionary<uint, object> dict = data[BaseDataConst.SYS_FOREST_VO];
			return dict.ContainsKey(id) ? (SysForest)dict[id] : null;
		}

        public SysMonsterAdaptRuleVo GetMonsterAdapt(uint id)
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_MONSTER_ADAPTER_RULE_VO];
            return dict.ContainsKey(id) ? (SysMonsterAdaptRuleVo)dict[id] : null;            
        }

		//获得签到奖励数据
		public SysSignVo GetSignVo(int month , int day)
		{
			uint unikey = (uint)( month * 100 + day);
			Dictionary<uint , object > dict = data[BaseDataConst.SYS_SIGN_VO];
			return dict.ContainsKey(unikey) ? (SysSignVo)dict[unikey] : null;
		}

        /// <summary>
        /// 根据数据的类型，返回此数据的字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<SysGuideVo> GetGuideVoList()
        {
            Dictionary<uint, object> dict = data[BaseDataConst.SYS_GUIDE_VO];
            return dict.Values.Select(guideVo => guideVo as SysGuideVo).ToList();
        }

        /// <summary>
        /// 通用的根据类型和id获取数据的方法
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="type">表类型</param>
        /// <param name="id">数据id</param>
        /// <returns></returns>
        public T GetDataByTypeAndId<T>(string type, uint id)
        {
            
            Dictionary<uint, object> dict = data[type];
            if(dict.ContainsKey(id)){
                return (T)(dict[id]);
            }
            return default(T);
        }


        public T GetDataById<T>(uint id) where T : class
        {
            Dictionary<uint, object> dict = data[typeof(T).Name];
           // Log.info(this, typeof(T).Name);
            if (dict.ContainsKey(id))
            {
                return dict[id] as T;
            }
            return null;
        }

        /// <summary>
        /// 根据数据的类型，返回此数据的字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Dictionary<uint, object> GetDicByType<T>() where T : class
        {
            Dictionary<uint, object> dict = data[typeof(T).Name];
            return dict;
        }
    }
}
