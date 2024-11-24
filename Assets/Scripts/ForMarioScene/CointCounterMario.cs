using UnityEngine.UI;
using UnityEngine;

public class CointCounterMario : MonoBehaviour
{
    public GameObject[] targetObjects; 
    public Text guiText;            

    private int[] missingCounts;

    void Update()
    {
        int missingCount = 0;
        for (int i = 0; i < targetObjects.Length; i++)
        {
            if (targetObjects[i] == null)
            {
                missingCount++;
            }
        }
        guiText.text = "x " + missingCount;
    }
}
