using System;
using System.Collections;
using com.game.consts;
using com.game.manager;
using com.game.module.test;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.debug;
using com.u3d.bases.display.controler;
using com.u3d.bases.display.vo;
using com.u3d.bases.interfaces;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Com.Game.Utils;
using com.game;


/**游戏对象基类，游戏显示对象实体*/

namespace com.u3d.bases.display
{
    public class BaseDisplay : IPoolable
    {
        public Animator Animator; //动画控制中心
        public GameObject DefenceEffect;
        public bool IsUsing; //使用状态
        protected DisplayVo Vo; //对象基础数据  
        private int _curDir;
        private Color _endColor;
        private int _keepFrame;
        private float _modelAlpha;
        private SpriteRenderer[] _spriteRenderers;
        private Color _startColor;
        private float _startTime;
        private float _timeLength;
        private Color _warnEndColor;
        private Color _warnStartColor;
        private float _warnStartTime;
        /**被谁抓起;*/
        private BaseControler _graspedController = null;
        /**抓取了谁;*/
        private BaseControler _grapController = null;
        /**抓取XX的parent*/
        private Transform _grapParent = null;

        public BaseDisplay()
        {
            GoBase = new GameObject(GetType().Name);
        }

        protected virtual string SortingLayer
        {
            get { return "Player"; }
        } //显示排序层次

        public int Type
        {
            get { return Vo.Type; }
        } //游戏对象类型

        public String Key
        {
            get { return Vo.Key; }
        } //游戏对象的主键
        public GameObject GoBase { get; private set; } //游戏模型的挂载体
        public GameObject GoCloth { get; set; } //衣服模型物体
        public BaseControler Controller { get; set; }

        public int CurDire
        {
            get
            {
                if (Controller == null) return 0;
                return _curDir;
            }
        }

        /**取得脸部的朝向
        * @return 0:找不到方向,2：面朝右,4:面朝左
        * **/

        public int CurFaceDire
        {
            get
            {
                if (Controller == null) return 0;
                int faceDir = Controller.transform.localScale.x < 0 ? Directions.Left : Directions.Right;
                return faceDir;
            }
        }

        /**释放对象**/

        public virtual void Dispose()
        {
            IsUsing = false;
            Dispose(GoCloth);
            GoCloth = null;
            Animator = null;
            _spriteRenderers = null;
            if (Controller != null)
            {
                Controller.Dispose();
            }
        }

        public void getBefore()
        {
            ResetWhileNew();
        }

        //游戏对象控制中心 Controller

        public T GetMeVoByType<T>() where T : DisplayVo
        {
            return Vo as T;
        }

        public T GetMeByType<T>() where T : BaseDisplay
        {
            return this as T;
        }

        internal virtual void SetVo(DisplayVo vo)
        {
            ResetWhileNew();
            Vo = vo;
            CreateMode(vo.ClothUrl);
        }

        public virtual DisplayVo GetVo()
        {
            return Vo;
        }

        /// <summary>
        /// 通过tag获取对应部件;
        /// </summary>
        /// <param name="_boneTag"></param>
        /// <returns></returns>
        public GameObject GetPartBonesByTag(string _boneTag)
        {
            GameObject rtnGo = PlayUtils.GetPartBonesByHostAndTag(this.GetVo().Id.ToString(), _boneTag);
            return rtnGo;
        }


        /**设置播放动作**/

        public virtual void SetAction(string actionType)
        {
        }

        public void Pos(Vector3 vec3)
        {
            Pos(vec3.x, vec3.y, vec3.z);
        }

        /**设置位置**/

        public void Pos(float x, float y, float z)
        {
            if (Controller == null) return;
            Vector3 v;// = Controller.transform.position;
            v.x = x;
            v.y = y;
            v.z = z;
            Controller.transform.position = v;
        }

        /**设置位置**/

        public void Pos(float x, float y)
        {
            if (Controller == null) return;
            Pos(x, y, 5*y);
        }

        /// <summary>
        ///     通过绕Y轴的旋转更改模型的朝向
        /// </summary>
        /// <param name="dire"></param>
        public virtual void ChangeDire(int dire)
        {
            if (Controller == null) return;
            var baseDisplayVo = Vo as BaseRoleVo;
            if (baseDisplayVo != null && baseDisplayVo.NeedKeep)
            {
                return;
            }
            //被抓取的时候记录左右摇杆次数;
            if (IsGrasped)
            {
                recordDirectionKeyCount(dire);
            }
            _curDir = dire;
            Vector3 v3 = Controller.transform.localScale;
            switch (dire)
            {
                case Directions.Right:
                case Directions.TopRight:
                case Directions.DownRight:
                    v3.x = v3.x > 0 ? v3.x : -v3.x;
                    break;
                case Directions.Left:
                case Directions.TopLeft:
                case Directions.DownLeft:
                    v3.x = v3.x < 0 ? v3.x : -v3.x;
                    break;
            }
            Controller.transform.localScale = v3;
        }

        /// <summary>
        /// 清空按键方向次数;
        /// </summary>
        public void clearDirectionKeyCount()
        {
            Global.Times_Left = 0;
            Global.Times_Right = 0;
            Global.Times_Top = 0;
            Global.Times_Down = 0;
        }

        /// <summary>
        /// 记录按键方向次数;
        /// </summary>
        /// <param name="_nextDire"></param>
        private void recordDirectionKeyCount(int _nextDire)
        {
            if (AppMap.Instance.me.GetVo().Id == GetVo().Id)
            {
                if (_curDir != _nextDire)
                {
                    if (_nextDire == Directions.Right)
                    {
                        Global.Times_Right++;
                    }
                    else if (_nextDire == Directions.TopRight)
                    {
                        Global.Times_Top++;
                        Global.Times_Right++;
                    }
                    else if (_nextDire == Directions.DownRight)
                    {
                        Global.Times_Down++;
                        Global.Times_Right++;
                    }
                    else if (_nextDire == Directions.Left)
                    {
                        Global.Times_Left++;
                    }
                    else if (_nextDire == Directions.TopLeft)
                    {
                        Global.Times_Top++;
                        Global.Times_Left++;
                    }
                    else if (_nextDire == Directions.DownLeft)
                    {
                        Global.Times_Down++;
                        Global.Times_Left++;
                    }
                }
            }
        }

        /**取得当前朝向
         * @return 0:找不到方向,其他：上下左右、左上、左下、右上、右下八个方向中的一个方向
         * **/

        /**销毁指定物体**/

        protected void Dispose(GameObject target)
        {
            if (target == null) return;
            target.transform.parent = null;
            Object.Destroy(target);
        }

        /**添加脚本**/

        protected virtual void AddScript(GameObject go)
        {
            if (go.GetComponent<BaseControler>() != null) return;
            Controller = go.AddComponent<BaseControler>();
            Controller.Me = this;
        }

        /**使用对象时初始化**/

        protected virtual void ResetWhileNew()
        {
            Vo = null;
            GoCloth = null;
            IsUsing = true;
        }


        /// <summary>
        ///     加载模型
        /// </summary>
        /// <param name="url">模型资源地址</param>
        protected virtual void CreateMode(string url)
        {

                /*if (Type == DisplayType.ROLE)
                {
                    AssetManager.Instance.LoadAssetFromResources<GameObject>(url, LoadModelCallBack); //测试用的接口
                }
                else
                {*/
            if (Type == DisplayType.ROLE || Type == DisplayType.ROLE_MODE)
                {
                    UnityEngine.Debug.Log("****cache, url = " + url);
                    AssetManager.Instance.LoadAsset<GameObject>(url, LoadModelCallBack,null,true);//角色的模型资源缓存处理
                }
                else
                {
                    UnityEngine.Debug.Log("****not cache, url = " + url);
                    AssetManager.Instance.LoadAsset<GameObject>(url, LoadModelCallBack);
                }
                //}

        }

        /// <summary>
        ///     加载完模型后的处理逻辑
        /// </summary>
        /// <param name="gameObject">模型的Asset文件</param>
        protected void LoadModelCallBack(GameObject gameObject)
        {
            var mode = (GameObject) Object.Instantiate(gameObject); // 实例化怪物资源
            if (mode == null || GoBase == null) return;
            mode.name = Key;
            GoCloth = mode;
            Relation(mode, GoBase.transform); // GoBase 就是MonsterDisplay
            AddScript(GoBase); //模型加载完后在挂载脚本，否则脚本挂载不上去
            AddFootShadow();
            //AddBossFootEffect();
            Pos(Vo.X, Vo.Y);
            //动态设置层次
            SetSortingOrder(true);
            if (Vo.ModelLoadCallBack != null)
            {
                Vo.ModelLoadCallBack(this);
            }
            mode.transform.localPosition = new Vector3(0, 0, -30); //使模型靠近相机，远离背景
        }

        private void AddFootShadow()
        {
            if (Type == DisplayType.ROLE || Type == DisplayType.MONSTER || Type == DisplayType.NPC||Type == DisplayType.PET)
            {
                var footShadow =
                    Object.Instantiate(CommonModelAssetManager.Instance.GetFootShadowGameObject()) as GameObject;
                Vector3 scale = Vector3.one;
                Vector3 parentScale = GoCloth.transform.localScale;
                BoxCollider2D boxCollider = GetMeByType<ActionDisplay>().BoxCollider2D;
                if (boxCollider && footShadow && Type != DisplayType.ROLE)
                {
                    scale.x = boxCollider.size.x*parentScale.x*1.2f;
                    scale.y = boxCollider.size.y*parentScale.y*1.2f;
                    footShadow.transform.localScale = scale;
                }
                Relation(footShadow, GoBase.transform); //增加脚底阴影
            }
        }

        private void AddBossFootEffect()
        {
            if (Type == DisplayType.MONSTER || Type == DisplayType.NPC)
            {
                var footShadow =
                    Object.Instantiate(CommonModelAssetManager.Instance.GetFootShadowGameObject()) as GameObject;
                Vector3 scale = Vector3.one;
                Vector3 parentScale = GoCloth.transform.localScale;
                BoxCollider2D boxCollider = GetMeByType<ActionDisplay>().BoxCollider2D;
                if (boxCollider && footShadow)
                {
                    scale.x = boxCollider.size.x*parentScale.x*1.5f;
                    scale.y = boxCollider.size.y*parentScale.y*1.5f;
                    footShadow.transform.localScale = scale;
                }
                Relation(footShadow, GoCloth.transform); //增加脚底阴影
            }
        }

        public void SetSortingOrder(bool isFirst)
        {
            if (Controller == null) return;
            GameObject mode = GoCloth;
            if (isFirst)
            {
                _spriteRenderers = mode.GetComponentsInChildren<SpriteRenderer>(true);
            }
            int y = (int) (Controller.transform.position.y*100)*100;
            foreach (SpriteRenderer sprite in _spriteRenderers)
            {
                int idL = Convert.ToInt32(Vo.Id%100)*20;
                int order = 0;
                if (isFirst)
                {
                    order = (sprite.sortingOrder + 10)%20 + idL - y;
                }
                else
                {
                    order = sprite.sortingOrder%20 + idL - y;
                }
                sprite.sortingOrder = order;
                /*var pos = sprite.transform.localPosition;
                pos.z = sprite.sortingOrder * 0.1f;
                sprite.transform.localPosition = pos;
                sprite.sortingOrder = 0;*/
                sprite.sortingLayerName = SortingLayer;
            }
        }

        /**子物体关联父物体**/

        protected void Relation(GameObject child, Transform parent)
        {
            if (parent == null) return;
            Vector3 p = Clone(child.transform.position);
            Vector3 r = Clone(child.transform.eulerAngles);
            Vector3 s = Clone(child.transform.lossyScale);
            child.transform.parent = parent;
            child.transform.localPosition = p;
            child.transform.localEulerAngles = r;
            child.transform.localScale = s;
        }

        /**克隆坐标**/

        protected Vector3 Clone(Vector3 v3)
        {
            var clone3 = new Vector3 {x = v3.x, y = v3.y, z = v3.z};
            return clone3;
        }

        /// <summary>
        ///     重置模型为标准状态
        /// </summary>
        protected void SetStandClothGoPosition()
        {
            //GoCloth.transform.localEulerAngles = Vector3.zero;
        }

        /**-------------------------华丽的分割线，这部分用于处理模型显示特效--------------------------*/

        public void StartShowModelColorEffect(Color startColor, Color endColor)
        {
            _startColor = startColor;
            _endColor = endColor;
            _startTime = Time.time;
            CoroutineManager.StartCoroutine(ShowModelColorEffect());
        }

        public void StartShowModelBeAttackedEffect()
        {
            if (_keepFrame > 0)
            {
                _keepFrame = 0;
            }
            else
            {
                CoroutineManager.StartCoroutine(ShowModelBeAttackedEffect());
            }
        }

        private IEnumerator ShowModelBeAttackedEffect()
        {
            ModelEffectManager.ShowSpriteColorEffect(GoCloth, ColorConst.BeAttackStep1Color);
            yield return 0;
            ModelEffectManager.ShowSpriteColorEffect(GoCloth, ColorConst.BeAttackStep1Color);
            yield return 0;
            ModelEffectManager.ShowSpriteColorEffect(GoCloth, ColorConst.BeAttackStep2Color);
            yield return 0;
            ModelEffectManager.ShowSpriteColorEffect(GoCloth, ColorConst.BeAttackStep2Color);
            yield return 0;
            ModelEffectManager.ShowSpriteColorEffect(GoCloth, ColorConst.RedEnd);
            while (_keepFrame < 8)
            {
                _keepFrame++;
                yield return 0;
            }
            yield return 0;
            _keepFrame = 0;
            ModelEffectManager.ShowSpriteColorEffect(GoCloth, ColorConst.BeAttackStep2Color);
            yield return 0;
            ModelEffectManager.ShowSpriteColorEffect(GoCloth, ColorConst.BeAttackStep2Color);
            yield return 0;
            ModelEffectManager.ShowSpriteColorEffect(GoCloth, ColorConst.BeAttackStep1Color);
            yield return 0;
            ModelEffectManager.ShowSpriteColorEffect(GoCloth, ColorConst.BeAttackStep1Color);
            yield return 0;
            ModelEffectManager.RemoveSpriteColorEffect(GoCloth);
            yield return 0;
        }

        private IEnumerator ShowModelColorEffect()
        {
            while (Time.time - _startTime < 0.6f)
            {
                var curColor = new Color
                {
                    a = Random.Range(_startColor.a, _endColor.a),
                    b = Random.Range(_startColor.b, _endColor.b),
                    r = Random.Range(_startColor.r, _endColor.r),
                    g = Random.Range(_startColor.g, _endColor.g)
                };
                if (GoCloth == null)
                {
                    break;
                }
                ModelEffectManager.ShowSpriteColorEffect(GoCloth, curColor);
                yield return 0;
            }
            ModelEffectManager.RemoveSpriteColorEffect(GoCloth);
            yield return 0;
        }

        public void EndModelColorEffect()
        {
            ModelEffectManager.RemoveSpriteColorEffect(GoCloth);
        }

        public void ShowWhiteColor()
        {
            ModelEffectManager.ShowPuleColor(GoCloth);
        }

        public void RemoveWhiteColor()
        {
            ModelEffectManager.RemovePuleColor(GoCloth);
        }

        public void ShowBornModelEffect()
        {
            _modelAlpha = 0.1f;
            vp_Timer.In(0, UpdateBornEffect, 3, 1f);
        }

        private void UpdateBornEffect()
        {
            _modelAlpha += 0.3f;
            ModelEffectManager.ChangeAlpha(GoCloth, _modelAlpha);
        }

        public void SetGraspedController(BaseControler bctroller)
        {
            if (bctroller != null)
            {
                bctroller.Me.GetMeVoByType<BaseRoleVo>().RateMoveSpeed = 0.5f;
            }
            else
            {
                if (_graspedController != null)
                {
                    _graspedController.Me.GetMeVoByType<BaseRoleVo>().RateMoveSpeed = 1f;
                }
            }
            _graspedController = bctroller;
        }

        /// <summary>
        /// 设置抓取物体; 速率会下降0.5, 并将bctroller放到自己对象下，使得可以和一起移动;
        /// </summary>
        /// <param name="bctroller"></param>
        public void SetGraspController(BaseControler bctroller)
        {
            if (bctroller != null)
            {
                bctroller.MoveController.StopMove();
                _grapParent = bctroller.gameObject.transform.parent;
                bctroller.gameObject.transform.parent = GoBase.transform;
                if (bctroller is ActionControler)
                {
                    (bctroller as ActionControler).CanMove = false;
                    (bctroller as ActionControler).CanChangeStatus = false;
                }
            }
            else
            {
                if (_grapController != null)
                {
                    if (_grapController is ActionControler)
                    {
                        _grapController.gameObject.transform.parent = _grapParent;
                        (_grapController as ActionControler).CanMove = true;
                        (_grapController as ActionControler).CanChangeStatus = true;
                    }
                }
            }
            _grapController = bctroller;
        }

        /// <summary>
        /// 是否抓取了什么东西;
        /// </summary>
        public bool IsGrasp
        {
            get
            {
                return _grapController != null;
            }
        }

        /// <summary>
        /// 获取抓取的物体;
        /// </summary>
        public BaseControler GrapController
        {
            get
            {
                return _grapController;
            }
        }

        /// <summary>
        /// 是否被抓取着;
        /// </summary>
        public bool IsGrasped
        {
            get
            {
                return _graspedController != null;
            }
        }


        //---------------------------------------------预警技能特效--------------------------------------

        public void StartShowWarningEffect(Color startColor, Color endColor, float timeLength)
        {
            _timeLength = timeLength;
            _warnStartTime = Time.time;
            _warnStartColor = startColor;
            _warnEndColor = endColor;
            CoroutineManager.StartCoroutine(ShowWarningEffect());
        }

        public IEnumerator ShowWarningEffect()
        {
            while (Time.time - _warnStartTime < _timeLength)
            {
                var curColor = new Color
                {
                    a = Random.Range(_warnStartColor.a, _warnEndColor.a),
                    b = Random.Range(_warnStartColor.b, _warnEndColor.b),
                    r = Random.Range(_warnStartColor.r, _warnEndColor.r),
                    g = Random.Range(_warnStartColor.g, _warnEndColor.g)
                };
                if (GoCloth == null)
                {
                    break;
                }
                ModelEffectManager.ShowSpriteColorEffect(GoCloth, curColor);
                yield return 0;
            }
            ModelEffectManager.RemoveSpriteColorEffect(GoCloth);
            yield return 0;
        }
    }
}