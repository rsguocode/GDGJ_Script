

using System.Collections.Generic;
using System.Linq;
using com.game.consts;
using com.game.data;
using com.game.manager;
using Com.Game.Module.Copy;
using Com.Game.Module.DaemonIsland;
using Com.Game.Module.Role;
using com.game.module.test;
using com.game.Public.Message;
using com.game.utils;
using PCustomDataType;
using UnityEngine;

namespace Com.Game.Module.Pet
{
    internal class PetStoneTipsView : Singleton<PetStoneTipsView>
    {
        private GameObject gameObject;
        private List<GameObject> fbList;
        public TweenPlay play;
        private bool close;
        private SysPet currentPet;

        public EventDelegate.Callback AfterOpenViewGuideDelegate;
        public Button OkButton;

        public void Init(GameObject obj)
        {
            gameObject = obj;
            obj.SetActive(false);
            fbList = new List<GameObject>();
            fbList.Add(NGUITools.FindChild(gameObject, "info/fb/1"));
            fbList.Add(NGUITools.FindChild(gameObject, "info/fb/2"));
            fbList.Add(NGUITools.FindChild(gameObject, "info/fb/3"));

            NGUITools.FindInChild<Button>(fbList[0], "").onClick = OnFbClick;
            NGUITools.FindInChild<Button>(fbList[1], "").onClick = OnFbClick;
            NGUITools.FindInChild<Button>(fbList[2], "").onClick = OnFbClick;

            close = false;

            play = NGUITools.FindInChild<TweenPlay>(gameObject, "info");
            EventDelegate.Add(play.onFinished,Close);

            NGUITools.FindInChild<UIWidgetContainer>(gameObject, "background").onClick = OnCloseClick;
            OkButton = NGUITools.FindInChild<Button>(gameObject, "info/button");
            OkButton.onClick = OnCloseClick;

            NGUITools.FindInChild<UISprite>(gameObject, "info/fb/current/icn").atlas =
                Singleton<PetView>.Instance.PropAltas;
        }

        public void OpenView(uint petId)
        {
            SysPet pet = BaseDataMgr.instance.GetDataById<SysPet>(petId);
            currentPet = pet;

            SetStoneInfo();
            SetFBInfo();
            gameObject.SetActive(true);
            close = false;
            play.PlayReverse();
            Singleton<GoodsMode>.Instance.dataUpdated += DataUpdated;
            Singleton<CopyMode>.Instance.dataUpdated += DataUpdated;
            Singleton<DaemonIslandMode>.Instance.dataUpdated += DataUpdated;
            if (AfterOpenViewGuideDelegate != null)
            {
                EventDelegate.Add(play.onFinished, AfterOpenViewGuideDelegate);
                AfterOpenViewGuideDelegate = null;
            }
        }

        private void SetStoneInfo()
        {
            SysPet pet = currentPet;
            NGUITools.FindInChild<UILabel>(gameObject,"info/top/name").text = pet.name;
            SysItemVo itemVo = BaseDataMgr.instance.GetDataById<SysItemVo>((uint)currentPet.stone_id);
            NGUITools.FindInChild<UISprite>(gameObject, "info/fb/current/icn").spriteName = itemVo.icon.ToString();
            NGUITools.FindInChild<UISprite>(gameObject, "info/fb/current/gradeicn").spriteName = "";

            int own = Singleton<GoodsMode>.Instance.GetCountByGoodsId((uint)pet.stone_id);

            PPet p = Singleton<PetMode>.Instance.GetPetByPetId((uint)pet.id);
            int need = 0;
            if (p != null)
            {
                need = PetLogic.GetNeedStone(p.star, true);
            }
            else
            {
                need = PetLogic.GetNeedStone((uint)pet.star, false);
            }
            NGUITools.FindInChild<UILabel>(gameObject, "info/fb/num").text = "("+own+"/"+need+")";
        }

        private void SetFBInfo()
        {
            SysItemVo itemVo = BaseDataMgr.instance.GetDataById<SysItemVo>((uint)currentPet.stone_id);
            int[] fbs = StringUtils.GetStringToInt(itemVo.source);
            PetLogic.SetFBInfo(fbList,fbs);
        }

        private void DataUpdated(object sender, int code)
        {
            if (sender == Singleton<GoodsMode>.Instance && code == Singleton<GoodsMode>.Instance.UPDATE_PET_GOODS)
            {
                SetStoneInfo();
            }
            else if(sender == Singleton<CopyMode>.Instance || sender== Singleton<DaemonIslandMode>.Instance)
            { 
                SetFBInfo();
            }
        }

        private void OnFbClick(GameObject obj)
        {
            uint id = uint.Parse(obj.name);

            if (Singleton<CopyControl>.Instance.IsCopyOpened(id))
            {
                Singleton<CopyControl>.Instance.OpenCopyById(id);
            }
            else
            {
                MessageManager.Show("副本尚未开启！");
            }

        }

        private void OnCloseClick(GameObject obj)
        {
            close = true;
            play.PlayForward();
        }

        private void Close()
        {
            if (close)
            {
                CloseView();
            }

        }

        private void CloseView()
        {
            close = false;
            gameObject.SetActive(false);
            Singleton<GoodsMode>.Instance.dataUpdated -= DataUpdated;
            Singleton<CopyMode>.Instance.dataUpdated -= DataUpdated;
            Singleton<DaemonIslandMode>.Instance.dataUpdated -= DataUpdated;
        }

    }
}
