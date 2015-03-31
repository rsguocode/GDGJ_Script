using com.game.data;
using com.game.manager;
using Com.Game.Module.Role;
using PCustomDataType;
using UnityEngine;
using System.Collections;

public class ItemContainer : Button
{
    private PGoods _goodsInfo;
    public uint Id;//
    private SysItemVo _itemVo;
    private SysEquipVo _equipVo;
    private bool _isEquip;

    public PGoods GoodsInfo
    {
        get
        {
            if (_goodsInfo == null)
            {
                _goodsInfo = GoodsMode.Instance.GetPGoodsById(Id);
            }
            return _goodsInfo;
        }
    }

    public SysItemVo ItemVo
    {
        get
        {
            if (_itemVo == null)
            {
                _itemVo = BaseDataMgr.instance.getGoodsVo(GoodsInfo.goodsId);
            }
            return _itemVo;
        }
    }

    public SysEquipVo EquipVo
    {
        get
        {
            if (_equipVo == null)
            {
                _equipVo = BaseDataMgr.instance.GetDataById<SysEquipVo>(GoodsInfo.goodsId);
            }
            return _equipVo;
        }
    }

    public bool IsEquip
    {
        get
        {
            if (GoodsInfo == null) return false;
            return  GoodsInfo.goodsId < 100000;
        }
    }
}
