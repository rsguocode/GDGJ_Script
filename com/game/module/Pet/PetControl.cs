
using System.IO;
using com.game;
using com.game.cmd;
using com.game.module.test;
using com.game.Public.Message;
using com.net.interfaces;
using Proto;


namespace Com.Game.Module.Pet
{
	class PetControl:BaseControl<PetControl>
	{
        //注册
	    protected override void NetListener()
	    {
            AppNet.main.addCMD(CMD.CMD_21_1, ReceivePetList);
            AppNet.main.addCMD(CMD.CMD_21_2, ReceivePetFigth);
            AppNet.main.addCMD(CMD.CMD_21_3, ReceivePetEvolve);
            AppNet.main.addCMD(CMD.CMD_21_4, ReceivePetUpgrade);
            AppNet.main.addCMD(CMD.CMD_21_5, ReceivePetSkillUpgrade); 
            AppNet.main.addCMD(CMD.CMD_21_6, ReceivePetUpdate);
            AppNet.main.addCMD(CMD.CMD_21_7, ReceivePetSkillPointInfo);
            AppNet.main.addCMD(CMD.CMD_21_8, ReceiveActiveAPet);
            AppNet.main.addCMD(CMD.CMD_21_9, ReceiveBuySkillPointInfo);
            AppNet.main.addCMD(CMD.CMD_21_10, ReceivePetExpItem);
            AppNet.main.addCMD(CMD.CMD_24_1, ReceivePetWearEquip); 
            AppNet.main.addCMD(CMD.CMD_24_2, ReceiveEquipCombineEquip);

            PetLogic.InitConfig();
            SendRequestForPetList();
            SendRequestFoSkillPointInfo();
	    }

        //获取宠物列表信息
	    public void SendRequestForPetList()
	    {
	        MemoryStream mem = new MemoryStream();
	        Module_21.write_21_1(mem);
	        AppNet.gameNet.send(mem, 21, 1);
	    }

	    //接收到宠物列表信息
	    private void ReceivePetList(INetData data)
	    {
           PetListMsg_21_1 msg = new PetListMsg_21_1();
           msg.read(data.GetMemoryStream());
           Singleton<PetMode>.Instance.SetPetList(msg.pet);
	    }


	    public void ActiveAPet(uint petId)
	    {
            MemoryStream mem = new MemoryStream();
            Module_21.write_21_8(mem,(ushort)petId);
            AppNet.gameNet.send(mem,21,8);
	    }

        private void ReceiveActiveAPet(INetData data)
	    {
            PetActivateMsg_21_8 msg = new PetActivateMsg_21_8();
            msg.read(data.GetMemoryStream());
            if (msg.code != 0)
            {
                ErrorCodeManager.ShowError(msg.code);
            }
            else
            {
                Singleton<PetMode>.Instance.AddPet(msg.pet);
                
            }
	     }

	    public void SendPetFight(uint petid,bool fight)
	    {
	        byte state = 0;
	        if (fight)
	        {
	            state = 2;
	        }
	        MemoryStream mem = new MemoryStream();
            Module_21.write_21_2(mem, petid,state);
            AppNet.gameNet.send(mem, 21, 2);
	    }

	    private void ReceivePetFigth(INetData data)
	    {
            PetStateMsg_21_2 msg = new PetStateMsg_21_2();
            msg.read(data.GetMemoryStream());
	        if (msg.code != 0)
	        {
	            ErrorCodeManager.ShowError(msg.code);
	        }
	    }

        public void SendPetEvolve(uint petid)
        {
            MemoryStream mem = new MemoryStream();
            Module_21.write_21_3(mem, petid);
            AppNet.gameNet.send(mem, 21, 3);
        }

        private void ReceivePetEvolve(INetData data)
        {
            PetStarUpgradeMsg_21_3 msg = new PetStarUpgradeMsg_21_3();
            msg.read(data.GetMemoryStream());
            if (msg.code != 0)
            {
                ErrorCodeManager.ShowError(msg.code);
            }
        }

        //宠物升阶
        public void SendPetUpgrade(uint petid)
        {
            MemoryStream mem = new MemoryStream();
            Module_21.write_21_4(mem, petid);
            AppNet.gameNet.send(mem, 21, 4);
        }

        private void ReceivePetUpgrade(INetData data)
        {
            PetGradeUpgradeMsg_21_4 msg = new PetGradeUpgradeMsg_21_4();
            msg.read(data.GetMemoryStream());
            if (msg.code != 0)
            {
                ErrorCodeManager.ShowError(msg.code);
            }
        }

	    private void ReceivePetUpdate(INetData data)
	    {
            PetItemUpdateMsg_21_6 msg = new PetItemUpdateMsg_21_6();
            msg.read(data.GetMemoryStream());
            Singleton<PetMode>.Instance.UpdatePet(msg.id,msg.info);

            
	    }

        //升级宠物的技能
	    public void UpgradePetSkill(uint petid, uint pos){
            MemoryStream mem = new MemoryStream();
	        byte attrId = (byte)pos;
	        Module_21.write_21_5(mem, petid, attrId);
            AppNet.gameNet.send(mem,21,5);
	    }

        private void ReceivePetSkillUpgrade(INetData data)
	    {
            PetSkillUpgradeMsg_21_5 msg = new PetSkillUpgradeMsg_21_5();
            msg.read(data.GetMemoryStream());
            if (msg.code != 0)
            {
                ErrorCodeManager.ShowError(msg.code);
            }
	    }

        //穿装备
        public void WearPetEquip(uint uid,int pos,uint equipid)
        {
            MemoryStream mem = new MemoryStream();
            Module_24.write_24_1(mem,uid,equipid,(byte)pos);
            AppNet.gameNet.send(mem, 24, 1);
        }

	    private void ReceivePetWearEquip(INetData data)
	    {
            PetEquipOnMsg_24_1 msg = new PetEquipOnMsg_24_1();
            msg.read(data.GetMemoryStream());
            if (msg.code != 0)
            {
                ErrorCodeManager.ShowError(msg.code);
            }
	    }

	    public void CombinePetEquip(uint equipId)
	    {
            MemoryStream mem = new MemoryStream();
            Module_24.write_24_2(mem, equipId);
            AppNet.gameNet.send(mem, 24, 2);
	    }

	    private void ReceiveEquipCombineEquip(INetData data)
	    {
            PetEquipMaterialMsg_24_2 msg = new PetEquipMaterialMsg_24_2();
            msg.read(data.GetMemoryStream());
	        if (msg.code != 0)
	        {
	            ErrorCodeManager.ShowError(msg.code);
	        }
	        else
	        {
                Singleton<PetEquipView>.Instance.PlayCombine();
	        }

	    }

        public void SendRequestFoSkillPointInfo()
        {
            MemoryStream mem = new MemoryStream();
            Module_21.write_21_7(mem);
            AppNet.gameNet.send(mem, 21, 7);
        }

        private void ReceivePetSkillPointInfo(INetData data)
	    {
            PetSkillPointInfoMsg_21_7 msg = new PetSkillPointInfoMsg_21_7();
            msg.read(data.GetMemoryStream());
            Singleton<PetMode>.Instance.SetSkillPointInfo(msg);

	    }

        public void BuySkillPoint()
        {
            MemoryStream mem = new MemoryStream();
            Module_21.write_21_9(mem);
            AppNet.gameNet.send(mem, 21, 9);
        }


        private void ReceiveBuySkillPointInfo(INetData data)
        {
            PetBuySkillPointMsg_21_9 msg = new PetBuySkillPointMsg_21_9();
            msg.read(data.GetMemoryStream());
            if (msg.code != 0)
            {
                ErrorCodeManager.ShowError(msg.code);
            }
           

        }

        //使用宠物经验丹
	    public void UsePetExpItem(uint petId,uint itemId,uint num)
	    {
            MemoryStream mem = new MemoryStream();
            Module_21.write_21_10(mem, petId, itemId,(byte) num);
            AppNet.gameNet.send(mem, 21, 10);
	    }

	    private void ReceivePetExpItem(INetData data)
	    {
            PetExpUpMsg_21_10 msg = new PetExpUpMsg_21_10();
            msg.read(data.GetMemoryStream());
	        if (msg.code != 0)
	        {
                ErrorCodeManager.ShowError(msg.code);
	        }

	    }
	}
}

