using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class TaskPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _taskText;

    internal async Task InitAsync(TaskSO descr)
    {
        _taskText.text = await Localizations.GetLocalizedText(
            Localizations.Tables.Tasks,
            descr.KeyLocDesc);
    }
}