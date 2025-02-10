using UnityEngine;

public class UIFSM : MonoBehaviour
{


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            EventBus.Publish(new EventKey(KeyCode.L));
        }

    }
}