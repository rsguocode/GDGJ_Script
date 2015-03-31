
using System.Collections.Generic;
using System.Linq;
using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Copy;
using Com.Game.Module.DaemonIsland;
using Com.Game.Module.Role;
using com.game.module.test;
using com.game.Public.Message;
using com.game.utils;
using com.game.vo;
using com.u3d.bases.debug;
using PCustomDataType;
using UnityEngine;

namespace Com.Game.Module.Pet
{
    class PetEquipView : Singleton<PetEquipView>
    {

        private GameObject gameObject;

        private GameObject rightObj;
        private GameObject leftObj;

        private TweenPlay openPlay;

        private TweenPlay rightPlay;
        private TweenPlay leftPlay;

        private Vector3 rightObjStartPos;
        private Vector3 leftObjStartPos;

        private GameObject leftButton;
        private GameObject rightButton;

        private UISprite equipIcn;

        private bool close = false;

        private SysEquipVo currentEquip;
        private List<SysEquipVo> equipList;

        private List<GameObject> topList;

        private List<GameObject> fbList;

        private List<GameObject> hcList;
        private GameObject light;

        private GameObject chose;

        private int equipPos;
        private uint petId;

        private bool rightOpen;
        public EventDelegate.Callback AfterOpenPetEquipView;
        public Button GuideLeftButton;
        public bool IsClosed;

        public void Init(GameObject obj)
        {
            gameObject = obj;
            obj.SetActive(false);
            rightObj = NGUITools.FindChild(gameObject, "info/right");
            rightObjStartPos = rightObj.transform.localPosition;

            leftObj = NGUITools.FindChild(gameObject, "info/left");
            leftObjStartPos = leftObj.transform.localPosition;

            rightPlay = NGUITools.FindInChild<TweenPlay>(rightObj, "");
            leftPlay = NGUITools.FindInChild<TweenPlay>(leftObj, "");
            openPlay = NGUITools.FindInChild<TweenPlay>(gameObject, "info");
                
            NGUITools.FindInChild<UIWidgetContainer>(gameObject, "background").onClick = OnCloseClick;

            leftButton = NGUITools.FindChild(leftObj, "button");
            rightButton = NGUITools.FindChild(rightObj, "button");

            GuideLeftButton = leftButton.GetComponent<Button>();
            NGUITools.FindInChild<UILabel>(leftButton, "name").text = "查看合成";
            GuideLeftButton.onClick = OnLeftButtonClick;

            NGUITools.FindInChild<Button>(rightButton, "").onClick = OnRightButtonClick;
            NGUITools.FindInChild<UILabel>(rightButton, "name").text = "合成";

            EventDelegate.Add(openPlay.onFinished, Close);

            //装备的按钮
            equipIcn = NGUITools.FindInChild<UISprite>(leftObj, "icn");
            equipIcn.atlas = Singleton<PetView>.Instance.EquipAltas;
            equipIcn.normalAtlas = Singleton<PetView>.Instance.EquipAltas;
            equipIcn.grayAtlas = Singleton<PetView>.Instance.EquipAltas_Gray;

            close = false;
            rightOpen = false;

            equipList = new List<SysEquipVo>();

            topList = new List<GameObject>();
            topList.Add(NGUITools.FindChild(rightObj,"top/1"));
            topList.Add(NGUITools.FindChild(rightObj, "top/2"));
            topList.Add(NGUITools.FindChild(rightObj, "top/3"));
            topList.Add(NGUITools.FindChild(rightObj, "top/4"));
            chose = NGUITools.FindChild(rightObj, "top/chose");

            for (int i = 0; i < topList.Count; i++)
            {
                NGUITools.FindInChild<UISprite>(topList[i], "icn").atlas = Singleton<PetView>.Instance.EquipAltas;
                NGUITools.FindInChild<Button>(topList[i], "").onClick = OnTopEquipClick;
            }


            fbList = new List<GameObject>();
            fbList.Add(NGUITools.FindChild(rightObj, "fb/1"));
            fbList.Add(NGUITools.FindChild(rightObj, "fb/2"));
            fbList.Add(NGUITools.FindChild(rightObj, "fb/3"));

            NGUITools.FindInChild<Button>(fbList[0], "").onClick = OnFbClick;
            NGUITools.FindInChild<Button>(fbList[1], "").onClick = OnFbClick;
            NGUITools.FindInChild<Button>(fbList[2], "").onClick = OnFbClick;


            hcList = new List<GameObject>();
            hcList.Add(NGUITools.FindChild(rightObj, "hc/1"));
            hcList.Add(NGUITools.FindChild(rightObj, "hc/2"));
            hcList.Add(NGUITools.FindChild(rightObj, "hc/3"));
            NGUITools.FindInChild<UISprite>(hcList[0], "icn").atlas = Singleton<PetView>.Instance.EquipAltas;
            NGUITools.FindInChild<UISprite>(hcList[1], "icn").atlas = Singleton<PetView>.Instance.EquipAltas;
            NGUITools.FindInChild<UISprite>(hcList[2], "icn").atlas = Singleton<PetView>.Instance.EquipAltas;
            NGUITools.FindInChild<UISprite>(hcList[0], "icn1").atlas = Singleton<PetView>.Instance.EquipAltas;
            NGUITools.FindInChild<UISprite>(hcList[1], "icn1").atlas = Singleton<PetView>.Instance.EquipAltas;
            NGUITools.FindInChild<UISprite>(hcList[2], "icn1").atlas = Singleton<PetView>.Instance.EquipAltas;
            light = NGUITools.FindChild(rightObj, "hc/light");

            NGUITools.FindInChild<Button>(hcList[0], "").onClick = OnCombineEquipClick;
            NGUITools.FindInChild<Button>(hcList[1], "").onClick = OnCombineEquipClick;
            NGUITools.FindInChild<Button>(hcList[2], "").onClick = OnCombineEquipClick;


            NGUITools.FindInChild<UISprite>(rightObj, "hc/current/icn").atlas = Singleton<PetView>.Instance.EquipAltas;
            NGUITools.FindInChild<UISprite>(rightObj, "fb/current/icn").atlas = Singleton<PetView>.Instance.EquipAltas;

            rightObj.SetActive(false);

        }

        public void OpenView(uint equipId,int pos,uint petId)
        {
            IsClosed = false;
            equipPos = pos;
            this.petId = petId;

            Singleton<GoodsMode>.Instance.dataUpdated += DataUpdated;
            MeVo.instance.DataUpdated += DataUpdated;
            Singleton<CopyMode>.Instance.dataUpdated += DataUpdated;
            Singleton<DaemonIslandMode>.Instance.dataUpdated += DataUpdated;


            SetEquipInfo(equipId);
            close = false;
            gameObject.SetActive(true);
            openPlay.PlayReverse();
            if (AfterOpenPetEquipView != null)
            {
                AfterOpenPetEquipView();
                AfterOpenPetEquipView = null;
            }
        }

        public  void DataUpdated(object sender, int code)
        {
            if (sender == Singleton<GoodsMode>.Instance && code == Singleton<GoodsMode>.Instance.UPDATE_PET_GOODS)
            {
                SetEquipInfo((uint)currentEquip.id);
                if (rightOpen)
                {
                    ShowRightObjInfo();
                }
            }

            if (sender == MeVo.instance)
            {
                if (rightOpen)
                {
                    ShowRightObjInfo();
                }
            }
            if (sender == Singleton<CopyMode>.Instance || sender == Singleton<DaemonIslandMode>.Instance)
            {
                SetFBInfo();
            }
        }

        private void SetEquipInfo(uint equipId)
        {
            SysEquipVo equipVo = BaseDataMgr.instance.GetDataById<SysEquipVo>(equipId);
            currentEquip = equipVo;
            if (currentEquip != null)
            {
                equipIcn.spriteName = equipVo.icon.ToString();

                NGUITools.FindInChild<UISprite>(leftObj, "gradeicn").spriteName = "epz_" + equipVo.grade;
                NGUITools.FindInChild<UILabel>(leftObj, "labels/name").text = equipVo.name;
                NGUITools.FindInChild<UILabel>(leftObj, "labels/des").text = equipVo.desc;
                SetEquipProperty(equipVo);
                int num = Singleton<GoodsMode>.Instance.GetCountByGoodsId((uint)equipVo.id);

                NGUITools.FindInChild<UILabel>(leftObj, "labels/numvalue").text = num.ToString();
                if (num == 0)
                {
                    NGUITools.FindInChild<UILabel>(leftObj, "labels/numvalue").color = ColorConst.FONT_RED;
                }
                else
                {
                    NGUITools.FindInChild<UILabel>(leftObj, "labels/numvalue").color = ColorConst.FONT_YELLOW;
                }
                

                int need = PetLogic.CheckEquip(petId, equipPos);
                if (need > 4) //已装备
                {
                    NGUITools.FindInChild<UILabel>(leftObj, "labels/level").text = "需要幻兽等级: " + equipVo.lvl.Replace("[","").Replace("]","");
                    NGUITools.FindInChild<UILabel>(leftObj, "labels/level").color = ColorConst.FONT_YELLOW;
                    NGUITools.FindInChild<UILabel>(leftObj, "button/name").text = "确认";
                }
                else if (need == 1) //可装备
                {
                    NGUITools.FindInChild<UILabel>(leftObj, "labels/level").text = "装备后将与幻兽绑定";
                    NGUITools.FindInChild<UILabel>(leftObj, "labels/level").color = ColorConst.FONT_YELLOW;
                    NGUITools.FindInChild<UILabel>(leftObj, "button/name").text = "装备";
                }else if (need == 2) //存在不可装备
                {
                    NGUITools.FindInChild<UILabel>(leftObj, "labels/level").text = "需要幻兽等级: " + equipVo.lvl.Replace("[", "").Replace("]", "");
                    NGUITools.FindInChild<UILabel>(leftObj, "labels/level").color = ColorConst.FONT_RED;
                    NGUITools.FindInChild<UILabel>(leftObj, "button/name").text = "装备";
                }
                else if (need == 3) //可合成可装备
                {
                    NGUITools.FindInChild<UILabel>(leftObj, "labels/level").text = "装备后将与幻兽绑定";
                    NGUITools.FindInChild<UILabel>(leftObj, "labels/level").color = ColorConst.FONT_YELLOW;
                    NGUITools.FindInChild<UILabel>(leftObj, "button/name").text = "合成方式";
                }
                else if(need ==4) //可合成不可装备
                {
                    NGUITools.FindInChild<UILabel>(leftObj, "labels/level").text = "需要幻兽等级: " + equipVo.lvl.Replace("[", "").Replace("]", "");
                    NGUITools.FindInChild<UILabel>(leftObj, "labels/level").color = ColorConst.FONT_RED;
                    NGUITools.FindInChild<UILabel>(leftObj, "button/name").text = "合成方式";
                }else if (need == 0) //不存在也不可合成
                {
                    PPet pet = Singleton<PetMode>.Instance.GetPetByPetId(petId);
                    NGUITools.FindInChild<UILabel>(leftObj, "labels/level").text = "需要幻兽等级: " + equipVo.lvl.Replace("[", "").Replace("]", "");                    
                    int needlvl =int.Parse( equipVo.lvl.Replace("[", "").Replace("]", ""));

                    if (needlvl > pet.lvl)
                    {
                        NGUITools.FindInChild<UILabel>(leftObj, "labels/level").color = ColorConst.FONT_RED;
                    }
                    else
                    {
                        NGUITools.FindInChild<UILabel>(leftObj, "labels/level").color = ColorConst.FONT_YELLOW;
                    }

                    //有合成途径
                    if (equipVo.material.Length > 4)
                    {
                        NGUITools.FindInChild<UILabel>(leftObj, "button/name").text = "合成方式";
                    }
                    else //产出途径
                    {
                        NGUITools.FindInChild<UILabel>(leftObj, "button/name").text = "获取途径";
                    }
                }
            }
        }

        private void SetEquipProperty(SysEquipVo equipVo)
        {
            int num = 0;
            if (equipVo.hp.Length>4)
            {
                num++;
                NGUITools.FindInChild<UILabel>(leftObj, "labels/property" + num).text = "生命：+" + PetLogic.GetEquipProperty(equipVo.hp);
            }

            if (equipVo.att_max>0)
            {
                num++;
                NGUITools.FindInChild<UILabel>(leftObj, "labels/property" + num).text = "攻击：+" + equipVo.att_max;
            }
            if (equipVo.def_p.Length > 4)
            {
                num++;
                NGUITools.FindInChild<UILabel>(leftObj, "labels/property" + num).text = "物防：+" + PetLogic.GetEquipProperty(equipVo.def_p);
            }
            if (equipVo.def_m.Length > 4)
            {
                num++;
                NGUITools.FindInChild<UILabel>(leftObj, "labels/property" + num).text = "魔防：+" + PetLogic.GetEquipProperty(equipVo.def_m);
            }
            if (equipVo.hurt_re.Length > 4)
            {
                num++;
                NGUITools.FindInChild<UILabel>(leftObj, "labels/property" + num).text = "减伤：+" + PetLogic.GetEquipProperty(equipVo.hurt_re);
            }

            for (int i=1; i< 6; i++)
            {
                if (i <= num)
                {
                    NGUITools.FindChild(leftObj, "labels/property" + i).SetActive(true);
                }
                else
                {
                    NGUITools.FindChild(leftObj, "labels/property" + i).SetActive(false);
                }
            }

        }

        private void OnLeftButtonClick(GameObject obj)
        {
            if (currentEquip != null)
            {
                int need = PetLogic.CheckEquip(petId, equipPos);
                if (need == 1) //可装备
                {
                    PPet pet = Singleton<PetMode>.Instance.GetPetByPetId(petId);

                    uint bagid = 0;
                    foreach (PGoods goods in Singleton<GoodsMode>.Instance.GetEquipListInBag())
                    {
                        if (goods.goodsId == currentEquip.id)
                        {
                            bagid = goods.id;
                            break;
                        }
                    }
                    Singleton<PetControl>.Instance.WearPetEquip(pet.id, equipPos, bagid);
                }
                else if (need == 2) //不可装备
                {
                    MessageManager.Show("需要幻兽等级:" + currentEquip.lvl.Replace("[","").Replace("]",""));
                }else if (need == 3 || need == 4 || need == 0) //合成或者获取途径
                {
                    if (!rightOpen)
                    {
                        OpenRigthPanel();
                    }

                }else if (need > 4)
                {
                    CloseView();
                }
            }
        }

        //合成页面打开
        private void OpenRigthPanel()
        {
            equipList.Clear();
            equipList.Add(currentEquip);

            ShowRightObjInfo();
            rightObj.SetActive(true);
            rightPlay.PlayForward();
            rightOpen = true;

            leftPlay.PlayForward();
        }

        private void ShowRightObjInfo()
        {
            ShowTop();
            SysEquipVo evo = equipList[equipList.Count - 1];
            if (evo.material.Length < 4) //不可合成
            {
                NGUITools.FindChild(rightObj, "hc").SetActive(false);
                NGUITools.FindChild(rightObj, "fb").SetActive(true);
                NGUITools.FindInChild<UILabel>(rightButton, "name").text = "返回";

                NGUITools.FindInChild<UISprite>(rightObj, "fb/current/gradeicn").spriteName = "epz_" + evo.grade;
                NGUITools.FindInChild<UISprite>(rightObj, "fb/current/icn").spriteName = evo.icon.ToString();

                SetFBInfo();
            }
            else //可以合成
            {
                int[] material = StringUtils.GetArrayStringToInt(evo.material);
                NGUITools.FindChild(rightObj, "fb").SetActive(false);
                NGUITools.FindChild(rightObj, "hc").SetActive(true);
                NGUITools.FindInChild<UISprite>(rightObj, "hc/current/gradeicn").spriteName = "epz_" + evo.grade;
                NGUITools.FindInChild<UISprite>(rightObj, "hc/current/icn").spriteName = evo.icon.ToString();

                if (material.Count() == 2)
                {
                    hcList[0].SetActive(false);
                    hcList[2].SetActive(false);
                    hcList[1].SetActive(true);
                    NGUITools.FindChild(rightObj, "hc/xian").SetActive(false);
                    SetCombineEquipInfo(1, material[0], material[1]);
                }
                else if (material.Count() == 4)
                {
                    hcList[0].SetActive(true);
                    hcList[2].SetActive(true);
                    hcList[1].SetActive(false);
                    NGUITools.FindChild(rightObj, "hc/xian").SetActive(true);
                    SetCombineEquipInfo(0, material[0], material[1]);
                    SetCombineEquipInfo(2, material[2], material[3]);
                }
                else if(material.Count() == 6)
                {
                    hcList[0].SetActive(true);
                    hcList[2].SetActive(true);
                    hcList[1].SetActive(true);
                    NGUITools.FindChild(rightObj, "hc/xian").SetActive(true);
                    SetCombineEquipInfo(0, material[0], material[1]);
                    SetCombineEquipInfo(1, material[2], material[3]);
                    SetCombineEquipInfo(2, material[4], material[5]);
                }

                int cost = evo.spend;
                NGUITools.FindInChild<UILabel>(rightObj, "hc/costvalue").text = cost.ToString();
                if (cost > MeVo.instance.diam)
                {
                    NGUITools.FindInChild<UILabel>(rightObj, "hc/costvalue").color = ColorConst.FONT_RED;
                }
                else
                {
                    NGUITools.FindInChild<UILabel>(rightObj, "hc/costvalue").color = ColorConst.FONT_YELLOW;
                }

                NGUITools.FindInChild<UILabel>(rightButton, "name").text = "合成";
            }
        }


        private void SetFBInfo()
        {
            if (equipList.Count > 0)
            {
                SysEquipVo evo = equipList[equipList.Count - 1];
                int[] fbs = StringUtils.GetStringToInt(evo.source);
                PetLogic.SetFBInfo(fbList, fbs);
            }
        }

        //设置合成装备信息
        private void SetCombineEquipInfo(int pos ,int equipId,int num)
        {
            GameObject obj = hcList[pos];
            SysEquipVo equipVo = BaseDataMgr.instance.GetDataById<SysEquipVo>((uint)equipId);
            obj.name = equipId.ToString();
            NGUITools.FindInChild<UISprite>(obj, "gradeicn").spriteName = "epz_"+equipVo.grade;
            NGUITools.FindInChild<UISprite>(obj, "icn").spriteName = equipVo.icon.ToString();


            NGUITools.FindInChild<UISprite>(obj, "icn1").spriteName = equipVo.icon.ToString();



            int cNum = Singleton<GoodsMode>.Instance.GetCountByGoodsId((uint) equipId);
            NGUITools.FindInChild<UILabel>(obj, "num").text = cNum + "/" + num;
            if (cNum >= num)
            {
                NGUITools.FindInChild<UILabel>(obj, "num").color = ColorConst.FONT_YELLOW;
            }
            else
            {
                if (PetLogic.CanCombine((uint) equipId, true, num) > 0) //子项可以合成
                {
                    NGUITools.FindInChild<UILabel>(obj, "num").color = ColorConst.FONT_GREEN;
                }
                else
                {
                    NGUITools.FindInChild<UILabel>(obj, "num").color = ColorConst.FONT_RED;
                }
            }

        }

        private void ShowTop()
        {
            if (equipList.Count > 0)
            {
                int eIndex = equipList.Count - topList.Count;
                if (eIndex < 0)
                {
                    eIndex = 0;
                }
                for (int i = 0; i < topList.Count; i++)
                {
                    if (i < equipList.Count)
                    {
                        topList[i].SetActive(true);
                        SysEquipVo equipvo = equipList[eIndex + i];
                        topList[i].name = equipvo.id.ToString();
                        NGUITools.FindInChild<UISprite>(topList[i], "gradeicn").spriteName = "epz_" + equipvo.grade;
                        NGUITools.FindInChild<UISprite>(topList[i], "icn").spriteName = equipvo.icon.ToString();

                        chose.transform.localPosition = topList[i].transform.localPosition;
                    }
                    else
                    {
                        topList[i].SetActive(false);
                    }
                }
                SysEquipVo eqvo = equipList[equipList.Count - 1];
                NGUITools.FindInChild<UILabel>(rightObj, "top/name").text = eqvo.name;
            }
        }

        public  void PlayCombine()
        {
            if (!ReferenceEquals(rightObj, null) && rightObj.activeInHierarchy)
            {
                NGUITools.FindInChild<TweenPlay>(hcList[0], "icn1").PlayForward();
                NGUITools.FindInChild<TweenPlay>(hcList[1], "icn1").PlayForward();
                NGUITools.FindInChild<TweenPlay>(hcList[2], "icn1").PlayForward();
                light.SetActive(true);
                NGUITools.FindInChild<TweenPlay>(light, "").PlayForward();
            }
        }

        private void OnRightButtonClick(GameObject obj)
        {
            SysEquipVo evo = equipList[equipList.Count - 1];
            if (evo.material.Length < 4) //不可合成
            {
                if (equipList.Count > 1) //返回上一物品
                {
                    equipList.RemoveAt(equipList.Count - 1);
                    ShowRightObjInfo();
                }
                else //关闭右边页面
                {
                    rightPlay.PlayReverse();
                    rightOpen = false;
                    leftPlay.PlayReverse();
                }
            }
            else //检查合成
            {
                int[] material = StringUtils.GetArrayStringToInt(evo.material);
                bool oneCanCombine = false;
                bool allCanCombine = true;
                for(int i =0;i<material.Count();) //检查子项目
                {

                    int cnum = Singleton<GoodsMode>.Instance.GetCountByGoodsId((uint) material[i]);
                    if (cnum< material[i+1]) //检查当前已有的数量
                    {
                        allCanCombine = false;
                        if (PetLogic.CanCombine((uint)material[i], true, material[i + 1]) > 0) //检查已有数量+可合成数量
                        {
                            oneCanCombine = true;
                        }
                    }
                    i = i + 2;
                }

                if (allCanCombine)
                {
                    //检查钱
                    if (evo.spend > MeVo.instance.diam)
                    {
                        MessageManager.Show("当前金币不足!");
                    }
                    else
                    {
                        Singleton<PetControl>.Instance.CombinePetEquip((uint)evo.id);
                    }
                    
                }
                else
                {
                    if (oneCanCombine)
                    {
                        MessageManager.Show("请先合成绿色数字提示装备!");
                    }
                    else
                    {
                        MessageManager.Show("材料不足，请先去收集一些吧!");
                    }
                }

            }
        }

        private void OnCombineEquipClick(GameObject obj)
        {
            uint id = uint.Parse(obj.name);
            SysEquipVo evo = BaseDataMgr.instance.GetDataById<SysEquipVo>(id);
            equipList.Add(evo);
            ShowRightObjInfo();
        }

        private void OnFbClick(GameObject obj)
        {
            uint id = uint.Parse(obj.name);

            if (Singleton<CopyControl>.Instance.IsCopyOpened(id))
            {
                Singleton<CopyControl>.Instance.OpenCopyById(id);
            }
            else
            {
                MessageManager.Show("副本尚未开启！");
            }

        }

        private void OnTopEquipClick(GameObject obj)
        {
            uint id = uint.Parse(obj.name);
            int i = equipList.Count - 1;
            bool removed = false;
            for (; i > 0; i-- )
            {
                if (equipList[i].id != id)
                {
                    equipList.RemoveAt(i);
                    removed = true;
                }
                else
                {
                    break;
                }

            }
            if (removed)
            {
                ShowRightObjInfo();
            }
        }
        private void OnCloseClick(GameObject obj)
        {
            CloseView();
        }

        public void CloseView()
        {
            IsClosed = true;
            if(gameObject.activeSelf){
                close = true;
                openPlay.PlayForward();
            }
        }

        private void Close()
        {
            if (close)
            {
                gameObject.SetActive(false);
                rightObj.transform.localPosition = rightObjStartPos;
                leftObj.transform.localPosition = leftObjStartPos;
                equipList.Clear();
                rightOpen = false;
                rightObj.SetActive(false);
                Singleton<GoodsMode>.Instance.dataUpdated -= DataUpdated;
                Singleton<CopyMode>.Instance.dataUpdated -= DataUpdated;
                Singleton<DaemonIslandMode>.Instance.dataUpdated -= DataUpdated;
                MeVo.instance.DataUpdated -= DataUpdated;
            }
        }



    }
}
