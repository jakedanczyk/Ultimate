using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Urth
{
    public class UrthResponse
    {
        public bool success = true;
        public string msg = "";
        public UrthResponse(bool isuccess, string imsg)
        {
            success = isuccess;
            msg = imsg;
        }
        public UrthResponse(bool isuccess)
        {
            success = isuccess;
            msg = "";
        }
    }
}