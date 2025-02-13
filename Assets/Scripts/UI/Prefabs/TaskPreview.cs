using UnityEngine;
using TMPro;

public class TaskPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _taskText;

    internal void Init(TaskSO descr)
    {
        _taskText.text = descr.GetDescription;
    }
}