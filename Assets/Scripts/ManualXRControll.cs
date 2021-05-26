using System.Collections;

using UnityEngine;
 
using UnityEngine.XR.Management;
 
using TMPro;
using UnityEngine.SceneManagement;

public class ManualXRControll : MonoBehaviour
{
    [Tooltip("Wait time in seconds before we start and stop XR.")]
    public float waitTime = 2.0f;
 
    public TextMeshProUGUI test;

    void Start()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if(index == 1)
            StartCoroutine(StartXR());
        else
            StopXR();
    }
 
    IEnumerator StartXR()
    {
        Debug.Log($"Waiting {waitTime}s till we start XR...");
        test.text = $"Waiting {waitTime}s till we start XR...";
        yield return new WaitForSecondsRealtime(waitTime);
 
        Debug.Log("Initializing XR...");
        test.text =  $"Waiting {waitTime}s till we start XR...";
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();
 
 
        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
            test.text="FUCK";
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.activeLoader.Start();
 
            // Debug.Log($"Waiting {waitTime}s to stop XR...");
            // yield return new WaitForSecondsRealtime(waitTime);
 
            // StopXR();
        }
    }
 
    void StopXR()
    {
 
        Debug.Log("Stopping XR...");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        Debug.Log("XR stopped.");

        Debug.Log("Deinitializing XR...");
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR deinitialized.");

        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            XRGeneralSettings.Instance.Manager.activeLoader.Stop();
            XRGeneralSettings.Instance.Manager.activeLoader.Deinitialize();
            Debug.Log("XR stopped completely.");
        }
    }
}