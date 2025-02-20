using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = this.GetComponent<PlayerMovement>();

        _animator = this.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("Speed", _playerMovement.Speed);
    }
}
