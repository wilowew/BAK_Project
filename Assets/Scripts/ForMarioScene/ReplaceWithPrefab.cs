using UnityEngine;
using UnityEditor;

public class ReplaceWithPrefabEditor : EditorWindow {
    public GameObject prefabToReplace; // Префаб, на который вы хотите заменить объекты
    private string objectTag = "brick"; // Замените на ваш тег

    [MenuItem("Tools/Replace With Prefab")]
    public static void ShowWindow() {
        GetWindow<ReplaceWithPrefabEditor>("Replace With Prefab");
    }

    private void OnGUI() {
        prefabToReplace = (GameObject)EditorGUILayout.ObjectField("Prefab to Replace", prefabToReplace, typeof(GameObject), false);
        objectTag = EditorGUILayout.TextField("Tag to Replace", objectTag);

        if (GUILayout.Button("Replace")) {
            ReplaceObjects();
        }
    }

    private void ReplaceObjects() {
        GameObject[] objectsToReplace = GameObject.FindGameObjectsWithTag(objectTag);

        foreach (GameObject obj in objectsToReplace) {
            Vector3 position = obj.transform.position;
            Quaternion rotation = obj.transform.rotation;

            GameObject newPrefabInstance = PrefabUtility.InstantiatePrefab(prefabToReplace) as GameObject;
            newPrefabInstance.transform.position = position;
            newPrefabInstance.transform.rotation = rotation;

            // Удаляем оригинальный объект
            DestroyImmediate(obj);
        }

        Debug.Log("Replacement complete!");
    }
}
