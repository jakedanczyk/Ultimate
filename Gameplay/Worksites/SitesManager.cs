using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public enum URTHSITE
    {
        PHYSICS,
        STATIC,
        WORKSITE,
    }
    public class UrthSite
    {
        public URTHSITE type;
        public string id;
        public UrthSite(string iid, URTHSITE itype)
        {
            id = iid; type = itype;
        }
    }

    public class UrthPhysicsSite
    {
        public double x;
        public double y;
        public double z;
        public double vx;
        public double vy;
        public double vz;
        public float mass;
    }
    public class UrthStaticSite
    {
        public double x;
        public double y;
        public double z;
        public float strength;
    }

    /*SitesManager
     * Handles placement, loading, unloading, saving
     * for "sites", which are: worksites, physics-items, static-items
     * -worksites can be attached to: terrain, plant, items (static or physics)
     * 
     * for now, all sites will be organized in a heap. 
     * during Update, we will cycle through the list of items
     * and spawn/despawn based on range
     * 
     * once we start having larger numbers of items
     * we will need to organize in a tile system
     * 
     * 
     */
    public class SitesManager : MonoBehaviour
    {
        public Dictionary<string, UrthSite> sites;

        void Awake()
        {
            LoadSites();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void LoadSites()
        {
            string[] sitesIO = System.IO.File.ReadAllLines(GameManager.Instance.savepath + "\\" + "Sites.json");
            sites = new Dictionary<string, UrthSite>(sitesIO.Length);
            foreach(string data in sitesIO)
            {
                sites[data] = new UrthSite(data, URTHSITE.WORKSITE);
            }
        }

    }

}