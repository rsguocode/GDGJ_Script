using System;
using com.game;
using com.game.vo;
using com.u3d.bases.consts;
using com.u3d.bases.display.vo;
using com.u3d.bases.map;
using UnityEngine;
using com.u3d.bases.physics;

/**活动游戏对象行为控制类*/

namespace com.u3d.bases.display.controler
{
    public delegate void MoveEndCallback(BaseControler controler); //移动到目标后，委托回调

    public delegate void PlayEndCallback(BaseControler controler); //指定动作的动画播放完成后，委托回调


    public class ActionControler : BaseControler
    {
        private const float MoveStepX = 0.2f; //X方向移动步径
        private const float MoveStepY = 0.1f; //Y方向移动步径
        protected Vector2 Target; //移到目标点的坐标
        private bool _isSynMove; //是否需要同步移动
        private MoveEndCallback _moveEndCallback; //移动结束回调 
        public bool CanMove = true;

        public ActionControler()
        {
            Target = new Vector3();
            MoveSpeed = Global.ROLE_RUN_SPEED;
        }

        public int MoveSpeed { get; set; }

        public bool IsWalking { get; set; }

        public bool IsSynMove
        {
            get { return _isSynMove; }
            set
            {
                _isSynMove = value;
                if (_isSynMove == false)
                {
                    StopWalk();
                }
            }
        }

        public Vector3 GetMePos()
        {
            return transform.position;
        }

        public bool IsFloating()
        {
            return transform.position.y - transform.position.z * 0.2f > 0.001f;
        }

        /**移动**/

        protected override void Move()
        {
            if (StatuController != null && StatuController.GetAnimator() != null 
                && StatuController.GetAnimator().speed <= 0) return;
            if (_isSynMove)
            {
                MoveByDir();
            }
            if (GetMeVo() != null && !GetMeVo().IsHitRecover && (IsFloating() || GetMeVo().FloatingYSpeed != 0f || GetMeVo().FloatingXSpeed != 0f))
            {
                var meVo = GetMeVo();
                Vector3 pos = Vector3.zero;
                float ySpeed = 0f;
                GamePhysics.CalculateParabolic(GetMePos(), meVo.FloatingXSpeed, meVo.FloatingYSpeed, meVo.AtkerDirection, GamePhysics.Gravity, out pos, out ySpeed);
                Me.Pos(pos.x, pos.y, pos.z);
                meVo.FloatingYSpeed = ySpeed;
                if(!IsFloating())//不再浮空的话把属性清0
                {
                    if (StatuController.CurrentStatu == Status.HURT4)//击飞状态中
                    {
                        StatuController.SetStatu(Status.HURTDOWN);
                    }
                    meVo.FloatingXSpeed = 0f;
                    meVo.FloatingYSpeed = 0f;
                    meVo.AtkerDirection = 0;
                }
            }
            if (IsWalking)
            {
                MoveForward();
            }
        }

        /**向前移动**/

        private void MoveForward()
        {
            if (Math.Abs(transform.position.x - Target.x) < 0.01f && 
                Math.Abs(transform.position.y - Target.y) < 0.01f/*这判断不应该放这里，会影响到除主角以外的移动
                 &&
                                !NGUIJoystick.IsPressed && !MeControler.IsPressedMoveButton*/
                )
            {
                StopWalk(); //移动结束
            }
            else
            {
                Forward();
            }
        }

        /**向前移动并定位**/

        private void Forward()
        {
            if (StatuController.CurrentStatu != Status.RUN || CanMove == false)
            {
                return;
            }
            int direction = CalDiretion(transform.position.x, Target.x);
            Vector3 localscale = transform.localScale;
            localscale.x = direction == Directions.Left ? -1 : 1;
            transform.localScale = localscale; //计算角度*/
            Vector2 curXy = transform.position;
            Vector2 posXy = Vector2.MoveTowards(curXy, Target, Time.deltaTime*MoveSpeed);
            Vector3 posXyz = posXy;
            posXyz.z = posXyz.y*5;
            transform.position = posXyz;
            Me.SetSortingOrder(false);
        }

        /// <summary>
        ///     移动到某一个位置
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="callback">移动完后的回调处理</param>
        public override void MoveTo(float x, float y, MoveEndCallback callback = null)
        {
            if (CanMove)
            {
                Target.x = x;
                Target.y = y;
                Target = AdjustVector3(Target);
                _moveEndCallback = callback;
                if (StatuController != null)
                {
                    StatuController.SetStatu(Status.RUN);
                }
                IsWalking = true;
            }
        }

        /**停止移动**/

        public void StopWalk()
        {
            IsWalking = false;
            if (StatuController != null && StatuController.CurrentStatu == Status.RUN)
            {
                StatuController.SetStatu(Status.IDLE);
            }
            if (_moveEndCallback != null) _moveEndCallback(this);
            _moveEndCallback = null;
            Me.SetSortingOrder(false);
        }

        /**更改移动速度**/
        public void ChangeSpeed(int speed)
        {
            if (speed < 1) return;
            MoveSpeed = speed;
        }

        /**限制行走范围**/
        public Vector3 AdjustVector3(Vector3 v)
        {
            MapRange mapRange = AppMap.Instance.mapParser.CurrentMapRange;
            if (v.y > mapRange.MaxY) v.y = mapRange.MaxY;
            if (v.y < mapRange.MinY) v.y = mapRange.MinY;
            if (v.x > mapRange.MaxX) v.x = mapRange.MaxX;
            if (v.x < mapRange.MinX) v.x = mapRange.MinX;
            return v;
        }

        /**当前移动速度**/

        /**销毁方法**/

        public override void Dispose()
        {
            _moveEndCallback = null;
            base.Dispose();
        }

        /**帧行为处理接口**/

        protected override void Render()
        {
        }

        /// <summary>
        ///     固定方向上的移动
        /// </summary>
        public void MoveByDir()
        {
            switch (Me.CurDire)
            {
                case Directions.Left:
                    MoveTo(transform.position.x - MoveStepX, transform.position.y);
                    break;
                case Directions.Right:
                    MoveTo(transform.position.x + MoveStepX, transform.position.y);
                    break;
                case Directions.Top:
                    MoveTo(transform.position.x, transform.position.y + MoveStepY);
                    break;
                case Directions.Down:
                    MoveTo(transform.position.x, transform.position.y - MoveStepY);
                    break;
                case Directions.TopLeft:
                    MoveTo(transform.position.x - MoveStepX, transform.position.y + MoveStepY);
                    break;
                case Directions.TopRight:
                    MoveTo(transform.position.x + MoveStepX, transform.position.y + MoveStepY);
                    break;
                case Directions.DownLeft:
                    MoveTo(transform.position.x - MoveStepX, transform.position.y - MoveStepY);
                    break;
                case Directions.DownRight:
                    MoveTo(transform.position.x + MoveStepX, transform.position.y - MoveStepY);
                    break;
            }
        }

        public virtual void MoveByDir(int dir)
        {
            if ((StatuController.CurrentStatu == Status.RUN || StatuController.CurrentStatu == Status.IDLE))
            {
                Me.ChangeDire(dir);
                //如果当前状态不是待机状态或者跑动状态或者连击状态，则不响应移动要求
                if (StatuController.CurrentCombStatu == Status.COMB_0)
                {
                    MoveByDir();
                }
            }
        }

        /// <summary>
        ///     根据XY方向的速度进行移动
        /// </summary>
        /// <param name="xSpeed">x方向的速度</param>
        /// <param name="ySpeed">y方向的速度</param>
        public void MoveByXySpeed(float xSpeed, float ySpeed)
        {
            MoveTo(transform.position.x + 0.1f*xSpeed, transform.position.y + 0.1f*ySpeed);
        }

        public BaseRoleVo GetMeVo()
        {
            return Me.GetVo() as BaseRoleVo;
        }

        public T GetMeVoByType<T>() where T : DisplayVo
        {
            return Me.GetVo() as T;
        }

        public bool CanChangeStatus
        {
            set
            {
                StatuController.CanChangeStatus = value;
            }
            get
            {
                return StatuController.CanChangeStatus;
            }
        }
    }
}