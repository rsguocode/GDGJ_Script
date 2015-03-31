using com.u3d.bases.consts;
using com.game.vo;
using Com.Game.Speech;

/**普通攻击控制类
 * @author 骆琦
 * @date  2013-11-6
 * 怪物的死亡控制处理
 * */
namespace com.u3d.bases.controller
{
    public class MonsterDeathController : DeathController
    {
		private bool speechPlaying = false;

        protected override void CheckDeath()
        {
            base.CheckDeath();
            if (MeBaseRoleVo.CurHp <= 0 && MeController.StatuController.CurrentStatu != Status.DEATH)
            {
                MeController.StatuController.SetStatu(Status.DEATH);
                MeController.GraspThrowController.InterceptGrasp(true);
				MonsterVo monsterVo = (MeController.Me.GetVo() as MonsterVo);
				if (3 == monsterVo.MonsterVO.quality && !speechPlaying)
                {
                    speechPlaying = true;
                    SpeechMgr.Instance.PlayKillBossSpeech();
				}
            }
        }
    }
}
