using System;
using System.Collections;
using Core.Singleton;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : Singleton<BattleManager>
{
    public Enemy enemy;
    public PlayerController player;

    [SerializeField] private string _pastScene;
    private TextMeshProUGUI _enemyName;
    private bool _prototypeSetupMade;
    private bool _enemyIsAttacking;

    void Start()
    {
        _enemyName = GameObject.FindGameObjectWithTag("EnemyName").GetComponent<TextMeshProUGUI>();
        _prototypeSetupMade = false;
        _enemyIsAttacking = false;

        _pastScene = PlayerPrefs.GetString("pastScene");

        if(enemy == null) enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
        if(player == null) player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.SetMyTurn(true);

        if(_pastScene == "PrototypeScene"){PrototypeBattleSetup();}
    }

    void Update()
    {
        if(_pastScene == "PrototypeScene"){PrototypeBattle();}

        BattleUpdate();
    }

    private void BattleUpdate()
    {
        if(!player.GetMyTurn() && !_enemyIsAttacking)
        {
            _enemyIsAttacking = true;
            enemy.Attack(player);
        }
    }

    public void SetEnemyIsAttacking(bool isAttacking){_enemyIsAttacking = isAttacking;}

#region Prototype
    private void PrototypeBattleSetup()
    {
        _enemyName.text = "Vergosulo";

        _prototypeSetupMade = true;
    }

    private void PrototypeBattle()
    {
        if(!_prototypeSetupMade) PrototypeBattleSetup();
        
    }
#endregion

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

    public void DamageEnemy(float damage){if(enemy != null) enemy.TakeDamage(damage);}

    public void SetPastScene(string pastScene) {_pastScene = pastScene;}
}
