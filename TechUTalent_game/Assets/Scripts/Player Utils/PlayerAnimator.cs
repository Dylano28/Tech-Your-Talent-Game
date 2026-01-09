using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AdvancedPlatformerController controller;
    [SerializeField] private Transform spriteTransform;

    private const string GROUND_TRIGGER = "grounded";
    private const string JUMP_TRIGGER = "jump";
    private const string STOP_TRIGGER = "stop";
    private const string WALK_TRIGGER = "walk";

    private Vector3 _lastVelocity;
    private float _xScale;

    private void Start()
    {
        _xScale = spriteTransform.localScale.x;

        controller.onJump.AddListener(() => 
        {
            _lastVelocity = Vector3.zero;

            animator.ResetTrigger(GROUND_TRIGGER);
            animator.ResetTrigger(WALK_TRIGGER);
            animator.SetTrigger(JUMP_TRIGGER); 
        });

        controller.onGrounded.AddListener(() =>
        {
            animator.ResetTrigger(JUMP_TRIGGER);
            animator.SetTrigger(GROUND_TRIGGER);
        });

        controller.onStartMove.AddListener(() =>
        {
            animator.ResetTrigger(STOP_TRIGGER);
        });

        controller.onMoving.AddListener(() =>
        {
            SetDirection(controller.Velocity);
            animator.SetTrigger(WALK_TRIGGER);
        });

        controller.onStopMove.AddListener(() =>
        {
            animator.ResetTrigger(WALK_TRIGGER);
            animator.SetTrigger(STOP_TRIGGER);
        });
    }

    private void Update()
    {
        if (controller.lockMovement)
        {
            animator.ResetTrigger(WALK_TRIGGER);
            animator.SetTrigger(STOP_TRIGGER);

            return;
        }

        if (controller.Velocity == _lastVelocity || controller.Velocity == Vector3.zero) return;
        SetDirection(controller.Velocity);
    }

    private void SetDirection(Vector3 newVelocity)
    {
        _lastVelocity = controller.Velocity;

        var spriteXScale = Mathf.RoundToInt(_lastVelocity.normalized.x) * _xScale;
        if (spriteXScale == 0) return;

        spriteTransform.localScale = new Vector3(spriteXScale, spriteTransform.localScale.y, spriteTransform.localScale.z);
    }
}
