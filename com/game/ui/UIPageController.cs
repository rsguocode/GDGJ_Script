﻿﻿using UnityEngine;
using System.Collections.Generic;

namespace com.game.ui
{

    //[AddComponentMenu("NGUI/Interaction/Page Controller")]
    public class UIPageController : MonoBehaviour
    {
        public List<GameObject> pageBtns;
        public List<GameObject> pages = new List<GameObject>();
        public GameObject firstPage;
        private float lastPostionX;
        private bool moveStart = false;
        public int lastPage = 0;

        public int pageCount
        {
            get { return pages.Count; }
        }

        void clear()
        {
            lastPostionX = 0f;
            moveStart = false;
            lastPage = 0;
        }


        public void addPage(GameObject page)
        {
            pages.Add(page);
        }

        public List<GameObject> GetAllPages()
        {
            return pages;
        }

        public void OnReset()
        {
            pageBtns = null;
            pages = null;
            firstPage = null;
            clear();
        }

        public int GetLastPage()
        {
            return lastPage;
        }
        // be set page
        void OnTurnPage(int page)
        {
            if (pageBtns != null && page >= 0 && page < pageBtns.Count)
            {
                pageBtns[page].SendMessage("OnChooseButton", SendMessageOptions.DontRequireReceiver);
                lastPage = page;
            }
            else if (page >= 0 && page < pages.Count)
            {
                lastPage = page;
            }
        }

        void OnEnable()
        {
            //clear();
            if (firstPage != null)
                firstPage.SendMessage("OnImmediatelyTurnToPage", lastPage, SendMessageOptions.DontRequireReceiver);
            if (pageBtns != null && lastPage >= 0 && lastPage < pageBtns.Count)
            {
                pageBtns[lastPage].SetActiveRecursively(true);
                pageBtns[lastPage].SendMessage("OnChooseButton", SendMessageOptions.DontRequireReceiver);
            }
            LeavePage();
        }

        // press btn
        void OnTurnToPage(int page)
        {
            if (pages != null && page >= 0 && page < pages.Count)
            {
                OpenAllPage();
                if (firstPage != null)
                    firstPage.SendMessage("OnTurnToPage", page, SendMessageOptions.DontRequireReceiver);
                lastPage = page;
            }
        }

        // page Start LastMoveX
        void OnDragLastMoveX(DragXPostion xData)
        {
            moveStart = xData.moved;
            lastPostionX = xData.x;
        }

        void LateUpdate()
        {
            if (moveStart)
            {
                SpringPosition sp = GetComponent<SpringPosition>();
                if (sp != null && sp.enabled == false)
                {
                    Vector3 localPostion = transform.localPosition;
                    localPostion.x = lastPostionX;
                    transform.localPosition = localPostion;
                    moveStart = false;
                    LeavePage();
                    gameObject.SendMessage("OnTurnStop", lastPage, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        public void OpenAllPage()
        {
            if (pages != null)
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    if (pages[i].active == false)
                        pages[i].SetActiveRecursively(true);
                }
            }
        }

        public void LeavePage()
        {
            if (pages != null)
            {
                for (int i = 0; i < pages.Count; i++)
                {
                    if (pages[i].active && i != lastPage)
                    {
                        pages[i].SetActiveRecursively(false);
                    }
                }
                if (lastPage >= 0 && lastPage < pages.Count && pages[lastPage].active == false)
                    pages[lastPage].SetActiveRecursively(true);
            }
        }

        public void SimulatorOnClickTurnPage(int page)
        {
            if (pages != null && pageBtns != null && page >= 0 && page < pages.Count)
            {
                pageBtns[page].SendMessage("OnClick", SendMessageOptions.DontRequireReceiver);
            }
        }

        public void SimulatorOnClickTurnPageWithoutButton(int page)
        {
            OnTurnToPage(page);
        }

        public void SimulatorOnClickTurnPageImmediately(int page)
        {
            if (pages != null && pageBtns != null && page >= 0 && page < pages.Count)
            {
                OpenAllPage();
                if (firstPage != null)
                    firstPage.SendMessage("OnImmediatelyTurnToPage", page, SendMessageOptions.DontRequireReceiver);
                if (page < pageBtns.Count)
                    pageBtns[page].SendMessage("OnChooseButton", SendMessageOptions.DontRequireReceiver);
                lastPage = page;
                LeavePage();
                gameObject.SendMessage("OnTurnStop", lastPage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}