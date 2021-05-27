using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

///<summary>
/// Loading JSON file and generateing MENU section
///</summary>
public class FileLoader : MonoBehaviour
{   

    public SceneController instance;

    public GameObject content;
    public GameObject videoOption;
    List<GameObject> videos;

    private string filePath;
    private string result;

    void Awake(){
        #if UNITY_ANDROID
            filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "VideoList.json");
            StartCoroutine(CopyJsonToPersistent());   
        #endif   
    }

    void Start(){

        instance = SceneController.Instance;

        videos = new List<GameObject>();

        ReadJSON();
    }


    /// <summary>
    /// Copy the json file placed in the StreamingAssets folder to a accessable folder for Android application
    /// </summary>
    IEnumerator CopyJsonToPersistent() 
    {
        filePath = Path.Combine(Application.streamingAssetsPath + "/", "VideoList.json");
      
        if (filePath.Contains(": //") || filePath.Contains (":///")) 
        {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            result = www.downloadHandler.text;
        } 
    
        else {
            result = File.ReadAllText(filePath);
        }
       
        File.WriteAllText(Application.persistentDataPath + "/VideoList.json", result);

    }


    /// <summary>
    /// Read the json data from particular path and call for options to be placed in the menu
    /// </summary>
    public void ReadJSON(){
        #if UNITY_ANDROID
            filePath = Application.persistentDataPath + "/VideoList.json";
        #else
            filePath = Path.Combine(Application.streamingAssetsPath + "/", "VideoList.json");
        #endif

        string jsonData = File.ReadAllText(filePath);
        GenerateOptions(jsonData);
    }


    /// <summary>
    /// Dynmaically load options in the content section with provided videos in the json file
    /// </summary>
    private void GenerateOptions(string videoList){

        SimpleJSON.JSONNode jsonData = SimpleJSON.JSON.Parse(videoList);
    
        for(int i = 0 ; i < jsonData["data"]["books"].Count ; ++i){
            // Debug.Log(jsonData["data"]["books"][i]);
            
            string video_name = jsonData["data"]["books"][i]["name"];
            string video_url = jsonData["data"]["books"][i]["thumbnail_url"];

            //  instantiate video option for ith entry in the json file 
            GameObject tmp = Instantiate(videoOption,Vector3.zero,Quaternion.identity,content.transform);

            // updating name of video in TextMeshPro test under video 
            tmp.GetComponentInChildren<TextMeshProUGUI>().text = video_name;

            // uploading thumbnail image from url of video option 
            StartCoroutine(loadThumbnail(tmp,video_url));

            // adding button onlick listner to the image
            tmp.GetComponent<Button>().onClick.AddListener(delegate { instance.OnVideoSelected(video_name); } );

            // adding video option in the list
            videos.Add(tmp);
        }


    }

    /// <summary>
    /// Load thumbnail by downloading the texture and set the texture to the obj image
    /// </summary>
    IEnumerator loadThumbnail(GameObject obj,string url){

        using(UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url)){
            yield return uwr.SendWebRequest();
            if(uwr.isNetworkError || uwr.isHttpError) Debug.Log(uwr.error);
            else{
                Debug.Log("Image is downloaded!");
                Texture tmpTexture = DownloadHandlerTexture.GetContent(uwr);
                byte[] result = uwr.downloadHandler.data;
                obj.GetComponent<RawImage>().texture = tmpTexture;
            }
        }

    }


}

// Future changes 
// In loadThumnail instead of downloding image again an again we can save it in a folder to load it again whenever needed!