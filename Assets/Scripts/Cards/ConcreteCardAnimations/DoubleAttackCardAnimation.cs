using DG.Tweening;
using UnityEngine;

public class DoubleAttackCardAnimation : CardAnimation
{
    public override void CardPlayAnimation(Sequence animationSeq)
    {
        Vector3 leftUpPos = (Vector3.up + Vector3.left) * 200;
        Vector3 rightUpPos = (Vector3.up + Vector3.right) * 200;
        float slashSpeed = 0.1f;

        animationSeq.Append(transform.DOLocalMove(rightUpPos, slashSpeed).SetEase(Ease.Linear));
        animationSeq.Append(transform.DOLocalMove(-rightUpPos, slashSpeed).SetEase(Ease.Linear));
        animationSeq.Append(transform.DOLocalMove(leftUpPos, slashSpeed).SetEase(Ease.Linear));
        animationSeq.Append(transform.DOLocalMove(-leftUpPos, slashSpeed).SetEase(Ease.Linear));
    }
}