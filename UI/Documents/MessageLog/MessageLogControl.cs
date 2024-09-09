using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MessageLogControl : UIPanelControl
{

    public static GameObject messageLogGameObject;
    public VisualElement messagePanel;
    public VisualElement msgListPanel;
    public VisualElement msgTemplate;

    public static MessageLogControl Instance { get; private set; }
    public override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        messagePanel = document.rootVisualElement.Query("Panel").First();
        msgListPanel = messagePanel.Query("content");
        msgTemplate = msgListPanel.Query("MsgTemplate");
    }

    public Text text;

    public void NewMessage(string msg)
    {
        Debug.Log(msg);
        //text.text = msg + "\n" + text.text;
    }
    public void DebugMessage(string msg)
    {
        Debug.LogWarning(msg);
    }
}
