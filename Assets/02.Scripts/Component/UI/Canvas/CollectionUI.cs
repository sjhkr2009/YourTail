using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionUI : UIBase_Popup
{
    enum Windows
    {
        CockCollection
    }
    enum Contents
    {
        CocktailRecipes
    }
    
    
    void Start() => Init();
    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(Windows));
        Bind<RectTransform>(typeof(Contents));

        SetRecipes();
    }

    void SetRecipes()
    {
        List<Cocktail> list = GameManager.Data.CocktailList;
        RectTransform parent = Get<RectTransform>((int)Contents.CocktailRecipes);
        for (int i = 0; i < list.Count; i++)
        {
            GameObject gameObject = GameManager.Resource.Instantiate("UI/Others/CocktailInfoCard", parent);
            gameObject.GetOrAddComponent<CocktailInfoCard>().MyCocktail = list[i];
        }

        int height = list.Count / 4;
        parent.sizeDelta = new Vector2(0f, (520f * height) + 20f);
    }
}
