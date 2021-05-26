using UnityEngine.Video;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VideoControllerVR : MonoBehaviour{

    private VideoPlayer videoPlayer;


    void Start(){
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
    }


    public void StopVideo(){
        if(!videoPlayer.isPlaying) return;
        Debug.Log("Stop Video");
        videoPlayer.Stop();
    }

    public void StartVideo(){
        if(videoPlayer.isPlaying) return;
        Debug.Log("Start Video");
        videoPlayer.Play();
    }

    public void NextVideo(){

    }

    public void ExitScene(){
        SceneManager.LoadScene(0);
    }
    

}
