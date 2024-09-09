using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public class InventoryPanelItem : VisualElement
    {
        public TextElement label;
        public void SetItem(UItemData item)
        {
            label.text = item.GetName();
        }
    }
}