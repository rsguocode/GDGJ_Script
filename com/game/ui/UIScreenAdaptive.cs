﻿﻿
using UnityEngine;

namespace com.game.ui
{
    /// <summary>
    /// ��Ļ����Ӧ
    /// </summary>
    public class UIScreenAdaptive : MonoBehaviour
    {
        public enum Style
        {
            None,
            Horizontal,
            Vertical,
            Both,
            BasedOnHeight,
            BasedOnWidth,
            BasedOnScreen,
        }

        public enum ScreenSize
        {
            SIZE_16TO9,
            SIZE_16TO10,
            SIZE_3TO2,
            SIZE_4TO3,
            SIZE_5TO4,
        }

        private float scr = 1.0f;
        public Camera uiCamera = null;
        public UIWidget widgetContainer = null;
        public UIPanel panelContainer = null;
        public Style style = Style.None;
        public float correction = 1.0f;
        public Vector2 relativeSize = Vector2.one;
        public Vector2 screenSize = new Vector2(960f, 640f);
        Transform mTrans;
        UIRoot mRoot;
        Animation mAnim;
        Rect mRect;

        void Awake()
        {
            mAnim = animation;
            mRect = new Rect();
            mTrans = transform;

            if (Screen.height < 640) Destroy(this);

            if (style == Style.None)
            {
                ScreenSize size = GetRatio();
                if (size == ScreenSize.SIZE_4TO3 || size == ScreenSize.SIZE_5TO4)
                {
                    style = Style.BasedOnWidth;
                }
                else
                {
                    style = Style.BasedOnHeight;
                }
            }
        }

        void Start()
        {
            //correction = (1280f/720f)/(Screen.width/Screen.height)*1.08f;
            if (uiCamera == null) uiCamera = NGUITools.FindCameraForLayer(gameObject.layer);
            mRoot = NGUITools.FindInParents<UIRoot>(gameObject);

            adaptive();
        }

        void Update()
        {
            adaptive();
        }

        void adaptive()
        {
            if (mAnim != null && mAnim.isPlaying) return;

            if (style != Style.None)
            {
                float adjustment = 1f;

                if (panelContainer != null)
                {
                    if (panelContainer.clipping == UIDrawCall.Clipping.None)
                    {
                        mRect.xMin = -Screen.width * 0.5f;
                        mRect.yMin = -Screen.height * 0.5f;
                        mRect.xMax = -mRect.xMin;
                        mRect.yMax = -mRect.yMin;
                    }
                    else
                    {
                        Vector4 pos = panelContainer.clipRange;
                        mRect.x = pos.x - (pos.z * 0.5f);
                        mRect.y = pos.y - (pos.w * 0.5f);
                        mRect.width = pos.z;
                        mRect.height = pos.w;
                    }
                }
                else if (widgetContainer != null)
                {
                    Transform t = widgetContainer.cachedTransform;
                    Vector3 ls = t.localScale;
                    Vector3 lp = t.localPosition;

                    Vector3 size = widgetContainer.relativeSize;
                    Vector3 offset = widgetContainer.pivotOffset;
                    offset.y -= 1f;

                    offset.x *= (widgetContainer.relativeSize.x * ls.x);
                    offset.y *= (widgetContainer.relativeSize.y * ls.y);

                    mRect.x = lp.x + offset.x;
                    mRect.y = lp.y + offset.y;

                    mRect.width = size.x * ls.x;
                    mRect.height = size.y * ls.y;
                }
                else if (uiCamera != null)
                {
                    mRect = uiCamera.pixelRect;
                    if (mRoot != null) adjustment = mRoot.pixelSizeAdjustment;
                }
                else return;

                float rectWidth = mRect.width;
                float rectHeight = mRect.height;

                if (adjustment != 1f && rectHeight > 1f)
                {
                    float scale = mRoot.activeHeight / rectHeight;
                    rectWidth *= scale;
                    rectHeight *= scale;
                }

                Vector3 localScale = mTrans.localScale;

                if (style == Style.BasedOnHeight)
                {
                    localScale.x = relativeSize.x * Screen.height / screenSize.y * correction;
                    localScale.y = relativeSize.y * Screen.height / screenSize.y * correction;
                    localScale.z = localScale.x;
                    if (mTrans.localScale != localScale) mTrans.localScale = localScale;
                }
                else
                {
                    if (style == Style.Both || style == Style.Horizontal) localScale.x = relativeSize.x * rectWidth;
                    if (style == Style.Both || style == Style.Vertical) localScale.y = relativeSize.y * rectHeight;
                }

                if (style == Style.BasedOnScreen)
                {
                    localScale.x = relativeSize.x * Screen.width / screenSize.x * correction;
                    localScale.y = relativeSize.y * Screen.height / screenSize.y * correction;
                    localScale.z = localScale.x;
                    if (mTrans.localScale != localScale) mTrans.localScale = localScale;
                }
                if (style == Style.BasedOnWidth)
                {
                    localScale.x = relativeSize.x * Screen.width / screenSize.x * correction;
                    localScale.y = relativeSize.y * Screen.width / screenSize.x * correction;
                    localScale.z = localScale.x;
                    if (mTrans.localScale != localScale) mTrans.localScale = localScale;
                }
            }
        }

        public static ScreenSize GetRatio()
        {
            float ratio = (float)Screen.width / (float)Screen.height;
            ScreenSize size = ScreenSize.SIZE_5TO4;

            if (Mathf.Abs(ratio - 16f / 9f) < 0.01f)
            {
                size = ScreenSize.SIZE_16TO9;
            }
            else if (Mathf.Abs(ratio - 16f / 10f) < 0.01f)
            {
                size = ScreenSize.SIZE_16TO10;
            }
            else if (Mathf.Abs(ratio - 3f / 2f) < 0.01f)
            {
                size = ScreenSize.SIZE_3TO2;
            }
            else if (Mathf.Abs(ratio - 4f / 3f) < 0.01f)
            {
                size = ScreenSize.SIZE_4TO3;
            }
            else if (Mathf.Abs(ratio - 5f / 4f) < 0.01f)
            {
                size = ScreenSize.SIZE_5TO4;
            }

            return size;
        }

    }
}