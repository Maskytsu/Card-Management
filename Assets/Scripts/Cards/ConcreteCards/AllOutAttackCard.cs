using DG.Tweening;
using UnityEngine;

public class AllOutAttackCard : Card
{
    public override void CardPlayAnimation()
    {
        Vector3 leftUpPos = (Vector3.up + Vector3.left) * 200;
        Vector3 rightUpPos = (Vector3.up + Vector3.right) * 200;
        Vector3 upPos = Vector3.up * 200;
        float slashSpeed = 0.1f;

        Sequence animationSeq = DOTween.Sequence();
        animationSeq.Append(CardTransform.DOLocalMove(rightUpPos, slashSpeed).SetEase(Ease.Linear));
        animationSeq.Append(CardTransform.DOLocalMove(-rightUpPos, slashSpeed).SetEase(Ease.Linear));
        animationSeq.Append(CardTransform.DOLocalMove(leftUpPos, slashSpeed).SetEase(Ease.Linear));
        animationSeq.Append(CardTransform.DOLocalMove(-leftUpPos, slashSpeed).SetEase(Ease.Linear));
        animationSeq.Append(CardTransform.DOLocalMove(upPos, slashSpeed).SetEase(Ease.Linear));
        animationSeq.Append(CardTransform.DOLocalMove(-upPos, slashSpeed).SetEase(Ease.Linear));

        animationSeq.onComplete += () =>
        {
            OnPlayAnimationEnd?.Invoke();
        };
    }
}