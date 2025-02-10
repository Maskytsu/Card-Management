using DG.Tweening;
using UnityEngine;

public class AllOutAttackCardAnimation : CardAnimation
{
    private const float _slashDistance = 200f;
    private const float _slashSpeed = 0.1f;

    public override void CardPlayAnimation(Sequence animationSeq, Transform transformToAnimate)
    {
        Vector3 leftUpPos = (Vector3.up + Vector3.left) * _slashDistance;
        Vector3 rightUpPos = (Vector3.up + Vector3.right) * _slashDistance;
        Vector3 upPos = Vector3.up * _slashDistance;

        animationSeq.Append(transformToAnimate.DOLocalMove(rightUpPos, _slashSpeed).SetEase(Ease.Linear));
        animationSeq.Append(transformToAnimate.DOLocalMove(-rightUpPos, _slashSpeed).SetEase(Ease.Linear));
        animationSeq.Append(transformToAnimate.DOLocalMove(leftUpPos, _slashSpeed).SetEase(Ease.Linear));
        animationSeq.Append(transformToAnimate.DOLocalMove(-leftUpPos, _slashSpeed).SetEase(Ease.Linear));
        animationSeq.Append(transformToAnimate.DOLocalMove(upPos, _slashSpeed).SetEase(Ease.Linear));
        animationSeq.Append(transformToAnimate.DOLocalMove(-upPos, _slashSpeed).SetEase(Ease.Linear));
    }
}