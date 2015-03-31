using com.game.consts;
using com.game.module.battle;
using com.game.module.map;
using Com.Game.Module.Copy;
using com.game.module.test;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.debug;
using com.u3d.bases.display.controler;
using UnityEngine;

/**应用层--摄像机**/
using Com.Game.Module.GoGo;
using com.game.manager;
using com.game.data;

namespace com.game
{
    public class MyCamera : MonoBehaviour
    {
        public static float MainCameraSizeHalfX = 5.12f; //主相机X一半的大小
        public static float MainCameraSize;
        //public bool _testIsOrthographic = false;
        //public float _testPosY = 3f;
        //public float _testPosX;
        //public float _testPosZ = -28f;
        //public float _testRotateX = 20f;
        //public float _testRotateY = 0f;
        //public float _testRotateZ = 0f;
        //public float _testFieldOfView = -1f;

        public float LeftBoundX; //场景左边界的X值
        public float RightBoundX; //场景右边界的X值
        private float _borderMax;
        private float _borderMin;
        private float _changeMapPhaseSpeedOffset = 1;
        private Transform _farLayer1; //地图背景层1;
        private Transform _farLayer2; //地图背景层2;
        private Transform _farLayer3; //地图背景层3;
        private Transform _forLayer1; //地图前景层1;
        private Camera _mainCamera; //主摄像机
        private Transform _meTransform; //本角色(摄像机关注的焦点对象)
        private Vector2 _offset;
        private float _scaleTime;
        private Vector2 _screenWh; //屏幕尺寸
        private float _speed;
        private float _speedOffset;
        private Vector3 _target;
        private float _targetX;
        private float _targetY;
        private float _targetZ;
        private Camera _uiCamera; //UI摄像机
        private float _zoomScale;
        public bool IsRuning { get; set; }

        public Camera MainCamera
        {
            get { return _mainCamera; }
        }


        public Camera UiCamera
        {
            get { return _uiCamera; }
        }


        /// <summary>
        ///     初始化相机设置
        /// </summary>
        public void Init()
        {
            _mainCamera = Camera.main;
            _uiCamera = NGUITools.FindCameraForLayer(LayerMask.NameToLayer("UI"));
            _meTransform = AppMap.Instance.me.GoBase.transform;
            var meControler = AppMap.Instance.me.Controller as MeControler;
            if (meControler != null)
                _speed = meControler.MoveSpeed;
            _screenWh.x = Screen.width;
            _screenWh.y = Screen.height;
            MainCameraSize = MainCameraSizeHalfX * Screen.height / Screen.width;
            MainCamera.orthographicSize = MainCameraSize;

            InitPos();
        }

        private void Update()
        {
            if (IsRuning)
            {
                /*
                if (Application.loadedLevelName == "20003") // 三维场景,应该放在切换场景的地方
                {
                    MainCamera.orthographic = _testIsOrthographic;
                    MainCamera.transform.rotation = Quaternion.Euler(_testRotateX, _testRotateY, _testRotateZ);
                }
                else // 二维场景
                {
                    MainCamera.orthographic = true;
                    MainCamera.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    MainCamera.transform.position = new UnityEngine.Vector3(transform.position.x, transform.position.y, -100f);
                }
                */
                Move();
            }
        }

        /**移动镜头和远景层**/

        private void Move()
        {
            GetTargetPos();
            _speedOffset = MapMode.AutoChangeMap
                ? _changeMapPhaseSpeedOffset
                : Mathf.Abs(transform.position.x - _target.x) * 2;
            transform.position = Vector3.MoveTowards(transform.position, _target, Time.deltaTime * _speed * _speedOffset);
            //移动远景层

            LeftBoundX = transform.position.x - _offset.x; //远景层和相机的左边对其
            RightBoundX = transform.position.x + _offset.x;
            if (_farLayer1 != null)
            {
                Vector3 v1 = _farLayer1.position;
                v1.x = LeftBoundX;
                _farLayer1.position = v1;
            }
            if (_farLayer2 != null)
            {
                Vector3 v2 = _farLayer2.position;
                v2.x = LeftBoundX * 0.75f;
                _farLayer2.position = v2;
            }
            if (_farLayer3 != null)
            {
                Vector3 v3 = _farLayer3.position;
                v3.x = LeftBoundX * 0.5f;
                _farLayer3.position = v3;
            }
            if (_forLayer1 != null)
            {
                Vector3 v4 = _forLayer1.position;
                v4.x = -1 * LeftBoundX * 0.25f;
                _forLayer1.position = v4;
            }
        }

        /**获取摄像机移动的目标位置**/

        private void GetTargetPos() // call by Move() by Update()
        {
            //设置目标位置和边界范围
            if (_meTransform == null) return;
            _targetX = _meTransform.position.x; // 摄像机 X 跟随 主角X轴
            _targetY = MainCameraSize;

            _targetZ = transform.position.z;

            /*]
            if (Application.loadedLevelName == "20003") // 三维场景
            {
                _targetY += _testPosY;
                _targetZ = (_testPosZ == -1f ? transform.position.z : _testPosZ);
                if (_testFieldOfView != -1f)
                    MainCamera.fieldOfView = _testFieldOfView;
            }
            */
            _borderMax = AppMap.Instance.mapParser.EachStageLength[MapMode.CUR_MAP_PHASE - 1] +
                         AppMap.Instance.mapParser.PosX - _offset.x; // _offset.x 差不多 半屏吧


            //自动移动到下一阶段地图时时的相应处理
            if (MapMode.AutoChangeMap && !MapMode.InStageEndStory)
            {
                if (transform.position.x >= AppMap.Instance.mapParser.PosX - _offset.x + 0.1f)
                //在玩家移动到距离本地图阶段只剩下'半个摄像机长度'时，摄像机开始加速----modify by lixi
                {
                    //在玩家视野中出现第二阶段地图时摄像机速度和修改摄像机的移动范围（每次切换地图阶段只更新一次）
                    if (_changeMapPhaseSpeedOffset <= 1.1f)
                    {
                        InitMap(); // set -> _borderMin = AppMap.Instance.mapParser.PosX + _offset.x;
                        _changeMapPhaseSpeedOffset = (_borderMin - transform.position.x) /
                                                     (AppMap.Instance.mapParser.PosX + Global.HERO_WEIGHT -
                                                      transform.position.x);
                        Log.info(this, "-check()更新摄像机最小边界和移动速度偏移量: " + _changeMapPhaseSpeedOffset);
                    }


                    if (transform.position.x >= AppMap.Instance.mapParser.PosX + _offset.x - 0.1f)
                    {
                        Log.info(this, "-check()自动移动结束,更新战斗UI阶段信息");
                        Singleton<BattleMode>.Instance.StageInfo = MapMode.CUR_MAP_PHASE + "/" +
                                                                   AppMap.Instance.mapParser.AccumulatedStagesLength
                                                                       .Count;
                        MapMode.AutoChangeMap = false;
                        MapMode.DisableInput = false;
                        MapMode.CanGoToNextPhase = false;
                        MapMode.WaitRefreshMonster = false;
                        Singleton<GoGoView>.Instance.CloseView();
                        _changeMapPhaseSpeedOffset = 1;
                        Singleton<MapControl>.Instance.PlayStartStageStory();
                        //						Singleton<MapMode>.Instance.EnterNewPhase ((byte)MapMode.CUR_MAP_PHASE);
                        Singleton<MapControl>.Instance.RefreshMonster();
                    }
                }
            }
            uint mapId = AppMap.Instance.mapParser.MapId;
            SysMapVo mapVo = BaseDataMgr.instance.GetMapVo(mapId);
            if (mapVo.type != MapTypeConst.CITY_MAP)
            {
                //Log.debug(this, "我移动的坐标点X：" + _meTransform.position.x + "    我移动的坐标点Y:" + _meTransform.position.y + "   我移动的坐标点Z:" + _meTransform.position.z);
                //bool isTrigger = AppMap.Instance.isTrigger(_meTransform.position.x, _meTransform.position.y, _meTransform.position.z);
                bool isTrigger = AppMap.Instance.isTrigger(7f, 2f, 1f);
                if (isTrigger == true && MapMode.CanGoToNextPhase == false&& MapMode.IsTriggered == false)
                {
                    Log.debug(this, "天啊，我触发了一堆怪物，要死了要死了");
                    Singleton<CopyMode>.Instance.TriggerMonList();
                    MapMode.IsTriggered = true;
                }
            }

            //边界检测
            if (_targetX < _borderMin) _targetX = _borderMin;
            if (_targetX > _borderMax) _targetX = _borderMax;

            //设置摄像机移动的目标位置
            _target.x = _targetX;
            _target.y = _targetY;
            _target.z = _targetZ;
        }

        /**初始化地图**/

        private void InitMap()
        {
            _farLayer1 = AppMap.Instance.mapParser.FarLayer1;
            _farLayer2 = AppMap.Instance.mapParser.FarLayer2;
            _farLayer3 = AppMap.Instance.mapParser.FarLayer3;
            _forLayer1 = AppMap.Instance.mapParser.ForLayer1;
            //计算人物离开摄像机视野的距离（即半个屏幕的宽）
            MainCamera.orthographicSize = MainCameraSize;
            _offset.x = _screenWh.x / _screenWh.y * MainCameraSize;
            _borderMin = AppMap.Instance.mapParser.PosX + _offset.x;
            _borderMax = AppMap.Instance.mapParser.EachStageLength[MapMode.CUR_MAP_PHASE - 1] +
                         AppMap.Instance.mapParser.PosX - _offset.x;
        }

        /**调整地图到初始位置**/

        public void InitPos()
        {
            InitMap();
            GetTargetPos();
            transform.position = _target;
            IsRuning = true;
            LeftBoundX = transform.position.x - _offset.x; //远景层和相机的左边对其
            RightBoundX = transform.position.x + _offset.x;
        }

        /// <summary>
        /// 通关特效
        /// </summary>
        public void ShowCopyWinEffect()
        {
            /* Time.timeScale = 0.25f;
             iTween.ValueTo(gameObject,
                 iTween.Hash("from", MainCameraSize, "to", MainCameraSize*0.7f, "time", 1.0, "onupdate",
                     "OnMyCameraItweenUpdate", "oncomplete", "OnMyCameraScaleOut", "ignoretimescale", true));*/
        }

        private void OnMyCameraItweenUpdate(float value)
        {
            MainCamera.orthographicSize = value;
        }

        private void OnMyCameraScaleOut()
        {
            Time.timeScale = 1;
            AppMap.Instance.me.Controller.StatuController.SetStatu(Status.Win);
        }

        public void EndMyCameraSceneGrayEffect()
        {
            MainCamera.orthographicSize = MainCameraSize;
            AppMap.Instance.mapParser.ShowMap();
            if (AppMap.Instance.me != null)
            {
                AppMap.Instance.me.Animator.speed = 1.0f;
                AppMap.Instance.SetMonsterAnimationSpeed(1.0f);
            }
        }

        public void ScaleCamera(float delayTime, float scaleTime)
        {
            _scaleTime = scaleTime;
            iTween.ValueTo(gameObject,
                iTween.Hash("from", MainCameraSize, "to", MainCameraSize - 0.1, "time", _scaleTime * 0.5f, "onupdate",
                    "OnMyCameraItweenUpdate", "oncomplete", "OnMyCameraScaleHalf", "delay", delayTime, "ignoretimescale",
                    true));
        }

        public void ZoomCamera(float zoom, float duration)
        {
            _zoomScale = zoom;
            _scaleTime = duration;
            iTween.ValueTo(gameObject,
                iTween.Hash("from", MainCameraSize, "to", MainCameraSize * _zoomScale, "time", duration * 0.5f, "onupdate",
                    "OnMyCameraItweenUpdate", "oncomplete", "OnMyCameraZoomHalf", "delay", 0f, "ignoretimescale", true));
        }

        private void OnMyCameraZoomHalf()
        {
            iTween.ValueTo(gameObject,
                iTween.Hash("from", MainCameraSize * _zoomScale, "to", MainCameraSize, "time", _scaleTime * 0.5f, "onupdate",
                    "OnMyCameraItweenUpdate", "oncomplete", "EndMyCameraScaleEnd", "ignoretimescale", true));
        }

        private void OnMyCameraScaleHalf()
        {
            iTween.ValueTo(gameObject,
                iTween.Hash("from", MainCameraSize - 0.1, "to", MainCameraSize, "time", _scaleTime * 0.5f, "onupdate",
                    "OnMyCameraItweenUpdate", "oncomplete", "EndMyCameraScaleEnd", "ignoretimescale", true));
        }

        public void EndMyCameraScaleEnd()
        {
            MainCamera.orthographicSize = MainCameraSize;
        }
    }
}