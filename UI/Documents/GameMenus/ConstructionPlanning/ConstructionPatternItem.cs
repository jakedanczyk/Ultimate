using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Urth
{
    public class ConstructionPatternItem : VisualElement
    {
        public TextElement label;
        public void SetItem(ConstructionPreview item)
        {
            label.text = item.staticTypeName;
        }
    }
}