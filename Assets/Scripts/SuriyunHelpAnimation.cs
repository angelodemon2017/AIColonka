using UnityEngine;

public class SuriyunHelpAnimation : MonoBehaviour
{
    const string animationTag = "animation";
    [SerializeField] private Animator _animator;

    public void PlayAnimate(int numAnimation)
    {
        _animator.SetInteger(animationTag, numAnimation);
    }
}