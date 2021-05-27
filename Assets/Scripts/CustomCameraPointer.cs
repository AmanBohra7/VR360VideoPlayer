using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CustomCameraPointer : MonoBehaviour
{
    private const float _maxDistance = 50;

    public Image pointer;
    public Image blueSection;
    private bool pointerActive = false;

    private GameObject gazedAt;

    public void Update()
    {
        
        RaycastHit hit;
        // raycast hit for hitting buttons 
        if (Physics.Raycast(transform.position, transform.forward, out hit, _maxDistance)  )
        {
            if(!pointerActive) StartCoroutine(CursorAnim("active"));
            gazedAt = hit.collider.gameObject;

        }else{
            
            if(pointerActive) StartCoroutine(CursorAnim("unactive"));
            gazedAt = null;
        }
    }


    IEnumerator CursorAnim(string state){

        yield return new WaitForSeconds(0);

        // animtion time for the cursor
        float animTime = .5f;

        // increase the size aniation for the center marker when object is on target
        if(state=="active"){
            blueSection.gameObject.SetActive(true);
            blueSection.fillAmount = 0;
            LeanTween.scale(pointer.GetComponent<RectTransform>(),new Vector3(.25f,.25f,.25f),animTime).setEaseOutQuint();
            LeanTween.value(gameObject,UpdateFilledValue,0,1,1.5f).setDelay(animTime);
            pointerActive = true;
        }
            
        //  decrease the size aniation for the center marker when object is not on target
        if(state=="unactive"){
            LeanTween.scale(pointer.GetComponent<RectTransform>(),new Vector3(.1f,.1f,.1f),animTime);
            blueSection.gameObject.SetActive(false);
            pointerActive = false;
            LeanTween.cancel(gameObject);
            blueSection.fillAmount = 0;
        }

    }


    // helper function for the leantween to incrase the fill amount | filling the circle animation 
    void UpdateFilledValue(float val, float ratio){

        if(blueSection.fillAmount == 1) return;

        blueSection.fillAmount = val;

        // perform the befow task when the fill amount of the blue circle is completed | selection animation 
        if(blueSection.fillAmount == 1){
            // Inovking the onclick event on the btn currently selected 
            gazedAt.GetComponentInParent<Button>().onClick.Invoke();
            return;
        }

    }


}
