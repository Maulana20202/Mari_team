using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{

    [HideInInspector] public Spawner spawner;
    public float Health;

    public MeshRenderer[] skinnedMeshes;

    private List<Color> startColor = new List<Color>();

    void Start(){
        foreach(MeshRenderer skin in skinnedMeshes){
            startColor.Add(skin.material.color);
        }
    }
    
    public void GotHit(float Damage){
        Health -= Damage;
        AttackFeedback();
        CheckDeathOrNot();

    }


    void CheckDeathOrNot(){
        if(Health <= 0 ){
            spawner.OnEnemyDeath();
            Destroy(this.gameObject);
        }
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
