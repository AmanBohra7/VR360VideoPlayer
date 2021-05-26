using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FileLoader : MonoBehaviour
{   
    // public TextMeshProUGUI dataText;
    // public TextMeshProUGUI filePathText;
    // public TextMeshProUGUI errorText;
    // public TextMeshProUGUI latestText;

    public SceneController instance;

    public GameObject content;
    public GameObject videoOption;
    List<GameObject> videos;

    private string filePath;
    private string result;

    void Awake(){
        #if UNITY_ANDROID
            Debug.Log("TEST");
            filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "VideoList.json");
            StartCoroutine(Example());   
        #endif   
    }

    void Start(){

        instance = SceneController.Instance;

        videos = new List<GameObject>();
        // content.SetActive(false);
        ReadTheData();
    }

    IEnumerator Example() 
    {
        filePath = Path.Combine(Application.streamingAssetsPath + "/", "VideoList.json");
        // filePathText.text = filePath;
      
        if (filePath.Contains(": //") || filePath.Contains (":///")) 
        {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            result = www.downloadHandler.text;
        } 
    
        else {
            result = File.ReadAllText(filePath);
            // errorText.text = "In else section!";
        }
       
        File.WriteAllText(Application.persistentDataPath + "/VideoList.json", result);
        // dataText.text = result;
    }


    public void ReadTheData(){
        #if UNITY_ANDROID
            filePath = Application.persistentDataPath + "/VideoList.json";
        #else
            filePath = Path.Combine(Application.streamingAssetsPath + "/", "VideoList.json");
        #endif

        string jsonData = File.ReadAllText(filePath);
        GenerateOptions(jsonData);
    }

    private void GenerateOptions(string videoList){

        SimpleJSON.JSONNode jsonData = SimpleJSON.JSON.Parse(videoList);
    
        for(int i = 0 ; i < jsonData["data"]["books"].Count ; ++i){
            // Debug.Log(jsonData["data"]["books"][i]);
            
            string video_name = jsonData["data"]["books"][i]["name"];
            string video_url = jsonData["data"]["books"][i]["thumbnail_url"];

            //  instantiate video option
            GameObject tmp = Instantiate(videoOption,Vector3.zero,Quaternion.identity,content.transform);

            // setting name of video 
            tmp.GetComponentInChildren<TextMeshProUGUI>().text = video_name;

            // uploading thumbnail image from url of video option 
            StartCoroutine(loadThumbnail(tmp,video_url));


            // adding button onlick listner to the image
            tmp.GetComponent<Button>().onClick.AddListener(delegate { instance.OnVideoSelected(video_name); } );

            videos.Add(tmp);
        }


    }

    IEnumerator loadThumbnail(GameObject tmp,string url){

        using(UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url)){
            yield return uwr.SendWebRequest();
            if(uwr.isNetworkError || uwr.isHttpError) Debug.Log(uwr.error);
            else{
                Debug.Log("Image is downloaded!");
                Texture tmpTexture = DownloadHandlerTexture.GetContent(uwr);
                byte[] result = uwr.downloadHandler.data;
                tmp.GetComponent<RawImage>().texture = tmpTexture;
            }
        }

    }


}




// IEnumerator loadImage(GameObject image,string url){
//     UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
//     print("before request!");
//     yield return request.SendWebRequest();
//     print("after request!");
//     if(request.isNetworkError || request.isHttpError) 
//         Debug.Log(request.error);
//     else
//         image.GetComponent<RawImage>().texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
// }