using UnityEngine.Video;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class VideoControllerVR : MonoBehaviour{

    private VideoPlayer videoPlayer;

    public List<VideoClip> m_videos;

    private string selectedVideoName;
    private static int currentIndex;

    void Start(){

        selectedVideoName = SceneController.SELECTED_VIDEO;

        if(selectedVideoName == null) selectedVideoName = "Video01";

        videoPlayer = gameObject.GetComponent<VideoPlayer>();
        for(int i = 0 ; i < m_videos.Count ; ++i){
            if(m_videos[i].name == selectedVideoName){
                videoPlayer.clip = m_videos[i];
                currentIndex = i;
                print("video played at index:  "+currentIndex);
                break;
            }
        }
        
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
        print("PLay next video!");
        print(currentIndex);
        currentIndex += 1;
        videoPlayer.Stop();
        if(currentIndex == m_videos.Count) currentIndex = 0;
        videoPlayer.clip = m_videos[currentIndex];
        videoPlayer.Play();
    }

    public void ExitScene(){
        SceneManager.LoadScene(0);
    }
    

}
