using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Helpers
{
    class GameObjectPlus
    {
        public GameObject Obj { get; set; }
        public Vector3 CurrentRotation { get; set; }
        public Vector3 RequestedRotation { get; set; }
        public Vector3 MinRotation { get; set; }
        public Vector3 MaxRotation {get; set;}        

        public GameObjectPlus(GameObject useMe) : this(useMe, useMe.transform.localEulerAngles) { }

        public GameObjectPlus(GameObject useMe, Vector3 curRot) : this(useMe, curRot, Vector3.zero) { }

        public GameObjectPlus(GameObject useMe, Vector3 curRot, Vector3 reqRot) : this(useMe, curRot, reqRot, Vector3.one * (-180f)) { }

        public GameObjectPlus(GameObject useMe, Vector3 curRot, Vector3 reqRot, Vector3 minRot) : this(useMe, curRot, reqRot, minRot, Vector3.one * 180f) { }
        
        public GameObjectPlus(GameObject useMe, Vector3 curRot, Vector3 reqRot, Vector3 minRot, Vector3 maxRot)
        {
            Obj = useMe;
            CurrentRotation = curRot;
            RequestedRotation = reqRot;
            MinRotation = minRot;
            MaxRotation = maxRot;            
        }
    }
}
