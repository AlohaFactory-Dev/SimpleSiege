using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
using System.Collections;
using System.Text;
using Zenject;

[Serializable]
public struct InputEvent
{
    public string ActionId; // 행동 id를 string으로 변경
    public float Time;
}

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
    [Inject] StageManager _stageManager;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => _stageManager.isInit);
        StageConainer.Container.BindInstance(this).AsSingle().NonLazy();
        _startTime = Time.time;
        string path = Path.Combine(Application.persistentDataPath, "InputEvents.json");
        if (!isRecording && File.Exists(path))
        {
            string json = File.ReadAllText(path, Encoding.UTF8);
            InputEventList wrapper = JsonUtility.FromJson<InputEventList>(json);
            if (wrapper?.events != null && wrapper.events.Count > 0)
            {
                _events = wrapper.events;
                PlayEvents();
            }
        }
    }

    public void AddEvent(string actionId)
    {
        _events.Add(new InputEvent
        {
            ActionId = actionId,
            Time = Time.time - _startTime
        });
    }

#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        if (_events.Count > 0)
            SaveEventsToJson();
    }
#endif

    private void PlayEvents()
    {
        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine()
    {
        foreach (var e in _events)
        {
            yield return new WaitForSeconds(e.Time);
            if (e.ActionId == "MoveCameraSequence")
            {
                StageConainer.Get<CPIManager>().MoveCameraSequence();
            }
            else if (e.ActionId == "StartBattle")
            {
                StageConainer.Get<CPIManager>().StartBattle();
            }
        }
    }

    private void SaveEventsToJson()
    {
        string path = Path.Combine(Application.persistentDataPath, "InputEvents.json");
        InputEventList wrapper = new InputEventList { events = _events };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, json, Encoding.UTF8);
        Debug.Log($"입력 이벤트가 저장되었습니다: {path}");
    }
}