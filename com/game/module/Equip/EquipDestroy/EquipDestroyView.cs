using System.Collections.Generic;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Manager;
using Com.Game.Module.Role;
using com.game.module.test;
using com.game.Public.Message;
using com.game.vo;
using PCustomDataType;
using UnityEngine;
using System.Collections;

namespace Com.Game.Module.Equip
{
    public class EquipDestroyView : BaseView<EquipDestroyView>
    {
        public override ViewLayer layerType
        {
            get { return ViewLayer.NoneLayer; }
        }
        private List<ItemContainer> itemList = new List<ItemContainer>();

        //private UILabel tips;//提示
        private UILabel refineLabel;//获得精炼石

        private int current = 0;
        private Button refineButton;//精炼按钮

        private UILabel refineNum ;//精炼石数量

        private GameObject refineIcon; //精炼石图标

        private int refineStone;//精炼石数量
         
        private List<uint> idList = new List<uint>();  //销毁装备列表

        protected override void Init()
        {
            for (int i = 1; i < 10; i ++)
            {
                ItemContainer ic = FindChild("item" + i).AddMissingComponent<ItemContainer>();
                ic.onClick = ItemDestroy;
                itemList.Add( ic);
            }
            refineLabel = FindInChild<UILabel>("refinelabel");
            refineNum = FindInChild<UILabel>("refinenum");

            refineIcon = FindChild("refine");
            ItemManager.Instance.InitItem(refineIcon, GoodsMode.Instance.RefineGoodsId, 0);

            refineButton = FindInChild<Button>("btn_qh");
            refineButton.clickDelegate = DestroyClick;
            
        }

        protected override void  HandleAfterOpenView()
        {
            InitItemData();
            Singleton<EquipLeftView>.Instance.ItemClickCallBack = LeftViewClick;
            EquipLeftView.Instance.NoEquipHandle = null;
            EquipLeftView.Instance.ReposChange(GoodsMode.Instance.GOODS_REPOS);
        }
        protected override void HandleBeforeCloseView()
        {
            //EquipLeftView.Instance.Id = 0;
        }
        

        public override void RegisterUpdateHandler()
        {
            Equip1Mode.Instance.dataUpdated += GoodsUpdateHandle;
        }
        public override void CancelUpdateHandler()
        {
            Equip1Mode.Instance.dataUpdated -= GoodsUpdateHandle;
        }

        private void GoodsUpdateHandle(object sender, int code)
        {
            if (code == Equip1Mode.Instance.UPDATE_EQUIP_DESTORY)
            {
                InitItemData();
            }

        }

        private void InitItemData()
        {
            idList.Clear();
            foreach (ItemContainer ic in itemList)
            {
                Singleton<ItemManager>.Instance.InitItem(ic, ItemManager.Instance.EMPTY_ICON, 0);
                ic.FindInChild<UISprite>("tips").SetActive(false);
                ic.Id = 0;
            }
            refineStone = 0;
            refineNum.text = "x 0";
        }
        public void LeftViewClick(uint uid )
        {
            
            if(idList.Contains(uid) )
				return;
            foreach (ItemContainer ic in itemList)
            {
                if (ic.Id == 0)
                {
                    idList.Add(uid);
                    PGoods goods = Singleton<GoodsMode>.Instance.GetPGoodsById(uid);
                    SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(uid);
                    Singleton<ItemManager>.Instance.InitItem(ic,goods.goodsId,0);
                    ic.FindInChild<UISprite>("tips").SetActive(true);
                    ic.Id = uid;
                    refineStone +=Singleton<GoodsMode>.Instance.GetDestroyCount(uid);
                    refineNum.text = "x "+ refineStone;
                    break;
                }
            }
        }
        private void DestroyClick()
        {
            if (idList.Count == 0)
            {
                MessageManager.Show("请先选择分解装备");
                return;
            }
            Singleton<Equip1Control>.Instance.EquipDestroy(idList);
        }
        private void ItemDestroy(GameObject go)
        {
            ItemContainer ic = go.GetComponent<ItemContainer>();
            if (ic.Id != 0)
            {
                Singleton<ItemManager>.Instance.InitItem(ic, ItemManager.Instance.EMPTY_ICON, 0);
                refineStone -= Singleton<GoodsMode>.Instance.GetDestroyCount(ic.Id);
                refineNum.text = "x " + refineStone;
				idList.Remove(ic.Id);
                EquipLeftView.Instance.SetItemActive(ic.Id, false);
                ic.Id = 0;
                ic.FindInChild<UISprite>("tips").SetActive(false);
                EquipLeftView.Instance.SetItemActive(ic.Id,false);
                //EquipLeftView.Instance.CancelCurrentSelect();
                
            }
            else
            {
                //EquipLeftView.Instance.ReposChange(GoodsMode.Instance.EQUIP_REPOS);
            }
            
        }


    }

}

