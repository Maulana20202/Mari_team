using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public float Health;

    public SkinnedMeshRenderer[] skinnedMeshes;

    private List<Color> startColor = new List<Color>();

    private void Start() {
        UIManager.Instance.UpdateHealth(Health);
        foreach(SkinnedMeshRenderer skin in skinnedMeshes){
            startColor.Add(skin.material.color);
        }
    }

    public void OnHealthDecrease(float Amount){
        Health -= Amount;
        AttackFeedback();
        UIManager.Instance.UpdateHealth(Health);
    }



    void AttackFeedback(){
        for(int i = 0;i < skinnedMeshes.Length; i++){

            Material targetMaterial;
            int index;
            targetMaterial = skinnedMeshes[i].material;
            index = i;

            StartCoroutine(AttackFeedbackTime(targetMaterial, index));
        }
    }


    IEnumerator AttackFeedbackTime(Material targetMaterial, int index){
        targetMaterial.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        targetMaterial.color = startColor[index];
    }
}
