using System.Collections;
using UnityEngine;

public class CardFusions : Singleton<CardFusions>
{
    [SerializeField]
    private FuseData[] FuseDatas;

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
        CardData newCard = fuse(a.Data, b.Data);

        if (newCard != null)
        {
            GameManager.Instance.Deck.AddToGrave(a.Data);
            GameManager.Instance.Deck.AddToGrave(b.Data);

            b.SetCardData(newCard, true);
            a.SetBlank();

            // remove 2 material
        }
    }
}

[System.Serializable]
public class FuseData
{
    public CardData[] FusionMaterial = new CardData[2];

    public CardData FusionProduct;
}