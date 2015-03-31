﻿﻿﻿using System;
﻿﻿﻿using UnityEngine;
using com.game.utils;
using com.u3d.bases.debug;
using com.game.module.test;
using com.u3d.bases.loader;
using com.game.manager;
using System.Collections.Generic;
using com.game.consts;
using com.game.data;

namespace Com.Game.Module.Role
{
    /// <summary>
    /// 技能提醒视图
    /// </summary>
    public class SkillTipsView : BaseView<SkillTipsView>
    {
        /// <summary>
        /// 预设路径
        /// </summary>
        public override string url { get { return "UI/Skill/SkillTips.assetbundle"; } }

        private SysSkillBaseVo SkillVo;

        private UISprite skillIcn;//技能图标

        private GameObject title;//技能title 包括技能名称 等级 状态等信息

        private GameObject info; //技能信息 类型信息，攻击距离，消耗，冷却，描述信息

        private GameObject condition; //条件信息， 等级，技能点，金币

        private GameObject skillButton; //技能按钮

        private SkillState skillState = SkillState.Upgrade;//状态，学习，升级，进阶

        public  void OpenView(SysSkillBaseVo skillVo,GameObject obj)
        {
            if (base.gameObject == null)
            {
                base.gameObject = obj;
                Init();
            }
            base.gameObject.SetActive(true);
            SetSkillDetailInfo(skillVo);
        }

        protected override void  Init()
        {
            skillIcn = FindInChild<UISprite>("tips/icon/icn");
            title = FindChild("tips/title");
            condition = FindChild("tips/condition");
            info = FindChild("tips/info");

            skillButton = FindChild("tips/btn_learn");
            FindInChild<Button>("tips/btn_close").onClick = closeClick;

            initLabels();
            //默认升级状态
            setSkillSate(SkillState.Upgrade);
        }


        public void SetSkillDetailInfo(SysSkillBaseVo svo)
        {
            
            if (gameObject != null)
            {
                SkillVo = svo;
                SysSkillBaseVo skillVo = SkillVo;
                SkillState state = SkillViewLogic.GetSkillState(skillVo);

                //当前技能不是学习状态则已经激活
                bool enable = state != SkillState.Learn;

                //技能-升级或者进阶
                bool showUpgrade = (state != SkillState.Change);

                bool showChange = (state == SkillState.Change);
                //满级
                bool limit = (skillVo.next == 0);

                //获取下一级信息
                uint nextId = (uint)skillVo.next;
                if (nextId == 0)
                {
                    //下一级技能为进阶技能
                    nextId = (uint) skillVo.evolve;
                }

                SysSkillBaseVo nextSkill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>(nextId);

                //当前技能为0级，则显示1级技能信息
                if (!enable && nextSkill!=null)
                {
                    skillVo = nextSkill;
                }

                string skillType;

                if (skillVo.active)
                {
                    skillType = LanguageManager.GetWord("SkillTipsView.TypeZD");
                    if (skillVo.group)
                    {
                        skillType += LanguageManager.GetWord("SkillTipsView.Group");
                    }
                    else
                    {
                        skillType += LanguageManager.GetWord("SkillTipsView.Single");
                    }
                }
                else
                {
                    skillType = LanguageManager.GetWord("SkillTipsView.TypeBD");
                }

                //当前技能的信息
                string icn = skillVo.icon.ToString();
                if (!SkillViewLogic.IsLevelEnough(skillVo))
                {
                    icn = "suo";
                }
                setSkillIcn(icn);
                setSkillEnable(enable);
                setSkillTitle(skillVo.name, skillVo.skill_lvl.ToString(), limit);
                setSkillInfo(skillType, skillVo.cover_width, skillVo.need_value, skillVo.cd / 1000f, skillVo.desc);

                //升级的信息
                if (nextId != 0)
                {
                    //升级相关的信息
                    bool levelEnough = SkillViewLogic.IsLevelEnough(nextSkill);
                    bool moneyEnough = SkillViewLogic.IsMoneyEnough(nextSkill);
                    bool pointEnough = SkillViewLogic.IsSkillPointEnough(nextSkill);

                    string preskill = null;
                    bool preSkillEnough = true;

                    if (skillVo.pre != 0)
                    {
                        SysSkillBaseVo pSkill = BaseDataMgr.instance.GetDataById<SysSkillBaseVo>((uint)skillVo.pre);
                        if (pSkill.skill_lvl != 0)
                        {
                            preskill = pSkill.name;
                            preSkillEnough = SkillViewLogic.IsPreSkillEnough(nextSkill);
                        }
                    }

                    setSkillCondition(nextSkill.lvl, nextSkill.point, nextSkill.beiley, preskill, levelEnough, pointEnough,
                        moneyEnough, preSkillEnough);
                    setNextSkillTitle(nextSkill.name, nextSkill.skill_lvl, nextSkill.desc);
                    condition.SetActive(true);
                    skillButton.SetActive(true);

                }
                else
                {
                    condition.SetActive(false);
                    skillButton.SetActive(false);
                }
            }
            
        }

        //初始化固定文字信息

        public void initLabels() {

            NGUITools.FindInChild<UILabel>(title, "level").text = LanguageManager.GetWord("SkillTipsView.Level");
            
            NGUITools.FindInChild<UILabel>(info, "title").text = LanguageManager.GetWord("SkillTipsView.Info");
            NGUITools.FindInChild<UILabel>(info, "type/title").text = LanguageManager.GetWord("SkillTipsView.SkillType");
            NGUITools.FindInChild<UILabel>(info, "cost/title").text = LanguageManager.GetWord("SkillTipsView.Cost");
            NGUITools.FindInChild<UILabel>(info, "distance/title").text = LanguageManager.GetWord("SkillTipsView.Distance");
            NGUITools.FindInChild<UILabel>(info, "time/title").text = LanguageManager.GetWord("SkillTipsView.Time");
            NGUITools.FindInChild<UILabel>(info, "describe/title").text = LanguageManager.GetWord("SkillTipsView.Describe");

            NGUITools.FindInChild<UILabel>(condition, "money/title").text = LanguageManager.GetWord("SkillTipsView.Money");
            NGUITools.FindInChild<UILabel>(condition, "level/title").text = LanguageManager.GetWord("SkillTipsView.RoleLevel");
            NGUITools.FindInChild<UILabel>(condition, "point/title").text = LanguageManager.GetWord("SkillTipsView.Point");
            NGUITools.FindInChild<UILabel>(condition, "preskill/title").text = LanguageManager.GetWord("SkillTipsView.PreSkill");
            NGUITools.FindInChild<UILabel>(condition, "nextlevel").text = LanguageManager.GetWord("SkillTipsView.Level");
        }

        /// <summary>
        /// 关闭按钮点击
        /// </summary>
        /// <param name="obj"></param>
        private void closeClick(GameObject obj) {
            this.gameObject.SetActive(false);
            SkillVo = null;
            gameObject = null;
        }

        /// <summary>
        /// 技能升级触发按钮
        /// </summary>
        private void skillUpgradeClick(GameObject obj)
        {
            Singleton<SkillControl>.Instance.LearnSkill((uint)SkillVo.id);

        }


        // 激活技能图标
        private  void setSkillEnable(bool enable){
            if (enable)
            {
                skillIcn.color = ColorConst.Normal;
            }
            else {
                setSkillSate(SkillState.Learn);
                skillIcn.color = ColorConst.GRAY;
            }
        }

        // 设置技能图标名
        private void setSkillIcn(string icnName) {
            skillIcn.spriteName = icnName;
        }

        /// <summary>
        /// 设置技能title信息
        /// </summary>
        /// <param name="skillName">技能名称</param>
        /// <param name="level">技能等级</param>
        /// <param name="limit">技能满级</param>
        private void setSkillTitle(string skillName, string level, bool limit) {
            NGUITools.FindInChild<UILabel>(title, "name").text = skillName;
            NGUITools.FindInChild<UILabel>(title, "levelvalue").text = level.ToString();
            if (limit)
            {
                NGUITools.FindInChild<UILabel>(title, "state").color = ColorConst.RED_NO;
                NGUITools.FindInChild<UILabel>(title, "state").text = LanguageManager.GetWord("SkillTipsView.Limit");
                
                setSkillSate(SkillState.Change);

                Log.info(this, ""+NGUITools.FindInChild<UILabel>(title, "state").color.r);
            }
            else {
                NGUITools.FindInChild<UILabel>(title, "state").color = ColorConst.GREEN_YES;
                NGUITools.FindInChild<UILabel>(title, "state").text = LanguageManager.GetWord("SkillTipsView.UnLimit");
            }
        }

        /// <summary>
        /// 设置技能详细信息
        /// </summary>
        /// <param name="skilltype">技能类型</param>
        /// <param name="distance">技能攻击距离</param>
        /// <param name="cost">技能消耗</param>
        /// <param name="second">冷却时间，秒</param>
        /// <param name="describe">技能描述</param>
        private void setSkillInfo(string skilltype, int distance, int cost, float second,string describe) {
            
            NGUITools.FindInChild<UILabel>(info, "type/value").text = skilltype;
            NGUITools.FindInChild<UILabel>(info, "distance/value").text = distance.ToString();
            NGUITools.FindInChild<UILabel>(info, "time/value").text = second.ToString() + LanguageManager.GetWord("SkillTipsView.Seconds");
            NGUITools.FindInChild<UILabel>(info, "cost/value").text = cost.ToString();
            NGUITools.FindInChild<UILabel>(info, "describe/value").text = describe;
        }


        /// <summary>
        /// 设置技能升级条件
        /// </summary>
        /// <param name="level">角色等级</param>
        /// <param name="point">技能点</param>
        /// <param name="money">钱</param>
        /// <param name="levelEnough">等级满足</param>
        /// <param name="pointEnough">技能点满足</param>
        /// <param name="moneyEnough">钱满足</param>
        private void setSkillCondition(int level,int  point,int money,string preSkill,bool levelEnough,bool pointEnough,bool moneyEnough,bool preSkillEnough)
        {
            NGUITools.FindInChild<UILabel>(condition, "level/value").text = level.ToString();
            NGUITools.FindInChild<UILabel>(condition, "point/value").text = point.ToString();
            NGUITools.FindInChild<UILabel>(condition, "money/value").text = money.ToString();
            if (preSkill == null || preSkill.Equals(String.Empty))
            {
                FindChild("tips/condition/preskill").SetActive(false);
            }
            else
            {
                FindChild("tips/condition/preskill").SetActive(true);

                NGUITools.FindInChild<UILabel>(condition, "preskill/value").text = preSkill;

                if (preSkillEnough)
                {
                    NGUITools.FindInChild<UILabel>(condition, "preskill/state").text = LanguageManager.GetWord("SkillTipsView.Enough");
                    NGUITools.FindInChild<UILabel>(condition, "preskill/state").color = ColorConst.GREEN_YES;
                }
                else
                {
                    NGUITools.FindInChild<UILabel>(condition, "preskill/state").text = LanguageManager.GetWord("SkillTipsView.NotEnough");
                    NGUITools.FindInChild<UILabel>(condition, "preskill/state").color = ColorConst.RED_NO;
                }
            }


            if(levelEnough){
                NGUITools.FindInChild<UILabel>(condition, "level/state").text = LanguageManager.GetWord("SkillTipsView.Enough");
                NGUITools.FindInChild<UILabel>(condition, "level/state").color = ColorConst.GREEN_YES;
            }else{
                NGUITools.FindInChild<UILabel>(condition, "level/state").text = LanguageManager.GetWord("SkillTipsView.NotEnough");
                NGUITools.FindInChild<UILabel>(condition, "level/state").color = ColorConst.RED_NO;
            }

            if (pointEnough)
            {
                NGUITools.FindInChild<UILabel>(condition, "point/state").text = LanguageManager.GetWord("SkillTipsView.Enough");
                NGUITools.FindInChild<UILabel>(condition, "point/state").color = ColorConst.GREEN_YES;
            }
            else
            {
                NGUITools.FindInChild<UILabel>(condition, "point/state").text = LanguageManager.GetWord("SkillTipsView.NotEnough");
                NGUITools.FindInChild<UILabel>(condition, "point/state").color = ColorConst.RED_NO;
            }


            if (moneyEnough)
            {
                NGUITools.FindInChild<UILabel>(condition, "money/state").text = LanguageManager.GetWord("SkillTipsView.Enough");
                NGUITools.FindInChild<UILabel>(condition, "money/state").color = ColorConst.GREEN_YES;
            }
            else
            {
                NGUITools.FindInChild<UILabel>(condition, "money/state").text = LanguageManager.GetWord("SkillTipsView.NotEnough");
                NGUITools.FindInChild<UILabel>(condition, "money/state").color = ColorConst.RED_NO;
            }

            setBtnEnable(levelEnough && pointEnough && moneyEnough && preSkillEnough);
        }

        /// <summary>
        /// 设置下一技能信息
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="level">等级</param>
        private void setNextSkillTitle(string name,int level,string des)
        {
            NGUITools.FindInChild<UILabel>(condition, "nextname").text = name;
            NGUITools.FindInChild<UILabel>(condition, "nextlevelvalue").text = level.ToString();
            NGUITools.FindInChild<UILabel>(condition, "describe/value").text = des;
        }

        /// <summary>
        /// 设置按钮文本
        /// </summary>
        /// <param name="state">按钮状态</param>
        private void setSkillSate(SkillState state)
        {
            skillState = state;
            string stateLable;
            if (state == SkillState.Learn)
            {
                stateLable = LanguageManager.GetWord("SkillTipsView.Learn");
            }
            else if (state == SkillState.Upgrade)
            {
                stateLable = LanguageManager.GetWord("SkillTipsView.Upgrade");
            }else{
                stateLable = LanguageManager.GetWord("SkillTipsView.Change");
            }

            NGUITools.FindInChild<UILabel>(condition, "title").text = stateLable + LanguageManager.GetWord("SkillTipsView.NextSkill"); ;
            NGUITools.FindInChild<UILabel>(skillButton, "label").text = stateLable;
        }

        /// <summary>
        /// 激活升级按钮
        /// </summary>
        /// <param name="state"></param>
        private void setBtnEnable(bool enable)
        {
            if (enable)
            {
                NGUITools.FindInChild<UISprite>(skillButton, "background").color = ColorConst.Normal;
                skillButton.GetComponent<Button>().onClick = skillUpgradeClick;
            }
            else {
                NGUITools.FindInChild<UISprite>(skillButton, "background").color = ColorConst.GRAY;
                skillButton.GetComponent<Button>().onClick = null;
            }
        }

    }
}