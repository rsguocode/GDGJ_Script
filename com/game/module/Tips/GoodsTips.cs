using Com.Game.Module.Tips;
using UnityEngine;
using System.Collections;
using com.game.module.test;
using com.game.manager;
using com.game.data;
using Com.Game.Module.Manager;
using PCustomDataType;
using Com.Game.Module.Role;
using com.game.utils;
using com.u3d.bases.debug;

namespace Com.Game.Module.Tips
{
    public class GoodsTips : BaseView<GoodsTips>
    {

        public readonly int GOODS_TYPE = 1; //背包样式
        public readonly int AUCTION_TYPE = 2; //商城样式

        public override string url
        {
            get { return "UI/Tips/GoodsTips/GoodsTipsView1.assetbundle"; }
        }

        public override ViewLayer layerType
        {
            get { return ViewLayer.HighLayer; }
        }

        private Button btn_left; //使用按钮
        public Button btn_right; //另一个按钮
        //private Button btn_dq; //丢弃按钮
        private Button btn_close; //关闭按钮
        private UIWidgetContainer background; //关闭按钮
        private UILabel goodsName; //物品名字
        private UILabel goodsLevel; //物品等级信息
        private UIWidgetContainer goodsIcon; //物品图标
        private UILabel descLabel1; //物品描述
        //private UILabel descLabel2;
        //private UILabel descLabel3;

        //private UILabel useLvl; //使用等级标签
        //private UILabel desBQ; //描述标签



        public UIWidgetContainer.ClickDelegate mLeftclickDelegate; //按钮事件委托
        public UIWidgetContainer.ClickDelegate mRightclickDelegate;

        public uint id;
        private PGoods goods;
        private string leftBtnLabel; //左边按钮文本
        private string rightBtnLabel; //右边按钮文本
        private TipsType tipsType;

        protected override void Init()
        {
            btn_left = FindInChild<Button>("info/btn_left");
            btn_right = FindInChild<Button>("info/btn_right");
            base.showTween = FindInChild<TweenPlay>("info");
            background = FindInChild<UIWidgetContainer>("background");
            background.clickDelegate = CloseView;
            btn_close = FindInChild<Button>("info/btn_close");
            btn_close.clickDelegate = CloseView;
            goodsIcon = FindInChild<UIWidgetContainer>("info/item");
            goodsName = FindInChild<UILabel>("info/name");
            goodsLevel = FindInChild<UILabel>("info/level");
            descLabel1 = FindInChild<UILabel>("info/attr1");

        }

        public void OpenViewById(uint id, string leftText, string rightText,
            UIWidgetContainer.ClickDelegate mLeftclickDelegate, UIWidgetContainer.ClickDelegate mRightclickDelegate,
            TipsType tipsType)
        {
            this.id = id;
            goods = Singleton<GoodsMode>.Instance.GetPGoodsById(id);
            this.tipsType = tipsType;
            this.mLeftclickDelegate = mLeftclickDelegate;
            //this.mMiddleclickDelegate = mMiddleclickDelegate;
            this.mRightclickDelegate = mRightclickDelegate;
            this.leftBtnLabel = leftText;
            this.rightBtnLabel = rightText;
            OpenView();
            //miaoShuList[1].text = vo.
            //jiBenSX.text = vo.
        }

        public void OpenViewByGoodsId(uint goodsId, string leftText, string rightText,
            UIWidgetContainer.ClickDelegate mLeftclickDelegate, UIWidgetContainer.ClickDelegate mRightclickDelegate,
            TipsType tipsType)
        {
            this.id = id;
            goods = new PGoods();
            goods.goodsId = goodsId;
            this.tipsType = tipsType;
            this.mLeftclickDelegate = mLeftclickDelegate;
            //this.mMiddleclickDelegate = mMiddleclickDelegate;
            this.mRightclickDelegate = mRightclickDelegate;
            this.leftBtnLabel = leftText;
            this.rightBtnLabel = rightText;
            OpenView();
        }

        protected override void HandleAfterOpenView()
        {
            SysItemVo vo = BaseDataMgr.instance.GetDataById<SysItemVo>(goods.goodsId);
            if (vo != null)
            {
                goodsName.text = vo.name;
                goodsLevel.text = "使用等级： " + vo.lvl;
                descLabel1.text = vo.desc;
                Singleton<ItemManager>.Instance.InitItem(goodsIcon, goods.goodsId, 0);
                SetType();
            }
        }

        private void SetType()
        {
            //按钮回调和位置调整 
            btn_right.FindInChild<UILabel>("label").text = rightBtnLabel; //文本显示
            btn_left.FindInChild<UILabel>("label").text = leftBtnLabel;
            btn_left.clickDelegate = mLeftclickDelegate; //响应事件指定
            bool isCenter = false;
            if (mLeftclickDelegate == null)
            {
                btn_left.SetActive(false);
                isCenter = true;
            }
            else
            {
                btn_left.SetActive(true);
                if (tipsType != TipsType.DELEGATENOCLOSE)
                    btn_left.clickDelegate += CloseView;
            }
            btn_right.clickDelegate = mRightclickDelegate;

            if (mRightclickDelegate == null)
            {
                btn_right.SetActive(false);
                isCenter = true;
            }
            else
            {
                btn_right.SetActive(true);
                if (tipsType != TipsType.DELEGATENOCLOSE)
                    btn_right.clickDelegate += CloseView;
            }
            if (isCenter) //调整位置
            {
                btn_left.transform.localPosition = new Vector3(-10f, -32f, 0f);
                btn_left.FindInChild<UISprite>("background").width = 280;
                btn_left.FindInChild<UISprite>("highlight").width = 298;
                btn_right.transform.localPosition = new Vector3(-10f, -32f, 0f);
                btn_right.FindInChild<UISprite>("background").width = 280;
                btn_right.FindInChild<UISprite>("highlight").width = 298;
            }
            else
            {
                btn_left.transform.localPosition = new Vector3(-98.5f, -32f, 0f);
                btn_left.FindInChild<UISprite>("background").width = 167;
                btn_left.FindInChild<UISprite>("highlight").width = 179;
                btn_right.transform.localPosition = new Vector3(78.5f, -32f, 0f);
                btn_right.FindInChild<UISprite>("background").width = 167;
                btn_right.FindInChild<UISprite>("highlight").width = 179;
            }
            NGUITools.AddWidgetCollider(btn_left.gameObject);
            NGUITools.AddWidgetCollider(btn_right.gameObject);
        }
    }
}


