using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("ValueButton")]
public class ValueButton : Button
{
    public float baseValue = 0.01f;
    public float baseIncrement = 0.01f;

    [HideInInspector]
    public float value = 0.01f;
    float increment = 0.01f;

    public bool isInteger = false;
    [HideInInspector]
    public bool pressed = false;

    public float timeTrigger = 0.3f;
    public float timeTriggerDecay = 0.05f;
    [HideInInspector]
    public float msPressed = 0;

    public bool triggered = false;

    public AudioSource audioSource;

    protected override void Start() {
        value = baseValue;
        increment = baseIncrement;

    }

    private void Update() {

        if (pressed && interactable) {
            msPressed += Time.deltaTime;

            if (msPressed > timeTrigger) {

                if (timeTrigger > 0.05f) timeTrigger -= timeTriggerDecay;
                else if (timeTrigger < 0.05f) timeTrigger = 0.05f;
                else if (timeTrigger == 0.05f) {
                    if (isInteger) {
                        value = Mathf.Round(value + increment);
                    } else {
                        value += increment;
                    }
                    increment += value * 0.1f;
                }

                timeTriggerDecay *= 1.05f;
                msPressed = 0;
            }
            else if(msPressed == 0 || msPressed == Time.deltaTime) {
                audioSource.Stop();
                audioSource.Play();
                triggered = true;
            }else {
                triggered = false;
            }

        }
    }

    public override void OnPointerDown(PointerEventData eventData) {

        pressed = true;

        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData) {

        if (isInteger) {
            value = Mathf.Round(baseValue);
        } else {
            value = baseValue;
        }
        
        increment = baseIncrement;

        msPressed = 0;
        pressed = false;
        triggered = false;

        timeTriggerDecay = 0.05f;
        timeTrigger = 0.3f;

        base.OnPointerUp(eventData);
    }

#if UNITY_EDITOR
    protected override void OnValidate() {
        if (baseValue <= 0) {
            baseValue = 0.01f;
        }
        if (baseIncrement < 0) {
            baseIncrement = 0.01f;
        }

        if (timeTrigger < 0) {
            timeTrigger = 0;
        }
    }
#endif

}
