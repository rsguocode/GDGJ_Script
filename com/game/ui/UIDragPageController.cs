﻿﻿using UnityEngine;
using System.Collections.Generic;

namespace com.game.ui
{

    [AddComponentMenu("NGUI/Interaction/DragPageController")]
    public class UIDragPageController : MonoBehaviour
    {
        public List<GameObject> pages = new List<GameObject>();
        public int firstPage;

        public float width;
        public Transform target;
        public bool disableColliderInChildren;

        public int pageCount { get { return pages.Count; } }

        public int lastPage { get; protected set; }

        void Awake()
        {
            immediatelyTurnToPage(firstPage);
        }

        public void setLastPage(int lastPage)
        {
            this.lastPage = lastPage;
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

        public void turnToPage(int page)
        {
            if (page >= 0 && page < pageCount)
            {
                stop();

                if (disableColliderInChildren) {
                    disableColliderInChildrenExcept(page);
                }

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
            if (page != null && !pages.Contains(page))
            {
                pages.Add(page);
            }
        }

        public GameObject getPage(int id)
        {
            if (id >= 0 && id < pageCount)
            {
                return pages[id];
            }

            return null;
        }

        public void disableColliderInChildrenExcept(int id)
        {
            for (int i = 0; i < pageCount; i++)
            {
                GameObject go = getPage(i);
                if (go != null)
                {
                    BoxCollider[] colliders = go.GetComponentsInChildren<BoxCollider>();
                    foreach (BoxCollider collider in colliders) {
                        collider.enabled = (id == i) ? true : false;
                    }
                }
            }
        }
    }
}