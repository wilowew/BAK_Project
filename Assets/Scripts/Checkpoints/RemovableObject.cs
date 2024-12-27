using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovableObject : MonoBehaviour
{
    public string ID;

    public ObjectState GetState()
    {
        return new ObjectState { ID = ID, isRemoved = !gameObject.activeSelf };
    }

    public void RestoreState(ObjectState state)
    {
        gameObject.SetActive(!state.isRemoved);
    }
}

[System.Serializable]
public class ObjectState
{
    public string ID;
    public bool isRemoved;
}