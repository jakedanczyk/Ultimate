using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum BORDER
{

}

public class ResizeBorder : VisualElement
{
    public int lr;
    public int ud;

    public Vector3 clickPos;
    private void Awake()
    {
        this.RegisterCallback<ClickEvent>(OnClicked);

    }

    void Update()
    {

    }

    private void OnClicked(ClickEvent evt)
    {
        clickPos = evt.position;
    }

}
