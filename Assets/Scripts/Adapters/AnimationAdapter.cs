using System;
using UnityEngine;
using UnityEngine.Events;

public class AnimationAdapter : MonoBehaviour
{
    private EnumAnimations _currentAnimation;

    [System.Serializable]
    public class AnimationEvent
    {
        public EnumAnimations animationName;
        public UnityEvent response;
    }

    public AnimationEvent[] animationEvents;

    public Action EndAnimation;

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

    //===========================================================

    public void EndAnimate()
    {
        EndAnimation?.Invoke();
    }
}