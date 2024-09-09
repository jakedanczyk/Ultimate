using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    public virtual void Awake()
    {
    }

    public virtual void Start()
    {

    }
    
    public void RegisterBorderCallbacks()
    {
        left = rootElement.Query("lBorder").First();
        right = rootElement.Query("rBorder").First();
        top = rootElement.Query("uBorder").First();
        bottom = rootElement.Query("bBorder").First();
        topLeft = rootElement.Query("ulBorder").First();
        topRight = rootElement.Query("urBorder").First();
        bottomLeft = rootElement.Query("blBorder").First();
        bottomRight = rootElement.Query("brBorder").First();
        titleBar = rootElement.Query("titleBar").First();
        var i = 0;
        foreach (VisualElement ve in new List<VisualElement>() { left, right, top, bottom, topLeft, topRight, bottomLeft, bottomRight })
        {
            ve.RegisterCallback<ClickEvent, int>(OnItemClick, i);
            i++;
        }
    }

    public void OnItemClick(ClickEvent evt, int idx)
    {
        // Only perform this action at the target, not in a parent
        if (evt.propagationPhase != PropagationPhase.AtTarget)
            return;

        Debug.Log("border item click" + idx);
        rootElement.style.position = Position.Absolute;
        rootElement.style.top = 10 * (idx + 1);
    }

}
