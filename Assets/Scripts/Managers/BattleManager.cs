using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Tween GoToDefaultPosition(GameObject gameO, bool _isMoving, Tween _currentTween, Vector3 defaultPosition, float attackTime)
    {
        if(!_isMoving && gameO.activeSelf)
        {
            _currentTween?.Kill();
            return gameO.transform.DOLocalMove(defaultPosition, attackTime);
        }

        return null;
    }

    public Coroutine FadeToColor(Color color, Coroutine _currentCoroutine, SpriteRenderer spriteRenderer, float fadeTime)
    {
        if(_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        Color oldColor = spriteRenderer.color;

        return StartCoroutine(Fade(oldColor, color, fadeTime, spriteRenderer));
    }

    private IEnumerator Fade(Color old, Color color, float time, SpriteRenderer spriteRenderer)
    {
        float elapsedTime = 0f;

        while(elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;

            float lerpAmount = Mathf.Clamp01(elapsedTime/time);
            spriteRenderer.color = Color.Lerp(old, color, lerpAmount);

            yield return null;
        }
    }
}
