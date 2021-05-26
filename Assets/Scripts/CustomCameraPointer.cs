//-----------------------------------------------------------------------
// <copyright file="CameraPointer.cs" company="Google LLC">
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sends messages to gazed GameObject.
/// </summary>
public class CustomCameraPointer : MonoBehaviour
{
    private const float _maxDistance = 50;

    public Image pointer;
    public Image blueSection;
    private bool pointerActive = false;

    private bool optionSelected = false;


    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    public void Update()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance) )
        {
            if(!pointerActive) StartCoroutine(startAnim("active"));
            if(optionSelected){
                optionSelected = false;
                print(hit.collider.gameObject.tag);
                hit.collider.gameObject.GetComponentInParent<Button>().onClick.Invoke();
            }

        }else{
            
            if(pointerActive) StartCoroutine(startAnim("unactive"));

        }
    }


    IEnumerator startAnim(string state){

        yield return new WaitForSeconds(0);

        float animTime = .5f;

        if(state=="active"){
            blueSection.gameObject.SetActive(true);
            blueSection.fillAmount = 0;
            LeanTween.scale(pointer.GetComponent<RectTransform>(),new Vector3(.4f,.4f,.4f),animTime).setEaseOutQuint();
            LeanTween.value(gameObject,UpdateFilledValue,0,1,1.5f).setDelay(animTime);
            pointerActive = true;
        }
            
        if(state=="unactive"){
            LeanTween.scale(pointer.GetComponent<RectTransform>(),new Vector3(.2f,.2f,.2f),animTime);
            blueSection.gameObject.SetActive(false);
            pointerActive = false;
            LeanTween.cancel(gameObject);
            blueSection.fillAmount = 0;
        }

    }


    void UpdateFilledValue(float val, float ratio){
        if(optionSelected) return;
        blueSection.fillAmount = val;
        if(blueSection.fillAmount == 1){
            optionSelected = true;
        }
    }


}
