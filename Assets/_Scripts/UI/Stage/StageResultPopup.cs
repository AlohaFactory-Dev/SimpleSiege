using Aloha.Coconut.UI;
using UnityEngine;
using UnityEngine.UI;

public class StageResultPopup : UISlice
{
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private Button restartButton;
    private bool _isInitialized = false;

    public class Args : UIOpenArgs
    {
        public TeamType winner;
    }

    protected override void Open(UIOpenArgs args)
    {
        Init();
        var resultArgs = args as Args;
        if (resultArgs == null) return;

        // 결과에 따라 UI 업데이트 로직 구현
        if (resultArgs.winner == TeamType.Player)
        {
            // 승리 UI 설정
            winUI.SetActive(true);
            loseUI.SetActive(false);
        }
        else
        {
            // 패배 UI 설정
            winUI.SetActive(false);
            loseUI.SetActive(true);
        }
    }

    private void Init()
    {
        if (_isInitialized) return;
        _isInitialized = true;
        restartButton.onClick.AddListener(OnRestartButtonClicked);
    }

    private void OnRestartButtonClicked()
    {
        CloseView();
        GlobalConainer.Get<GameManager>().ReLoadLobby();
    }
}