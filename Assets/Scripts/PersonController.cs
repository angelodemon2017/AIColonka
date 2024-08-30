using UnityEngine;

public class PersonController : MonoBehaviour
{
    private float MoveSpeed = 0.01f;

    void Start()
    {
        InputController.Instance.moveAction += MoveTo;
    }

    public void MoveTo(Vector3 vector)
    {
        transform.position += vector * MoveSpeed;
        transform.LookAt(transform.position + vector);
    }
}