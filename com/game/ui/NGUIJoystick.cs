using System;
using com.u3d.bases.consts;
using UnityEngine;
using Com.Game.Speech;

public class NGUIJoystick : MonoBehaviour
{
    private Vector3 center;
    private Vector3 mLastPos;
    private UIPanel mPanel;
    private Plane mPlane;
    [HideInInspector] public Vector2 position;
    public float radius = 2.0f;
    public Vector3 scale = Vector3.one;

	public bool IsBattleJoystick = false;
	private bool battleJoyHasPressed = false;
	private float lastPressTime = 0f;
	private float lastReleaseTime = 0f;
    public static bool IsPressed;
    public static bool IsLeft;
    public static bool IsRight;

    private void Start()
    {
        center = transform.localPosition;
    }

    private void OnDrop(GameObject go)
    {
        
    }
    /// <summary>
    ///     Create a plane on which we will be performing the dragging.
    /// </summary>
    private void OnPress(bool pressed)
    {
        if (enabled && gameObject.activeInHierarchy)
        {
            if (pressed)
            {
				if (IsBattleJoystick)
				{
					battleJoyHasPressed = true;
					lastPressTime = Time.time;
				}

                mLastPos = UICamera.lastHit.point;
                transform.position = mLastPos;
                SetPosition();
                mPlane = new Plane(Vector3.back, mLastPos);
            }
            else
            {
				transform.localPosition = center;
                position = Vector2.zero;
                SetPosition();
				if (IsBattleJoystick && battleJoyHasPressed)
				{
					battleJoyHasPressed = false;
					lastReleaseTime = Time.time;

					if (lastReleaseTime-lastPressTime > 20f)
					{
						if (SpeechMgr.Instance.IsMagicSpeech)
						{
							SpeechMgr.Instance.PlaySpeech(SpeechConst.MagicContinueMoveDirection);
						}
					}
				}
            }
            IsPressed = pressed;
        }
    }

    public void Reset()
    {
        OnPress(false);
    }

    /// <summary>
    ///     Drag the object along the plane.
    /// </summary>
    private void OnDrag(Vector2 delta)
    {
        if (enabled && gameObject.activeInHierarchy)
        {
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;

            Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
            float dist = 0f;

            if (mPlane.Raycast(ray, out dist))
            {
                Vector3 currentPos = ray.GetPoint(dist);
                Vector3 offset = currentPos - mLastPos;
                mLastPos = currentPos;

                if (Math.Abs(offset.x) > 0 || Math.Abs(offset.y) > 0)
                {
                    offset = transform.InverseTransformDirection(offset);
                    offset.Scale(scale);
                    offset = transform.TransformDirection(offset);
                }

                offset.z = 0;
                transform.position += offset;

                SetPosition();
            }
        }
    }

    private void SetPosition()
    {
        float length = transform.localPosition.magnitude;
        if (length > radius)
        {
            transform.localPosition = Vector3.ClampMagnitude(transform.localPosition, radius);
        }
        position = new Vector2((transform.localPosition.x - center.x),
            (transform.localPosition.y - center.y));
        position = position.normalized;
        int dir = Directions.GetDirByVector2(position);
        IsLeft = dir == Directions.Left;
        IsRight = dir == Directions.Right;
    }
}