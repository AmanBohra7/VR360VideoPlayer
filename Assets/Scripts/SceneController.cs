using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour{
   
    // Singleton instance of SceneController
    public static SceneController Instance;

    // static value to be used over both scene to get which is the selected video option
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
    
    [HideInInspector]
    public List<GameObject> options;

    void Start(){
        options = new List<GameObject>();
    }

    /// <summary>
    /// Function triggered by the video options in menu to load next scene with the selected video
    /// </summary>
    public void OnVideoSelected(string selectedVideo){
        Debug.Log("Selected video name: "+selectedVideo);
        SELECTED_VIDEO = selectedVideo;
        SceneManager.LoadScene(1);
    }

}
