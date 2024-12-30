using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Urth
{
    public class Message
    {
        public string realtime;//real-world time
        public double gametime;//simulation run time
        public double urthtime;//in-game time
        public string msg;
        public string source;

        public Message(string m)
        {
            msg = m;
            realtime = System.DateTime.UtcNow.ToString();
            gametime = Time.timeAsDouble;
            urthtime = UrthTime.Instance.totalGameSeconds;
        }
    }

    public class MessageLogControl : UIPanelControl
    {

        public static GameObject messageLogGameObject;
        public VisualElement messageLog;
        public VisualElement panel;
        public VisualElement content;

        public VisualElement msgListPanel;

        public List<Message> messageList;
        public ListView msgListView;
        [SerializeField]
        VisualTreeAsset msgTemplate;


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

            messageLog = document.rootVisualElement.Query("MessageLog").First();
            panel = messageLog.ElementAt(0).ElementAt(0);
            msgListPanel = messageLog.Query("content");
        }

        public Text text;

        public void NewMessage(string msg)
        {
            Debug.Log(msg);
            //text.text = msg + "\n" + text.text;
            messageList.Add(new Message(msg));
        }
        public void DebugMessage(string msg)
        {
            Debug.LogWarning(msg);
        }

        public void UpdateLog()
        {



            msgListView.itemsSource = messageList;
            msgListView.makeItem = () => msgTemplate.Instantiate();
            msgListView.bindItem = (VisualElement element, int index) =>
            {
                VisualElement msgElement = element.Query("message").First();
                VisualElement click = msgElement.Query("click").First();
                click.RegisterCallback<ClickEvent, int>(OnItemClick, index);
                //click.RegisterCallback<ClickEvent>(OnItemClick);
                Label label = msgElement.Query("messageLabel").First() as Label;
                //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);
                //itemNameLabel.RegisterCallback<ClickEvent>(OnItemClick);

                label.text = messageList[index].msg;
            };

        }
        public new void OnItemClick(ClickEvent evt, int idx)
        {
            // Only perform this action at the target, not in a parent
            if (evt.propagationPhase != PropagationPhase.AtTarget)
                return;

            // Assign a random new color
            var targetBox = evt.target as VisualElement;
            VisualElement parent = targetBox.parent;
            var currColor = parent.style.backgroundColor.value;
            parent.style.backgroundColor = Color.green;// new Color(currColor.r,currColor.g,currColor.b,1f);

            //selectedItem = uiItemDataList[idx];

            //itemDisplayControl.SetDisplayItem(selectedItem);
        }
    }
}