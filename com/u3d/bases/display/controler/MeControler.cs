using System;
using com.game;
using com.game.consts;
using com.game.module.battle;
using com.game.module.effect;
using com.game.module.fight.vo;
using com.game.module.Guide;
using com.game.module.Guide.GuideLogic;
using com.game.module.map;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.controller;
using com.u3d.bases.display.character;
using Com.Game.Module.Chat;
using com.game.module.test;
using UnityEngine;
using System.Collections.Generic;
using Com.Game.Utils;

/**游戏对象--本玩家行为控制器*/

namespace com.u3d.bases.display.controler
{
    public class MeControler : PlayerControler
    {
        public static bool IsPressedMoveButton;
        private float _releasePressTime;
        private bool mouseDownStart; //鼠标长按是否开始
        private int moveStep = 1;

        protected override void Render()
        {

            //如果是编辑环境，则可根据键盘按键来进行相应的移动
            if (Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.OSXEditor ||
                Application.platform == RuntimePlatform.WindowsPlayer ||
                Application.platform == RuntimePlatform.WindowsWebPlayer)
            {
				//向上箭头键
				if (Input.GetKeyDown(KeyCode.UpArrow))
				{
					Singleton<ChatMode>.Instance.DataUpdate(Singleton<ChatMode>.Instance.UPDATE_UPARROW);
				}

				//向下箭头键
				if (Input.GetKeyDown(KeyCode.DownArrow))
				{
					Singleton<ChatMode>.Instance.DataUpdate(Singleton<ChatMode>.Instance.UPDATE_DOWNARROW);
				}

                if (AiController == null) return;
                if (Input.GetKeyDown(KeyCode.J)) //使用普通攻击
                {
                    addNormalAttack(false);
                }
                else if(GameConst.IS_NORMAL_COMBO_BY_PRESS_LONG && 
                    Status.IDLE == AppMap.Instance.me.Controller.StatuController.CurrentStatu &&
                    Input.GetKey(KeyCode.J) )
                {
                    addNormalAttack(false);
                }
                if (Input.GetKeyDown(KeyCode.K)) //使用技能1
                {
                    AiController.SetAi(false);
                    SkillController.RequestUseSkill(SkillController.Skill1);
                }
                if (Input.GetKeyDown(KeyCode.U)) //使用技能2
                {
                    AiController.SetAi(false);
                    SkillController.RequestUseSkill(SkillController.Skill2);
                }
                if (Input.GetKeyDown(KeyCode.I)) //使用技能3
                {
                    AiController.SetAi(false);
                    SkillController.RequestUseSkill(SkillController.Skill3);
                }
                if (Input.GetKeyDown(KeyCode.O)) //使用技能3
                {
                    AiController.SetAi(false);
                    SkillController.RequestUseSkill(SkillController.Skill4);
                }
                if (Input.GetKeyDown(KeyCode.Space)) //使用瞬移
                {
                    AiController.SetAi(false);
                    SkillController.RequestUseSkill(SkillController.Roll);
                }
                if (Input.GetKeyDown(KeyCode.X)) //使用AI, 测试用
                {
                    print("****MeControler, 使用AI, 测试用");
                    AiController.SetAi(true);
                }
                if (Input.GetKeyDown(KeyCode.BackQuote))
                {
                    #if UNITY_EDITOR
                    GuideBase.TriggerGuideForTest(GuideType.GuideGoldBoxOpen);
                    #endif
                    /*#if UNITY_EDITOR
                                        if (UnityEditor.EditorApplication.isPlaying)
                                        {
                                            UnityEditor.EditorApplication.isPaused = true;
                                        }
                                        else
                                        {
                                            UnityEditor.EditorApplication.isPaused = false;
                                        }
                    #endif*/
                }
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                if (horizontal > 0 || vertical > 0 || horizontal < 0 || vertical < 0)
                {
                    AiController.SetAi(false);
                    Vector2 speed = new Vector2(horizontal, vertical).normalized;
                    int dir = Directions.GetDirByVector2(speed);
                    BattleMode.Instance.IsLeft = dir == Directions.Left;
                    BattleMode.Instance.IsRight = dir == Directions.Right;

					if (!MapMode.InStory)
					{
                    	MoveByDir(dir);
					}

                    IsPressedMoveButton = true;
                    AiController.SetAi(false);
                }
                else
                {
                    BattleMode.Instance.IsLeft = false;
                    BattleMode.Instance.IsRight = false;
                    IsPressedMoveButton = false;
                }
            }
        }

        public void addNormalAttack(bool _useAi)
        {
                AiController.SetAi(_useAi);
                var vo = new ActionVo { ActionType = Actions.ATTACK };
                AttackController.AddAttackList(vo);
        }
        
        //处理鼠标长按
        private void SortMouseDown()
        {
            if (Input.GetMouseButton(0))
            {
                if (canSortMouseDown())
                {
                    if (!mouseDownStart)
                    {
                        mouseDownStart = true;

                        Vector3 targetPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                        targetPosition = Camera.main.ViewportToWorldPoint(targetPosition);

                        if (targetPosition.x > transform.position.x)
                        {
                            moveStep = 1;
                        }
                        else
                        {
                            moveStep = -1;
                        }
                    }

                    MoveToAndTellServer(transform.position.x + moveStep, transform.position.y);
                }
            }
        }

        /// <summary>
        ///     主角自己多一个点击事件处理
        /// </summary>
        protected override void Execute()
        {
            MouseClick();
            CheckHit();
            base.Execute();
        }

        /**移动并发送目标点给服务器**/

        public void MoveToAndTellServer(float x, float y, MoveEndCallback callback = null)
        {
            x = (float) Math.Round(x, 2);
            y = (float) Math.Round(y, 2);
            MoveTo(x, y, callback);
            AppMap.Instance.tellServer(x, y);
        }

        private bool canSortMouseDown()
        {
            if (UICamera.lastHit.collider != null)
            {
                return false;
            }

            if (AppMap.Instance.monseClickEnable() == false)
            {
                if (MapTypeConst.CITY_MAP == AppMap.Instance.mapParser.MapVo.type)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        /**鼠标点击**/

        private void MouseClick()
        {
            if (UICamera.lastHit.collider != null)
            {
                return;
            }
            if (AppMap.Instance.monseClickEnable() == false && Input.GetMouseButtonUp(0))
            {
                RaycastHit hitInfo;
                if(ReferenceEquals(Camera.main,null))return;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //Log.info(this, "世界坐标:" + transform.position);
                //Log.info(this, "屏幕坐标:" + Input.mousePosition);
                //Log.info(this, "世界坐标→屏幕坐标:" + Camera.mainCamera.WorldToScreenPoint(transform.position));
                //Log.info(this, "屏幕坐标→视口坐标:" + Camera.mainCamera.ScreenToViewportPoint(Input.mousePosition));
                //Log.info(this, "世界坐标→视口坐标:" + Camera.mainCamera.WorldToViewportPoint(transform.position)); 

                if (Physics.Raycast(ray, out hitInfo))
                {
                    GameObject hitGo = hitInfo.transform.gameObject; //记录点中游戏物体
                    if (hitGo.name.IndexOf("_", StringComparison.Ordinal) != -1)
                    {
                        BaseDisplay display = AppMap.Instance.GetDisplay(hitGo.name);
                        if (display == null)
                        {
                            return;
                        }
                        if (display is MeDisplay) return;
                        if (display is PlayerDisplay || display is ModeDisplay)
                        {
                            AppMap.Instance.clicker.Call(display);
                            return;
                        }
                        ClickPoint(hitInfo.point.x, hitInfo.point.y);
                        AppMap.Instance.clicker.SaveClicker(display);
                    }
                    else
                    {
                        Target.x = hitInfo.point.x;
                        Target.y = hitInfo.point.y;
                        if (Target.y > -0.1f) Target.y = -0.1f;
                        AppMap.Instance.clicker.SaveClicker(null);
                    }
                    AppMap.Instance.stopAutoRoad();
                    MoveToAndTellServer(Target.x, Target.y);
                }
                else if (AppMap.Instance.mapParser.MapVo.type == MapTypeConst.CITY_MAP)
                {
//					//显示点击光标
//					Vector3 targetPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
//					if (targetPosition.x >= 1 || targetPosition.x <0)
//					{
//						return; //目标在屏幕外，则不响应，主要是为了屏幕UI滑动事件
//					}
//					targetPosition = Camera.main.ViewportToWorldPoint(targetPosition);
//					showCursor(targetPosition);
//
//					//鼠标点击地面后将位置坐标通知服务器
//                  MoveToAndTellServer(targetPosition.x, targetPosition.y);
//
//                  //一点击地面，就收缩控制的图标
//                  Singleton<MainBottomRightView>.Instance.CloseKongzhi();
                }

                //长按结束
                mouseDownStart = false;
            }
        }

        //主城点击地面后增加光标提示
        private void showCursor(Vector3 pos)
        {
            Vector3 realPos = pos;
            realPos.y = transform.position.y;
            realPos.z = 0;
            EffectMgr.Instance.CreateMainEffect(EffectId.Main_CityClick, realPos, false);
        }

        /**碰撞检测**/

        private void CheckHit()
        {
            if (!AppMap.Instance.clicker.IsHit()) return;
            StopWalk();
            AppMap.Instance.clicker.Call();
        }

        /**计算点中对象周围可走点**/

        private void ClickPoint(float x, float y)
        {
            Target.y = y - 0.8f;
            Vector3 p = transform.position;
            if (x > p.x) Target.x = x - 0.5f;
            if (x < p.x) Target.x = x + 0.5f;
        }

        public void Relive()
        {
            StatuController.SetStatu(Status.IDLE);
        }

        protected override void Logic()
        {
            if (MePlayerVo.CurMp < MePlayerVo.Mp)
            {
                if (AccuTime > 1)
                {
                    MePlayerVo.CurMp += 1;
                    AccuTime = 0;
                    MeVo.instance.DataUpdate(MeVo.DataHpMpUpdate);
                }
                else
                {
                    AccuTime += Time.deltaTime;
                }
            }
        }
    }
}