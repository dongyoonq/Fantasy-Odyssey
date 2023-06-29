using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] Slider slider;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        animator.SetBool("Active", true);
    }

    public void FadeOut()
    {
        animator.SetBool("Active", false);
    }

    public void SetProgress(float progress)
    {
        slider.value = progress;
    }
}
