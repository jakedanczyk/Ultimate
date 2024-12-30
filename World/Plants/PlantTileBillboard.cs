using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Urth
{
    //PlantTileBillboards are owned by a PlantTile (full, meso, or mico size)
    //There are several billboards to cover different groups of tree types
    //In PlantTiles.MakeBillboards, data is written into the fields of its billboards for each plant of a certain size.
    //Which billboard depends on the plants species
    //At the end, Render() is called on each billboard
    //This design seems poor to me as I'm writing it, but its my best for now without excessive memory 
    //the alternatives I considered involved passing plant data down to each sub-tile and billboard
    public class PlantTileBillboard : MonoBehaviour
    {
        public PLANT type;
        public Shader shader;
        public MeshRenderer coniferBillboard;
        public MeshRenderer palmBillboard;
        public MeshRenderer broadleafBillboard;
        public List<MeshFilter> meshFilters = new List<MeshFilter>();
        public Mesh mesh;
        public GameObject selectedTreeBillboard;
        public GameObject selectedTreePrefab;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public int triangleIndex = 0;
        public List<Vector3> vertices = new List<Vector3>();
        public List<int> triangles = new List<int>();
        public List<Vector2> uvs = new List<Vector2>();
        public List<Vector2> localPos = new List<Vector2>();
        public List<Vector2> frameIndices = new List<Vector2>();
        public List<Color> colors = new List<Color>();
        
        public void Render()
        {
            mesh.vertices = vertices.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv2 = localPos.ToArray();
            mesh.uv3 = frameIndices.ToArray();
            mesh.colors = colors.ToArray();

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            triangleIndex = 0;
            vertices.Clear();
            triangles.Clear();
            uvs.Clear();
            localPos.Clear();
            frameIndices.Clear();
            colors.Clear();
        }

        public void Clear()
        {
            mesh.Clear();
        }
    }
}
