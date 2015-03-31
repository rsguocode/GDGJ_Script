﻿﻿﻿
using Com.Game.Module.Pet;
using com.game.module.test;
using com.game.vo;
using com.u3d.bases.display;
using com.u3d.bases.display.character;
using UnityEngine;

/* *******************************************************
 * author :  qi luo
 * email  :  408176274@qq.com  
 * history:  created by qi luo   2014/03/05 07:58:06 
 * function: 宠物管理类
 * *******************************************************/

namespace com.game.module.map
{
    public class PetMgr
    {
        public static PetMgr Instance = new PetMgr();
        public PetDisplay MyPetDisplay;

        public PetMgr()
        {
            //Singleton<PetMode>.Instance.dataUpdated += PetDataUpdateHandle;
        }

        public void Start()
        {
            //CreateMyPet();
        }

        public void Clear()
        {
            MyPetDisplay = null;
        }


        /// <summary>
        ///     响应宠物数据中心的数据改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="code"></param>
        private void PetDataUpdateHandle(object sender, int code)
        {
            switch (code)
            {
                case PetMode.FightPet:
                        UpdateFightPet();
                    break;
            }
        }

        /// <summary>
        /// 更新出战宠物
        /// </summary>
        private void UpdateFightPet()
        {
            if (AppMap.Instance.me == null)
            {
                return;
            }
            UpdateMyPet();
        }

        private void UpdateMyPet()
        {
            var petVo = PetMode.Instance.GetFightPetVo();
            if (petVo == null && MyPetDisplay != null)
            {
                Object.Destroy(MyPetDisplay.GoBase);
                MyPetDisplay = null;
            }
            else
            {
                CreateMyPet();
            }
        }

        public void CreateMyPet()
        {
            if (MyPetDisplay == null)
            {
                var petVo = PetMode.Instance.GetFightPetVo();
                if (petVo != null&&AppMap.Instance.MeControler()!=null)
                {
                    var target = AppMap.Instance.MeControler().transform.position;
                    petVo.X = target.x - 0.5f;
                    petVo.Y = target.y;
                    petVo.ModelLoadCallBack = LoadPetCallback;
                    petVo.MasterVo = MeVo.instance;
                    petVo.MasterDisplay = AppMap.Instance.me;
                    MyPetDisplay = AppMap.Instance.CreatePet(petVo);
                }
            }
        }

        private void LoadPetCallback(BaseDisplay petDisplay)
        {
            MyPetDisplay.Controller.AiController.SetAi(true);
        }

    }
}