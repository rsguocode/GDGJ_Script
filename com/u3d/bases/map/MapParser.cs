using System.Collections;
using System.Collections.Generic;
using com.game;
using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.module.loading;
using com.game.module.map;
using com.game.module.test;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.debug;
using com.u3d.bases.display;
using com.u3d.bases.display.vo;
using com.u3d.bases.loader;
using UnityEngine;

/**地图加载,解析类*/

namespace com.u3d.bases.map
{
    //地图切换完成委托
    public delegate void MapCallback(uint mapId);

    public class MapParser
    {
        private readonly BaseMap _map; //当前场景游戏对象
        public List<float> AccumulatedStagesLength; //地图阶段长度，累计长度
        public List<float> EachStageLength; //地图阶段长度，每个阶段的长度
        
        public List<float> EachPosX; // 每个阶段的起始点，new;

        private List<MapRange> _mapRangeList;
        public SysMapVo MapVo; //缓存MapVo信息，避免多次查表
        private MapCallback _callback; //场景加载完回调事件
        private Transform _farLayer1; //远景层1
        private Transform _farLayer2; //远景层2
        private Transform _farLayer3; //远景层3
        private Transform _forLayer1; //前景层1
        private bool _isChange = true; //地图切换状态[true:切换中,false:非切换中]
        private ILoadingBar _loadingBar; //加载条
        private GameObject _mapGameObject; //地图对象
        private uint _mapId; //当前场景的mapId
        private Vector2 _mapSize; //地图尺寸
        private bool _needSyn; //当前地图玩家的数据是否需要同步
        private float _startPosX; //地图位置
		private float _worldTransPosX; //世界地图传送点坐标X
		private float _worldTransPosY; //世界地图传送点坐标Y
		private float _transPosX; //副本传送点坐标X
		private float _transPosY; //副本传送点坐标Y

        public MapParser(BaseMap map)
        {
            _map = map;
            _mapSize = new Vector2();
            _startPosX = 0;
            AccumulatedStagesLength = new List<float>();
            EachStageLength = new List<float>();
            _mapRangeList = new List<MapRange>();

            EachPosX = new List<float>();
        }

        public uint MapId
        {
            get { return _mapId; }
            set
            {
                _mapId = value;
                MapVo = BaseDataMgr.instance.GetMapVo(_mapId);
            }
        }

        public Transform FarLayer1
        {
            get { return _farLayer1; }
            set { _farLayer1 = value; }
        }

        /**远景层1**/

        public Transform FarLayer2
        {
            get { return _farLayer2; }
            set { _farLayer2 = value; }
        }

        public Transform FarLayer3
        {
            get { return _farLayer3; }
            set { _farLayer3 = value; }
        }

        public Transform ForLayer1
        {
            get { return _forLayer1; }
            set { _forLayer1 = value; }
        }

        /**地图尺寸**/

        public Vector2 MapWh
        {
            get { return _mapSize; }
            set { _mapSize = value; }
        }

        /// <summary>
        /// 地图起始位置
        /// </summary>
        public float PosX
        {
            get { return _startPosX; }
            set { _startPosX = value; }
        }

		/**传送点坐标***/
		public float WorldTransPosX
		{
            get { return _worldTransPosX; }
            set { _worldTransPosX = value; }
		}
		public float WorldTransPosY
		{
			get { return _worldTransPosY; }
			set { _worldTransPosY = value; }
		}
		public float TransPosX
		{
			get { return _transPosX; }
			set { _transPosX = value; }
		}
		public float TransPosY
		{
			get { return _transPosY; }
			set { _transPosY = value; }
		}

        public bool IsChange
        {
            get { return _isChange; }
            set
            {
                _isChange = value;
                if (_isChange)
                {
                    MeVo.instance.IsUnbeatable = true;
                }
                else
                {
                    MeVo.instance.IsUnbeatable = false;
                }
            }
        }

        public GameObject MapGameObject
        {
            get { return _mapGameObject; }
        }

        /// <summary>
        ///     当前副本是否需要同步
        /// </summary>
        public bool NeedSyn
        {
            get { return MapVo.need_synchronization; }
        }


        /**加载地图
         * @param MapId  加载地图ID
         * @param mapUrl 地图资源地址  
         * **/

        public void LoadMap(uint mapId)
        {
            Log.info(this, "-loadMap()设置MapParser里的isChange为true并更新mapId");
            IsChange = true;
            MapId = mapId;
            Log.info(this, "-loadMap()停止地图渲染");
            _map.stopRender();
            Log.info(this, "-loadMap()销毁场景中所有物体");
            _map.dispose();
            SysMapVo mapVo = BaseDataMgr.instance.GetMapVo(mapId);
            var resourceId = (uint)mapVo.resource_id;
            if (Application.platform == RuntimePlatform.WindowsEditor && AppNet.main.TestSceneId != 0)
            {
               resourceId = (uint)AppNet.main.TestSceneId;   //给测试场景用
            }
            Singleton<StartLoadingView>.Instance.loaderList.Clear();
            Singleton<StartLoadingView>.Instance.loaderList.Add(
                AssetManager.Instance.LoadSceneLevel(resourceId, LoadSceneCallBack));
            if (Singleton<MapMode>.Instance.IsFirstInToScene)
            {
                //第一次开始加载场景时执行的预加载：
                AddFirstSceneLoad();
            }
        }

        private void AddFirstSceneLoad()
        {
//            Singleton<CopyView>.Instance.PreLoadCopyViewFirstSceneLoad(); //预加载CopyView资源
        }


        /**加载进度回调**/

        private void OnProgreessFun(int progress)
        {
            _loadingBar.update(progress);
        }

        public void LoadSceneCallBack(Object obj)
        {
            LoadSceneBack();
        }

        /// <summary>
        ///     加载完场景回调
        /// </summary>
        private void LoadSceneBack()
        {
            _mapGameObject = GameObject.Find("Map");
            ResetLayer();
            CalculateStageLength();
            AppMap.Instance.mapParser.PosX = MapMode.CUR_MAP_PHASE == 1
                ? 0
                : EachPosX[MapMode.CUR_MAP_PHASE - 1];// AccumulatedStagesLength[MapMode.CUR_MAP_PHASE - 2];
            _mapSize.x = EachStageLength[MapMode.CUR_MAP_PHASE - 1];
            IsChange = false;
            CoroutineManager.StartCoroutine(ShowNpc());
            ShowTrap();
            if (_callback != null) _callback(MapId);
        }

        //显示陷阱
        private void ShowTrap()
        {
            string[] trapList = StringUtils.SplitVoString(MapVo.trapList, "],[");
            for (int i = 0; i < trapList.Length; i++)
            {
                string[] trapInfo = StringUtils.SplitVoString(trapList[i]);
                if (trapInfo.Length == 1) continue;
                if (trapInfo.Length < 3)
                {
                    Log.error(this, "地图陷阱Trap信息配置错误，地图id" + MapVo.id);
                    continue;
                }
                uint id = uint.Parse(trapInfo[0]);
                var trapVo = new TrapVo
                {
                    Id = id,
                    X = float.Parse(trapInfo[1]),
                    Y = float.Parse(trapInfo[2]),
                    ModelLoadCallBack = LoadTrapBack,
                    Type = DisplayType.Trap,
                    TrapId = id
                };
                AppMap.Instance.CreateTrap(trapVo);
            }
        }

        private void LoadTrapBack(BaseDisplay trapDisplay)
        {
        }

        /// <summary>
        ///     根据场景的信息显示NPC
        /// </summary>
        private IEnumerator ShowNpc()
        {            

            string[] npcList = StringUtils.SplitVoString(MapVo.npcList, "],[");

            for (int i = 0; i < npcList.Length; i++)
            {
                string[] npcInfo = StringUtils.SplitVoString(npcList[i]);
                if (npcInfo.Length == 1) continue;
                if (npcInfo.Length < 3)
                {
                    Log.error(this, "地图NPC信息配置错误，地图id" + MapVo.id);
                    continue;
                }
                uint id = uint.Parse(npcInfo[0]);
                var npcVo = new DisplayVo
                {
                    Id = id,
                    X = float.Parse(npcInfo[1]),
                    Y = float.Parse(npcInfo[2]),
                    ModelLoadCallBack = LoadNpcBack,
                    Type = DisplayType.NPC
                };
                AppMap.Instance.CreateNpc(npcVo);
                yield return 0;
            }
        }

        private void LoadNpcBack(BaseDisplay npcDisplay)
        {
        }

        /// <summary>
        ///     计算副本阶段长度
        /// </summary>
        private void CalculateStageLength()
        {
            AccumulatedStagesLength.Clear();
            EachStageLength.Clear();
            EachPosX.Clear();// new
            _mapRangeList.Clear();
            float accLength = 0f;
            Transform stagesTransform = MapGameObject.transform.Find("Stages").gameObject.transform;
            for (int i = 0; i < stagesTransform.childCount; i++)
            {
                //获取行走区域信息
                Transform child = stagesTransform.Find("Stage" + (i + 1));
                var boxCollider2D = child.GetComponent<BoxCollider2D>();
                if (boxCollider2D == null)
                {
                    Log.error(this, "地图没有配置阶段行走区域，地图id：" + MapId);
                }
                else
                {
                    var childPos = child.position;
                    var mapRange = new MapRange
                    {
                        MinX = childPos.x + boxCollider2D.center.x - boxCollider2D.size.x * 0.5f,
                        MaxX = childPos.x + boxCollider2D.center.x + boxCollider2D.size.x * 0.5f,
                        MinY = childPos.y + boxCollider2D.center.y - boxCollider2D.size.y * 0.5f,
                        MaxY = childPos.y + boxCollider2D.center.y + boxCollider2D.size.y * 0.5f
                    };
                    _mapRangeList.Add(mapRange);
                    boxCollider2D.enabled = false;
                }
                //获取阶段长度信息
                Transform stage = child.Find("Bg1");
                boxCollider2D = stage.GetComponent<BoxCollider2D>();
                float length = boxCollider2D.size.x;
                accLength = stage.position.x + length;
                AccumulatedStagesLength.Add(accLength); // Bg1起点 + Bg1长度
                EachStageLength.Add(length);
                EachPosX.Add(stage.position.x); // new
                boxCollider2D.enabled = false;
            }
        }

        public MapRange CurrentMapRange {
            get { return _mapRangeList[MapMode.CUR_MAP_PHASE-1]; }
        }

        /// <summary>
        ///     重新设置地图各层级的层次
        /// </summary>
        private void ResetLayer()
        {
            
            _farLayer1 = MapGameObject.transform.Find("Background1");
            _farLayer2 = MapGameObject.transform.Find("Background2");
            _farLayer3 = MapGameObject.transform.Find("Background3");
            _forLayer1 = MapGameObject.transform.Find("Forground1");
            if (_farLayer1 != null)
            {
                for (int j = 0; j < _farLayer1.childCount; j++)
                {
                    _farLayer1.GetChild(j).GetComponent<SpriteRenderer>().sortingLayerName =
                        SortingLayerType.BACKGROUND_LAYER;
                }
            }
            if (_farLayer2 != null)
            {
                for (int j = 0; j < _farLayer2.childCount; j++)
                {
                    _farLayer2.GetChild(j).GetComponent<SpriteRenderer>().sortingLayerName =
                        SortingLayerType.BACKGROUND_LAYER;
                }
            }
            if (_farLayer3 != null)
            {
                for (int j = 0; j < _farLayer3.childCount; j++)
                {
                    _farLayer3.GetChild(j).GetComponent<SpriteRenderer>().sortingLayerName =
                        SortingLayerType.BACKGROUND_LAYER;
                }
            }
            if (_forLayer1 != null)
            {
                for (int j = 0; j < _forLayer1.childCount; j++)
                {
                    _forLayer1.GetChild(j).GetComponent<SpriteRenderer>().sortingLayerName =
                        SortingLayerType.Default;
                }
            }
            Transform stages = MapGameObject.transform.Find("Stages");
            for (int i = 0; i < stages.childCount; i++)
            {
                Transform stage = stages.GetChild(i).transform;
                Transform bg0 = stage.Find("Bg0");
                Transform bg1 = stage.Find("Bg1");
                Transform bg2 = stage.Find("Bg2");
                Transform bg3 = stage.Find("Bg3");
                if (bg0 != null)
                {
                    for (int k1 = 0; k1 < bg0.childCount; k1++)
                    {
                        bg0.GetChild(k1).GetComponent<SpriteRenderer>().sortingLayerName =
                            SortingLayerType.ROAD_BACKGROUND_LAYER;
                    }
                }
                if (bg1 != null)
                {
                    for (int k1 = 0; k1 < bg1.childCount; k1++)
                    {
                        bg1.GetChild(k1).GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerType.ROAD_GROUND;
                    }
                }
                if (bg2 != null)
                {
                    for (int k2 = 0; k2 < bg2.childCount; k2++)
                    {
                        bg2.GetChild(k2).GetComponent<SpriteRenderer>().sortingLayerName =
                            SortingLayerType.ROAD_FOR_ROUND_LAYER;
                    }
                }
                if (bg3 != null)
                {
                    for (int k3 = 0; k3 < bg3.childCount; k3++)
                    {
                        bg3.GetChild(k3).GetComponent<SpriteRenderer>().sortingLayerName = SortingLayerType.Default;
                    }
                }
            }
        }

        /**远景层1**/

        /**根据当前x返回允许的x,用于玩家移动范围的限制*/

        public float GetFinalPlayerX(float curX)
        {
            if (MapMode.AutoChangeMap)
            {
                return curX;
            }
            float finalX = Mathf.Min(curX, CurrentMapRange.MaxX - 1.0f);
            finalX = Mathf.Max(finalX, CurrentMapRange.MinX + 1.0f);
            return finalX;
        }

        /**根据当前x返回允许的x,用于怪物移动范围的限制*/

        public float GetFinalMonsterX(float curX)
        {
            if (MapMode.AutoChangeMap)
            {
                return curX;
            }
            float finalX = Mathf.Min(curX, CurrentMapRange.MaxX);
            finalX = Mathf.Max(finalX, CurrentMapRange.MinX);
            return finalX;
        }


        /**注册地图加载完成时--回调处理**/

        public void AddMapCallback(MapCallback mapLoadCallBack)
        {
            _callback = mapLoadCallBack;
        }

        /**注册加载条**/

        public void AddLoadingBar(ILoadingBar mapLoadingBar)
        {
            _loadingBar = mapLoadingBar;
        }

        public void SetMapLayer(int layer)
        {
            if (MapGameObject == null) return;
            NGUITools.SetLayer(MapGameObject, layer);
        }

        public void HideMap()
        {
            if (MapGameObject == null) return;
            MapGameObject.SetActive(false);
        }

        public void ShowMap()
        {
            if (MapGameObject == null) return;
            MapGameObject.SetActive(true);
        }
    }
}