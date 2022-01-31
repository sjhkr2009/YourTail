using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : UIBase_Popup
{
	enum Buttons {
		ClickHandler
	}

	[SerializeField] private List<GameObject> tutorialRoots = new List<GameObject>();

	private int currentIndex;
	
	void Start() => Init();
	public override void Init() {
		base.Init();
		
		Bind<Button>(typeof(Buttons));
		GetButton((int) Buttons.ClickHandler).onClick.AddListener(ShowNextTutorial);
		
		SetTutorialIndex(currentIndex = 0);
	}

	private void ShowNextTutorial() => SetTutorialIndex(currentIndex + 1);
	
	private void SetTutorialIndex(int targetIndex) {
		int maxCount = tutorialRoots.Count;
		if (targetIndex >= maxCount || targetIndex < 0) {
			GameManager.UI.ClosePopupUI<TutorialUI>();
			return;
		}

		currentIndex = targetIndex;
		for (int i = 0; i < maxCount; i++) {
			tutorialRoots[i].SetActive(i == currentIndex);
		}
	}
}
