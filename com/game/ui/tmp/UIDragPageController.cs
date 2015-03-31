﻿﻿using UnityEngine;
using System.Collections.Generic;

namespace com.game.ui.tmp
{

    //[AddComponentMenu("NGUI/Interaction/Page Controller")]
    public class UIDragPageController : MonoBehaviour
    {
        public List<GameObject> pages = new List<GameObject>();
        public int firstPage;

        public float width;
        public Transform target;

        public int pageCount { get { return pages.Count; } }

        void Awake() {
            immediatelyTurnToPage(firstPage);
        }

        void Update() {
            Debug.Log(getCurrentPage());
        }

        public void stop()
        {
            SpringPosition sp = target.GetComponent<SpringPosition>();
            if (sp != null)
                sp.enabled = false;
        }

        public void moveToTarget(Vector3 dest)
        {
            SpringPosition sp = SpringPosition.Begin(target.gameObject, dest, 5f);
            sp.ignoreTimeScale = true;
            sp.worldSpace = false;
        }

        public int getCurrentPage()
        {
            return Mathf.Abs((int)(target.transform.localPosition.x / width));
        }

        public void turnToPage(int page)
        {
            if (page >= 0 && page < pageCount)
            {
                stop();

                Vector3 localPostion = target.transform.localPosition;
                localPostion.x = -width * page;
                moveToTarget(localPostion);
            }
        }

        public float getX(int page)
        {
            return -width * page;
        }

        public void immediatelyTurnToPage(int page)
        {
            if (page >= 0 && page < pageCount)
            {
                stop();

                Vector3 localPostion = target.transform.localPosition;
                localPostion.x = getX(page);
                target.transform.localPosition = localPostion;
            }
        }

        public void addPage(GameObject page)
        {
            pages.Add(page);
        }
    }
}