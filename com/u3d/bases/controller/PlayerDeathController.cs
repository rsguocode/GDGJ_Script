using com.game.vo;
using com.u3d.bases.consts;

/**普通攻击控制类
 * @author 骆琦
 * @date  2013-11-6
 * 玩家的死亡控制处理
 * */
namespace com.u3d.bases.controller{

    public class PlayerDeathController : DeathController{

        protected override void CheckDeath()
        {
            base.CheckDeath();
            if (MeBaseRoleVo.CurHp > 0 && (MeController.StatuController.PreStatuNameHash == Status.NAME_HASH_DEATH_END
                ||MeController.StatuController.CurrentStatu==Status.DEATH))
            {
                MeController.StatuController.SetStatu(Status.IDLE);
            }
            if (MeBaseRoleVo.CurHp <= 0 && MeController.StatuController.CurrentStatu != Status.DEATH &&
                MeBaseRoleVo.Id != MeVo.instance.Id)
            {
                MeController.StatuController.SetStatu(Status.DEATH);
                MeController.GraspThrowController.InterceptGrasp(true);
            }
        }
    }
}
