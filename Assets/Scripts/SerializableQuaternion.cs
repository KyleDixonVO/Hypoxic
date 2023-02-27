using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace UnderwaterHorror 
{
    [Serializable]
    public class SerializableQuaternion 
    { 
        public SerializableQuaternion() { }
        public float w;
        public float x;
        public float y;
        public float z;

        public void SetSerializableQuaternion(Quaternion quaternionToSaveOut)
        {
            w = quaternionToSaveOut.w;
            x = quaternionToSaveOut.x;
            y = quaternionToSaveOut.y;
            z = quaternionToSaveOut.z;
        }

        public void GetSerializableQuaternion(Quaternion quaternionToBeOverwritten)
        {
            quaternionToBeOverwritten.w = w;
            quaternionToBeOverwritten.x = x;
            quaternionToBeOverwritten.y = y;
            quaternionToBeOverwritten.z = z;
        }
    }
}


