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
        // dataText.text = jsonData;

        // SimpleJSON.JSONNode test = SimpleJSON.JSON.Parse(jsonData);
        // Debug.Log(test["data"]["books"][0]);

        // latestText.text =  test["data"]["books"][0].ToString();
    }

    private void GenerateOptions(string videoList){

        SimpleJSON.JSONNode jsonData = SimpleJSON.JSON.Parse(videoList);
    
        for(int i = 0 ; i < jsonData["data"]["books"].Count ; ++i){
            Debug.Log(jsonData["data"]["books"][i]);
            GameObject tmp = Instantiate(videoOption,Vector3.zero,Quaternion.identity,content.transform);
            tmp.GetComponentInChildren<TextMeshProUGUI>().text = jsonData["data"]["books"][i]["name"];
            videos.Add(tmp);
        }

        GameObject test = videos[0];
        StartCoroutine(
            loadImage(test,jsonData["data"]["books"][0]["thumbnail_url"])
        );

    }


    IEnumerator loadImage(GameObject image,string url){

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if(request.isNetworkError || request.isHttpError) 
            Debug.Log(request.error);
        else
            image.GetComponent<RawImage>().texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
    }
}