using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NewSlimeFaceState
{ Idle, Dropped, Falling, Collide, }

public class SlimeController : MonoBehaviour
{
    [SerializeField] private Face faces; // Idleface = 😶, WalkFace = 😋, jumpFace = 😗, attackFace = 😠, damageFace = 😵
    [SerializeField] private GameObject SlimeBody;

    public NewSlimeFaceState currentSlimeFaceState;

    private Animator animator;
    private Material faceMaterial;
    private ObjectController objectControllerScript;

    private void Start()
    {
        animator = GetComponent<Animator>();
        faceMaterial = SlimeBody.GetComponent<Renderer>().materials[1];
        animator.SetFloat("Speed", 0);
    }

    private void Update()
    {
        switch (currentSlimeFaceState)
        {
            case NewSlimeFaceState.Idle:
                SetFace(faces.WalkFace);
                break;

            case NewSlimeFaceState.Dropped:
                SetFace(faces.jumpFace);
                break;

            case NewSlimeFaceState.Falling:
                SetFace(faces.Idleface);
                break;

            case NewSlimeFaceState.Collide:
                SetFace(faces.damageFace);
                break;
        }
    }

    public void SetSlimeFaceState(NewSlimeFaceState state)
    {
        currentSlimeFaceState = state;
    }

    private void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }
}