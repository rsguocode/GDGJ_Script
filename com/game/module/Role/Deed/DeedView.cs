using System.Collections.Generic;
using com.game.manager;
using com.game.module.test;
using com.game.utils;
using com.game.vo;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2013/12/31 03:49:08 
 * function: 契约功能
 * *******************************************************/

namespace Com.Game.Module.Role.Deed
{
    public class DeedView : BaseView<DeedView>
    {
		public override ViewLayer layerType {
			get {
				return ViewLayer.NoneLayer;
			}
		}
        private Button _btnStrength;             //力量契约按钮
        private Button _btnPhysique;             //体力契约按钮
        private Button _btnAgility;              //敏捷契约按钮
        private Button _btnIntelligence;         //智力契约按钮
        private UILabel _labelStrength;          //力量契约文本
        private UILabel _labelPhysique;          //体力契约文本
        private UILabel _labelAgility;           //敏捷契约文本
        private UILabel _labelIntelligence;      //智力契约文本
        private GameObject _itemParent;          //契约Item的挂载点
        private GameObject _deedItem;            //契约Item模版
        private List<List<DeedItem>> _deedItems; //契约模版集合
        private DeedMode _deedMode;              //契约数据中心
        private List<Button> buttons;            //按钮的集合，便于程序的处理
        private UILabel _labelSelectedInfo;      //被选中的契约的信息

        public new void Init()
        {
            _btnStrength = FindChild("Center/Buttons/BtnStrength").GetComponent<Button>();
            _btnPhysique = FindChild("Center/Buttons/BtnPhysique").GetComponent<Button>();
            _btnAgility = FindChild("Center/Buttons/BtnAgility").GetComponent<Button>();
            _btnIntelligence = FindChild("Center/Buttons/BtnIntelligence").GetComponent<Button>();
            _labelStrength = FindChild("Center/Buttons/BtnStrength/LabelStrength").GetComponent<UILabel>();
            _labelPhysique = FindChild("Center/Buttons/BtnPhysique/LabelPhysique").GetComponent<UILabel>();
            _labelAgility = FindChild("Center/Buttons/BtnAgility/LabelAgility").GetComponent<UILabel>();
            _labelIntelligence = FindChild("Center/Buttons/BtnIntelligence/LabelIntelligence").GetComponent<UILabel>();
            _labelSelectedInfo = FindChild("Center/ItemInfoLabel").GetComponent<UILabel>();
            _itemParent = FindChild("Center/Items");
            _deedItem = FindChild("Center/DeedItem");
            DeedItemPool.Instance.Init(_deedItem.transform);
            _deedMode = Singleton<DeedMode>.Instance;
            buttons = new List<Button>{null,_btnStrength,_btnPhysique,_btnAgility,_btnIntelligence};
            _btnStrength.onClick += OnDeedTypeBtnClick;
            _btnPhysique.onClick += OnDeedTypeBtnClick;
            _btnAgility.onClick += OnDeedTypeBtnClick;
            _btnIntelligence.onClick += OnDeedTypeBtnClick;
            _labelSelectedInfo.text = "";
            _deedMode.TypeStrings = new string[]
            {
                LanguageManager.GetWord("Equip.Str"),
                LanguageManager.GetWord("Equip.Phy"),
                LanguageManager.GetWord("Equip.Agi"),
                LanguageManager.GetWord("Equip.Wit")
            };
        }

        private void OnDeedTypeBtnClick(GameObject gameObject)
        {
            int type = buttons.IndexOf(gameObject.GetComponent<Button>());
            _deedMode.CurrentSelectedDeedItem = null;
            SetData(type);
        }



        /*public override void OpenView()
        {
            gameObject.SetActive(true);
            RegisterUpdateHandler();
            HandleAfterOpenView();
        }

        public override void CloseView()
        {
            gameObject.SetActive(false);
            HandleBeforeCloseView();
            CancelUpdateHandler();
        }*/

        public override void RegisterUpdateHandler()
        {
            _deedMode.dataUpdated += OnDeedInfoUpdate;
        }

        private void OnDeedInfoUpdate(object sender, int code)
        {
            switch (code)
            {
                case DeedMode.EventUpdateDeedInfo:
                    SetDeedInfo(_deedMode.CurrentDeedType);
                    break;
                case DeedMode.EventUpdateSelectedDeedItem:
                    UpdateSelectedDeedInfo();
                    break;
            }
        }

        private void UpdateSelectedDeedInfo()
        {
            if (_deedMode.CurrentSelectedDeedItem != null)
            {
                _labelSelectedInfo.text = string.Format("{0}{1}{2}", LanguageManager.GetWord("DeedView.Add"),
                   _deedMode.TypeStrings[_deedMode.CurrentDeedType - 1], _deedMode.CurrentSelectedDeedItem.Value);
            }
            else
            {
                _labelSelectedInfo.text = "";
            }
        }

        protected override void HandleAfterOpenView()
        {
            base.HandleAfterOpenView();
            Singleton<DeedMode>.Instance.RequestDeedInfo(); //第一次打开的时候请求一次契约
            SetData(DeedMode.TypeStrength);
        }

        private void SetData(int type)
        {
            if (_deedItems == null)
            {
                InitDeedItem();
            }
            for (int i = 1; i < buttons.Count; i++)
            {
                buttons[i].FindChild("Background1").SetActive(false);
            }
            buttons[type].FindChild("Background1").SetActive(true);
            _deedMode.CurrentDeedType = type;
            SetDeedInfo(type);
        }

        private void SetDeedInfo(int type)
        {
            for (int i = 0; i < _deedItems.Count; i++)
            {
                var items = _deedItems[i];
                var deedId = DeedMode.DeedTypeIds[type - 1, i];
                var deedVo = BaseDataMgr.instance.GetSysDeedVo(deedId);
                if (!_deedMode.DeedCountDirectory.ContainsKey(deedId))
                {
                    for (int j = 0; j < items.Count; j++)
                    {
                        var item = items[j];
                        item.SetDeedInfo(deedVo, j, false);
                    }
                }
                else
                {
                    var count = _deedMode.DeedCountDirectory[deedId];
                    for (int j = 0; j < items.Count; j++)
                    {
                        var item = items[j];
                        item.SetDeedInfo(deedVo, j,count > j);
                    }
                }
            }
            UpdateDeedResult();
        }

        private void InitDeedItem()
        {
            _deedItems = new List<List<DeedItem>>();
            for (int i = 1; i < 8; i++)
            {
                var items = new List<DeedItem>();
                for (int j = 0; j <= i; j++)
                {
                    var item = DeedItemPool.Instance.SpawnDeedItem();
                    item.parent = _itemParent.transform;
                    item.localPosition = new Vector3(j * 66,-(i-1) * 66,0);
                    item.localScale = Vector3.one;
                    var deed = item.gameObject.AddComponent<DeedItem>();
                    deed.Init();
                    items.Add(deed);
                }
                _deedItems.Add(items);
            }
        }

        private void UpdateDeedResult()
        {
            var result = new List<int>();
            var length1 = DeedMode.DeedTypeIds.GetLength(0);
            var length2 = DeedMode.DeedTypeIds.GetLength(1);
            for (var i = 0; i < length1; i++)
            {
                int res = 0;
                for (var j = 0; j < length2; j++)
                {
                    uint deedId = DeedMode.DeedTypeIds[i,j];
                    var deedVo = BaseDataMgr.instance.GetSysDeedVo(deedId);
                    var values = StringUtils.GetValueListFromString(deedVo.val);
                    uint count = 0;
                    if (_deedMode.DeedCountDirectory.ContainsKey(deedId))
                    {
                        count = _deedMode.DeedCountDirectory[deedId];
                    }
                    for (var k = 0; k < count; k++)
                    {
                        res += int.Parse(values[k]);
                    }
                }
                result.Add(res);
            }
            _labelStrength.text = string.Format("{0} +{1}", _deedMode.TypeStrings[0], result[0]);
            _labelPhysique.text = string.Format("{0} +{1}", _deedMode.TypeStrings[1], result[1]);
            _labelAgility.text = string.Format("{0} +{1}", _deedMode.TypeStrings[2], result[2]);
            _labelIntelligence.text = string.Format("{0} +{1}", _deedMode.TypeStrings[3], result[3]); 
        }
    }
}