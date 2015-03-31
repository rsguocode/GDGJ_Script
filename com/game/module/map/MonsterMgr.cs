using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.com.game.consts;
using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.module.battle;
using Com.Game.Module.Boss;
using com.game.module.effect;
using com.game.module.hud;
using com.game.module.loading;
using com.game.module.test;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.debug;
using com.u3d.bases.display;
using com.u3d.bases.display.character;
using com.u3d.bases.display.controler;
using com.u3d.bases.map;
using UnityEngine;
using Random = UnityEngine.Random;

namespace com.game.module.map
{
    public enum MonsterBornType
    {
        BornFromLeft = 1,
        BornFromSky = 2,
        BornFromRight = 3
    }

    public class MonsterMgr : BaseMode<MonsterMgr>
    {
        public const int EventBossUpdate = 1; //Boss信息更新                
        private const float AiStartDelayTime = 2.5f; //AI延迟生效的时间 
        public static bool CanSetAi = true;
        public readonly float MONSTER_POSY = 1.5f; //默认怪物出生Y坐标
        public GameObject FootEffect; //boss脚底特效对象
        public int LiveMonsterNumber;
        private MonsterVo _bossVo;
        private float _beginTime; //开始时间 
        private string _curMonsterUrl;
        private float delay = 1f; //间隔时间(1秒)
        private string effectUrl = null;
        private bool _isRuning; //运行状态
        private IDictionary<string, GameObject> _monsterPreloadList; //场景预加载怪物列表
        private BaseDisplay _monsterPresent; //当前正在刷的怪物
        private IDictionary<string, IList<MonsterVo>> _monsterVoList; //场景怪物vo列表<MapId,怪物列表>
        private int _preloadIndex;

        public MonsterMgr()
        {
            _monsterVoList = new Dictionary<string, IList<MonsterVo>>();
            _monsterPreloadList = new Dictionary<string, GameObject>();
        }

        public IList<string> PreMonloadList { get; set; }

        public MonsterVo BossVo
        {
            get { return _bossVo; }
            set
            {
                _bossVo = value;
                DataUpdate(EventBossUpdate);
            }
        }

        /**放入队列**/

        public void addMonster(string mapId, MonsterVo vo)
        {
            if (vo == null) return;
            if (StringUtils.isEmpty(mapId)) return;
            IList<MonsterVo> list = null;
            if (_monsterVoList.ContainsKey(mapId))
            {
                list = _monsterVoList[mapId];
            }
            else
            {
                list = new List<MonsterVo>();
                _monsterVoList.Add(mapId, list);
            }
            list.Add(vo);
            if (vo.MonsterVO.quality == 3)
            {
                BossVo = vo;
            }
            LiveMonsterNumber++;
        }

        /**移除怪物**/

        public void RemoveMonster(string id)
        {
            if (StringUtils.isEmpty(id)) return;
            String mapId = AppMap.Instance.mapParser.MapId.ToString();
            //底层移除怪物display列表中对应id的信息
			MonsterDisplay monsterDisp = AppMap.Instance.GetMonster(id);
			//还原Boss脚底Buff特效
			if (MonsterType.TypeBoss == monsterDisp.MonsterVo.quality)
			{
				RemoveFootEffect();
			}
			AppMap.Instance.remove(monsterDisp);

            //移除怪物vo信息
            IList<MonsterVo> list = _monsterVoList.ContainsKey(mapId) ? _monsterVoList[mapId] : null;
            if (list == null) return;
            foreach (MonsterVo item in list)
            {
                if (item.Id.Equals(id))
                {
                    if (item.MonsterVO.quality == 3)
                    {
                        BossVo = null;
                    }
                    list.Remove(item);
                    break;
                }
            }
            BattleMode.Instance.DoMonsterDeath(); // 给服务器发送通知
        }

        /**获取缓冲区中怪物信息**/

        public MonsterVo GetMonster(string mapId, string monsterId)
        {
            IList<MonsterVo> list = _monsterVoList.ContainsKey(mapId) ? _monsterVoList[mapId] : null;
            if (list == null || list.Count < 1) return null;
            return list.FirstOrDefault(item => item.Id.Equals(monsterId));
        }

        public MonsterVo GetCurrentMonster(string mapId)
        {
            IList<MonsterVo> list = _monsterVoList.ContainsKey(mapId) ? _monsterVoList[mapId] : null;
            if (list == null || list.Count < 1) return null;
            return list[0];
        }


        /**根据怪物UID--查找其中一个怪物信息
         * @param uid
         * **/

        public MonsterVo GetMonster(string id)
        {
            if (StringUtils.isEmpty(id)) return null;
            string mapId = AppMap.Instance.mapParser.MapId.ToString();
            IList<MonsterVo> list = _monsterVoList.ContainsKey(mapId) ? _monsterVoList[mapId] : null;
            if (list == null || list.Count < 1) return null;
            return list.FirstOrDefault(item => item.Id.Equals(id));
        }

        //丛配置文件中的id查找怪物
        public MonsterVo GetMonsterWithConfigId(string id)
        {
            if (StringUtils.isEmpty(id)) return null;

            List<MonsterDisplay> monsterList = AppMap.Instance.GetMonster();
            foreach (MonsterDisplay item in monsterList)
            {
                if (item.MonsterVo.id.ToString().Equals(id))
                {
                    return item.Monster;
                }
            }

            return null;
        }

        public void stop()
        {
            _isRuning = false;
            RemoveFootEffect();
            _monsterVoList.Clear();
            LiveMonsterNumber = 0;
        }

        public void start()
        {
            if (_isRuning) return;
            _isRuning = true;
            _beginTime = Time.time;
        }

        /**添加怪物到场景**/

        public void execute()
        {
            if (!_isRuning) return;
            float time = Time.time;
            if (time - _beginTime < delay) return;
            try
            {
                _beginTime = time;
                String mapId = AppMap.Instance.mapParser.MapId.ToString();

                IList<MonsterVo> list = _monsterVoList.ContainsKey(mapId) ? _monsterVoList[mapId] : null;
                //_monsterVoList里面存的怪是服务器返回的                

                if (list == null || list.Count < 1)
                {
                    return;
                }
                MonsterVo item = list[0];
                if (AppMap.Instance.GetMonster(item.Id.ToString()) != null) return;
                list.Remove(item);
                if (item.isDie)
                {
                    Log.info(this, "这个怪物已经死了，不用创建！");
                    return;
                }
                //创建怪物
                Log.info(this, "创建怪物 ，怪物类型ID：" + item.monsterId);
                item.ModelLoadCallBack = LoadMonsterCallBack;
                if (MeVo.instance.mapId == MapTypeConst.GoldHit_MAP)
                {
                    item.X = Random.Range(0.2f, 20f);
                    item.Y = Random.Range(AppMap.Instance.mapParser.CurrentMapRange.MinY + 0.5f,
                        AppMap.Instance.mapParser.CurrentMapRange.MaxY);
                }
                else if (MeVo.instance.mapId == MapTypeConst.WORLD_BOSS)
                {
                    item.X = 16.8f;
                    item.Y = 1.8f;
                    BossMode.Instance.BossName = item.MonsterVO.name;  //缓存世界Boss名字
                    BossMode.Instance.BoosIcon = item.MonsterVO.icon;

                }
                else
                {
                    float minX = MapControl.Instance.MyCamera.LeftBoundX;
                    float maxX = MapControl.Instance.MyCamera.RightBoundX;
                    switch (item.bornType)
                    {
                        case 1:
                            maxX = minX - 1;
                            minX = minX - 3;
                            break;
                        case 3:
                            minX = maxX + 1;
                            maxX = maxX + 3;
                            break;
                        case 4://在镜头中随机点创建
                            minX = minX + 1;
                            maxX = maxX + 1;
                            break;
                    }
                    item.X = Random.Range(minX, maxX);
                    item.Y = Random.Range(AppMap.Instance.mapParser.CurrentMapRange.MinY,
                        AppMap.Instance.mapParser.CurrentMapRange.MaxY);
                }
                AppMap.Instance.CreateMonster(item);
            }
            catch (Exception ex)
            {
                Log.error(this, "-execute() 添加怪物到场景出错,reason:" + ex.StackTrace);
            }
        }

        private void LoadMonsterCallBack(BaseDisplay display)
        {
            var monsterVo = display.GetMeVoByType<MonsterVo>();
            var mapRange = AppMap.Instance.mapParser.CurrentMapRange;
            if (monsterVo.X > (mapRange.MinX + mapRange.MaxX)/2)
            {
                display.ChangeDire(Directions.Left);
            }
            else
            {
                display.ChangeDire(Directions.Right);
            }
            Log.info(this,string.Format("Monster Id : {0} Model Id: {1} Quality: {2}" ,monsterVo.MonsterVO.id,monsterVo.MonsterVO.res,monsterVo.MonsterVO.quality));
            //为boss添加脚底特效
            if (monsterVo.MonsterVO.quality ==  MonsterType.TypeBoss)
            {
                ShowBossFootEffect(display);
                //显示Boss预警特效
                EffectMgr.Instance.CreateUIEffect(EffectId.UI_BossComingWarning, Vector3.zero);
            }
            //从空中产生怪物
            if (monsterVo.bornType == (int) MonsterBornType.BornFromSky)
            {
                _monsterPresent = display;
                float x = GetRandomValueX();
                float heroX = AppMap.Instance.me.GoBase.transform.position.x;
                int direction = x < heroX ? Directions.Right : Directions.Left;
                display.ChangeDire(direction);
                display.GoBase.transform.position = new Vector3(x, 6f,
                    /*display.GoBase.transform.position.z*/Random.Range(mapRange.MinY, mapRange.MaxY) * 5f);//由重力来完成掉落
                display.Controller.MoveController.MonsterDrop(StartShowMonsterAdditionalView);
                StartShowMonsterAdditionalView(display);
            }
            else
            {
                StartShowMonsterAdditionalView(display);
            }
        }

        private void StartShowMonsterAdditionalView(BaseDisplay display)
        {
            //添加其他显示信息
            CoroutineManager.StartCoroutine(AddMonsterAdditionalView(display));
        }

        /// <summary>
        ///     过两帧再添加怪物的其他显示信息，避免出现这些其他信息提前出现引起混乱
        /// </summary>
        /// <param name="display"></param>
        /// <returns></returns>
        private IEnumerator AddMonsterAdditionalView(BaseDisplay display)
        {
            //隔1帧再显示血条信息
            //yield return 0;
            var monsterDisplay = display as MonsterDisplay;
            display.GoCloth.transform.localPosition = new Vector3(0, 0, -30);
            if (display.Controller != null)
            {
                //添加名字UI
                GameObject child = NGUITools.AddChild(display.Controller.gameObject);
                if (monsterDisplay != null)
                {
                    BoxCollider2D boxCollider2D = monsterDisplay.BoxCollider2D;
                    float y = (boxCollider2D.center.y + boxCollider2D.size.y/2)*display.GoCloth.transform.localScale.y;
                    child.transform.localPosition = new Vector3(0f, y + 0.3f, 0f);
                    child.name = "pivot";
                    var item = display.GetVo() as MonsterVo;
                    if (item != null)
                    {
                        var actionControler = display.Controller as ActionControler;
                        if (actionControler != null)
                        {
                            //以下三行预处理的是怪物说话信息
                            GameObject myBossMsg = HudView.Instance.AddItem("msg", child.transform, HudView.Type.Monster);
                            actionControler.GoBossMsg = myBossMsg;
                            monsterDisplay.InitSpeakView(myBossMsg);
                        }
                        AppMap.Instance.AddMonster(monsterDisplay);
                    }
                    //新增怪物刷出特效
                    /* if ((display.GetMeVoByType<MonsterVo>()).bornType != 3) //3为空中掉下来的怪物，不播放这个出生特效
                    {
                        ShowMonsterSpawnEffect(display.Controller.transform.position);
                    }*/
                    //display.ShowBornModelEffect();
                    monsterDisplay.ReadyToUseAi(AiStartDelayTime);
                }
            }
            yield return 0;
        }

        private void ShowMonsterSpawnEffect(Vector3 pos)
        {
            EffectMgr.Instance.CreateMainEffect(EffectId.Main_MonsterSpawn, pos, false);
        }

        //预加载怪物
        public IEnumerator PreloadMonsterList( /*IList<string> preLoadList*/)
        {
            if (PreMonloadList != null)
            {
                _preloadIndex = 0;
                PreloadResource();
                while (_preloadIndex < PreMonloadList.Count)
                {
                    yield return 0;
                }
                PreMonloadList = null;
            }
        }

        private void PreloadResource()
        {
            SysMonsterVo data;
            try
            {
                while (_preloadIndex < PreMonloadList.Count)
                {
                    data = BaseDataMgr.instance.getSysMonsterVo(UInt32.Parse(PreMonloadList[_preloadIndex]));
                    if (data != null)
                    {
                        UnityEngine.Debug.Log("****加载怪物2，BIP.assetbundle");

                        _curMonsterUrl = "Model/Monster/" + data.res + "/Model/BIP.assetbundle";

                        if (!_monsterPreloadList.ContainsKey(_curMonsterUrl))
                        {
                            Log.info(this, "preload effect asset " + _curMonsterUrl);
                            AssetManager.Instance.LoadAsset<GameObject>(_curMonsterUrl, MonsterPreLoaded);
                            break;
                        }
                    }
                    Singleton<StartLoadingView>.Instance.PreLoadedNum += 1;
                    _preloadIndex++;
                }
            }
            catch (Exception e)
            {
                Log.warin(this, "preloadResource error, exception is: " + e.Message);
            }
        }

        //清除预加载怪物列表
        public void ClearMonsterPreload()
        {
            //preMonloadList = null;
            if (_monsterPreloadList != null)
                _monsterPreloadList.Clear();
            Resources.UnloadUnusedAssets();
        }

        //获取预加载的怪物资源
        public GameObject GetMonsterPreload(string url)
        {
            if (!_monsterPreloadList.ContainsKey(url))
                return null;
            return _monsterPreloadList[url];
        }

        //怪物预加载后处理
        private void MonsterPreLoaded(GameObject monster)
        {
            try
            {
                if (null == monster)
                {
                    return;
                }

                //将对象缓存
                if (!_monsterPreloadList.ContainsKey(_curMonsterUrl))
                {
                    _monsterPreloadList.Add(_curMonsterUrl, monster);
                }
            }
            finally
            {
                Singleton<StartLoadingView>.Instance.PreLoadedNum += 1;
                _preloadIndex++;
                PreloadResource();
            }
        }

        private void RemoveFootEffect()
        {
            if (FootEffect)
            {
                FootEffect.transform.parent = EffectMgr.Instance.MainEffectRoot.transform;
                FootEffect.transform.name = "Effect/Main/20017.assetbundle";
                FootEffect.gameObject.SetActive(false);
            }
        }

        /// <summary>
        ///     显示boss脚底光环特效
        /// </summary>
        /// <param name="show">true,显示，false,隐藏</param>
        private void ShowBossFootEffect(BaseDisplay boss)
        {
            if (MeVo.instance.mapId == MapTypeConst.WORLD_BOSS)
            {
                FootEffect = EffectMgr.Instance.GetMainEffectGameObject(EffectId.Main_BossScene);
            }
            else
            {
                FootEffect = EffectMgr.Instance.GetMainEffectGameObject(EffectId.Main_BossFootBuff);
                Log.info(this,"FootEffect is Null " + (FootEffect == null));
            }
            float scale = boss.GetMeByType<ActionDisplay>().BoxCollider2D.size.x/5f;
            scale += 0.2f;
            if (FootEffect)
            {
                FootEffect.name = "FootEffect";
                FootEffect.transform.transform.localScale = new Vector3(scale, scale, scale);
                FootEffect.transform.position = boss.GetMeByType<ActionDisplay>().GoBase.transform.position +
                                                new Vector3(0, 0, -10); //位置
                FootEffect.transform.parent = boss.GetMeByType<ActionDisplay>().GoBase.transform;
                FootEffect.SetActive(true);
				ResetSpriteRender(FootEffect);
            }
        }

		public void ResetSpriteRender(GameObject effectObj)
		{
			SpriteRenderer[] spriteRenderers = effectObj.GetComponentsInChildren<SpriteRenderer>();
			
			foreach (SpriteRenderer item in spriteRenderers)
			{
				item.sortingLayerName = "BackEffect";
			}
		}


        /// <summary>
        ///     在min和max之间产生一个不在角色三个单位范围内的值
        /// </summary>
        /// <returns></returns>
        private float GetRandomValueX()
        {
            const float minRange = 2f; //半径范围
            float result = 0f;
            float meX = AppMap.Instance.me.Controller.transform.position.x;
            MapRange mapRange = AppMap.Instance.mapParser.CurrentMapRange;
            float minX = mapRange.MinX + 2;
            float maxX = mapRange.MaxX - 2;
            if (meX - minX < minRange)
            {
                result = Random.Range(meX + minRange, maxX);
            }
            else if (meX + minRange > maxX)
            {
                result = Random.Range(minX, meX - minRange);
            }
            else
            {
                result = Random.Range(-10, 10) > 0
                    ? Random.Range(minX, meX - minRange)
                    : Random.Range(meX + minRange, maxX);
            }
            return result;
        }
    }
}