using System;
using UnityEngine;
using UnityEngine.Events;

public class AnimationAdapter : MonoBehaviour
{
    public Action<EnumAnimations> endAnimation;
    private EnumAnimations _currentAnimation;

    [System.Serializable]
    public class AnimationEvent
    {
        public EnumAnimations animationName;
        public UnityEvent response;
    }

    public AnimationEvent[] animationEvents;

    public UnityEvent[] triggerEvents;

    public void PlayAnimationEvent(EnumAnimations animationName)
    {
        if (_currentAnimation != animationName)
        {
            foreach (var animationEvent in animationEvents)
            {
                if (animationEvent.animationName == animationName)
                {
                    animationEvent.response.Invoke();
                    _currentAnimation = animationName;
                }
            }
        }
    }

    public void EndAnimation(EnumAnimations animationName)
    {
        endAnimation?.Invoke(animationName);
    }

    public void TriggerEvent(int number)
    {
        triggerEvents[number].Invoke();
        Debug.Log($"TriggerEvent:{number}");
    }
}