using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProgressSetter : MonoBehaviour  
{
    public int currentCount;
    public int maxCount;
    private TextMeshPro textMeshComponent;
    private GameObject countObject;
    // Start is called before the first frame update
    void Start()
    {
        countObject = this.transform.Find("IconAndText/Count").gameObject;
        textMeshComponent = countObject.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        textMeshComponent.text = currentCount + " / " + maxCount;
    }
}
