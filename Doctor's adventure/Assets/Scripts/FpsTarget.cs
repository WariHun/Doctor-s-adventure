using UnityEngine;

public class FpsTarget : MonoBehaviour
{

    //Indulásnál az fps limit megadása
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
    }

    //Ha eltérő a frissítési gyakoriság, akkor frissítse azt
    void Update()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;

        if (Application.targetFrameRate != Screen.currentResolution.refreshRate)
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }
}
