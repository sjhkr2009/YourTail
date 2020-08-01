using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : UIBase_Scene
{
    enum Buttons
    {
        OpenBirdList,
        OpenCockList,
        OpenMusicList,
        MenuOpenButton
    }
    enum Transforms
    {
        OpenMusicList,
        OpenCockList,
        OpenBirdList,
        Count
    }

    bool isOpened = false;
    Vector3 pivot;
    
    void Start() => Init();
    public override void Init()
    {
        GameManager.UI.SetCanvasOrder(gameObject, false, int.MaxValue);

        Bind<Button>(typeof(Buttons));
        Bind<RectTransform>(typeof(Transforms));

        pivot = GetButton((int)Buttons.MenuOpenButton).transform.position;
        GetButton((int)Buttons.MenuOpenButton).onClick.AddListener(() => { MenuOnOff(isOpened); });

        for (int i = 0; i < (int)Transforms.Count; i++)
        {
            Get<RectTransform>(i).gameObject.SetActive(false);
        }

        //UI컬렉션 추가 시 컬렉션 UI를 여는 기능을 각 버튼에 할당할 것
        GetButton((int)Buttons.OpenCockList).onClick.AddListener(() =>
        {
            GameManager.UI.OpenPopupUI<CollectionUI>();
            MenuOnOff(true);
        });
    }
    private void OnDestroy()
    {
        ResetButtons();
    }

    void EnableIcon(int index)
    {
        RectTransform tr = Get<RectTransform>(index);

        if (!tr.gameObject.activeSelf) tr.gameObject.SetActive(true);

        tr.localPosition = pivot;
        tr.DOMove(pivot + new Vector3(0, Define.MenuIconSpacing * (index + 1), 0), Define.OpenMenuDuration).SetEase(Ease.OutBack);

    }
    void DisableIcon(int index)
    {
        RectTransform tr = Get<RectTransform>(index);

        tr.DOKill();
        tr.DOMove(pivot, Define.OpenMenuDuration);
        DOVirtual.DelayedCall(Define.OpenMenuDuration, () => { tr.gameObject.SetActive(false); });
    }
    void MenuOnOff(bool _isOpened)
    {
        Button button = GetButton((int)Buttons.MenuOpenButton);
        button.interactable = false;
        Transform tr = button.transform;
        tr.DOKill();

        if (_isOpened)
        {
            tr.DORotate(Vector3.zero, Define.OpenMenuDuration);

            for (int i = 0; i < (int)Transforms.Count; i++)
                DisableIcon(i);
        }
        else
        {
            tr.DORotate(Vector3.forward * 90f, Define.OpenMenuDuration);

            for (int i = 0; i < (int)Transforms.Count; i++)
                EnableIcon(i);
        }
        isOpened = !isOpened;
        DOVirtual.DelayedCall(Define.OpenMenuDuration * 1.1f, () => { button.interactable = true; });
    }
}
