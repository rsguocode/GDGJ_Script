using UnityEngine;
using System;
using System.Collections;
using com.game.module;
using com.game.utils;
using com.game.ui;
//using com.game.module.copy;
using com.u3d.bases.debug;
using com.u3d.bases.loader;
//using com.game.module.nav;

namespace com.game.dialog
{
    /// <summary>
    /// ��������
    /// </summary>
//    public class EvalDialog : BaseView
//    {
//        public static String NAME = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name;
//        public static EvalDialog instance;

//        //prefabs
//        public const string URL = "prefab/ui/dialog/copy/evaldialog";
//        public const string ITEM_URL = "prefab/ui/dialog/copy/widget/item";

//        //layout
//        public const float width = 292f;
//        public const float height = 47f;
//        public const int space = 10;
//        public const int count = 5;

//        //gameObjects
//        public GameObject itemsGo;
//        public GameObject confirmGo;

//        protected GameObject itemGo;
//        protected GameObject groupGo;

//        protected UIGrid grid;

//        private EvalDialog() { }

//        protected override void init()
//        {
//            //set instance
//            instance = this;

//            //load all widgets
//            itemGo = ResMgr.instance.load(ITEM_URL) as GameObject;

//            //only for test
//            makeClipPanel(itemsGo);
//            addItem("С����", 500, 50);

//            //register all events
//            addOnClickEvent(confirmGo, onConfirm);  //ȷ����ť

//            //flag
//            hasInited = true;
//        }

//        /// <summary>
//        /// �����
//        /// </summary>
//        /// <param name="name">�������</param>
//        /// <param name="exp">����</param>
//        /// <param name="story">����</param>
//        /// <returns>��Ϸ����</returns>
//        public GameObject addItem(string name, object exp, object story)
//        {
//            GameObject child = Tools.addChild(groupGo, itemGo, -1f);

//            UIDragObject dragObject = child.AddComponent<UIDragObject>();
//            dragObject.scale = new Vector3(0f, 1f, 0f);
//            dragObject.target = groupGo.transform;
//            //dragObject.backTop = true;
//            dragObject.restrictWithinPanel = true;
//            grid.repositionNow = true;

//            UILabel label = child.GetComponentInChildren<UILabel>();
//            label.text = String.Format("{0}��[00FF00]����+{1}[-] [FF9209]����+{2}[-]", name, exp, story);

//            return child;
//        }

//        protected void makeClipPanel(GameObject go)
//        {
//            UIPanel panel = go.AddComponent<UIPanel>();
//            panel.clipping = UIDrawCall.Clipping.SoftClip;
//            panel.clipSoftness = new Vector2(1f, 1f);
//            panel.clipRange = new Vector4(0f, 0f, width, height * count + space * (count - 1));
//            UIDraggablePanel draggablePanel = go.AddComponent<UIDraggablePanel>();
//            draggablePanel.scale = new Vector3(0f, 1f, 0f);
//            draggablePanel.momentumAmount = 0;

//            groupGo = NGUITools.AddChild(go);
//            groupGo.name = "group";
//            groupGo.transform.localPosition = new Vector3(0f, height * count * 0.5f, 0f);

//            grid = groupGo.AddComponent<Grid>();
//            grid.cellWidth = width + space;
//            grid.cellHeight = height + space;
//            grid.maxPerLine = 1;
//        }

//        /**ȷ����ť**/
//        protected void onConfirm(GameObject go)
//        {
////            string copyId = CopyMgr.instance.requestInCopyId;
//            navControl.openView();         //������ͼ
////            Log.info(this, "-onConfirm() �˳�����ID��" + copyId);
////            (control.mode as CopyMode).requestExitCopy(copyId);
//        }

//        private NavControl navControl { get { return (NavControl)AppFacde.instance.getControl(NavControl.NAME); } }
//    }
}