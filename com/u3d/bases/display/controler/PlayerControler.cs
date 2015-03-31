using com.game.vo;
using com.u3d.bases.consts;
/**游戏对象--玩家控制器玩家行为控制*/
using UnityEngine;

namespace com.u3d.bases.display.controler
{
    public class PlayerControler : ActionControler
    {
        public PlayerVo MePlayerVo;
        protected float AccuTime;

        private void Start()
        {
            MePlayerVo = GetMeVoByType<PlayerVo>();
        }
	
        protected override void Logic()
        {
            if (MePlayerVo.CurMp < MePlayerVo.Mp)
            {
                if (AccuTime > 1)
                {
                    MePlayerVo.CurMp += 1;
                    AccuTime = 0;
                }
                else
                {
                    AccuTime += Time.deltaTime;
                }
            }
        }

        public override void MoveByDir(int dir)
        {
            if ((StatuController.CurrentStatu == Status.RUN || StatuController.CurrentStatu == Status.IDLE))
            {
                Me.ChangeDire(dir);
                if (StatuController.CurrentCombStatu == Status.COMB_0) //如果当前状态不是待机状态或者跑动状态或者连击状态，则不响应移动要求
                {
                    MoveByDir();
                }
            }
        }
    }
}