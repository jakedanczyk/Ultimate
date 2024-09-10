using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace Urth
{
    [System.Serializable]
    public struct UItemPiece
    {
        public string key;
        public Transform component;
        public Transform point;
        public Vector3 defaultPos;
        public Vector3 defaultScale;
        public Material defaultMaterial;
    }
    public class UItemAssembly : MonoBehaviour
    {
        public Transform origin;
        public GameObject rootObject;


        [SerializeField]
        public List<UItemPiece> pieces;

        public void Start()
        {
            foreach (UItemPiece piece in pieces)
            {
                piece.component.transform.localPosition = piece.defaultPos - (piece.point.transform.position - this.transform.position);
            }
        }   

    }
}

