using System.Linq;
using com.game.consts;
using com.game.data;
using com.game.manager;
using com.game.module.Guide;
using Com.Game.Module.Manager;
using com.game.Public.Message;
using com.game.Public.UICommon;
using com.game.vo;
﻿﻿﻿using UnityEngine;
using com.game.module.test;
using System.Collections.Generic;


namespace Com.Game.Module.Role
{
	/// <summary>
	/// 登录视图。
	/// </summary>
    public class SkillView : BaseView<SkillView>
	{   
		/// <summary>
		/// 预设路径
		/// </summary>
        public override string url{get {return "UI/Skill/SkillView.assetbundle";}}

		public override ViewLayer layerType {
			get {
				return ViewLayer.MiddleLayer;
			}
		}

	    private int level;
	    private uint money;

	    private List<GameObject> skillList;//技能
	    private List<GameObject> skillPosList; //技能位置

	    private UIToggle zdSkill; //主动技能
	    private UIToggle bdSkill; //被动技能
	    private UIToggle currentUIToggel; //当前页面

	    private SysSkillBaseVo currentSkill;//当前选择技能

	    private GameObject chose;//选中图标

	    private UIAtlas skillAtlas; //技能图集

	    private GameObject posChose; //位置信息
	    public EventDelegate.Callback GuideSkillInitCallback;
	    public Button GuideSkillButton;
	    public Button UpgradeButton;
	    public Button GuideSkillPositionButton;
	    public Button CloseButton;
	    public int CurrentGuideId;

	    public override bool isDestroy
	    {
	        get { return true; }
	    }

	    public override bool IsFullUI
	    {
	        get { return true; }
	    }

	    protected override  void Init()
		{
            skillList = new List<GameObject>();
		    for (int i = 1; i < 13; i++)
		    {
                skillList.Add(FindChild("skilllist/"+i));
		        skillList[i - 1].GetComponent<Button>().onClick = OnSkillClick;
		    }

		    skillPosList = new List<GameObject>();
            for (int i = 1; i < 6; i++)
            {
                skillPosList.Add(FindChild("skillpos/" + i));
                skillPosList[i - 1].GetComponent<Button>().onClick = OnSkillPosClick;
            }
            chose = FindChild("skilllist/chose");
            chose.SetActive(false);

            posChose = FindChild("skillpos/chose");
            posChose.SetActive(false);

		    zdSkill = FindInChild<UIToggle>("button_zd");
            bdSkill = FindInChild<UIToggle>("button_bd");
		    zdSkill.onStateChange = OnStateChange;
		    bdSkill.onStateChange = OnStateChange;
		    currentUIToggel = zdSkill;
            CloseButton = FindInChild<Button>("close");
            CloseButton.onClick = OnCloseClick;
            UpgradeButton = NGUITools.FindInChild<Button>(gameObject, "skillinfo/upgrade");
            UpgradeButton.onClick = OnUpgradeClick;
		}


        protected override void HandleAfterOpenView()
        {
            gameObject.SetActive(false);

            if (skillAtlas == null)
            {
                Singleton<AtlasManager>.Instance.LoadAtlasHold(AtlasUrl.SkillIconHold, AtlasUrl.SkillIconNormal, LoadAtlas, true);
            }
            else
            {
                DelayShow();
            }
        }

        private void LoadAtlas(UIAtlas atlas)
        {
            if (skillAtlas == null)
            {
                skillAtlas = atlas;
                UIAtlas grayAtlas = Singleton<AtlasManager>.Instance.GetAtlas(AtlasUrl.SkillIconGray);
                chose.GetComponent<UISprite>().atlas = skillAtlas;
                posChose.GetComponent<UISprite>().atlas = skillAtlas;
                foreach (GameObject obj in skillList)
                {
                    UISprite icon = NGUITools.FindInChild<UISprite>(obj, "icon");
                    icon .atlas = skillAtlas;
                    icon.normalAtlas = skillAtlas;
                    icon.grayAtlas = grayAtlas;
                    NGUITools.FindInChild<UISprite>(obj, "jn").atlas = atlas;
                }

                foreach (GameObject obj in skillPosList)
                {
                    NGUITools.FindInChild<UISprite>(obj, "background").atlas = atlas;
                    NGUITools.FindInChild<UISprite>(obj, "highlight").atlas = atlas;
                    NGUITools.FindInChild<UISprite>(obj, "icon").atlas = atlas;
                }
                //设置职业普攻图标
                NGUITools.FindInChild<UISprite>(gameObject, "skillpos/jobicn").atlas = atlas;
                FindInChild<UISprite>("skillpos/jobicn").spriteName = MeVo.instance.job + "0";

                NGUITools.FindInChild<UISprite>(gameObject, "skillinfo/icon").atlas = atlas;
                NGUITools.FindInChild<UISprite>(gameObject, "skillinfo/jn").atlas = atlas;

                DelayShow();
            }
        }

        private void DelayShow()
        {
            //技能数据已经初始化
            if (Singleton<SkillMode>.Instance.dataInit)
            {
                level = MeVo.instance.Level;
                money = MeVo.instance.diam;
                SetAllSkillInfo();
            }
            else
            {
                //技能数据未初始化
                gameObject.SetActive(false);
                Singleton<SkillControl>.Instance.SendRequstForSKillsInfo();
            }
            gameObject.SetActive(true);
        }


	    private void InitSkillObjs()
	    {
	        foreach (GameObject obj in skillList)
	        {
	            obj.name = "0";
	            NGUITools.FindInChild<UISprite>(obj, "icon").spriteName = "suo23";
                NGUITools.FindInChild<UISprite>(obj, "icon").depth = 22;
                NGUITools.FindChild(obj, "tips").SetActive(false);
                NGUITools.FindChild(obj, "lvl").SetActive(false);
	        }
            foreach (GameObject obj in skillPosList)
	        {
                NGUITools.FindInChild<UISprite>(obj, "icon").spriteName = "suo23";
                NGUITools.FindChild(obj, "tips").SetActive(false);
	        }

	        NGUITools.FindInChild<UISprite>(gameObject, "skillinfo/icon").spriteName = "suo23";
            NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/skillname").text = "未开启";

            NGUITools.FindChild(gameObject,"skillinfo/1").SetActive(false);
            NGUITools.FindChild(gameObject, "skillinfo/2").SetActive(false);
            UpgradeButton.SetActive(false);
	    }


	    //技能位置点击
	    private void OnSkillClick(GameObject obj)
	    {
	        if (!obj.name.Equals("0"))
	        {
	            uint skillId = uint.Parse(obj.name);
	            SysSkillBaseVo skill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>(skillId);
                if (!chose.activeSelf || currentSkill != skill)
	            {   chose.SetActive(true);
	                chose.transform.localPosition = obj.transform.localPosition;
	                currentSkill = skill;
	                SetSkillInfo(skill);
	                SetSkillPosChoseInfo();
	            }
	        }
	        else
	        {
                MessageManager.Show("未开放");
	        }
	    }

        private void SetSkillInfo(SysSkillBaseVo skill)
        {
            NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/skillname").text = skill.name;
            NGUITools.FindInChild<UISprite>(gameObject, "skillinfo/icon").spriteName = skill.icon.ToString();

            SkillState state = SkillViewLogic.GetSkillState(skill);
            NGUITools.FindChild(gameObject,"skillinfo/1").SetActive(true);
            if (state == SkillState.Max) //满级
            {
                NGUITools.FindChild(gameObject, "skillinfo/2").SetActive(false);
                UpgradeButton.SetActive(true);

                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/lvl").text = "当前等级：" + skill.skill_lvl + " (满级)";
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/lvl").color = ColorConst.FONT_YELLOW;
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/time").text = "冷却时间：" + skill.cd / 1000 + "s";
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/cost").text = "魔法消耗：" + skill.need_value;
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/des").text = "技能说明：" + skill.desc;

            }
            else if(state == SkillState.Learn) //学习
            {
                skill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>((uint) skill.next);
                NGUITools.FindChild(gameObject, "skillinfo/2").SetActive(false);
                UpgradeButton.SetActive(true);
                UpgradeButton.FindInChild<UILabel>("needvalue").text = skill.point.ToString();
                UpgradeButton.FindInChild<UILabel>("label").text = "学  习";
                UpgradeButton.FindInChild<UILabel>("leftvalue").text = Singleton<SkillMode>.Instance.LeftPoint().ToString();

                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/lvl").text = "当前等级：" + skill.skill_lvl + " (未学习)";
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/lvl").color = ColorConst.FONT_WHITE;
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/time").text = "冷却时间：" + (float)skill.cd / 1000 + "s";
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/cost").text = "魔法消耗：" + skill.need_value;
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/des").text = "技能说明：" + skill.desc;

                if (skill.point > Singleton<SkillMode>.Instance.LeftPoint())
                {
                    UpgradeButton.FindInChild<UILabel>("needvalue").color = ColorConst.FONT_RED;
                }
                else
                {
                    UpgradeButton.FindInChild<UILabel>("needvalue").color = ColorConst.FONT_YELLOW;
                }
            }
            else if (state == SkillState.Upgrade)//升级
            {
                NGUITools.FindChild(gameObject, "skillinfo/2").SetActive(true);
                UpgradeButton.SetActive(true);

                SysSkillBaseVo nextSkill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>((uint)skill.next);

                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/lvl").text = "当前等级：" + skill.skill_lvl;
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/lvl").color = ColorConst.FONT_YELLOW;
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/time").text = "冷却时间：" + (float)skill.cd / 1000 + "s";
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/cost").text = "魔法消耗：" + skill.need_value;
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/1/des").text = "技能说明：" + skill.desc;

                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/2/lvl").text = "下一等级：" + nextSkill.skill_lvl;
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/2/time").text = "冷却时间：" + (float)nextSkill.cd / 1000 + "s";
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/2/cost").text = "魔法消耗：" + nextSkill.need_value;
                NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/2/des").text = "技能说明：" + nextSkill.desc;

                if (SkillViewLogic.IsConditionEnough(nextSkill))
                {
                    NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/2/lvl").color = ColorConst.FONT_YELLOW;
                }
                else
                {
                    NGUITools.FindInChild<UILabel>(gameObject, "skillinfo/2/lvl").color = ColorConst.FONT_RED;
                }
                UpgradeButton.FindInChild<UILabel>("label").text = "升  级";
                UpgradeButton.FindInChild<UILabel>("needvalue").text = nextSkill.point.ToString();
                UpgradeButton.FindInChild<UILabel>("leftvalue").text = Singleton<SkillMode>.Instance.LeftPoint().ToString();

                if (skill.point > Singleton<SkillMode>.Instance.LeftPoint())
                {
                    UpgradeButton.FindInChild<UILabel>("needvalue").color = ColorConst.FONT_RED;
                }
                else
                {
                    UpgradeButton.FindInChild<UILabel>("needvalue").color = ColorConst.FONT_YELLOW;
                }
            }
            NGUITools.FindChild(gameObject, "skillinfo/2/time").SetActive(skill.active);
            NGUITools.FindChild(gameObject, "skillinfo/2/cost").SetActive(skill.active);
            NGUITools.FindChild(gameObject, "skillinfo/1/time").SetActive(skill.active);
            NGUITools.FindChild(gameObject, "skillinfo/1/cost").SetActive(skill.active);
        }
                                              
	    //技能位置点击
        private void OnSkillPosClick(GameObject obj)
        {
            int pos = int.Parse(obj.name);
            uint position = Singleton<SkillMode>.Instance.SkillsPos[pos - 1];
            if (currentSkill != null && currentSkill.position != position)
            {
                SkillState state = SkillViewLogic.GetSkillState(currentSkill);
                if (currentUIToggel == bdSkill)
                {
                    MessageManager.Show("被动技能不可设置到技能栏!");
                }else if (state == SkillState.Learn)
                {
                    MessageManager.Show("该技能尚未学习!");
                }
                else
                {
                    uint[] skillPos = new uint[5];
                    uint[] oldSkillPos = Singleton<SkillMode>.Instance.SkillsPos;
                    for (int i = 0; i < oldSkillPos.Count();i++)
                    {
                        if (oldSkillPos[i] == currentSkill.position)
                        {
                            skillPos[i] = 0;
                        }
                        else
                        {
                            skillPos[i] = oldSkillPos[i];
                        }
                    }
                    skillPos[pos - 1] = (uint)currentSkill.position;
                    Singleton<SkillControl>.Instance.SetSkillPosValue(skillPos);
                }
            }
        }

	    private void OnUpgradeClick(GameObject obj)
	    {
	        if (currentSkill != null)
	        {
	            if (currentSkill.next != 0)
	            {
	                SysSkillBaseVo nextSkill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>((uint) currentSkill.next);
	                if (!SkillViewLogic.IsLevelEnough(nextSkill))
	                {
                        MessageManager.Show("角色等级不足,请升级主角等级到"+nextSkill.lvl+"级!");
	                }else

                    if (!SkillViewLogic.IsSkillPointEnough(nextSkill))
                    {
                        MessageManager.Show("灵魂点不足!");
                    }else if (!SkillViewLogic.IsPreSkillEnough(nextSkill))
                    {
                        SysSkillBaseVo preSkill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>((uint) nextSkill.pre);
                        MessageManager.Show("请先学习技能 " + preSkill.name + " 到 " + preSkill.skill_lvl + " 级");
                    }
                    else
                    {
                        Singleton<SkillControl>.Instance.LearnSkill((uint)currentSkill.id);
                    }
	            }
	            else
	            {
                    MessageManager.Show("当前技能已经满级");
	            }
	           
	        }
	    }

	    //技能类型切换
	    private void OnStateChange( bool state)
	    {
            if (state)
	        {
	            if (UIToggle.current == zdSkill && currentUIToggel != zdSkill)
	            {
	                currentUIToggel = zdSkill;
	                currentSkill = null;
                    SetToggleLable(NGUITools.FindChild(zdSkill.gameObject, "label"), true);
                    SetToggleLable(NGUITools.FindChild(bdSkill.gameObject, "label"), false);
                    SetAllSkillInfo();

	            }
                else if (UIToggle.current == bdSkill && currentUIToggel != bdSkill)
                {
                    currentUIToggel = bdSkill;
                    currentSkill = null;
                    SetToggleLable(NGUITools.FindChild(zdSkill.gameObject, "label"), false);
                    SetToggleLable(NGUITools.FindChild(bdSkill.gameObject, "label"), true);
                    SetAllSkillInfo();
                }
                
	        }
	    }

	    private void SetToggleLable(GameObject obj, bool chose)
	    {
	        if (chose)
	        {
	            obj.transform.localPosition = new Vector3(-22, 0, 0);
	            obj.GetComponent<UILabel>().color = ColorConst.FONT_WHITE;
	        }
	        else
	        {
                obj.transform.localPosition = new Vector3(-16, 0, 0);
                obj.GetComponent<UILabel>().color = ColorConst.FONT_GRAY;
	        }
	    }

	    /// <summary>
        /// 设置全部的技能信息
        /// </summary>
        private void SetAllSkillInfo()
	    {
	        InitSkillObjs();
            bool active = currentUIToggel == zdSkill;

	        if (active)
	        {
	            NGUITools.FindChild(bdSkill.gameObject, "tips").SetActive(Singleton<SkillMode>.Instance.IfShowTips(false));
                NGUITools.FindChild(zdSkill.gameObject, "tips").SetActive(false);
	        }
	        else
	        {
                NGUITools.FindChild(zdSkill.gameObject, "tips").SetActive(Singleton<SkillMode>.Instance.IfShowTips(true));
                NGUITools.FindChild(bdSkill.gameObject, "tips").SetActive(false);
	        }

	        List<SysSkillBaseVo> skills =  Singleton<SkillMode>.Instance.CurrentSkills(active);
            for (int i = 0;i<skills.Count;i++)
            {
                SysSkillBaseVo skill = skills[i];
                if (skills[i] != null)
                {
                    GameObject skillObj = skillList[i];
                    skillObj.name = skill.id.ToString();
                    SkillState state = SkillViewLogic.GetSkillState(skill);
                    NGUITools.FindInChild<UISprite>(skillObj, "icon").spriteName = skill.icon.ToString();
                    if (state == SkillState.Learn) //学习状态-置灰
                    {
                        UIUtils.ChangeGrayShader(NGUITools.FindInChild<UISprite>(skillObj, "icon"), 23);
                    }
                    else
                    {
                        NGUITools.FindChild(skillObj,"lvl").SetActive(true);
                        NGUITools.FindInChild<UILabel>(skillObj, "lvl").text = "Lv." + skill.skill_lvl;
                        UIUtils.ChangeNormalShader(NGUITools.FindInChild<UISprite>(skillObj, "icon"), 22);
                    }
                    if (state != SkillState.Max)
                    {
                        SysSkillBaseVo nextSkill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>((uint)skill.next);
                        if (SkillViewLogic.IsConditionEnough(nextSkill))
                        {
                            NGUITools.FindChild(skillObj,"tips").SetActive(true);
                        }
                    }
                    if (currentSkill != null && currentSkill.position == skills[i].position && currentSkill.group == skills[i].group)
                    {
                        OnSkillClick(skillObj);
                    }
                } 
            }
	        if (currentSkill == null)
	        {
                OnSkillClick(skillList[0]);
	        }
	        SetSkillsPosInfo();
	        SetGuideButton();
	        CheckGuide();
	    }


	    private void CheckGuide()
	    {
            if (GuideSkillInitCallback != null)
            {
                GuideSkillInitCallback();
                GuideSkillInitCallback = null;
            }
	    }


	    private void SetSkillsPosInfo()
	    {
	        bool chosed = false;
            uint[] skillposId = Singleton<SkillMode>.Instance.GetUsedSkill();
            for (int i = 0; i < skillPosList.Count; i++)
            {
                GameObject pos = skillPosList[i];
                uint skillId = skillposId[i];
                if (skillId == 0)
                {
                    NGUITools.FindChild(pos, "icon").SetActive(false);
                    NGUITools.FindChild(pos, "tips").SetActive(true);
                }
                else
                {
                    if (skillId == currentSkill.id)
                    {
                        posChose.transform.localPosition = pos.transform.localPosition;

                        chosed = true;
                    }
                    SysSkillBaseVo skill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>(skillId);
                    NGUITools.FindChild(pos, "icon").SetActive(true);
                    NGUITools.FindChild(pos, "tips").SetActive(false);
                    NGUITools.FindInChild<UISprite>(pos, "icon").spriteName = skill.icon.ToString();
                }
            }
	        if (chosed)
	        {
	            posChose.SetActive(true);
	        }
	        else
	        {
                posChose.SetActive(false);
	        }
	    }

	    private void SetSkillPosChoseInfo()
	    {
	        if (currentUIToggel == zdSkill)
	        {
	            bool chosed = false;
	            uint[] skillpos = Singleton<SkillMode>.Instance.SkillsPos;
	            for (int i = 0; i < skillPosList.Count; i++)
	            {
	                GameObject posObj = skillPosList[i];
	                uint pos = skillpos[i];
	                if (pos == currentSkill.position)
	                {
	                    posChose.transform.localPosition = posObj.transform.localPosition;
	                    chosed = true;
	                }
	            }
	            if (chosed)
	            {
	                posChose.SetActive(true);
	            }
	            else
	            {
	                posChose.SetActive(false);
	            }
	        }
	    }


	    private void SetLeftPoint()
	    {
            UpgradeButton.FindInChild<UILabel>("leftvalue").text = Singleton<SkillMode>.Instance.LeftPoint().ToString();
	    }


	    //注册数据更新器
        public override void RegisterUpdateHandler()
        {
            Singleton<SkillMode>.Instance.dataUpdated += SkillDataUpdated;
            MeVo.instance.DataUpdated += SkillDataUpdated;

        }


        //数据更新响应
        public void SkillDataUpdated(object sender, int code)
        {
            if (sender == MeVo.instance && code ==0)
            {
				if (level != MeVo.instance.Level || money != MeVo.instance.diam)
                {
                    level = MeVo.instance.Level;
					money = MeVo.instance.diam;
                    SetAllSkillInfo();
                }
            }
            else
            {
                level = MeVo.instance.Level;
				money = MeVo.instance.diam;
                if (sender == Singleton<SkillMode>.Instance && code == SkillMode.SkillList)
                {
                    SetAllSkillInfo();
                }
                else if (sender == Singleton<SkillMode>.Instance && code == SkillMode.SkillPoint)
                {
                    SetLeftPoint();
                }else if (sender == Singleton<SkillMode>.Instance && code == SkillMode.SkillPos)
                {
                    SetSkillsPosInfo();
                }
            }

        }

        //取消数据更新器
        public override void CancelUpdateHandler()
        {
            Singleton<SkillMode>.Instance.dataUpdated -= SkillDataUpdated;
            MeVo.instance.DataUpdated -= SkillDataUpdated;
        }

	    private void OnCloseClick(GameObject obj)
	    {

            CloseView();
	    }

	    public override void CloseView()
	    {
	        skillAtlas = null;
            chose.SetActive(false);
	        base.CloseView();
	    }


        # region Code For Guide
        private void SetGuideButton()
        {
            if (CurrentGuideId == GuideType.GuideSkillOpen)
            {
                GuideSkillButton = skillList[1].GetComponent<Button>();
                GuideSkillPositionButton = skillPosList[1].GetComponent<Button>();
            }
            else if (CurrentGuideId == GuideType.GuideSkill3Learn)
            {
                GuideSkillButton = skillList[2].GetComponent<Button>();
                GuideSkillPositionButton = skillPosList[2].GetComponent<Button>();
            }
            else if (CurrentGuideId == GuideType.GuideSkill4Learn)
            {
                GuideSkillButton = skillList[3].GetComponent<Button>();
                GuideSkillPositionButton = skillPosList[3].GetComponent<Button>();
            }
            else if (CurrentGuideId == GuideType.GuideSkillRollLearn)
            {
                GuideSkillButton = skillList[4].GetComponent<Button>();
                GuideSkillPositionButton = skillPosList[4].GetComponent<Button>();
            }
        }
        # endregion
    }
}
