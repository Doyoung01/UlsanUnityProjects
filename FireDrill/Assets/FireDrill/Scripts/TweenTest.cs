using DG.Tweening;
using System.Security.Cryptography;
using UnityEngine;

public class TweenTest : MonoBehaviour
{
    public AnimationCurve curve;
    void Start()
    {
        DOTween.Init(false, false, LogBehaviour.Default);
        transform.DOMoveX(5, 1).SetEase(Ease.InBounce);
        // origin = transform.position;
    }

    Vector3 origin;
    void Update()
    {
        // float y = curve.Evaluate(Time.time);
        // ������� ���� * y (�ش� �ð��� �� ũ��)
        // transform.position += origin + Vector3.right * y * 5;
    }
}
