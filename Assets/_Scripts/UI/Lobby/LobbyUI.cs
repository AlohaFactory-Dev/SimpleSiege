using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ProtoTypeUI
{
    public class LobbyUI : MonoBehaviour
    {
        void Start()
        {
            var stageButtons = GetComponentsInChildren<Button>();
            int index = 1;
            foreach (var button in stageButtons)
            {
                int capturedIndex = index; // 클로저 문제 해결을 위해 로컬 변수에 저장
                button.onClick.AddListener(() => OnButtonClick(capturedIndex));
                index++;
            }
        }

        private void OnButtonClick(int index)
        {
            GlobalConainer.Get<SelectedStageManager>().SetStage(index);
            GlobalConainer.Get<GameManager>().LoadStage();
        }
    }
}