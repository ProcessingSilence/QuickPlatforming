using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPlayer: MonoBehaviour
{
    public PlayerController my_PlayerController_script;

    public Text midairJumps;
    public Slider midairSlider;
    public Text jumpsLeft;
    
    public Text jumpHeightLimit;
    public Slider jumpHeightLimitSlider;
    // Start is called before the first frame update
    void Start()
    {
        // Midair jumps
        SetSliderAndText(midairSlider,0,4,true, my_PlayerController_script.multiJumpLimit, 
            midairJumps, "Midair jumps: " + midairSlider.value);
        midairSlider.onValueChanged.AddListener(delegate{ValueChangeCheck();});
        
        // Jump height limit
        SetSliderAndText(jumpHeightLimitSlider,1,5,false, my_PlayerController_script.maxJumpHeight, 
            jumpHeightLimit, "Jump height limit: " + jumpHeightLimitSlider.value);
        jumpHeightLimitSlider.onValueChanged.AddListener(delegate{ValueChangeCheck();});
        ValueChangeCheck();
    }

    void Update()
    {
        jumpsLeft.text = "Midair jumps left: " + my_PlayerController_script.currentJumpsLeft;
    }

    public void ValueChangeCheck()
    {
        my_PlayerController_script.multiJumpLimit = (int) midairSlider.value;
        midairJumps.text = "Midair jumps: " + midairSlider.value;
        
        jumpHeightLimitSlider.value = my_PlayerController_script.maxJumpHeight = Mathf.Round(jumpHeightLimitSlider.value * 10f) * 0.1f;
        jumpHeightLimit.text = "Jump height limit: " + jumpHeightLimitSlider.value;
    }
    
    void SetSliderAndText(Slider currentSlider, float min, float max, bool wholeNumber, float currentVal, Text myText, string textValue)
    {
        currentSlider.minValue = min;
        currentSlider.maxValue = max;
        currentSlider.wholeNumbers = wholeNumber;
        currentSlider.value = currentVal;
        myText.text = textValue;
    }
}
