using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHandler : MonoBehaviour
{
    [System.Serializable]
    public class AnimationEvent
    {
        public string eventName;
        public UnityEvent response;
    }

    public AnimationEvent[] animationEvents;

    public void OnAnimationEvent(string eventName)
    {
        foreach (var animationEvent in animationEvents)
        {
            if (animationEvent.eventName == eventName)
            {
                animationEvent.response.Invoke();
                break;
            }
        }
    }
}