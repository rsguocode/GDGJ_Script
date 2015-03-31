
using System.Collections.Generic;
using com.bases.utils;
using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Chat;
using Com.Game.Module.Role;
using com.game.module.SystemData;
using com.game.module.test;
using Com.Game.Module.VIP;
using com.game.Public.Confirm;
using com.game.Public.Message;
using com.game.Public.UICommon;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.debug;
using PCustomDataType;
using Proto;
using UnityEngine;
using StringUtils = com.game.utils.StringUtils;

namespace Com.Game.Module.Pet
{
    class PetInfoView : Singleton<PetInfoView>
    {

        private GameObject gameObject;
        private UILabel name;

        private UIToggle skill;
        private UIToggle property;
        private UIToggle exp;
        private UIToggle current;

        private GameObject skillObj;
        private GameObject propetyObj;
        private GameObject expObj;
     

        private TweenPlay skillPlay;
        private TweenPlay propertyPlay;
        private TweenPlay expPlay;


        private List<GameObject> equipObjs;

        private List<GameObject> expObjs; 

        private List<UISprite> stars;

        private PPet currentPPet;

        private List<GameObject> skillObjs;

        private GameObject skillTips;
        private GameObject petTips;

        private TweenPlay equipTipPlay;
        private GameObject equipTip;

        private GameObject light;

        private GameObject chose;

        private uint choseItemId;
        public EventDelegate.Callback GuideAfterOpenPetInfoView;
        public Button GuideEquipButton;
        public UIWidgetContainer GuideSkillButton;
        public Button CloseButton;
        public Button FightButton;

        private uint[] expItems = new uint[] { 115531, 115532, 115533, 115534 };

        public void Init(GameObject obj)
        {
            gameObject = obj;
            obj.SetActive(false);
            name = NGUITools.FindInChild<UILabel>(gameObject, "info/name");

            CloseButton = NGUITools.FindInChild<Button>(gameObject, "close");
            CloseButton.onClick = OnCloseClick;

            skill = NGUITools.FindInChild<UIToggle>(gameObject, "info/function/skill");
            skill.onStateChange = OnStateChange;

            property = NGUITools.FindInChild<UIToggle>(gameObject, "info/function/property");
            property.onStateChange = OnStateChange;

            exp = NGUITools.FindInChild<UIToggle>(gameObject, "info/function/exp");
            exp.onStateChange = OnStateChange;

            skillObj = NGUITools.FindChild(gameObject, "skill");
            propetyObj = NGUITools.FindChild(gameObject, "property");
            expObj = NGUITools.FindChild(gameObject, "exp");

            skillPlay = NGUITools.FindInChild<TweenPlay>(skillObj,"");
            propertyPlay = NGUITools.FindInChild<TweenPlay>(propetyObj, "");
            expPlay = NGUITools.FindInChild<TweenPlay>(expObj, "");

            equipObjs = new List<GameObject>();
            GameObject equip = NGUITools.FindChild(gameObject, "info/equips/1");
            NGUITools.FindInChild<Button>(equip, "").onClick = OnEquipClick;
            equipObjs.Add(equip);
            equip = NGUITools.FindChild(gameObject, "info/equips/2");
            NGUITools.FindInChild<Button>(equip, "").onClick = OnEquipClick;
            equipObjs.Add(equip);
            equip = NGUITools.FindChild(gameObject, "info/equips/3");
            NGUITools.FindInChild<Button>(equip, "").onClick = OnEquipClick;
            equipObjs.Add(equip);
            equip = NGUITools.FindChild(gameObject, "info/equips/4");
            NGUITools.FindInChild<Button>(equip, "").onClick = OnEquipClick;
            equipObjs.Add(equip);
            equip = NGUITools.FindChild(gameObject, "info/equips/5");
            NGUITools.FindInChild<Button>(equip, "").onClick = OnEquipClick;
            equipObjs.Add(equip);
            equip = NGUITools.FindChild(gameObject, "info/equips/6");
            NGUITools.FindInChild<Button>(equip, "").onClick = OnEquipClick;
            equipObjs.Add(equip);

            GuideEquipButton = NGUITools.FindChild(gameObject, "info/equips/1").GetComponent<Button>();

            foreach (GameObject equipObj in equipObjs)
            {
                UISprite sprite = NGUITools.FindInChild<UISprite>(equipObj, "bicn");
                sprite.atlas = Singleton<PetView>.Instance.EquipAltas;
                sprite.normalAtlas = Singleton<PetView>.Instance.EquipAltas;
                sprite.grayAtlas = Singleton<PetView>.Instance.EquipAltas_Gray;

                NGUITools.FindInChild<UISprite>(equipObj, "icn").atlas = Singleton<PetView>.Instance.EquipAltas;
                NGUITools.FindChild(equipObj, "icn").SetActive(false);

                NGUITools.FindInChild<UISprite>(equipObj, "tipicn").atlas = Singleton<PetView>.Instance.EquipAltas;
            }

            EventDelegate.Add(NGUITools.FindInChild<TweenPlay>(equipObjs[5], "icn").onFinished, PlayEquipsFinish);
           

            stars = new List<UISprite>();
            UISprite star1 = NGUITools.FindInChild<UISprite>(gameObject, "info/stars/1");
            UISprite star2 = NGUITools.FindInChild<UISprite>(gameObject, "info/stars/2");
            UISprite star3 = NGUITools.FindInChild<UISprite>(gameObject, "info/stars/3");
            UISprite star4 = NGUITools.FindInChild<UISprite>(gameObject, "info/stars/4");
            UISprite star5 = NGUITools.FindInChild<UISprite>(gameObject, "info/stars/5");

            stars.Add(star1);
            stars.Add(star2);
            stars.Add(star3);
            stars.Add(star4);
            stars.Add(star5);

            currentPPet = null;

            FightButton = NGUITools.FindInChild<Button>(gameObject, "info/function/fight");
            FightButton.onClick = OnFightClick;
            NGUITools.FindInChild<Button>(gameObject, "info/function/evolve").onClick = OnEvolveClick;
            NGUITools.FindInChild<Button>(gameObject, "info/function/upgrade").onClick = OnUpgradeClick;


            skillObjs = new List<GameObject>();
            skillObjs.Add(NGUITools.FindChild(gameObject,"skill/1"));
            skillObjs.Add(NGUITools.FindChild(gameObject, "skill/2"));
            skillObjs.Add(NGUITools.FindChild(gameObject, "skill/3"));
            skillObjs.Add(NGUITools.FindChild(gameObject, "skill/4"));
            GuideSkillButton = NGUITools.FindChild(gameObject, "skill/1/add").GetComponent<Button>();
            foreach (GameObject sobj in skillObjs)
            {
                NGUITools.FindInChild<UIWidgetContainer>(sobj, "").onPress = OnSkillPress;
            }

            skillObjs.Add(NGUITools.FindChild(gameObject, "skill/5"));

            skillTips = NGUITools.FindChild(Singleton<PetView>.Instance.gameObject, "tips/skilltips");
   

            petTips = NGUITools.FindChild(Singleton<PetView>.Instance.gameObject, "tips/labeltips");
            

            equipTip = NGUITools.FindChild(gameObject, "info/equips/tips");
            equipTipPlay = equipTip.GetComponent<TweenPlay>();

            light = NGUITools.FindChild(gameObject, "info/light");
            light.SetActive(false);

            NGUITools.FindInChild<Button>(skillObj, "add").onClick = OnAddSkillPointClick;

            NGUITools.FindInChild<Button>(gameObject, "info/stone/add").onClick = OnAddStoneClick;

            expObjs = new List<GameObject>();

            expObjs.Add(NGUITools.FindChild(expObj,"1"));
            expObjs.Add(NGUITools.FindChild(expObj, "2"));
            expObjs.Add(NGUITools.FindChild(expObj, "3"));
            expObjs.Add(NGUITools.FindChild(expObj, "4"));

            expObjs[0].GetComponent<Button>().onClick = OnExpItemClick;
            expObjs[1].GetComponent<Button>().onClick = OnExpItemClick;
            expObjs[2].GetComponent<Button>().onClick = OnExpItemClick;
            expObjs[3].GetComponent<Button>().onClick = OnExpItemClick;

            NGUITools.FindInChild<UISprite>(expObjs[0], "icn").atlas = Singleton<PetView>.Instance.PropAltas;
            NGUITools.FindInChild<UISprite>(expObjs[1], "icn").atlas = Singleton<PetView>.Instance.PropAltas;
            NGUITools.FindInChild<UISprite>(expObjs[2], "icn").atlas = Singleton<PetView>.Instance.PropAltas;
            NGUITools.FindInChild<UISprite>(expObjs[3], "icn").atlas = Singleton<PetView>.Instance.PropAltas;

            chose = NGUITools.FindChild(expObj, "chose");

            NGUITools.FindInChild<Button>(expObj, "use").onClick = OnUseClick;
            NGUITools.FindInChild<Button>(expObj, "useten").onClick = OnUseTenClick;

            NGUITools.FindInChild<UIWidgetContainer>(gameObject, "info/tupian").onDrag = OnPetDrag;

            SetExpItemInfo();

        }

        private void OnPetDrag(GameObject obj, Vector2 dis)
        {
            Singleton<PetView>.Instance.PetImage.transform.Rotate(-Vector3.up*dis.x);
        }

        private void OnAddStoneClick(GameObject obj)
        {
            if(currentPPet!=null)
                Singleton<PetStoneTipsView>.Instance.OpenView(currentPPet.petId);
        }

        private void OnStateChange(bool state)
        {
            if (state)
            {
                if (UIToggle.current == skill && current != skill)
                {
                    PlayReverse();
                    current = skill;
                    skillPlay.PlayForward();
              
                }
                else if (UIToggle.current == property && current != property)
                {
                    PlayReverse();
                    current = property;
                    propertyPlay.PlayForward();

                }
                else if (UIToggle.current == exp && current != exp)
                {
                    PlayReverse();
                    current = exp;
                    expPlay.PlayForward();

                }
            }
        }

        private void OnEquipClick(GameObject obj)
        {
            int num = int.Parse(obj.name);
            uint equipdId = 0;
            if (currentPPet != null)
            {
                equipdId  = (uint) PetLogic.GetNeedEquip(currentPPet.petId, num);
            }
            Singleton<PetEquipView>.Instance.OpenView(equipdId,num,currentPPet.petId);

        }

        private void OnExpItemClick(GameObject obj)
        {
            chose.transform.localPosition = obj.transform.localPosition;
            choseItemId = uint.Parse(obj.name);
        }

        private void OnUseClick(GameObject obj)
        {
            PGoods goods = Singleton<GoodsMode>.Instance.GetPGoodsByGoodsId(choseItemId);
            if (goods != null && goods.count > 0)
            {
                Singleton<PetControl>.Instance.UsePetExpItem(currentPPet.id,goods.id,1);
            }
            else
            {
                MessageManager.Show("物品数量不足!");
            }
        }

        private void OnUseTenClick(GameObject obj)
        {
            PGoods goods = Singleton<GoodsMode>.Instance.GetPGoodsByGoodsId(choseItemId);
            if (goods != null && goods.count > 0)
            {
                if (goods.count > 10)
                {
                    Singleton<PetControl>.Instance.UsePetExpItem(currentPPet.id, goods.id, 10);
                }
                else
                {
                    Singleton<PetControl>.Instance.UsePetExpItem(currentPPet.id, goods.id, goods.count);
                }

            }
            else
            {
                MessageManager.Show("物品数量不足!");
            }
        }

        private void PlayReverse()
        {
            if (current == property)
            {
                propertyPlay.PlayReverse();
            }

            if (current == skill)
            {
                skillPlay.PlayReverse();
            }
            if (current == exp)
            {
                expPlay.PlayReverse();
            }
        }


        private void PlayPetTips(string text)
        {
            petTips.SetActive(true);
            petTips.GetComponent<UILabel>().text = text;
            petTips.GetComponent<TweenPlay>().PlayForward();
        }

        public void OpenView(uint petId)
        {
            PPet ppet = Singleton<PetMode>.Instance.GetPetByPetId(petId);
            currentPPet = ppet;

            ShowPet();
            gameObject.SetActive(true);
            SetPetInfo();

            if (current == null)
            {
                skill.value = true;
            }

            SetExpItemNumInfo();

            Singleton<PetMode>.Instance.dataUpdated += DataUpdated;
            MeVo.instance.DataUpdated += DataUpdated;
            Singleton<GoodsMode>.Instance.dataUpdated += DataUpdated;

            if (GuideAfterOpenPetInfoView != null)
            {
                GuideAfterOpenPetInfoView();
                GuideAfterOpenPetInfoView = null;
            }
            PetControl.Instance.SendRequestFoSkillPointInfo();
        }

        public void ShowPet()
        {

            Singleton<PetView>.Instance.LoadPet(currentPPet.petId, gameObject.transform, new Vector3(-100, -10, 0));
        }

        private void SetPetInfo()
        {
            SysPet pet = BaseDataMgr.instance.GetDataById<SysPet>(currentPPet.petId);
            NGUITools.FindInChild<UISprite>(gameObject, "info/gradeicn").spriteName = PetLogic.GetGradeIcn(currentPPet.grade);
            if (currentPPet != null)
            {
                NGUITools.FindInChild<UISprite>(gameObject, "info/typeicn").spriteName =
                    PetLogic.GetTypeIcnName(pet.type);
                name.text = PetLogic.GetGradeDes(currentPPet.grade, pet.name);
                SetStars(currentPPet.star);

                NGUITools.FindInChild<UILabel>(gameObject, "info/levelvalue").text = currentPPet.lvl.ToString();

                SysPetGrowUp petExp = BaseDataMgr.instance.GetDataById<SysPetGrowUp>(currentPPet.lvl);
                float exp = petExp.exp_upgrade;
                NGUITools.FindInChild<UISlider>(gameObject, "info/exp").value = currentPPet.exp / exp;
                NGUITools.FindInChild<UILabel>(gameObject, "info/exp/label").text = currentPPet.exp + "/" + exp;

                int stonenum = Singleton<GoodsMode>.Instance.GetCountByGoodsId((uint)pet.stone_id);

                float need = 0f;
                if (currentPPet.star < 5)
                {
                    need = PetLogic.GetNeedStone(currentPPet.star, true);
                    NGUITools.FindInChild<UISlider>(gameObject, "info/stone/num").value = stonenum / need;
                }
                else
                {
                    NGUITools.FindInChild<UISlider>(gameObject, "info/stone/num").value = 1;
                }

                NGUITools.FindInChild<UILabel>(gameObject, "info/stone/num/label").text = stonenum + "/" + need;


                //出战处理
                if (currentPPet.state == 0)
                {
                    FightButton.FindInChild<UILabel>("lk").text = "出战";
                }
                else
                {
                    FightButton.FindInChild<UILabel>("lk").text = "休息";
                }
                SetPetSkill(currentPPet);
                SetPetEquips(currentPPet);
                SetPetProperty(currentPPet);
            }
        }

        private void SetExpItemInfo()
        {
            for (int i = 0;i<expObjs.Count;i++)
            {
                uint id = expItems[i];
                SysItemVo item = BaseDataMgr.instance.GetDataById<SysItemVo>(id);
                GameObject exp = expObjs[i];
                exp.name = id.ToString();
                NGUITools.FindInChild<UILabel>(exp, "name").text = item.name;
                NGUITools.FindInChild<UILabel>(exp, "exp").text = item.other.Replace("[", "(+").Replace("]",")");

                NGUITools.FindInChild<UISprite>(exp, "gradeicn").spriteName = "pz_"+item.color;
                //道具图集
                NGUITools.FindInChild<UISprite>(exp, "icn").spriteName = item.icon.ToString();
            }

            //默认选择第一个
            chose.transform.localPosition = expObjs[0].transform.localPosition;
            choseItemId = expItems[0];

        }

        private void SetExpItemNumInfo()
        {
            for (int i = 0; i < expObjs.Count; i++)
            {
                uint id = expItems[i];
                int num = Singleton<GoodsMode>.Instance.GetCountByGoodsId(id);
                UILabel numLabel = NGUITools.FindInChild<UILabel>(expObjs[i], "numvalue");
                numLabel.text =  num.ToString();
                if (num > 0)
                {
                    numLabel.color = ColorConst.FONT_YELLOW;
                }
                else
                {
                    numLabel.color = ColorConst.FONT_RED;
                }
            }
        }

        private void SetPetProperty(PPet pet)
        {
            PetVo petVo = Singleton<PetMode>.Instance.PetVos[pet.id];
            SysPetEvolutionVo petEvo = BaseDataMgr.instance.GetDataById<SysPetEvolutionVo>(pet.star);

            NGUITools.FindInChild<UILabel>(propetyObj, "growvalue").text =  (petEvo.grow/10000f).ToString();
            NGUITools.FindInChild<UILabel>(propetyObj, "hpvalue").text = petVo.Hp.ToString();
            NGUITools.FindInChild<UILabel>(propetyObj, "attackvalue").text = petVo.AttPMax.ToString();
            NGUITools.FindInChild<UILabel>(propetyObj, "pdvalue").text = petVo.DefP.ToString();
            NGUITools.FindInChild<UILabel>(propetyObj, "mdvalue").text = petVo.DefM.ToString();
            NGUITools.FindInChild<UILabel>(propetyObj, "decvalue").text = petVo.HurtRe.ToString();

            SysPet spet = BaseDataMgr.instance.GetDataById<SysPet>(pet.petId);
            SysSkillBaseVo skill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>((uint)spet.unique_skill);

            NGUITools.FindInChild<UILabel>(propetyObj, "des").text = "天赋技能:" + skill.desc;
            NGUITools.FindInChild<UILabel>(gameObject, "info/fightvalue").text = petVo.fight.ToString();

        }

        //设置此宠物的技能面板信息
        private void SetPetSkill(PPet pet)
        {
            int pos = 0;
            int[] skill1 = PetLogic.GetPetSkillInfo((int)pet.petId, 1); 
            uint[] skill = Singleton<PetMode>.Instance.GetPetSkill(pet.id, 1);
            NGUITools.FindInChild<UILabel>(skillObjs[0], "name").text = PetLogic.GetSkillName(skill1[0], pos);
            NGUITools.FindInChild<UISprite>(skillObjs[0], "skillicn").spriteName = PetLogic.GetSkillIcn(skill1[0]);
            SetSkillInfo(skill,pet,1);

            int[] skill2 = PetLogic.GetPetSkillInfo((int)pet.petId, 2);
            if (skill2[0] == skill1[0])
            {
                pos = 1;
            }
            skill = Singleton<PetMode>.Instance.GetPetSkill(pet.id, 2);
            NGUITools.FindInChild<UILabel>(skillObjs[1], "name").text = PetLogic.GetSkillName(skill2[0], pos);
             NGUITools.FindInChild<UISprite>(skillObjs[1], "skillicn").spriteName = PetLogic.GetSkillIcn(skill2[0]);
            SetSkillInfo(skill, pet, 2);

            int[] skill3 = PetLogic.GetPetSkillInfo((int)pet.petId, 3);
            pos = 0;
            if (skill3[0] == skill1[0])
            {
                pos ++;
            }
            if (skill3[0] == skill2[0])
            {
                pos++;
            }

            skill = Singleton<PetMode>.Instance.GetPetSkill(pet.id, 3);
             NGUITools.FindInChild<UILabel>(skillObjs[2], "name").text = PetLogic.GetSkillName(skill3[0],pos);
             NGUITools.FindInChild<UISprite>(skillObjs[2], "skillicn").spriteName = PetLogic.GetSkillIcn(skill3[0]);
            SetSkillInfo(skill, pet, 3);

            int[] skill4 = PetLogic.GetPetSkillInfo((int)pet.petId, 4);
            pos = 0;
            if (skill4[0] == skill1[0])
            {
                pos++;
            }
            if (skill4[0] == skill2[0])
            {
                pos++;
            }
            if (skill4[0] == skill3[0])
            {
                pos++;
            }

            skill = Singleton<PetMode>.Instance.GetPetSkill(pet.id, 4);
             NGUITools.FindInChild<UILabel>(skillObjs[3], "name").text = PetLogic.GetSkillName(skill4[0],pos);
             NGUITools.FindInChild<UISprite>(skillObjs[3], "skillicn").spriteName = PetLogic.GetSkillIcn(skill4[0]);
            SetSkillInfo(skill, pet, 4);

            //宠物天赋技能
            SysPet spet = BaseDataMgr.instance.GetDataById<SysPet>(pet.petId);
            SysSkillBaseVo skillVo = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>((uint) spet.unique_skill + pet.star -1);
            NGUITools.FindInChild<UILabel>(skillObjs[4], "name").text = skillVo.name;
            NGUITools.FindInChild<UILabel>(skillObjs[4], "level").text = "Lv."+skillVo.skill_lvl;
            NGUITools.FindInChild<UILabel>(skillObjs[4], "des").text = "说明;" + skillVo.desc;
            NGUITools.FindInChild<UISprite>(skillObjs[4], "skillicn").spriteName = skillVo.icon.ToString();

            SetPetSkillPointInfo();
        }

        private void SetPetSkillPointInfo()
        {
            PetSkillPointInfoMsg_21_7 skillPoint = Singleton<PetMode>.Instance.SkillPoint;
            NGUITools.FindInChild<UILabel>(skillObj, "pointvalue").text = skillPoint.point.ToString();
            if (skillPoint.point == 0)
            {
                NGUITools.FindInChild<UILabel>(skillObj, "pointvalue").color = ColorConst.FONT_RED;
                NGUITools.FindChild(skillObj, "add").SetActive(true);
            }
            else
            {
                NGUITools.FindChild(skillObj, "add").SetActive(false);
                NGUITools.FindInChild<UILabel>(skillObj, "pointvalue").color = ColorConst.FONT_YELLOW;
            }
            UpdateTimeInfo();
        }

        public void UpdateTimeInfo()
        {
            if (!ReferenceEquals(gameObject, null) && gameObject.activeSelf)
            {
                PetSkillPointInfoMsg_21_7 skillPoint = Singleton<PetMode>.Instance.SkillPoint;
                NGUITools.FindInChild<UILabel>(skillObj, "pointvalue").text = skillPoint.point.ToString();
                if (skillPoint.point == 10)
                {
                    NGUITools.FindInChild<UILabel>(skillObj, "time").text = "(已满)";
                }
                else
                {
                    int leftTime = (int) skillPoint.timestamp - ServerTime.Instance.Timestamp;
                    NGUITools.FindInChild<UILabel>(skillObj, "time").text =
                        GetTimeDes(leftTime);
                }
            }
        }

        private string GetTimeDes(int s)
        {
            if (s > 0)
            {
                int m = s/60;
                s = s%60;
                string min = m.ToString();
                if (m < 10)
                {
                    m = 0 + m;
                }

                string sec = s.ToString();
                if (s < 10)
                {
                    sec = 0 + sec;
                }

                return "(" + min + ":" + sec + ")";
            }
            else
            {
                return "(00:00)";
            }
        }


        private void SetSkillInfo(uint[] pskill,PPet pet,int num)
        {
            if (pskill != null)
            {
                NGUITools.FindInChild<UILabel>(skillObjs[num - 1], "level").text = "Lv." + pskill[1];
                UIUtils.ChangeNormalShader(NGUITools.FindInChild<UISprite>(skillObjs[num - 1], "skillicn"), 20);
                var add = NGUITools.FindChild(skillObjs[num - 1], "add");
                if(add==null) return;
                add.SetActive(true);
                int cost = PetLogic.GetSkillSpend((int)pskill[1], num);
                NGUITools.FindInChild<UILabel>(skillObjs[num - 1], "add/costvalue").text = cost.ToString();
                if (pskill[1] >= pet.lvl)
                {
                    NGUITools.FindInChild<UISprite>(skillObjs[num - 1], "add/background").spriteName = "jiah";
                    NGUITools.FindInChild<BoxCollider>(skillObjs[num - 1], "add").enabled = false;
                }
                else
                {
                    NGUITools.FindInChild<UISprite>(skillObjs[num - 1], "add/background").spriteName = "jia";
                    NGUITools.FindInChild<BoxCollider>(skillObjs[num - 1], "add").enabled = true;
                    NGUITools.FindInChild<Button>(skillObjs[num - 1], "add").onClick = OnSkillUpgradeClick;
                    if (cost > MeVo.instance.diam)
                    {
                        NGUITools.FindInChild<UILabel>(skillObjs[num - 1], "add/costvalue").color = ColorConst.FONT_RED;
                    }
                    else
                    {
                        NGUITools.FindInChild<UILabel>(skillObjs[num - 1], "add/costvalue").color =
                            ColorConst.FONT_YELLOW;
                    }
                }
            }
            else
            {
                if (num == 1)
                {
                    NGUITools.FindInChild<UILabel>(skillObjs[num-1], "level").text = "(进阶到绿色开启)";
                }
                else if (num == 2)
                {
                    NGUITools.FindInChild<UILabel>(skillObjs[num-1], "level").text = "(进阶到蓝色开启)";
                }
                else if (num == 3)
                {
                    NGUITools.FindInChild<UILabel>(skillObjs[num-1], "level").text = "(进阶到紫色开启)";
                }
                else
                {
                    NGUITools.FindInChild<UILabel>(skillObjs[num-1], "level").text = "(进阶到橙色开启)";
                }

                UIUtils.ChangeGrayShader(NGUITools.FindInChild<UISprite>(skillObjs[num-1], "skillicn"), 21);

                NGUITools.FindChild(skillObjs[num-1],"add").SetActive(false);
            }
        }

        //设置宠物的装备信息
        private void SetPetEquips(PPet pet)
        {
            for (int i = 0; i < equipObjs.Count; i++)
            {
                int equip = PetLogic.CheckEquip(pet.petId, i + 1);
                int need = PetLogic.GetNeedEquip(pet.petId, i + 1);
                SysEquipVo equipVo = BaseDataMgr.instance.GetDataById<SysEquipVo>((uint)need);

                UISprite sprite = NGUITools.FindInChild<UISprite>(equipObjs[i], "bicn");
                sprite.spriteName = equipVo.icon.ToString();
                
                if (equip <= 3)
                {
                    //置灰
                    UIUtils.ChangeGrayShader(sprite,15);
                    NGUITools.FindChild(equipObjs[i], "label").SetActive(true);
                    NGUITools.FindInChild<UISprite>(equipObjs[i], "gradeicn").spriteName = "tbk";

                    if (equip == 0) //不存在
                    {
                        NGUITools.FindInChild<UILabel>(equipObjs[i], "label").text = "无装备";
                        NGUITools.FindInChild<UILabel>(equipObjs[i], "label").color = ColorConst.FONT_MILKWHITE;
                        NGUITools.FindChild(equipObjs[i], "tipicn").SetActive(false);

                    }
                    else if(equip == 1) //存在可装备
                    {
                        NGUITools.FindInChild<UILabel>(equipObjs[i], "label").text = "可装备";
                        NGUITools.FindInChild<UILabel>(equipObjs[i], "label").color = ColorConst.FONT_GREEN;
                        NGUITools.FindChild(equipObjs[i], "tipicn").SetActive(true);
                        NGUITools.FindInChild<UISprite>(equipObjs[i], "tipicn").spriteName = "add2";
                    }else if (equip == 2) //存在不可装备
                    {
                        NGUITools.FindInChild<UILabel>(equipObjs[i], "label").text = "未装备";
                        NGUITools.FindInChild<UILabel>(equipObjs[i], "label").color = ColorConst.FONT_MILKWHITE;
                        NGUITools.FindChild(equipObjs[i], "tipicn").SetActive(true);
                        NGUITools.FindInChild<UISprite>(equipObjs[i], "tipicn").spriteName = "add";
                    }else if (equip == 3) //可合成可装备
                    {
                        NGUITools.FindInChild<UILabel>(equipObjs[i], "label").text = "可合成";
                        NGUITools.FindInChild<UILabel>(equipObjs[i], "label").color = ColorConst.FONT_GREEN;
                        NGUITools.FindChild(equipObjs[i], "tipicn").SetActive(true);
                        NGUITools.FindInChild<UISprite>(equipObjs[i], "tipicn").spriteName = "add2";
                    }else if (equip == 4) //可合成不可装备
                    {
                        NGUITools.FindInChild<UILabel>(equipObjs[i], "label").text = "可合成";
                        NGUITools.FindInChild<UILabel>(equipObjs[i], "label").color = ColorConst.FONT_MILKWHITE;
                        NGUITools.FindChild(equipObjs[i], "tipicn").SetActive(true);
                        NGUITools.FindInChild<UISprite>(equipObjs[i], "tipicn").spriteName = "add";
                    }
                }
                else
                {
                    UIUtils.ChangeNormalShader(sprite, 14);
                    NGUITools.FindChild(equipObjs[i], "label").SetActive(false);
                    NGUITools.FindChild(equipObjs[i], "tipicn").SetActive(false);
                    NGUITools.FindInChild<UISprite>(equipObjs[i], "gradeicn").spriteName = "epz_"+equipVo.grade;
                }
            }
            if (PetLogic.CanUpgrade(currentPPet))
            {
                //装备已经满了
                NGUITools.FindChild(gameObject, "info/function/upgrade/highlight").SetActive(true);
                NGUITools.FindInChild<TweenAlpha>(gameObject, "info/function/upgrade/highlight").enabled = true;

            }
            else
            {
                NGUITools.FindChild(gameObject, "info/function/upgrade/highlight").SetActive(false);
                NGUITools.FindInChild<TweenAlpha>(gameObject, "info/function/upgrade/highlight").enabled = false;
            }

            if (PetLogic.CanEvolve(currentPPet))
            {
                NGUITools.FindChild(gameObject, "info/function/evolve/highlight").SetActive(true);
                NGUITools.FindInChild<TweenAlpha>(gameObject, "info/function/evolve/highlight").enabled = true;

            }
            else
            {
                NGUITools.FindChild(gameObject, "info/function/evolve/highlight").SetActive(false);
                NGUITools.FindInChild<TweenAlpha>(gameObject, "info/function/evolve/highlight").enabled = false;
            }


        }

        //升级宠物技能
        private void OnSkillUpgradeClick(GameObject obj)
        {
            if (currentPPet != null)
            {
                int num = int.Parse(obj.transform.parent.gameObject.name);
                int[] skill = PetLogic.GetPetSkillInfo((int) currentPPet.petId, num);
                uint[] pskill = Singleton<PetMode>.Instance.GetPetSkill(currentPPet.id,(uint) num);
                int cost = PetLogic.GetSkillSpend((int)pskill[1], num);
                if (cost > MeVo.instance.diam)
                {
                    MessageManager.Show("当前金币不足!");
                }
                else if(Singleton<PetMode>.Instance.SkillPoint.point<=0)
                {
                    MessageManager.Show("技能点不足!");
                }
                else
                {
                    Singleton<PetControl>.Instance.UpgradePetSkill(currentPPet.id, (uint)num);
                }
            }

        }

        private void OnSkillPress(GameObject obj,bool press)
        {
            int num = int.Parse(obj.name);
            int[] skill = PetLogic.GetPetSkillInfo((int)currentPPet.petId, num);
            uint[] pskill = Singleton<PetMode>.Instance.GetPetSkill(currentPPet.id,(uint) num);
            int lvl = 0;
            if (pskill != null)
            {
                lvl = (int)pskill[1];
            }
            if (press)
            {
                skillTips.transform.localPosition = new Vector3(-5,190-(num-1)*91);
                skillTips.SetActive(true);
                NGUITools.FindInChild<UILabel>(skillTips, "label").text = PetLogic.GetSkillDes((int)currentPPet.petId, num, lvl);
            }
            else
            {
                skillTips.SetActive(false);
            }

        }

        private  void DataUpdated(object sender, int code)
        {
            if (sender == Singleton<PetMode>.Instance)
            {
                if (code == PetMode.UpdatedPet && currentPPet == Singleton<PetMode>.Instance.CurrentPet)
                {
                    SetPetInfo();
                }
                if (code == PetMode.EvolvePet)
                {
                   Singleton<PetTipsView>.Instance.OpenViewForEvolve(Singleton<PetMode>.Instance.CurrentPet.id);
                }

                if (code == PetMode.SkillUpgrade && current == skill)
                {
                    int num = (int) Singleton<PetMode>.Instance.UpgradedSkill[0];
                    NGUITools.FindInChild<UILabel>(skillObjs[num - 1], "addtips").text =
                        PetLogic.GetSkillAddDes((int)currentPPet.petId, num);
                    NGUITools.FindChild(skillObjs[num - 1], "addtips").SetActive(true);
                    NGUITools.FindInChild<TweenPlay>(skillObjs[num - 1], "addtips").PlayForward();
                    Singleton<PetView>.Instance.PetPlay(Status.ATTACK1);
                }

                if (code == PetMode.WearEquip)
                {
                    Singleton<PetEquipView>.Instance.CloseView();

                    uint[] equip = Singleton<PetMode>.Instance.WearedEquip;

                    equipTip.transform.localPosition = equipObjs[(int)equip[0] - 1].transform.localPosition;
                    equipTip.SetActive(true);
                    equipTipPlay.PlayForward();

                    Singleton<PetView>.Instance.PetPlay(Status.ATTACK1);

                    SysEquipVo equipvo = BaseDataMgr.instance.GetDataById<SysEquipVo>(equip[1]);

                    PlayPetTips(PetLogic.GetEquipPropertyDes(equipvo));
                }

                if (code==PetMode.FightPet)
                {
                    Singleton<PetView>.Instance.PetPlay(Status.Win);
                }

                if (code == PetMode.GradeUpgrade)
                {
                    PlayEquips();
                    Singleton<PetView>.Instance.PetPlay(Status.Win);
                    PlayPetTips("幻兽成功升阶!");
                }
                if (code == PetMode.LevelUpgrade)
                {
                    Singleton<PetView>.Instance.PetPlay(Status.Win);
                    PlayPetTips("幻兽等级提升!");
                }
                if (code == PetMode.SkillPointInfo)
                {
                    SetPetSkillPointInfo();
                }
            }else 
            if (sender == MeVo.instance)
            {
                if (currentPPet != null)
                {
                    SetPetSkill(currentPPet);
                }

            }
            else if (sender == Singleton<GoodsMode>.Instance && code == Singleton<GoodsMode>.Instance.UPDATE_PET_GOODS)
            {
                if (currentPPet != null)
                {
                    SetPetEquips(currentPPet);
                }
                SetExpItemNumInfo();
            }
        }


        private void SetStars(uint starNum)
        {
            for (int i = 0; i < stars.Count; i++)
            {
                if (i < starNum )
                {
                    stars[i].spriteName = "xingxing1";
                }
                else
                {
                    stars[i].spriteName = "kongxing";
                }
            }
        }

        private void OnFightClick(GameObject obj)
        {
            if (currentPPet.state == 0)
            {
                Singleton<PetControl>.Instance.SendPetFight(currentPPet.id, true);
            }
            else
            {
                Singleton<PetControl>.Instance.SendPetFight(currentPPet.id, false);
            }
        }

        private void OnEvolveClick(GameObject obj)
        {
            //检查可以进化
            if (PetLogic.CanEvolve(currentPPet))
            {
                Singleton<PetControl>.Instance.SendPetEvolve(currentPPet.id);
            }
            else
            {
                if (currentPPet.star == 5)
                {
                    MessageManager.Show("已进化到满星!");
                }
                else
                {
                    MessageManager.Show("灵魂石不足");
                }
            }

        }

        private void OnUpgradeClick(GameObject obj)
        {
            if (PetLogic.CanUpgrade(currentPPet))
            {
                Singleton<PetControl>.Instance.SendPetUpgrade(currentPPet.id);
            }
            else
            {
                if (currentPPet.grade == 14)
                {
                    MessageManager.Show("幻兽已升到最高阶");
                }
                else
                {
                    MessageManager.Show("幻兽需穿齐装备才可以进阶");  
                }  
            }
        }

        //升阶动画
        private void PlayEquips()
        {
            if (currentPPet.grade > 1)
            {
                SysPet pet = BaseDataMgr.instance.GetDataById<SysPet>(currentPPet.petId);
                int[] equipConfigs = PetLogic.GetPetNeedEquips((uint) pet.type, (uint) currentPPet.grade - 1);
                for (int i = 0; i < equipObjs.Count; i++)
                {
                    GameObject icn = NGUITools.FindChild(equipObjs[i], "icn");
                    SysEquipVo equipVo = BaseDataMgr.instance.GetDataById<SysEquipVo>((uint) equipConfigs[i]);
                    icn.GetComponent<UISprite>().spriteName = equipVo.icon.ToString();
                    icn.SetActive(true);
                    icn.GetComponent<TweenPlay>().PlayForward();
                }
                light.SetActive(true);
                light.GetComponent<TweenPlay>().PlayForward();
            }
        }

        private void PlayEquipsFinish()
        {
            foreach (GameObject obj in equipObjs)
            {
                NGUITools.FindChild(obj, "icn").SetActive(false);
            }
        }

        private void OnCloseClick(GameObject obj)
        {
            currentPPet = null;
            Singleton<PetMode>.Instance.dataUpdated -= DataUpdated;
            MeVo.instance.DataUpdated -= DataUpdated;
            Singleton<GoodsMode>.Instance.dataUpdated -= DataUpdated;
            gameObject.SetActive(false);
           
        }

        private void OnAddSkillPointClick(GameObject obj)
        {
            int times = VIPLogic.GetVipNum(18, MeVo.instance.vip);
            if (times == 0) //次数为0
            {
                int vipLevel = VIPLogic.GetOpenVipLevel(18);
                MessageManager.Show("VIP等级不足，需要VIP等级"+vipLevel);
            }
            else if(times>0)
            {
                int usedTimes = Singleton<PetMode>.Instance.SkillPoint.buyTimes;
                int leftTimes = times - usedTimes;
                if (leftTimes <= 0)
                {
                    int nextVip = VIPLogic.GetNextVipLevel(18, MeVo.instance.vip);
                    if (nextVip != 0)
                    {
                        MessageManager.Show("剩余次数不足，升级到VIP" + nextVip + "开启更多次数！");
                    }
                    else
                    {
                        MessageManager.Show("今日可购买次数已用完!");
                    }
                }
                else
                {
                    //展示需要的砖石和剩余次数
                    SysPriceVo vo = BaseDataMgr.instance.GetDataById<SysPriceVo>(2100);
                    int[] cost = StringUtils.GetStringToInt(vo.diam);
                    int need = cost[0] + cost[1]*usedTimes;
                    if (need > cost[2])
                    {
                        need = cost[2];
                    }

                    ConfirmMgr.Instance.ShowSelectOneAlert("花费"+ColorConst.YELLOW+" "+need+"[-] 钻石购买10点技能点？",ConfirmCommands.SELECT_ONE,BuySkillPoint,"确定",null,"取消");
                    
                }
            }
        }

        private void BuySkillPoint()
        {
             Singleton<PetControl>.Instance.BuySkillPoint();
        }

    }
}
