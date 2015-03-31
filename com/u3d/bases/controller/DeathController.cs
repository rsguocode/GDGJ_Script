﻿﻿﻿﻿using UnityEngine;
using com.u3d.bases.display.controler;
using com.game.vo;


/**死亡控制类
 * @author 骆琦
 * @date  2013-11-7
 * 玩家怪物死亡处理逻辑
 * **/
namespace com.u3d.bases.controller
{
    public class DeathController : MonoBehaviour
    {
        public ActionControler MeController;
        protected BaseRoleVo MeBaseRoleVo;


        // Use this for initialization
        void Start()
        {
            MeBaseRoleVo = MeController.Me.GetVo() as BaseRoleVo;
        }

        // Update is called once per frame
        void Update()
        {
            CheckDeath();
        }

        protected virtual void CheckDeath() {

        }
    }
}
