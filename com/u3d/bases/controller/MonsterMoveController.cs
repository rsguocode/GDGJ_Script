using UnityEngine;
using System.Collections;
using com.u3d.bases.consts;
using com.game;
using com.u3d.bases.debug;
using com.u3d.bases.display;
using com.game.module.test;
using com.u3d.bases.display.character;
using com.game.vo;
using com.game.consts;

/**怪物移动控制
 * @author 骆琦
 * @date  2013-11-3
 * 基于animator的怪物移动控制器
 * **/
namespace com.u3d.bases.controller
{
    public class MonsterMoveController :MoveControllerBase
    {

        /// <summary>
        /// 状态开始触发的位移
        /// </summary>
        override public void MoveAtTheStatuBegin(int statuType)
        {
            switch (statuType)
            {
                case Status.HURT1: //受击1开始的位移
                    //StartCoroutine(MoveBackAndThenForward(0.5f, 0.2f));
                    break;
                case Status.HURT2: //受击2开始的位移
                    //StartCoroutine(MoveBackAndThenForward(0.5f, 0.2f));
                    break;
                case Status.HURT3: //受击3开始的位移
                    //StartCoroutine(MoveBackAndThenForward(0.7f, 0.2f));
                    break;
                case Status.HURT4: //受击4开始抛物线位移
                    /*
                    if (MeController.AiController.IsAi)
                                        {
                                            float f = 2 / ((MeController.GetMeByType<MonsterDisplay>()).BoxCollider2D.size.x); 
                                            StartY = ThisTransform.position.y - 0.1f;
                                            if (MeController.GetMeVo().CurHp == 0)
                                            {
                                                StartCoroutine(MoveByParabola(4f * f, 8.0f * f, 32.0f * f));  //死亡的时候抛物线更加夸张一点
                                            }
                                            else
                                            {
                                                StartCoroutine(MoveByParabola(0.5f * f, 6.0f * f, 30.0f * f));
                                            }
                                        }*/
                    
                    break;
            }
        }

        /// <summary>
        /// 模拟先后退一帧，然后再往前移一帧的运动
        /// </summary>
        /// <param name="backDistance">后退的距离</param>
        /// <param name="forwardDistance">前移的距离</param>
        /// <returns></returns>
        private IEnumerator MoveBackAndThenForward(float backDistance, float forwardDistance)
        {
            Vector3 position = ThisTransform.position;
            int dir = MeController.Me.CurFaceDire == Directions.Left ? 1 : -1;
            float x = position.x + backDistance * dir;
            x = AppMap.Instance.mapParser.GetFinalMonsterX(x);            //控制不让出界
            position.x = x;
            ThisTransform.position = position;
            yield return 0; //下一帧开始下面的操作
            yield return 0;
            yield return 0;
            position = ThisTransform.position;
            position.x += forwardDistance * dir * -1;
            ThisTransform.position = position;
        }

        /// <summary>
        /// 模拟抛物线运动
        /// </summary>
        /// <param name="xSpeed">X轴初始速度</param>
        /// <param name="ySpeed">Y轴初始速度</param>
        /// <param name="accelerate">重力加速度</param>
        /// <param name="setDire">设置指定方向 0表示用对象的自身方向</param>
        /// <returns></returns>
        public IEnumerator MoveByParabola(float xSpeed, float ySpeed, float accelerate, int setDire = 0)
        {
            Vector3 position;
            while (ThisTransform != null && ThisTransform.position.y >= StartY)
            {
                position = ThisTransform.position;
                int dir = MeController.Me.CurFaceDire == Directions.Left ? 1 : -1;
                if (0 != setDire)
                {
                    dir = setDire;
                }
                float x = position.x + xSpeed * dir * Time.deltaTime;
                x = AppMap.Instance.mapParser.GetFinalMonsterX(x);          //控制不让出界
                position.x = x;
                position.y += ySpeed * Time.deltaTime - accelerate * Time.deltaTime * Time.deltaTime * 0.5f;
                ySpeed -= accelerate * Time.deltaTime;
                ThisTransform.position = position;
                yield return 0;
            }
            if (ThisTransform != null)
            {
                position = ThisTransform.position;
                position.y = StartY + 0.1f;
                ThisTransform.position = position;
            }
            MeController.StatuController.SetStatu(Status.HURTDOWN);
            yield return 0;
        }


    }
}
