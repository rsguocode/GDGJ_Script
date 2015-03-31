using System.Collections.Generic;
using System.Net.Sockets;
using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Manager;
using Com.Game.Module.Medal;
using Com.Game.Module.Role;
using com.game.module.test;
using com.game.Public.Message;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.debug;
using PCustomDataType;
using UnityEngine;
using System.Collections;

namespace Com.Game.Module.Equip
{
    public class EquipRefineView : BaseView<EquipRefineView>
    {
        public override ViewLayer layerType
        {
            get { return ViewLayer.NoneLayer; }
        }

        public Button btn_Refine;//精炼按钮
        //private ItemContainer item;//当前装备

        private List<UILabel> attrList = new List<UILabel>();//当前属性

        private List<UISprite> refineStar = new List<UISprite>();//精炼星星

        private UILabel equipName;//装备名字
        //private UILabel equipStren;//装备强化
        private UILabel costValue;//花费金币
        private UILabel costLabel;
        private UILabel leftLabel;
        private UILabel leftValue;

        private UILabel refineStone;//精炼石数量
        private GameObject refineIcon;//精炼石图标
        private UILabel refineLvl;//精炼等级

        private ItemContainer equipIcon;//装备图标
        

        private uint uid = 0;//装备的唯一id;
        private int repos;//存放位置

        private int refineInBag;// 精炼石数量
        private int refineCost;

        protected override void Init()
        {
            btn_Refine = FindInChild<Button>("btn_qh");
            btn_Refine.clickDelegate = RefineOnClick;
            //item = FindChild("item").AddMissingComponent<ItemContainer>();
            //item.clickDelegate = ItemClick;
            for (int i = 1; i < 4; i++)
            {
                attrList.Add(FindInChild<UILabel>("attr"+i));
            }
            equipName = FindInChild<UILabel>("title");
            //equipStren = FindInChild<UILabel>("stren");
            costValue = FindInChild<UILabel>("costvalue");
            costLabel = FindInChild<UILabel>("costlabel");
            leftLabel = FindInChild<UILabel>("leftlabel");
            leftValue = FindInChild<UILabel>("leftvalue");

            refineStone = FindInChild<UILabel>("refinenum");
            refineLvl = FindInChild<UILabel>("refinelvl");
            refineIcon = FindChild("refine");
            ItemManager.Instance.InitItem(refineIcon,GoodsMode.Instance.RefineGoodsId,0);

            equipIcon = FindChild("item").AddMissingComponent<ItemContainer>();
            equipIcon.buttonType = Button.ButtonType.None;
            for (int i = 1; i < 16; i++)
            {
                refineStar.Add(FindInChild<UISprite>("xx/xx"+i));
            }
        }

        protected override void  HandleAfterOpenView()
        {
            ClearItemData();

            Singleton<EquipLeftView>.Instance.ItemClickCallBack = LeftViewClick;
            EquipLeftView.Instance.NoEquipHandle = NoEquipHandle;
            EquipLeftView.Instance.ReposChange(GoodsMode.Instance.EQUIP_REPOS);
            
        }

        private void NoEquipHandle()
        {
            this.CloseView();
            Equip1View.Instance.RightTips.SetActive(true);
            Equip1View.Instance.TipsSprite.text = "请先穿戴装备";
        }
        public override void RegisterUpdateHandler()
        {
            GoodsMode.Instance.dataUpdated += UpdateEquipHandle;
            RoleMode.Instance.dataUpdated += UpdateMoneyHandle;
        }

        private void UpdateEquipHandle(object sender, int code)
        {
            if (code == GoodsMode.Instance.UPDATE_EQUIP)
                UpdateEquipStrenInfo();
            else if (code == GoodsMode.Instance.UPDATE_GOODS)
            {
                refineInBag = GoodsMode.Instance.GetRefineGoodsCount();
                UpdateRefineCost();
            }
        }
        private void UpdateMoneyHandle(object sender, int code)
        {
            if (code == RoleMode.Instance.UPDATE_FORTUNE)
                UpdateMoney();
        }

        private void UpdateMoney()
        {
			leftValue.text = MeVo.instance.DiamStr;
        }
        private void UpdateEquipStrenInfo()
        {
            //LeftViewClick(uid);
        }
        public override void CancelUpdateHandler()
        {
            GoodsMode.Instance.dataUpdated -= UpdateEquipHandle;
            RoleMode.Instance.dataUpdated -= UpdateMoneyHandle;
        }

        public void LeftViewClick(uint uid)
        {
            this.uid = uid;
            this.repos = Singleton<EquipLeftView>.Instance.Repos;
            PGoods currentGoods = Singleton<GoodsMode>.Instance.GetPGoodsById(uid);
           
            SysRefineVo sv = BaseDataMgr.instance.GetDataById<SysRefineVo>((uint)(currentGoods.equip[0].refine ));
            if (sv != null)
            {
                costValue.text = string.Empty + sv.money;
                refineCost = int.Parse(sv.goods);
				//refineStone.text = sv.goods;//精炼石数量
            }
            UpdateRefineCost();

            ItemManager.Instance.InitItem(equipIcon,currentGoods.goodsId,0);
            int stren = 0;
            //if(currentGoods.equip.Count != 0)
            stren = currentGoods.equip[0].stren;
            int refine = currentGoods.equip[0].refine;
            refineLvl.text = refine + "/15";
            //equipStren.text = "+" + stren ;
            //item2.FindInChild<UILabel>("label").text = "+" + (stren + 1);
            SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(currentGoods.goodsId);  //先枚举强化属性显示
            equipName.text = vo.name + "  " +string.Format( ColorConst.YELLOW_FORMAT , "+" + stren );
            string[] strenType = StringUtils.SplitVoString(vo.stren_type);
            List<string> strs = new List<string>();
            foreach (string s in strenType)
            {
                strs.AddRange(StringUtils.GetValueListFromString(vo.GetEquipStrenType(int.Parse(s))));
            }
            //Log.info(this, Singleton<EquipMode>.Instance.GetEquipStrenType(vo, vo.stren_type) + "  " + vo.stren_type);
            
            for (int i = 0; i < 3; i++)
            {
                if (i < strenType.Length)
                {
                    int addValue = int.Parse(strs[i*2 + 1]);
                    int strenValue = int.Parse(strs[i * 2]) + addValue * stren;
                    if (sv.value == 0)
                    {
                        attrList[i].text = VoUtils.RoleAttrFormat1(int.Parse(strenType[i]), string.Empty +strenValue);
                    }
                    else
                        attrList[i].text = VoUtils.RoleAttrFormat1(int.Parse(strenType[i]), strenValue +
                           string.Format(ColorConst.GREEN_FORMAT, "  +" + Mathf.RoundToInt(strenValue * sv.GetRefineRate()) + "( " + string.Format("{0:0%}", sv.GetRefineRate()) + " )"));
                }
                else
                {
                    attrList[i].text = string.Empty;
                }
            }
            
            for (int i = 0; i < 15; i++)
            {
                if (refine > i)
                {
                    refineStar[i].spriteName = "xingxing1";
                }
                else
                    refineStar[i].spriteName = "kongxing";
            }


        }

        private void ClearItemData()
        {
            this.uid = 0;
            for (int i = 0; i < 3; i++)
            {
                attrList[i].text = string.Empty;
            }
            //ItemManager.Instance.InitItem(item, ItemManager.Instance.ADD_ICON, 0);
            costValue.text = "0";
            //moneyValue.text = "0";
            int refine = 0;
            for (int i = 0; i < 15; i++)
            {
                refineStar[i].spriteName = "kongxing";
            }
            UpdateMoney();
            equipName.text = "选择装备";
            //equipStren.text = "+0";
            ItemManager.Instance.InitItem(equipIcon,ItemManager.Instance.EMPTY_ICON,0);
            refineLvl.text = "0/15";
            //refineIcon.spriteName = "";
            refineCost = 0;
            refineInBag = GoodsMode.Instance.GetRefineGoodsCount();
            UpdateRefineCost();
        }

        private void UpdateRefineCost()
        {
            refineStone.text = refineInBag + "/" + refineCost;
            if (refineCost <= refineInBag)
                refineStone.color = Color.green;
            else
            {
                refineStone.color = Color.red;
            }
        }
        private void RefineOnClick()
		{
            if (uid == 0)
                MessageManager.Show("请先选择精炼装备");
            else
                Singleton<Equip1Control>.Instance.EquipRefine(uid, repos);
		}

    }

}
