using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
    }

    public void OnVideoSelected(string selectedVideo){
        Debug.Log("Selected video name: "+selectedVideo);
        SELECTED_VIDEO = selectedVideo;
        SceneManager.LoadScene(1);
    }

}
