using UnityEngine;
using UnityEditor;

public class ReplaceWithPrefabEditor : EditorWindow 
{
    public GameObject _prefabToReplace; 
    private string _objectTag = "brick"; 

    [MenuItem("Tools/Replace With Prefab")]

    public static void ShowWindow() 
    {
        GetWindow<ReplaceWithPrefabEditor>("Replace With Prefab");
    }

    private void OnGUI() 
    {
        _prefabToReplace = (GameObject)EditorGUILayout.ObjectField("Prefab to Replace", _prefabToReplace, typeof(GameObject), false);
        _objectTag = EditorGUILayout.TextField("Tag to Replace", _objectTag);

        if (GUILayout.Button("Replace")) 
        {
            ReplaceObjects();
        }
    }

    private void ReplaceObjects() 
    {

        GameObject[] _objectsToReplace = GameObject.FindGameObjectsWithTag(_objectTag);

        foreach (GameObject obj in _objectsToReplace) 
        {
            Vector3 position = obj.transform.position;
            Quaternion rotation = obj.transform.rotation;

            GameObject _newPrefabInstance = PrefabUtility.InstantiatePrefab(_prefabToReplace) as GameObject;
            _newPrefabInstance.transform.position = position;
            _newPrefabInstance.transform.rotation = rotation;

            DestroyImmediate(obj);
        }

    }
}
