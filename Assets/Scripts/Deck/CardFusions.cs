using System.Collections;
using UnityEngine;

public class CardFusions : Singleton<CardFusions>
{
    [SerializeField]
    private FuseData[] FuseDatas;

    private Coroutine fusingRoutine;

    private CardData fuse(CardData a, CardData b)
    {
        foreach (FuseData data in FuseDatas)
        {
            if ((a == data.FusionMaterial[0] && b == data.FusionMaterial[1]) ||
                (a == data.FusionMaterial[1] && b == data.FusionMaterial[0]))
            {
                return data.FusionProduct;
            }
        }

        return null;
    }

    public void Fusion(CardUI a, CardUI b)
    {
        fusingRoutine = StartCoroutine(fusingYield(a, b));
    }

    private IEnumerator fusingYield(CardUI a, CardUI b)
    {
        CardData newCard = fuse(a.Data, b.Data);

        if (newCard != null)
        {
            GameManager.Instance.Deck.AddToGrave(a.Data);
            GameManager.Instance.Deck.AddToGrave(b.Data);

            b.SetBlank();

            // FX
            LeanTween.scale(a.gameObject, Vector3.one * 1.5f, 0.2f).setLoopPingPong(1).setEaseOutQuint();
            yield return new WaitForSeconds(0.4f);
            AudioManager.Instance.PlaySfx(2);

            GameObject vfx = ObjectPool.Instance.Spawn("VFX_DisintegrateUI", a.transform);
            LeanTween.delayedCall(vfx, 0.5f, () =>
            {
                ObjectPool.Instance.BackToPool(vfx);
            });

            a.SetCardData(newCard, true);
        }
    }
}

[System.Serializable]
public class FuseData
{
    public CardData[] FusionMaterial = new CardData[2];

    public CardData FusionProduct;
}