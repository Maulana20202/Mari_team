using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealth : MonoBehaviour
{

    [SerializeField] private Slider HealthSlider;

    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.OnHealthChange += HealthChange;
    }

    private void OnDestroy() {
        UIManager.Instance.OnHealthChange -= HealthChange;
    }

    
    void HealthChange(float Health){
        HealthSlider.value = Health;

    }
}
