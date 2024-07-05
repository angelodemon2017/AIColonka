using UnityEngine;

public class PersonController : MonoBehaviour
{
    const string WalkAnimation = "Walk_wing";
    const string IdleAnimation = "IdleG_wing";
    private float MoveSpeed = 0.01f;
    private Vector3 lookTarget;
    [SerializeField] private Animator animator;
    private string CurrentAnimation;

    void Start()
    {
        InputController.Instance.moveAction += MoveTo;
    }

    void Update()
    {
        if (transform.position.magnitude > 0f)
        {
            PlayAnimation(IdleAnimation);
        }
        else
        {
            PlayAnimation(WalkAnimation);
        }
    }

    public void MoveTo(Vector3 vector)
    {
        transform.position += vector * MoveSpeed;
//        transform.rotation = 
        transform.LookAt(transform.position + vector);
    }

    private void PlayAnimation(string animation)
    {
        if (CurrentAnimation != animation)
        {
            animator.Play(animation);
            CurrentAnimation = animation;
        }
    }
}