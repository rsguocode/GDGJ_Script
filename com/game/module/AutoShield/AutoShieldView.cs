using System.Collections.Generic;
using com.game;
using com.game.module.Store;
using com.u3d.bases.display.controler;
using UnityEngine;
using System.Collections;
using com.game.module.test;

namespace Com.Game.Module.AutoShieldView
{
    public class AutoShieldView : BaseView<AutoShieldView>
    {
        public override string url { get { return "UI/AutoShield/AutoShield.assetbundle"; } }
        public override ViewLayer layerType { get { return ViewLayer.TopLayer; } }
        private Button btn_shield;

        protected override void Init()
        {
            btn_shield = FindInChild<Button>("shield");
            btn_shield.onPress = ShieldOnPress;
            btn_shield.onClick = ShieldOnClick;
            btn_shield.onDrag = ShieldOnDrag;
        }

        private struct HitGo
        {
            public int depth;
            public GameObject go;
        }

        private List<HitGo> hitGoList = new List<HitGo>();

        private void ShieldOnPress(GameObject go, bool state)
        {
            if(state)
                ShieldSendHelp("OnClick");
        }
        private void ShieldOnDrag(GameObject go1, Vector2 delta)
        {
            ShieldSendHelp("OnDrag",delta);
        }
        private void ShieldOnClick(GameObject go1)
        {
            ShieldSendHelp("OnClick");
        }

        private void ShieldSendHelp(string fuctionName,object param = null)
        {
            this.CloseView();
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
            Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            HitGo hitGo;
            hitGoList.Clear();
            if (hits.Length > 0)
            {
                for (int b = 0; b < hits.Length; ++b)
                {
                    GameObject go = hits[b].collider.gameObject;
                    UIWidget w = go.GetComponent<UIWidget>();
                    hitGo = new HitGo();
                    if (w != null)
                    {
                        if (!w.isVisible) continue;
                        if (w.hitCheck != null && !w.hitCheck(hits[b].point)) continue;
                    }
                    else
                    {
                        UIRect rect = NGUITools.FindInParents<UIRect>(go);
                        if (rect != null && rect.finalAlpha < 0.001f) continue;
                    }

                    hitGo.depth = NGUITools.CalculateRaycastDepth(go);
                    hitGo.go = go;
                    if (hitGo.depth != int.MaxValue)
                    {
                        hitGoList.Add(hitGo);
                    }
                }
                hitGoList.Sort(delegate(HitGo r1, HitGo r2) { return r2.depth.CompareTo(r1.depth); });
                (AppMap.Instance.me.Controller as ActionControler).StopWalk();
                hitGoList[0].go.SendMessage(fuctionName, param, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

}

