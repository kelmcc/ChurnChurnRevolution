using System;
using UnityEngine;
using UnityEngine.UI;

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

    private AnimatorOverrideController _runtimeOverrideController;
    private RuntimeAnimatorController _baseController;

    private const string TransInState = "TransIn";
    private const string TransOutState = "TransOut";
    private const string IdleInState = "IdleIn";
    private const string IdleOutState = "IdleOut";

    private void Awake()
    {
        _image.enabled = true;
        SetupRuntimeAnimatorController();

        if (_transIdleOutAnim != null)
        {
            _animator.Play(IdleOutState, 0, 0f);
        }
        else
        {
            _image.enabled = false;
        }
    }

    private void SetupRuntimeAnimatorController()
    {
        _baseController = _animator.runtimeAnimatorController;

        if (_baseController == null)
        {
            Debug.LogWarning("Animator does not have a base controller assigned. Transitions will not play.");
            return;
        }

        _runtimeOverrideController = new AnimatorOverrideController(_baseController);
        _animator.runtimeAnimatorController = _runtimeOverrideController;

        if (_transInAnim != null)
        {
            _runtimeOverrideController[TransInState] = _transInAnim;
        }

        if (_transOutAnim != null)
        {
            _runtimeOverrideController[TransOutState] = _transOutAnim;
        }

        if (_transIdleInAnim != null)
        {
            _runtimeOverrideController[IdleInState] = _transIdleInAnim;
        }

        if (_transIdleOutAnim != null)
        {
            _runtimeOverrideController[IdleOutState] = _transIdleOutAnim;
        }
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