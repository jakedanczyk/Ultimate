using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{

    /* Class for generic panel for UI that implements
     * -Draggable
     * -Sizeable
     */
    public class UIPanelControl : MonoBehaviour
    {
        public UIDocument document;
        public VisualElement rootElement;
        public VisualElement left;
        public VisualElement right;
        public VisualElement top;
        public VisualElement bottom;
        public VisualElement topLeft;
        public VisualElement topRight;
        public VisualElement bottomLeft;
        public VisualElement bottomRight;
        public VisualElement titleBar;
        public bool active;
        public Vector3 downPos;
        public UI_GRAB grab;
        public float lrAnchorVal, udAnchorVal;
        public float dAnchorVal, rAnchorVal;



        public virtual void Awake()
        {
        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {
            if (active)
            {
                Vector3 delta = Input.mousePosition - downPos;
                switch (grab)
                {
                    case UI_GRAB.TITLE:
                        rootElement.style.left = new StyleLength(lrAnchorVal + delta.x);
                        rootElement.style.right = new StyleLength(rAnchorVal - delta.x);
                        rootElement.style.top = new StyleLength(udAnchorVal - delta.y);
                        rootElement.style.bottom = new StyleLength(dAnchorVal + delta.y);
                        break;
                    case UI_GRAB.LEFT:
                        rootElement.style.left = new StyleLength(lrAnchorVal + delta.x);
                        break;
                    case UI_GRAB.RIGHT:
                        rootElement.style.right = new StyleLength(lrAnchorVal - delta.x);
                        break;
                    case UI_GRAB.TOP:
                        rootElement.style.top = new StyleLength(udAnchorVal - delta.y);
                        break;
                    case UI_GRAB.BOTTOM:
                        rootElement.style.bottom = new StyleLength(udAnchorVal + delta.y);
                        break;
                    case UI_GRAB.TOP_LEFT:
                        rootElement.style.top = new StyleLength(udAnchorVal + delta.y);
                        rootElement.style.left = new StyleLength(lrAnchorVal + delta.x);
                        break;
                    case UI_GRAB.TOP_RIGHT:
                        rootElement.style.top = new StyleLength(udAnchorVal + delta.y);
                        rootElement.style.right = new StyleLength(lrAnchorVal - delta.x);
                        break;
                    case UI_GRAB.BOTTOM_LEFT:
                        rootElement.style.bottom = new StyleLength(udAnchorVal - delta.y);
                        rootElement.style.left = new StyleLength(lrAnchorVal + delta.x);
                        break;
                    case UI_GRAB.BOTTOM_RIGHT:
                        rootElement.style.bottom = new StyleLength(udAnchorVal - delta.y);
                        rootElement.style.right = new StyleLength(lrAnchorVal - delta.x);
                        break;
                    default:
                        Debug.Log(grab);
                        break;
                }
            }
        }

        public void RegisterBorderCallbacks()
        {
            left = rootElement.Query("lBorder").First();
            left.RegisterCallback<PointerDownEvent>(evt =>
            {
                LeftBorderClick(evt);
            });
            right = rootElement.Query("rBorder").First();
            right.RegisterCallback<PointerDownEvent>(evt =>
            {
                RightBorderClick(evt);
            });
            top = rootElement.Query("uBorder").First();
            top.RegisterCallback<PointerDownEvent>(evt =>
            {
                TopBorderClick(evt);
            });
            bottom = rootElement.Query("bBorder").First();
            bottom.RegisterCallback<PointerDownEvent>(evt =>
            {
                BottomBorderClick(evt);
            });
            topLeft = rootElement.Query("ulBorder").First();
            topLeft.RegisterCallback<PointerDownEvent>(evt =>
            {
                TopLeftBorderClick(evt);
            });
            topRight = rootElement.Query("urBorder").First();
            topRight.RegisterCallback<PointerDownEvent>(evt =>
            {
                TopRightBorderClick(evt);
            });
            bottomLeft = rootElement.Query("blBorder").First();
            bottomLeft.RegisterCallback<PointerDownEvent>(evt =>
            {
                BottomLeftBorderClick(evt);
            }); 
            bottomRight = rootElement.Query("brBorder").First();
            bottomRight.RegisterCallback<PointerDownEvent>(evt =>
            {
                BottomRightBorderClick(evt);
            }); 
            titleBar = rootElement.Query("titleBar").First();
            titleBar.RegisterCallback<PointerDownEvent>(evt =>
            {
                TitleClick(evt);
            });
            //var i = 0;
            //foreach (VisualElement ve in new List<VisualElement>() { left, right, top, bottom, topLeft, topRight, bottomLeft, bottomRight })
            //{
            //    ve.RegisterCallback<ClickEvent, int>(OnItemClick, i);
            //    i++;
            //}
        }

        public void OnItemClick(ClickEvent evt, int idx)
        {
            // Only perform this action at the target, not in a parent
            if (evt.propagationPhase != PropagationPhase.AtTarget)
                return;

            Debug.Log("border item click" + idx);
            //rootElement.style.position = Position.Absolute;
            //rootElement.style.top = 10 * (idx + 1);
        }

        public void LeftBorderClick(PointerDownEvent evt)
        {
            if (UIManager.Instance.ClaimGlobalLock(this))
            {
                active = true;
                downPos = Input.mousePosition;
                lrAnchorVal = rootElement.style.left.value.value;
                grab = UI_GRAB.LEFT;
            }
        }
        public void RightBorderClick(PointerDownEvent evt)
        {
            if (UIManager.Instance.ClaimGlobalLock(this))
            {
                active = true;
                downPos = Input.mousePosition;
                lrAnchorVal = rootElement.style.right.value.value;
                grab = UI_GRAB.RIGHT;
            }
        }
        public void TopBorderClick(PointerDownEvent evt)
        {
            if (UIManager.Instance.ClaimGlobalLock(this))
            {
                active = true;
                downPos = Input.mousePosition;
                udAnchorVal = rootElement.style.top.value.value;
                grab = UI_GRAB.TOP;
            }
        }
        public void BottomBorderClick(PointerDownEvent evt)
        {
            if (UIManager.Instance.ClaimGlobalLock(this))
            {
                active = true;
                downPos = Input.mousePosition;
                udAnchorVal = rootElement.style.bottom.value.value;
                grab = UI_GRAB.BOTTOM;
            }
        }
        public void TopRightBorderClick(PointerDownEvent evt)
        {
            if (UIManager.Instance.ClaimGlobalLock(this))
            {
                active = true;
                downPos = Input.mousePosition;
                udAnchorVal = rootElement.style.top.value.value;
                lrAnchorVal = rootElement.style.right.value.value;
                grab = UI_GRAB.TOP_RIGHT;
            }
        }
        public void TopLeftBorderClick(PointerDownEvent evt)
        {
            if (UIManager.Instance.ClaimGlobalLock(this))
            {
                active = true;
                downPos = Input.mousePosition;
                udAnchorVal = rootElement.style.top.value.value;
                lrAnchorVal = rootElement.style.left.value.value;
                grab = UI_GRAB.TOP_LEFT;
            }
        }
        public void BottomRightBorderClick(PointerDownEvent evt)
        {
            if (UIManager.Instance.ClaimGlobalLock(this))
            {
                active = true;
                downPos = Input.mousePosition;
                udAnchorVal = rootElement.style.bottom.value.value;
                lrAnchorVal = rootElement.style.right.value.value;
                grab = UI_GRAB.BOTTOM_RIGHT;
            }
        }
        public void BottomLeftBorderClick(PointerDownEvent evt)
        {
            if (UIManager.Instance.ClaimGlobalLock(this))
            {
                active = true;
                downPos = Input.mousePosition;
                udAnchorVal = rootElement.style.bottom.value.value;
                lrAnchorVal = rootElement.style.left.value.value;
                grab = UI_GRAB.BOTTOM_LEFT;
            }
        }
        public void TitleClick(PointerDownEvent evt)
        {
            if (UIManager.Instance.ClaimGlobalLock(this))
            {
                active = true;
                downPos = Input.mousePosition;
                lrAnchorVal = rootElement.style.left.value.value;
                udAnchorVal = rootElement.style.top.value.value;
                rAnchorVal = rootElement.style.right.value.value;
                dAnchorVal = rootElement.style.bottom.value.value;
                grab = UI_GRAB.TITLE;
            }
        }
        public void ReleaseGrab()
        {
            active = false;
        }
    }
}
