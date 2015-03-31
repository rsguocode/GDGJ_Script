using com.game.manager;
using com.u3d.bases.consts;
using com.u3d.bases.controller;
using com.u3d.bases.display.character;
using UnityEngine;

/**游戏对象行为控制-基础控制器
 * 游戏行为控制中心，通过该控制中心可以将其他的控制关联起来
 */

namespace com.u3d.bases.display.controler
{
    public class BaseControler : MonoBehaviour
    {
        public AiControllerBase AiController; //AI控制器
        public AnimationEventController AnimationEventController; //动画事件控制器
        public AnimationParameter AnimationParameter; //动画参数控制器
        public AttackControllerBase AttackController; //攻击控制器
        public BeAttackedControllerBase BeAttackedController; //受击控制器
        public ContinueCutMgr ContCutMgr; //连斩控制
        public GameObject GoBossMsg; //怪物说话
        public GameObject GoHeadInfo; //头顶信息
        public GameObject GoNumber; //数字物体
        public Transform MeTransform;
        public MoveControllerBase MoveController; //移动控制器
        public SkillController SkillController; //技能控制器
        public StatuControllerBase StatuController; //状态控制器
        public PetTalentSkillController TalentSkillController; //
        private GameObject _goHp; //HP物体
        private GameObject _goName; //名称物体
        public GraspThrowController GraspThrowController;//投射物控制器;
        /**控制对象**/
        public BaseDisplay Me { get; set; }

        protected bool IsUsing
        {
            get { return Me != null && Me.IsUsing; }
        }

        public GameObject GoName
        {
            get { return _goName; }
            set { _goName = value; }
        }

        public GameObject GoHp
        {
            get { return _goHp; }
            set { _goHp = value; }
        }

        public T GetMeByType<T>() where T : BaseDisplay
        {
            return Me as T;
        }

        /**移动**/

        public virtual void MoveTo(float x, float y, MoveEndCallback callback = null)
        {
        }

        private void Update()
        {
            if (IsUsing) Execute();
        }

        protected virtual void Execute()
        {
            Move();
            Render();
            Logic();
        }

        /**移动**/

        protected virtual void Move()
        {
        }

        /**渲染**/

        protected virtual void Render()
        {
            if (Me.GoCloth == null) return; //模型未加载完成
        }

        /**逻辑**/

        protected virtual void Logic()
        {
        }

        /// <summary>
        ///     根据起始点和终点坐标计算方向
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="endX"></param>
        /// <returns></returns>
        protected int CalDiretion(float startX, float endX)
        {
            int direction = Me.CurFaceDire;
            if (endX > startX)
            {
                direction = Directions.Right;
            }
            else if (endX < startX)
            {
                direction = Directions.Left;
            }
            return direction;
        }

        /**游戏物体使用状态
         * @return [true:使用中,false:已销毁]
         * **/

        /// <summary>
        ///     头顶说话
        /// </summary>
        public void SpeakWord()
        {
            if (Me.GetVo().Type == DisplayType.MONSTER)
            {
                var monsterDisplay = Me as MonsterDisplay;
                if (monsterDisplay != null) monsterDisplay.SpeakWord();
            }
        }

        /**销毁方法**/

        public virtual void Dispose()
        {
            if (_goName != null) Destroy(_goName);
            if (GoNumber != null) Destroy(GoNumber);
            if (gameObject != null) Destroy(gameObject);
            if (GoBossMsg != null) Destroy(GoBossMsg);
            if (_goHp != null) Destroy(_goHp); //移除血条
        }

        public void DisposeHudView()
        {
            if (_goName != null) Destroy(_goName);
            if (_goHp != null) Destroy(_goHp); //移除血条
        }

        /// <summary>
        /// 判断抓取打断;
        /// </summary>
        public void JudgeGraspIntercept()
        {
            if (Me.IsGrasp && GraspThrowController != null)
            {
                GraspThrowController.InterceptGrasp();
            }
        }
    }
}