using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Collections;
using System.Text;

[Serializable]
public struct InputEvent
{
    public enum EventType { KeyDown, KeyUp, MouseClick }

    public EventType Type;
    public KeyCode Key;
    public Vector2 MousePosition;
    public float Time;
}

// 입력 이벤트 리스트를 직렬화하기 위한 래퍼 클래스
[Serializable]
public class InputEventList
{
    public List<InputEvent> events = new List<InputEvent>();
}

public class InputRecorder : MonoBehaviour
{
    [SerializeField] private bool isRecording = false;
    private List<InputEvent> _events = new List<InputEvent>();
    private float _startTime;

    void Start()
    {
        _startTime = Time.time;
    }

    void Update()
    {
        if (!isRecording) return;

        // 키 입력 저장
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
            {
                _events.Add(new InputEvent
                {
                    Type = InputEvent.EventType.KeyDown,
                    Key = kcode,
                    Time = Time.time - _startTime
                });
            }

            if (Input.GetKeyUp(kcode))
            {
                _events.Add(new InputEvent
                {
                    Type = InputEvent.EventType.KeyUp,
                    Key = kcode,
                    Time = Time.time - _startTime
                });
            }
        }

        // 마우스 클릭 저장
        if (Input.GetMouseButtonDown(0))
        {
            _events.Add(new InputEvent
            {
                Type = InputEvent.EventType.MouseClick,
                MousePosition = Input.mousePosition,
                Time = Time.time - _startTime
            });
        }
    }

#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        if (_events.Count > 0)
        {
            SaveEventsToJson();
        }
    }
#endif

    // 저장된 입력을 재생하는 예시
    public void PlayEvents()
    {
        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator<WaitForSeconds> PlayCoroutine()
    {
        foreach (var e in _events)
        {
            yield return new WaitForSeconds(e.Time);
            if (e.Type == InputEvent.EventType.KeyDown)
            {
                // 키 입력을 자동으로 발생시키는 것은 Unity에서 직접적으로 불가능
                // 대신 해당 키 입력에 대응하는 함수를 호출
                Debug.Log($"A키 입력됨");
            }
            else if (e.Type == InputEvent.EventType.MouseClick)
            {
                Debug.Log($"마우스 클릭: {e.MousePosition}");
            }
        }
    }

    // 입력 이벤트를 JSON 파일로 저장
    public void SaveEventsToJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "InputEvents.json");
        InputEventList wrapper = new InputEventList { events = _events };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, json, Encoding.UTF8);
        Debug.Log($"입력 이벤트가 저장되었습니다: {path}");
    }
}