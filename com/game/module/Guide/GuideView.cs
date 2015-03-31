using com.game.data;
using Com.Game.Module.Copy;
using com.game.module.Guide.GuideLogic;
using com.game.module.NpcDialog;
using com.game.module.test;
using com.u3d.bases.debug;
using Holoville.HOTween;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/02/14 01:11:41 
 * function: 指引面板
 * *******************************************************/

namespace com.game.module.Guide
{
    public enum GuideShowType
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum FunctionOpenType
    {
        DownLow = 1, //底部下排图标开启
        DownHight = 2, //底部上排图标开启
        Up = 3, //顶部图标开启
        Normal = 4 //普通功能指引，非图标开启
    }

    public class GuideView : BaseView<GuideView>
    {
        private GameObject _currentGuideArrow; //当前指引用到的指引箭头
        private GameObject _downGuideArrow; //向下的指引箭头
        private UILabel _functionOpenInfoLabel; //功能开启信息提示文本
        private GameObject _guideEffect;
        private Transform _guideItem; //被指引的对象
        private int _guideItemOriginalLayer; //被指引对象的原来的层次;
        private int _guideItemOriginalPanelDepth; //原始的层次
        private Transform _guideItemOriginalParentTransform; //被指引对象的原来的父节点;
        private GuideShowType _guideShowType; //当前的指引箭头类型
        private string _guideTip; //当前指引的说明文字
        private bool _isFlyButton; //功能开启按钮飘动指引
        private GameObject _leftGuideArrow; //向左的指引箭头
        private bool _needGuideOpen; //是否需要指引点开
        private Transform _parentTransform; //父节点
        private GameObject _rightGuideArrow; //向右的指引箭头
        private Vector3 _targetPosition; //功能开启按钮的目标位置
        private GameObject _upGuideArrow; //向上的指引箭头


        public override string url
        {
            get { return "UI/Guide/GuideView.assetbundle"; }
        }

        public override ViewLayer layerType
        {
            get { return ViewLayer.NoneLayer; }
        }

        public override bool waiting
        {
            get { return false; }
        }

        public override bool playClosedSound
        {
            get { return false; }
        }

        protected override void Init()
        {
            _parentTransform = FindChild("GuideParent").transform;
            _functionOpenInfoLabel = FindInChild<UILabel>("Buttom/FunctionOpenInfoLabel");
            _leftGuideArrow = FindChild("GuideArrow/LeftGuide");
            _rightGuideArrow = FindChild("GuideArrow/RightGuide");
            _upGuideArrow = FindChild("GuideArrow/UpGuide");
            _downGuideArrow = FindChild("GuideArrow/DownGuide");
            _guideEffect = FindChild("GuideEffect");
            _leftGuideArrow.SetActive(false);
            _rightGuideArrow.SetActive(false);
            _upGuideArrow.SetActive(false);
            _downGuideArrow.SetActive(false);
            _functionOpenInfoLabel.text = "";
            SetToLayerTopUI();
        }

        //设置对象层级
        private void SetToLayerTopUI()
        {
            NGUITools.SetLayer(gameObject, LayerMask.NameToLayer("TopUI"));
        }

        /// <summary>
        ///     显示箭头朝右的指引
        /// </summary>
        /// <param name="guideItem">被指引对象</param>
        /// <param name="guideTip">指引时的说明文字</param>
        public void OpenRightGuide(Transform guideItem, string guideTip)
        {
            _guideShowType = GuideShowType.Right;
            OpenGuide(guideItem, guideTip);
        }

        /// <summary>
        ///     显示箭头朝左的指引
        /// </summary>
        /// <param name="guideItem">被指引对象</param>
        /// <param name="guideTip">指引时的说明文字</param>
        public void OpenLeftGuide(Transform guideItem, string guideTip)
        {
            _guideShowType = GuideShowType.Left;
            OpenGuide(guideItem, guideTip);
        }

        /// <summary>
        ///     显示箭头朝上的指引
        /// </summary>
        /// <param name="guideItem">被指引对象</param>
        /// <param name="guideTip">指引时的说明文字</param>
        public void OpenUpGuide(Transform guideItem, string guideTip)
        {
            _guideShowType = GuideShowType.Up;
            OpenGuide(guideItem, guideTip);
        }

        /// <summary>
        ///     显示箭头朝下的指引
        /// </summary>
        /// <param name="guideItem">被指引对象</param>
        /// <param name="guideTip">指引时的说明文字</param>
        public void OpenDownGuide(Transform guideItem, string guideTip)
        {
            _guideShowType = GuideShowType.Down;
            OpenGuide(guideItem, guideTip);
        }

        private void OpenGuide(Transform guideItem, string guideTip)
        {
            if (GuideModel.Instance.IsShowGuide) return;
            _guideItem = guideItem;
            _guideTip = guideTip;
            OpenView();
        }

        /// <summary>
        ///     功能开启，按钮飘起效果
        /// </summary>
        /// <param name="guideItem">被指引对象</param>
        /// <param name="guideTip">指引说明文字</param>
        /// <param name="needGuideOpen">是否需要指引开启</param>
        public void OpenFlyButtonGuide(Transform guideItem, string guideTip, bool needGuideOpen = true)
        {
            if (GuideModel.Instance.IsShowGuide) return;
            _isFlyButton = true;
            _guideItem = guideItem;
            _targetPosition = guideItem.position;
            _guideTip = guideTip;
            _needGuideOpen = needGuideOpen;
            SysGuideVo sysGuideVo = GuideManager.Instance.CurrentTriggeredGuideVo;
            if (sysGuideVo == null)
            {
                return;
            }
            if (sysGuideVo.guide_type == (int) FunctionOpenType.Up)
            {
                _guideShowType = GuideShowType.Up;
            }
            else
            {
                _guideShowType = GuideShowType.Down;
            }
            OpenView();
        }

        /// <summary>
        ///     显示功能开启的图标飞行效果
        /// </summary>
        private void ShowButtonFly()
        {
            _isFlyButton = false;
            _guideItem.position = Vector3.zero;
            _guideItemOriginalParentTransform = _guideItem.parent.transform;
            _guideItemOriginalLayer = _guideItem.gameObject.layer;
            _guideItem.parent = _parentTransform;
            NGUITools.SetLayer(_parentTransform.gameObject, LayerMask.NameToLayer("TopUI"));
            _guideItem.GetComponent<Button>().background.SetActive(true);
            SetGuideItemVisiable();
            _functionOpenInfoLabel.text = GuideManager.Instance.CurrentTriggeredGuideVo.guide_describe;
            Vector3 scale = _guideItem.localScale;
            //图标动画序列
            var sequence = new Sequence(new SequenceParms().Loops(1, LoopType.Yoyo));
            sequence.Append(HOTween.To(_guideItem, 0.8f,
                new TweenParms().Prop("localScale", new Vector3(scale.x*2, scale.y*2, scale.z*2f), false)));
            sequence.Append(HOTween.To(_guideItem, 0.8f,
                new TweenParms().Prop("localScale", scale, false).Delay(0.8f)));
            sequence.Append(HOTween.To(_guideItem, 1.5f,
                new TweenParms().Prop("position", _targetPosition, false).OnComplete(ButtonFlyCallBack)));
            sequence.Play();
        }

        private void SetGuideItemVisiable()
        {
            _guideItem.gameObject.SetActive(true);
            int count = _guideItem.childCount;
            for (int i = 0; i < count; i++)
            {
                if (_guideItem.GetChild(i).name != "tips")
                {
                    _guideItem.GetChild(i).gameObject.SetActive(true);
                }
            }
        }

        /// <summary>
        ///     图标飞行到了目标位置时的回调处理
        /// </summary>
        private void ButtonFlyCallBack()
        {
            if (_needGuideOpen)
            {
                _parentTransform.gameObject.SetActive(false);
                _parentTransform.gameObject.SetActive(true);
                _functionOpenInfoLabel.text = "";
                ShowGuideArrow();
            }
            else
            {
                CloseView();
            }
        }

        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            if (_isFlyButton)
            {
                ShowButtonFly();
            }
            else
            {
                _guideItemOriginalParentTransform = _guideItem.parent.transform;
                _guideItemOriginalLayer = _guideItem.gameObject.layer;
                _guideItem.parent = _parentTransform;
                var panel = _guideItem.GetComponent<UIPanel>();
                if (panel != null)
                {
                    _guideItemOriginalPanelDepth = panel.depth;
                    panel.depth = 2005;
                }
                else
                {
                    _guideItemOriginalPanelDepth = 0;
                }
                NGUITools.SetLayer(_parentTransform.gameObject, LayerMask.NameToLayer("TopUI"));
                ShowGuideArrow();
            }
            _parentTransform.gameObject.SetActive(false);
            _parentTransform.gameObject.SetActive(true);
            SetPanelDepth();
            if (NpcDialogView.Instance.gameObject!=null&&
                NpcDialogView.Instance.gameObject.activeInHierarchy)
            {
                NpcDialogView.Instance.CloseView();
            }
            if (CopyPointView.Instance.gameObject!=null&&
                CopyPointView.Instance.gameObject.activeInHierarchy &&
                GuideManager.Instance.CurrentGuideType != GuideType.GuideCopy)
            {
                //CopyPointView.Instance.CloseView();
                CopyView.Instance.CloseView();
            }
        }

        protected override void HandleBeforeCloseView()
        {
            base.HandleBeforeCloseView();
            var panel = _guideItem.GetComponent<UIPanel>();
            if (panel != null)
            {
                panel.depth = _guideItemOriginalPanelDepth;
                _guideItemOriginalPanelDepth = 0;
            }
            _guideItem.parent = _guideItemOriginalParentTransform;
            _guideItem.gameObject.SetActive(false);
            _guideItem.gameObject.SetActive(true);
            NGUITools.SetLayer(_guideItem.gameObject, _guideItemOriginalLayer);
            _guideItem = null;
            _guideItemOriginalParentTransform = null;
            _leftGuideArrow.SetActive(false);
            _rightGuideArrow.SetActive(false);
            _upGuideArrow.SetActive(false);
            _downGuideArrow.SetActive(false);
            GuideModel.Instance.IsShowGuide = false;
        }

        /// <summary>
        ///     在打开指引面板前先设置指引状态
        /// </summary>
        public override void OpenView()
        {
            GuideModel.Instance.IsShowGuide = true;
            base.OpenView();
        }

        /// <summary>
        ///     根据箭头类型和被指引对象的位置来显示指引箭头和箭头上的说明文字
        /// </summary>
        private void ShowGuideArrow()
        {
            if (_guideItem == null)
            {
                return;
            }
            Vector3 pos = _guideItem.transform.position;
            var collider = _guideItem.GetComponent<BoxCollider>();
            if (collider == null)
            {
                Log.error(this, "被指引的对象需要有BoxCollider组件，来表示这个组件的宽高，便于指引箭头准确的计算指引位置");
                return;
            }
            Vector3 scale = _guideItem.transform.lossyScale;
            pos.x += collider.center.x*scale.x;
            pos.y += collider.center.y*scale.y;
            _guideEffect.transform.position = pos;
            switch (_guideShowType)
            {
                case GuideShowType.Left:
                    _currentGuideArrow = _leftGuideArrow;
                    pos.x += collider.size.x/2*scale.x;
                    break;
                case GuideShowType.Right:
                    _currentGuideArrow = _rightGuideArrow;
                    pos.x -= collider.size.x/2*scale.x;
                    break;
                case GuideShowType.Up:
                    _currentGuideArrow = _upGuideArrow;
                    pos.y -= collider.size.y/2*scale.y;
                    break;
                case GuideShowType.Down:
                    _currentGuideArrow = _downGuideArrow;
                    pos.y += collider.size.y/2*scale.y;
                    break;
            }
            if (_currentGuideArrow != null)
            {
                _currentGuideArrow.transform.position = pos;
                _currentGuideArrow.transform.FindChild("Arrow/Content/Label").GetComponent<UILabel>().text = _guideTip;
                _currentGuideArrow.SetActive(true);
            }
        }

        public void SetPanelDepth()
        {
            var panel = gameObject.GetComponent<UIPanel>();
            panel.depth = 2000;
        }
    }
}