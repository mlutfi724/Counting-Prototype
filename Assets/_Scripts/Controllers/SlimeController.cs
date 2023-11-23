using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NewSlimeFaceState
{ SavoringFace, CuteFace, NoMouthFace, CrossedEyesFace }

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
            case NewSlimeFaceState.SavoringFace:
                SetFace(faces.WalkFace);
                break;

            case NewSlimeFaceState.CuteFace:
                SetFace(faces.jumpFace);
                break;

            case NewSlimeFaceState.NoMouthFace:
                SetFace(faces.Idleface);
                break;

            case NewSlimeFaceState.CrossedEyesFace:
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