using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class MapScenes : ScriptableObject
{
    [SerializeField] private List<SceneAsset> scene;

    public void LoadLevel()
    {
        SceneManager.LoadScene(scene[0].name);
    }
}