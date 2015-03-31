using System;
using System.Collections.Generic;
using System.Net.Mime;
using com.game.consts;
using com.game.data;
using com.game.dialog;
using com.game.manager;
using Com.Game.Module.Manager;
using Com.Game.Module.Role;
using com.game.module.test;
using Com.Game.Module.Tips;
using com.game.vo;
using Proto;
using UnityEngine;
using PCustomDataType;

namespace Com.Game.Module.TopList
{
    class TopListView : BaseView<TopListView>
    {
        public override string url { get { return "UI/TopList/TopListView.assetbundle"; } }

        public override ViewLayer layerType
        {
            get { return ViewLayer.MiddleLayer; }
        }

        private bool[] RankDataInit  = new bool[3];

        public override bool IsFullUI
        {
            get { return true; }
        }

        public override bool isDestroy
        {
            get { return true; }
        }

        //ranktoggle
        private UIToggle goldRank;
        private UIToggle fightRank;
        private UIToggle levelRank;
        private UIToggle battleRank;
        private UIToggle currentToggle;
        private ushort type;

        //排行榜信息对象
        private List<GameObject> rankInfoList;
        private GameObject self;

        private GameObject selected;
        private int selectedPos = 1;


        private readonly int RankNum = 10; //排行版数量

        private GameObject left;

        private GameObject role;
        private SpinWithMouse spin;

        private List<ItemContainer> equipList;



        protected override void Init()
        {
            NGUITools.FindInChild<Button>(gameObject, "btn_close").onClick = CloseClick;

            goldRank = FindInChild<UIToggle>("right/rankbutton/gold");
            fightRank = FindInChild<UIToggle>("right/rankbutton/fight");
            levelRank = FindInChild<UIToggle>("right/rankbutton/level");
            battleRank = FindInChild<UIToggle>("right/rankbutton/battle");
            rankInfoList = new List<GameObject>();
            for (int i = 101; i <= 100+RankNum; i++)
            {
                rankInfoList.Add(FindChild("right/list/container/oncenter/" + i ));
                rankInfoList[i - 101].GetComponent<UIWidgetContainer>().onClick = OnRoleRankInfoClick;
            }
            self = FindChild("right/self");
            left = FindChild("left");
            left.SetActive(false);
            selected = FindChild("right/select");
            selected.SetActive(false);

            goldRank.onStateChange = OnRankButtonToggleChange;
            fightRank.onStateChange = OnRankButtonToggleChange;
            levelRank.onStateChange = OnRankButtonToggleChange;
            battleRank.onStateChange = OnRankButtonToggleChange;

            spin = FindInChild<SpinWithMouse>("mode");
            equipList = new List<ItemContainer>();
            GetLeftItemList();
            InitLabel();

        }

        /// <summary>
        /// 初始化Label文本信息
        /// </summary>
        private void InitLabel()
        {

            FindInChild<UILabel>("name").text = LanguageManager.GetWord("TopListView.TopList");
            NGUITools.FindInChild<UILabel>(self, "num").text = LanguageManager.GetWord("TopListView.MyRank");
            FindInChild<UILabel>("right/rankbutton/gold/label").text = LanguageManager.GetWord("TopListView.Gold");
            FindInChild<UILabel>("right/rankbutton/fight/label").text = LanguageManager.GetWord("TopListView.Fight");
            FindInChild<UILabel>("right/rankbutton/level/label").text = LanguageManager.GetWord("TopListView.Level");

            FindInChild<UILabel>("right/title/num").text = LanguageManager.GetWord("TopListView.Num");
            FindInChild<UILabel>("right/title/name").text = LanguageManager.GetWord("TopListView.Name");
            FindInChild<UILabel>("right/title/value").text = LanguageManager.GetWord("TopListView.Level");

            disableAllRankInfo();

        }

        private void InitDefalutEquipItem()
        {
            int index = 1;
            foreach (ItemContainer temp in equipList)
            {
                ItemManager.Instance.InitItem(temp, ItemManager.Instance.EMPTY_ICON, ItemType.Equip);
                temp.FindInChild<UILabel>("pos").text = LanguageManager.GetWord("Equip.Pos" + (index));
                temp.FindInChild<UILabel>("stren").text = string.Empty;
                temp.FindInChild<UISprite>("background").alpha = 0.3f;
                temp.FindInChild<UISprite>("icon").alpha = 0.3f;
                temp.isEnabled = false;
                temp.Id = 0;
                index++;
            }
        }
        private void GetLeftItemList()
        {
            ItemContainer temp;
            for (int i = 1; i < 11; i++)
            {
                temp = FindChild("left/item" + i).AddMissingComponent<ItemContainer>();
                temp.onClick = ShowLeftTips;
                equipList.Add(temp);
            }
            InitDefalutEquipItem();

        }

        private void SetEquipInfo(List<PGoods> equipInfoList)
        {
            InitDefalutEquipItem();
            SysEquipVo vo;
            ItemContainer ic;
            foreach (PGoods goods in equipInfoList)
            {
                vo = BaseDataMgr.instance.GetDataById<SysEquipVo>(goods.goodsId);
                if (vo.pos < 1 || vo.pos > 11)
                {
                    continue;
                }
                ic = equipList[vo.pos - 1];
                ic.Id = goods.id;
                ic.FindInChild<UILabel>("pos").text = string.Empty;
                Singleton<ItemManager>.Instance.InitItem(ic, goods.goodsId, ItemType.Equip);
                ic.FindInChild<UILabel>("stren").text = "+" + goods.equip[0].stren;
                ic.FindInChild<UISprite>("background").alpha = 1f;
                ic.FindInChild<UISprite>("icon").alpha = 1f;
                ic.isEnabled = true;
                ic.buttonType = Button.ButtonType.Toggle;

            }
        }



        //单击左边装备，显示 Tips 界面
        private void ShowLeftTips(GameObject go)
        {
            ItemContainer ic = go.GetComponent<ItemContainer>();
            PRank rank = Singleton<RankMode>.Instance.GetRankInfo(type).info[selectedPos-1];
            List<PGoods> goods = rank.goodsList;
            if (ic.Id != 0)
            {
                PGoods good = null;
                foreach (PGoods pg in goods)
                {
                    if (pg.id == ic.Id)
                    {
                        good = pg;
                        break;;
                    }
                }
                if (good != null)
                {
                    Singleton<TipsManager>.Instance.OpenPlayerEquipTipsByGoods(good);
                }
            }


        }


        protected override void HandleAfterOpenView()
        {

                ReSetRankDataUninit();

                if (currentToggle != fightRank)
                {
                    fightRank.value = true;
                }
                else
                {
                    SetRankInfo(RankMode.FIGHT);
                }
            
        }

        private void CloseClick(GameObject obj)
        {
            CloseView();
        }


        /// <summary>
        /// 数据初始化状态重置
        /// </summary>
        private void ReSetRankDataUninit()
        {
            for (int i = 0; i < RankDataInit.Length; i++)
            {
                RankDataInit[i] = false;
            }
        }


        private void SetRankInfo(ushort type)
        {
            this.type = type;
            RankInfoMsg_20_1 msg =  Singleton<RankMode>.Instance.GetRankInfo(type);
            if (msg == null || RankDataInit[type - 1] == false)
            {
                Singleton<RankControl>.Instance.SendRequestForRankInfo(type);
            }
            else
            {
                SetRankTitle(type);
                SetRankInfo(type, msg);
                if (msg.info.Count > 0)
                {
                    SetSelectedPos(1);
                }
                else
                {
                    selected.SetActive(false);
                }
            }
  
        }

        private void SetRankTitle(ushort type)
        {
            if (type == RankMode.FIGHT)
            {
                FindInChild<UILabel>("right/title/value").text = LanguageManager.GetWord("TopListView.Fight");
            }
            else if (type == RankMode.LEVEL)
            {
                FindInChild<UILabel>("right/title/value").text = LanguageManager.GetWord("TopListView.Level");
            }
            else if(type == RankMode.GOLD)
            {
                FindInChild<UILabel>("right/title/value").text = LanguageManager.GetWord("TopListView.Gold");
            }
        }

        //设置排行榜的信息
        private void SetRankInfo(ushort type, RankInfoMsg_20_1 msg)
        {
            RankDataInit[type-1] = true;
            disableAllRankInfo();
            List<PRank> rankList = msg.info;
            int max = rankInfoList.Count;
            for (int i = 0; i < rankList.Count && i < max; i++)
            {
                setSingleRankInfo(rankInfoList[i],rankList[i]);
            }
            setSelfRankInfo(msg);
        }

        private void setSelfRankInfo(RankInfoMsg_20_1 msg)
        {
            if (msg.pos != 0)
            {
                NGUITools.FindInChild<UILabel>(self, "numvalue").text = msg.pos.ToString();
            }
            else
            {
                NGUITools.FindInChild<UILabel>(self, "numvalue").text = LanguageManager.GetWord("TopListVIew.OutRank");
            }
        }

        private void setSingleRankInfo(GameObject obj,PRank rank)
        {
            NGUITools.FindInChild<UILabel>(obj,"name").text = rank.role[0].name;
            NGUITools.FindInChild<UILabel>(obj, "value").text = rank.data[0].ToString();

            string job = "zyjs";
            if (rank.role[0].job == 2)
            {
                job = "zyfs";

            }
            else if (rank.role[0].job == 3)
            {
                 job = "zyck";
            }
            NGUITools.FindInChild<UISprite>(obj, "job").spriteName = job;
            obj.SetActive(true);
        }

        private void disableAllRankInfo()
        {
            foreach (GameObject obj in rankInfoList)
            {
                obj.SetActive(false);
            }
        }

        //注册数据更新器
        public override void RegisterUpdateHandler()
        {
            Singleton<RankMode>.Instance.dataUpdated += RankDataUpdated;
        }

        //数据更新响应
        public void RankDataUpdated(object sender, int code)
        {   
            SetRankInfo((ushort)code);
            RankDataInit[code-1] = true;

        }

        //取消数据更新器
        public override void CancelUpdateHandler()
        {
            Singleton<RankMode>.Instance.dataUpdated -= RankDataUpdated;
        }

        private void OnRankButtonToggleChange(bool state)
        {
            if (state)
            {
                if (UIToggle.current != currentToggle)
                {
                    if (UIToggle.current == levelRank)
                    {
                        currentToggle = levelRank;
                        SetRankInfo(RankMode.LEVEL);

                    }else if (UIToggle.current == fightRank)
                    {
                        currentToggle = fightRank;
                        SetRankInfo(RankMode.FIGHT);
                    }
                    else if (UIToggle.current == goldRank)
                    {
                        currentToggle = goldRank;
                        SetRankInfo(RankMode.GOLD);
                    }
                }
            }
        }

        private void SetSelectedPos(int pos)
        {
            selected.transform.parent = rankInfoList[pos - 1].transform;
            selected.transform.localPosition = new Vector3(18,-1,1);
            selected.SetActive(true);
            selectedPos = pos;
            SetSelectedRoleInfo( pos) ;
        }

        private void SetSelectedRoleInfo(int pos)
        {   left.SetActive(true);
            PRank rank = Singleton<RankMode>.Instance.GetRankInfo(type).info[pos-1];
            if (!ReferenceEquals(role, null))
            {
                GameObject.Destroy(role);
            }
            new RoleDisplay().CreateRole(rank.role[0].job, LoadRoleImage);

            SetEquipInfo(rank.goodsList);
        }

        private void LoadRoleImage(GameObject obj)
        {
            role = obj;
            role.transform.localPosition = new Vector3(-266,-185,1);
            role.transform.parent = gameObject.transform;
            spin.target = NGUITools.FindChild(role, "101_0").transform;
        }

        private void OnRoleRankInfoClick(GameObject obj)
        {
            int pos = int.Parse(obj.name);
            SetSelectedPos(pos-100);
        }

    }
}
