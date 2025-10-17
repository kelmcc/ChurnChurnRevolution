using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Animations;

public class Transitions : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Animator _animator;

    [Header("Transition Animations")] 
    [SerializeField] private AnimationClip _transInAnim;
    [SerializeField] private AnimationClip _transOutAnim;

    [Header("Idle Animations (Loop)")] 
    [SerializeField] private AnimationClip _transIdleInAnim;
    [SerializeField] private AnimationClip _transIdleOutAnim;

    public Action OnTransInComplete;
    public Action OnTransOutComplete;

    private AnimatorController _runtimeController;

    private const string TransInState = "TransIn";
    private const string TransOutState = "TransOut";
    private const string IdleInState = "IdleIn";
    private const string IdleOutState = "IdleOut";

    private void Awake()
    {
        _image.enabled = false;
        SetupRuntimeAnimatorController();
    }

    private void SetupRuntimeAnimatorController()
    {
        _runtimeController = AnimatorController.CreateAnimatorControllerAtPath("Assets/Temp_TransitionController.controller");
        AnimatorStateMachine stateMachine = _runtimeController.layers[0].stateMachine;

        if (_transInAnim != null)
        {
            AnimatorState state = stateMachine.AddState(TransInState);
            state.motion = _transInAnim;
        }

        if (_transOutAnim != null)
        {
            AnimatorState state = stateMachine.AddState(TransOutState);
            state.motion = _transOutAnim;
        }

        if (_transIdleInAnim != null)
        {
            AnimatorState state = stateMachine.AddState(IdleInState);
            state.motion = _transIdleInAnim;
            state.speed = 1f;
            state.writeDefaultValues = true;
        }

        if (_transIdleOutAnim != null)
        {
            AnimatorState state = stateMachine.AddState(IdleOutState);
            state.motion = _transIdleOutAnim;
            state.speed = 1f;
            state.writeDefaultValues = true;
        }

        _animator.runtimeAnimatorController = _runtimeController;
    }

    public void TransitionIn()
    {
        if (_transInAnim == null)
        {
            return;
        }

        _image.enabled = true;
        _animator.Play(TransInState, 0, 0);
    }

    public void TransitionOut()
    {
        if (_transOutAnim == null)
        {
            return;
        }

        _image.enabled = true;
        _animator.Play(TransOutState, 0, 0);
    }

    public void TriggerTransInComplete()
    {
        OnTransInComplete?.Invoke();

        if (_transIdleInAnim != null)
        {
            _animator.Play(IdleInState, 0, 0);
        }
        else
        {
            _image.enabled = false;
            _animator.Rebind();
        }
    }

    public void TriggerTransOutComplete()
    {
        OnTransOutComplete?.Invoke();

        if (_transIdleOutAnim != null)
        {
            _animator.Play(IdleOutState, 0, 0);
        }
        else
        {
            _image.enabled = false;
            _animator.Rebind();
        }
    }
}