using System;
using com.game;
using com.game.consts;
using com.game.data;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.controller;
using com.u3d.bases.display.controler;
using UnityEngine;
using com.game.manager;
using com.game.module.map;
using com.u3d.bases.debug;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using com.game.utils;

/**怪物--游戏对象显示逻辑控制*/
namespace com.u3d.bases.display.character
{
    public class MonsterDisplay:ActionDisplay
    {
        override protected string SortingLayer { get { return "Player"; } }

        public SysMonsterVo MonsterVo
        {
            get { return ((Controller as ActionControler).GetMeVo() as MonsterVo).MonsterVO; }
        }

		public MonsterVo Monster
		{
			get { return ((Controller as ActionControler).GetMeVo() as MonsterVo); }
		}

        private GameObject _speakGameObject;
        public UILabel SpeakLabel;
        private bool _hasSpeakIdleWord;
        private bool _hasSpeakAttackWord;
        private bool _hasSpeakDeathWord;

        private Vector2[] _boxColliderStandData;
        private Vector2[] _boxColliderDownData;


        private AiData _aiData; // new
        public List<AiData> listAiDataCtr;

        /**添加控制脚本**/
        override protected void AddScript(GameObject go) // go 为 Box2d刚体对象 如400_2014
        {
            if (go.GetComponent<ActionControler>() != null) return;
            Controller = go.AddComponent<ActionControler>();
            Controller.Me = this;
            Controller.Me.Animator = Controller.Me.GoCloth.GetComponent<Animator>();
            //改变怪物的速度
            (Controller as ActionControler).ChangeSpeed((GetVo() as MonsterVo).MonsterVO.speed / GameConst.PIXEL_TO_UNITY);
            //增加状态控制脚本
            var statuController = go.AddComponent<MonsterStatuController>();
            statuController.MeControler = Controller as ActionControler;
            Controller.StatuController = statuController;

            //增加技能控制脚本
            SkillController skillController = Controller.SkillController = go.AddComponent<SkillController>();
            skillController.MeController = Controller as ActionControler;

            Controller.AnimationEventController = GoCloth.GetComponent<AnimationEventController>() ??  GoCloth.AddComponent<AnimationEventController>();
            Controller.AnimationEventController.skillController = Controller.SkillController;

            //增加抓投控制脚本;
            var projectileController = GoCloth.GetComponent<GraspThrowController>() ?? GoCloth.AddComponent<GraspThrowController>();
            projectileController.MeController = Controller as ActionControler;
            Controller.GraspThrowController = projectileController;
            Controller.AnimationEventController.GraspThrowController = Controller.GraspThrowController;

            //增加动画参数控制脚本
            Controller.AnimationParameter = GoCloth.GetComponent<AnimationParameter>() ??  GoCloth.AddComponent<AnimationParameter>();

            //增加移动控制脚本
            var monsterMoveController = go.AddComponent<MonsterMoveController>();
            monsterMoveController.AnimationEventController = Controller.AnimationEventController;
            monsterMoveController.MeController = Controller as ActionControler;
            Controller.MoveController = monsterMoveController;
            monsterMoveController.AnimationParameter = Controller.AnimationParameter;

            //增加攻击控制脚本
            var attackController = go.AddComponent<MonsterAttackController>();
            attackController.MeController = Controller as ActionControler;
            Controller.AttackController = attackController;

            //增加受击控制脚本
            var beAttackedController = go.AddComponent<MonsterBeAttackedController>();
            beAttackedController.meController = Controller as ActionControler;
            Controller.BeAttackedController = beAttackedController;

            //增加死亡控制脚本
            var deathController = go.AddComponent<MonsterDeathController>();
            deathController.MeController = Controller as ActionControler;

            //增加AI控制脚本
            string aiList = (GetVo() as MonsterVo).MonsterVO.ai_list;
            //var aiController = go.AddComponent<MonsterAiController>();
            //aiController.MeController = Controller as ActionControler;
            //Controller.AiController = aiController;


            
          
            if (aiList == "850002")
            {
                listAiDataCtr = AiDataParser.Me().Parser(aiList);
            }
              /*// 将要做
            _aiData = new AiData();
            listAiDataCtr = new List<AiData>();

            SysMonsterAiVo monsterAiVo = BaseDataMgr.instance.GetDataById<SysMonsterAiVo>(uint.Parse(aiList));
            string strAiValue = monsterAiVo.AiValue; // 122=720000,100|218=1;104=5|200=930035

            _aiData.ID = int.Parse(aiList);
            string[] strEachState = strAiValue.Split(';');
             for (int i = 0; i < strEachState.Length; i++)
             {
                 string[] strConditionState = strEachState[i].Split('|');

                 // 122=720000,100
                 string[] strConditionValue = strConditionState[0].Split('=');
                 _aiData.conditionType = int.Parse(strConditionValue[0]);
                     
                string []value = strConditionValue[1].Split(',');
                for (int k = 0; k < value.Length; k++)
                {
                    _aiData.conditionParamList.Add(int.Parse(value[k]));
                }

                 // 218=1 
                string[] strActionValue = strConditionState[1].Split('=');
                _aiData.actionType = int.Parse(strActionValue[0]);
                value = strActionValue[1].Split(',');
                for (int k = 0; k < value.Length; k++)
                    _aiData.actionParamList.Add(int.Parse(value[k]));

                listAiDataCtr.Add(_aiData); // 一个条件就一个 _aiData;
             }
              */
             
            


            if (aiList != null && aiList.Contains("1"))
            {
                Controller.AiController = go.AddComponent<MonsterAiController_Far>(); // 添加远程脚本
                go.GetComponent<MonsterAiController_Far>().MeController = Controller as ActionControler;
                //Debug.Log("****添加新AI脚本MonsterAiController_Far");
            }
            else if (aiList != null && aiList.Contains("2"))
            {
                Controller.AiController = go.AddComponent<MonsterAiController_Far2>(); // 添加远程脚本
                go.GetComponent<MonsterAiController_Far2>().MeController = Controller as ActionControler;
                Debug.Log("****添加新AI脚本MonsterAiController_Far2");
            }
            else
            {
                Controller.AiController = go.AddComponent<MonsterAiController>();
                go.GetComponent<MonsterAiController>().MeController = Controller as ActionControler;
            }

            BoxCollider2D = GoCloth.GetComponent<BoxCollider2D>();
            BoxCollider2D.enabled = false;
            SetStandClothGoPosition();

            GetMeVoByType<MonsterVo>().Controller = Controller as ActionControler;

            InitBoxColliderData();
        }

        //初始化对象包围盒基础数据
        private void InitBoxColliderData()
        {
            int[][] list = StringUtils.Get2DArrayStringToInt(MonsterVo.rect_stand);
            _boxColliderStandData = new Vector2[2];
            _boxColliderStandData[0] = new Vector2(list[0][0], list[0][1]) * 0.001f;
            _boxColliderStandData[1] = new Vector2(list[1][0], list[1][1]) * 0.001f;

            list = StringUtils.Get2DArrayStringToInt(MonsterVo.rect_down);
            _boxColliderDownData = new Vector2[2];
            _boxColliderDownData[0] = new Vector2(list[0][0], list[0][1]) * 0.001f;
            _boxColliderDownData[1] = new Vector2(list[1][0], list[1][1]) * 0.001f;
        }

        //设置对象包围为站立状态
        public void SetBoxColliderStand()
        {
            BoxCollider2D.size = _boxColliderStandData[0];
            BoxCollider2D.center = _boxColliderStandData[1];
        }

        //设置对象包围为倒卧状态
        public void SetBoxColliderDown()
        {
            BoxCollider2D.size = _boxColliderDownData[0];
            BoxCollider2D.center = _boxColliderDownData[1];
        }

        public void InitSpeakView(GameObject speakView)
        {
            _speakGameObject = speakView;
            SpeakLabel = NGUITools.FindInChild<UILabel>(speakView, "msg");
            speakView.SetActive(false);
        }

        public void SpeakWord()
        {
            switch (Controller.StatuController.CurrentStatu)
            {
                case Status.IDLE:
                    SpeakIdleWords();
                    break;
                case Status.ATTACK1:
                    SpeakAttackWords();
                    break;
                case Status.DEATH:
                    SpeakDeathWords();
                    break;
            }
        }

        private void SpeakIdleWords()
        {
            if(_hasSpeakIdleWord) return;
            _hasSpeakIdleWord = true;
            string word = MonsterVo.idle_speak;
            if(word=="0") return;
            _speakGameObject.SetActive(true);
            SpeakLabel.text = word;
            vp_Timer.In(2,RemoveWord);
        }

        private void RemoveWord()
        {
            if (_speakGameObject != null)
            {
                _speakGameObject.SetActive(false);
            }
        }

        private void SpeakAttackWords()
        {
            if (_hasSpeakAttackWord) return;
            _hasSpeakAttackWord = true;
            string word = MonsterVo.fight_speak;
            if (word == "0") return;
            _speakGameObject.SetActive(true);
            SpeakLabel.text = word;
            vp_Timer.In(2, RemoveWord);
        }

        private void SpeakDeathWords()
        {
            if (_hasSpeakDeathWord) return;
            _hasSpeakDeathWord = true;
            string word = MonsterVo.death_speak;
            if (word == "0") return;
            _speakGameObject.SetActive(true);
            SpeakLabel.text = word;
            vp_Timer.In(2, RemoveWord);
        }

        public void ReadyToUseAi(float delayTime)
        {
            vp_Timer.In(delayTime, UseAi);
        }

        private void UseAi()
        {
            if (AppMap.Instance.mapParser.MapId == MapTypeConst.WORLD_BOSS)
            {
                Controller.AiController.SetAi(false);  //世界BOSS刚开始不启用AI
            }
            else
            {
                Controller.AiController.SetAi(true);
            }
        }

		/// <summary>
		/// 加载模型,怪物先冲缓存中查找
		/// </summary>
		/// <param name="url">模型资源地址</param>
		override protected void CreateMode(string url)
		{
			try
			{
                GameObject monsterpre = MonsterMgr.Instance.GetMonsterPreload(url);
                if (monsterpre != null)
                    LoadModelCallBack(monsterpre);
                else
                    AssetManager.Instance.LoadAsset<GameObject>(url, LoadModelCallBack);
                //AssetManager.Instance.LoadAssetFromResources<GameObject>(url, LoadModelCallBack); //测试用的接口
			}
			catch (Exception ex)
			{
				Log.error(this, "-createMode() url:" + url + " [Error] " + ex.Message);
			}
		}

    }
}
