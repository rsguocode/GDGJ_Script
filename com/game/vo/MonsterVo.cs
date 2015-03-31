using com.game.data;
using com.game.manager;

/**怪物|Boss 属性**/
using System.Collections.Generic;

namespace com.game.vo
{
    public class MonsterParamStruct
    {
        public uint MonsterId;
    }

    public class MonsterVo : BaseRoleVo
    {
        private SysMonsterVo _monsterVo; //怪物类型VO
        private MonsterParamStruct _paramStruct;
		public List<uint> goods = new List<uint> ();  //怪物掉落物品
        public byte bornType; //出生类型，123分别对应出场的不同方式
        public uint monsterId; //怪物类型ID


        public override uint CurHp
        {
            get { return base.CurHp; }
            set
            {
                if (value != _curHp)
                {
                    if (_paramStruct == null)
                    {
                        _paramStruct = new MonsterParamStruct();
                    }
                    _paramStruct.MonsterId = (uint)MonsterVO.id;
                    MeVo.instance.DataUpdateWithParam(MeVo.MonsterDataHpUpdateWithParam, _paramStruct);
                    base.CurHp = value;
                }
            }
        }

        /// <summary>
        ///     缓存SysMonsterVo信息，避免使用到怪物相关信息时多次查表
        /// </summary>
        public SysMonsterVo MonsterVO
        {
            get
            {
                if (_monsterVo == null)
                {
                    _monsterVo = BaseDataMgr.instance.getSysMonsterVo(monsterId);
                }
                return _monsterVo;
            }
        }
    }
}