using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovableObjectsManager : MonoBehaviour
{
    public void SaveRemovableObjects(SceneData sceneData)
    {
        var objects = FindObjectsOfType<RemovableObject>();

        sceneData.removedObjects.Clear();
        foreach (var obj in objects)
        {
            if (!obj.gameObject.activeSelf)
            {
                sceneData.removedObjects.Add(obj.ID);
            }
        }
    }

    public void LoadRemovableObjects(SceneData sceneData)
    {
        var objects = FindObjectsOfType<RemovableObject>();

        foreach (var obj in objects)
        {
            bool isRemoved = sceneData.removedObjects.Contains(obj.ID);
            obj.RestoreState(new ObjectState { ID = obj.ID, isRemoved = isRemoved });
        }
    }

    private T FindObjectByID<T>(string id) where T : RemovableObject
    {
        var objects = FindObjectsOfType<T>();
        foreach (var obj in objects)
        {
            if (obj.ID == id)
            {
                return obj;
            }
        }
        return null;
    }
}

[System.Serializable]
public class RemovableObjectData
{
    public List<ObjectState> removableObjects;
}
