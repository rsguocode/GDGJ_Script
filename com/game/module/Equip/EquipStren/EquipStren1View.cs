using System.Collections.Generic;
using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Manager;
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
    public class EquipStren1View : BaseView<EquipStren1View>
    {
        public override ViewLayer layerType
        {
            get { return ViewLayer.NoneLayer; }
        }

        private List<UILabel> attrList = new List<UILabel>();

        private UILabel successValue;//成功率
        //private UILabel successLabel;
        private UILabel costValue;//花费金币
        private UILabel costLabel;
        private UILabel leftLabel;
        private UILabel leftValue;
         
        //装下信息
        private UILabel equipName;
        //private UILabel equipStren;
        private ItemContainer equipIcon;
        public Button strenButton ;//强化按钮

        private uint uid = 0;//装备的唯一id;
        private int repos;//存放位置
        private string strenRate;//保存原始的概率
        private int cost;//花费金币

        private GameObject tips;//强化满级提示
        private GameObject bottom;//强化满级隐藏

        protected override void Init()
        {
            bottom = FindChild("bottom");
            tips = FindChild("tips");
            strenButton = FindInChild<Button>("bottom/btn_qh");
            strenButton.clickDelegate = StrenClick;

            costValue = FindInChild<UILabel>("bottom/costvalue");
            costLabel = FindInChild<UILabel>("bottom/costlabel");
            leftLabel = FindInChild<UILabel>("bottom/leftlabel");
            leftValue = FindInChild<UILabel>("bottom/leftvalue");
            successValue = FindInChild<UILabel>("bottom/successvalue");
            //successLabel = FindInChild<UILabel>("bottom/successlabel");

            for (int i = 1; i < 4; i++)
            {
                attrList.Add(FindInChild<UILabel>("attr"+i));
            }
            equipName = FindInChild<UILabel>("title");
            equipIcon = FindChild("item").AddMissingComponent<ItemContainer>();
            equipIcon.isEnabled = false;
        }

        protected override void HandleAfterOpenView()
        {
            ClearItemData();
            UpdateStrenRateHandle();
            Singleton<EquipLeftView>.Instance.ItemClickCallBack =
                this.LeftViewClick;
            EquipLeftView.Instance.NoEquipHandle = NoEquipHandle;
            EquipLeftView.Instance.ReposChange(GoodsMode.Instance.EQUIP_REPOS);
            UpdateMoney();
            
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
            Equip1Mode.Instance.dataUpdated += UpdateStrenRate;
            RoleMode.Instance.dataUpdated += UpdateMoneyHandle;
        }
        //更新强化概率
        private void UpdateStrenRate(object sender, int code)
        {
            if (code == Equip1Mode.Instance.UPDATE_STREN_RATE)
                UpdateStrenRateHandle();
            if (code == Equip1Mode.Instance.UPDATE_EQUIP_STREN)
            {
                //UpdateStrenRateHandle();
                if (Equip1Mode.Instance.StrenCode != 0)
                {
                    Singleton<Equip1Control>.Instance.EquipRate(this.uid, Singleton<EquipLeftView>.Instance.Repos);
                }
                else
                {
                    UpdateStrenRateHandle();
                }
                
            }
        }

        private void UpdateStrenRateHandle()
        {
            if (this.uid != 0 && this.uid == Equip1Mode.Instance.IdPro)
            {
                successValue.text = string.Format(LanguageManager.GetWord("Equip.StrenRate"), strenRate,
                    "+" + Equip1Mode.Instance.Rate + "% ");
            }
            else
                successValue.text = string.Empty;
        }
        private void UpdateEquipHandle(object sender, int code)
        {
            if (code == GoodsMode.Instance.UPDATE_EQUIP)
            {
                
            }
        }


        private void UpdateMoneyHandle(object sender,int code)
        {
            if (code == RoleMode.Instance.UPDATE_FORTUNE)
                UpdateMoney();
        }
        public override void CancelUpdateHandler()
        {
            GoodsMode.Instance.dataUpdated -= UpdateEquipHandle;
            Equip1Mode.Instance.dataUpdated -= UpdateStrenRate;
            RoleMode.Instance.dataUpdated -= UpdateMoneyHandle;
        }

        private void UpdateMoney()
        {
			leftValue.text = MeVo.instance.DiamStr;
        }
        private void StrenClick()
        {
            if(uid == 0)
                MessageManager.Show("请先选择强化装备");
            else
                Singleton<Equip1Control>.Instance.EquipStren(uid,repos);
        }
        public void LeftViewClick(uint uuid)
        {
            if(this.uid != uuid)
                Singleton<Equip1Control>.Instance.EquipRate(uuid,Singleton<EquipLeftView>.Instance.Repos);
            this.uid = uuid;
            this.repos = Singleton<EquipLeftView>.Instance.Repos;
            PGoods currentGoods = Singleton<GoodsMode>.Instance.GetPGoodsById(uid);
            cost = 0;
            //初始化图标
            ItemManager.Instance.InitItem(equipIcon, currentGoods.goodsId, 0);
            int stren = 0;
            stren = currentGoods.equip[0].stren;
            if (stren == 100)
            {
                MessageManager.Show("已经强化到最高等级");
                if(bottom.activeInHierarchy)
                    bottom.SetActive(false);
                if(!tips.activeInHierarchy)
                    tips.SetActive(true);
                //return;
            }
            else
            {
                if (!bottom.activeInHierarchy)
                    bottom.SetActive(true);
                if (tips.activeInHierarchy)
                    tips.SetActive(false);
                SysStrenthVo sv = BaseDataMgr.instance.GetDataById<SysStrenthVo>((byte)(currentGoods.equip[0].stren + (byte)1));
                if (sv != null)
                {
                    costValue.text = string.Empty + sv.consume;
                    cost = sv.consume;
                    strenRate = string.Empty + (int) sv.success/10000f*100 + "%";
                    UpdateStrenRateHandle();
                }
            }
            
            //equipStren.text = "+" + stren;
            SysEquipVo vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(currentGoods.goodsId);  //先枚举强化属性显示
            equipName.text = vo.name + "  "+ ColorConst.YELLOW + "+" +stren + "[-]";
            string[] strenType = StringUtils.SplitVoString(vo.stren_type);
            List<string> strs = new List<string>();
            foreach (string s in strenType)
            {
                strs.AddRange(StringUtils.GetValueListFromString(vo.GetEquipStrenType( int.Parse(s))));
            }
            for (int i = 0; i < 3; i++)
            {
                if (i < strenType.Length)
                {
                    string key = LanguageManager.GetWord("Equip.StrenAttr" + int.Parse(strenType[i]));
                    int addValue = int.Parse(strs[i * 2 + 1]) ;
                    int strenValue = int.Parse(strs[i * 2]) + addValue *stren;
                    if (stren != 100)
                    {
                        attrList[i].text = string.Format(key, strenValue, string.Format(ColorConst.GREEN_FORMAT, "+" + addValue));
                    }
                    else
                    {
                        attrList[i].text = key + ": " + strenValue ;
                    }
                }
                else
                {
                    attrList[i].text = string.Empty;
                }
            }
        }

        private void ClearItemData()
        {
            this.uid = 0;
            for (int i = 0; i < 3; i++)
            {
                attrList[i].text = string.Empty;
            }
            ItemManager.Instance.InitItem(equipIcon,ItemManager.Instance.EMPTY_ICON,0);
            costValue.text = "0";
            //equipStren.text = "+0";
            strenRate = "+0%";
            successValue.text = "0%( +0%)";
            equipName.text = "选择装备";

        }
       
    }

}

