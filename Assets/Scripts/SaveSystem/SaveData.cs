using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public Vector3Data playerPosition;
    public Vector3Data playerRotation; // 👈 Adicionado
    public List<SaveableObjectData> objectsData;
}

[Serializable]
public class Vector3Data
{
    public float x, y, z;

    public Vector3Data(Vector3 position)
    {
        x = position.x;
        y = position.y;
        z = position.z;
    }

    public Vector3 ToVector3() => new Vector3(x, y, z);
}

[Serializable]
public class SaveableObjectData
{
    public string uniqueId;
    public Vector3Data position;
    public Vector3Data rotation;
    public bool isActive;
}
