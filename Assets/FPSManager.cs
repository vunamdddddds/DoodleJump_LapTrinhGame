using UnityEngine;

public class FPSManager : MonoBehaviour
{
    void Awake()
    {
            DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }
}