using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class ScaleController : MonoBehaviour
{
    [Header("UI")]
    public Slider scaleSlider;
    ARSessionOrigin m_ARSessionOrigin;

    private void Awake()
    {
        m_ARSessionOrigin = GetComponent<ARSessionOrigin>();
    }
    // Start is called before the first frame update
    void Start()
    {
        scaleSlider.onValueChanged.AddListener(OnSliderValueChanged);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSliderValueChanged(float value)
    {
        if (scaleSlider != null)
        {
            m_ARSessionOrigin.transform.localScale = Vector3.one / value;
        }
    }
}
