using DG.Tweening;
using UnityEngine;

public class AttackCard : Card
{
    public override void CardPlayAnimation()
    {
        Vector3 leftUpPos = (Vector3.up + Vector3.left) * 200;
        float slashSpeed = 0.1f;

        Sequence animationSeq = DOTween.Sequence();
        animationSeq.Append(transform.DOLocalMove(leftUpPos, slashSpeed).SetEase(Ease.Linear));
        animationSeq.Append(transform.DOLocalMove(-leftUpPos, slashSpeed).SetEase(Ease.Linear));

        animationSeq.onComplete += () =>
        {
            OnPlayAnimationEnd?.Invoke();
        };
    }
}
