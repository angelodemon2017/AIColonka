using UnityEngine;
using UnityEngine.Events;

public class SuriyunHelpAnimation : MonoBehaviour
{
    public UnityEvent[] triggerEvents;

    const string animationTag = "animation";
    [SerializeField] private Animator _animator;

    public void PlayAnimate(int numAnimation)
    {
        _animator.SetInteger(animationTag, numAnimation);
    }

    public void SuriyunEvent(int eventInt)
    {
        if (triggerEvents.Length > 0)
        {
            triggerEvents[eventInt].Invoke();
        }
    }
}