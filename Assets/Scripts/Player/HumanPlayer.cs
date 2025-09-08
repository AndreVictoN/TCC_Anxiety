using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Core.Singleton;
using UnityEditor.Animations;

public class HumanPlayer : PlayerController
{
    [SerializeField] private AnimatorController _noMask;
    [SerializeField] private AnimatorController _withMask;

    private bool _isMasked;

    protected override void Awake()
    {
        base.Awake();

        animator.runtimeAnimatorController = _noMask;
    }

    void Update()
    {
        _isBattleScene = false;

        if (_canMove) _moveDirection = move.action.ReadValue<Vector2>();

        if (!_isBattleScene && _canAct)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Notify(EventsEnum.Inventory);
            }
        }

        AnimateMovement();
    }

    public void SetIsMasked(bool isMasked)
    {
        _isMasked = isMasked;

        if (_isMasked) { animator.runtimeAnimatorController = _withMask; }
        else{ animator.runtimeAnimatorController = _noMask; }
    }
}
