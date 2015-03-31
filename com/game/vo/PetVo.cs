using System.Security.Policy;
using com.game.data;
using com.game.manager;
/**宠物属性**/
using com.u3d.bases.display.character;

namespace com.game.vo
{
    public class PetVo : BaseRoleVo
	{
        public uint petId;    //宠物类型ID
        private SysPet _syspet;  //宠物类型VO
        public uint SkillId;  //技能id
        public uint fight; //战斗力
        public PlayerVo MasterVo; //主人的信息
        public PlayerDisplay MasterDisplay; //主人的显示

        /// <summary>
        /// 缓存SysPet信息
        /// </summary>
        public SysPet SysPet
        {
            get {
                if (_syspet == null)
                {
                    _syspet = BaseDataMgr.instance.GetSysPet(petId);
                }
                return _syspet;
             }
        }
	}
}
