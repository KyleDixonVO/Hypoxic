using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UnderwaterHorror 
{
    [Serializable]
    public class SerializableVector3
    {
        public SerializableVector3() { }

        public float x = 0;
        public float y = 0;
        public float z = 0;
        
        public void SetSerializableVector(Vector3 vectorToSaveOut)
        {
            x = vectorToSaveOut.x;
            y = vectorToSaveOut.y;
            z = vectorToSaveOut.z;
        }

        public void GetSerializableVector(Vector3 vectorToBeOverwritten)
        {
            vectorToBeOverwritten.x = x;
            vectorToBeOverwritten.y = y;
            vectorToBeOverwritten.z = z;
        }
    }
}


