using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ToOtherScene : MonoBehaviour
{
    public Door door;

    void Start()
    {
        door = GetComponentInChildren<Door>();
    }
}
