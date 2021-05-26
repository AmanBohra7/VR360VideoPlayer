using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;


public class SceneController : MonoBehaviour{
   
    public static SceneController Instance;

    public static string SELECTED_VIDEO = null;

    void Awake(){
        if (Instance == null){
            Instance = this;
        }else{
            Destroy(gameObject);
            return;
	    }
        DontDestroyOnLoad(gameObject);
    }

    // public GameObject content;
    [HideInInspector]
    public List<GameObject> options;

    void Start(){
        options = new List<GameObject>();
        if(_isVrModeEnabled)
            StopXR();
    }

    public void OnVideoSelected(string selectedVideo){
        Debug.Log("Selected video name: "+selectedVideo);
        SELECTED_VIDEO = selectedVideo;
        StartXR();
        SceneManager.LoadScene(1);
    }




    /// <summary>
    /// Enters VR mode.
    /// </summary>
    private void EnterVR()
    {
        StartCoroutine(StartXR());
    }

    /// <summary>
    /// Exits VR mode.
    /// </summary>
    private void ExitVR()
    {
        StopXR();
    }


    private bool _isVrModeEnabled
    {
        get
        {
            return XRGeneralSettings.Instance.Manager.isInitializationComplete;
        }
    }

    private IEnumerator StartXR()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed.");
        }
        else
        {
            Debug.Log("XR initialized.");

            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            Debug.Log("XR started.");
        }
    }


     private void StopXR()
    {
        Debug.Log("Stopping XR...");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        Debug.Log("XR stopped.");

        Debug.Log("Deinitializing XR...");
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR deinitialized.");
    }

}
