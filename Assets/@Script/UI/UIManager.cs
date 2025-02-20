using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get; private set;}

    public Action<float> OnHealthChange;

    private void Awake() {

        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        } else {
            Instance = this;
        }
    }


    public void UpdateHealth(float newHealth){
        OnHealthChange?.Invoke(newHealth);
    }
}
