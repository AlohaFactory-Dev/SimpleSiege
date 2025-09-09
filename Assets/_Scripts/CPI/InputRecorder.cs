using UnityEngine;
using System.Collections.Generic;

public struct InputEvent
{
    public enum EventType { KeyDown, KeyUp, MouseClick }

    public EventType Type;
    public KeyCode Key;
    public Vector2 MousePosition;
    public float Time;
}

public class InputRecorder : MonoBehaviour
{
    private List<InputEvent> _events = new List<InputEvent>();
    private float _startTime;

    void Start()
    {
        _startTime = Time.time;
    }

    void Update()
    {
        // 키 입력 저장
        if (Input.GetKeyDown(KeyCode.A))
        {
            _events.Add(new InputEvent
            {
                Type = InputEvent.EventType.KeyDown,
                Key = KeyCode.A,
                Time = Time.time - _startTime
            });
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
}