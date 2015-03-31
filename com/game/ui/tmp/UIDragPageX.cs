﻿﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace com.game.ui.tmp
{
    //[AddComponentMenu("NGUI/Interaction/Drag XPage")]
    public class UIDragPageX : MonoBehaviour
    {
        public UIDragPageController controller;

        Vector3 lastHit;
        float lastX;
        float lastTime;
        Plane plane;

        float leftEdge;
        float rightEdge;

        void Awake()
        {
            if (controller == null) {
                controller = NGUITools.FindInParents<UIDragPageController>(gameObject);
            }
        }

        void OnPress(bool pressed)
        {
            leftEdge = -controller.width * (controller.pageCount - 1);
            rightEdge = 0f;

            if (controller.target != null && controller.width > 0 && controller.pageCount >= 1)
            {
                if (pressed)
                {
                    controller.stop();

                    //remember the hit position
                    lastHit = UICamera.lastHit.point;
                    lastTime = Time.timeSinceLevelLoad;
                    lastX = controller.target.transform.localPosition.x;

                    //create the plane to drag along
                    Transform trans = UICamera.currentCamera.transform;
                    plane = new Plane(trans.rotation * Vector3.back, lastHit);
                }
                else
                {
                    Vector3 localPostion = controller.target.transform.localPosition;
                    int dstPage = 0;
                    if (localPostion.x <= leftEdge)
                    {
                        //turn right
                        localPostion.x = leftEdge;
                        dstPage = controller.pageCount - 1;
                    }
                    else if (localPostion.x >= rightEdge)
                    {
                        //turn left
                        localPostion.x = rightEdge;
                        dstPage = 0;
                    }
                    else
                    {
                        float x = localPostion.x - lastX;
                        float time = Time.timeSinceLevelLoad - lastTime;
                        float speed = x / time;

                        float page = localPostion.x / controller.width;
                        if (Mathf.Abs(speed) > 200f)
                        {
                            page = speed > 0f ? Mathf.Ceil(page) : Mathf.Floor(page);
                        }

                        dstPage =  - Mathf.RoundToInt(page);
                        localPostion.x = controller.getX(dstPage);
                    }

                    controller.moveToTarget(localPostion);
                }
            }
        }

        void OnDrag(Vector2 delta)
        {
            if (enabled && NGUITools.GetActive(gameObject) && controller.target != null && controller.width > 0 && controller.pageCount >= 1)
            {
                Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.lastTouchPosition);
                float enter = 0f;
                if (plane.Raycast(ray, out enter))
                {
                    Vector3 hit = ray.GetPoint(enter);
                    Vector3 offset = hit - lastHit;
                    lastHit = hit;

                    //ajust the position
                    controller.target.position += new Vector3(offset.x, 0f, 0f);
                }
            }
        }

    }

}