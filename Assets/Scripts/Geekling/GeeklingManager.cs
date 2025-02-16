using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;
using UnityEngine.SceneManagement;

public class GeeklingManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text characterText;
    [SerializeField]
    private GameObject characterBody;
    [SerializeField]
    private GameObject geeklingPanel;
    [SerializeField]
    private LoadingManager loadingManager;
    [SerializeField]
    [SerializedDictionary("Key", "Models")]
    private SerializedDictionary<string, CharacterModel> characterModels;
    [SerializedDictionary("Key", "Models")]
    private SerializedDictionary<string, OutfitModel> outfitModels;

    private void Start()
    {
        loadingManager.gameObject.SetActive(true);
        geeklingPanel.gameObject.SetActive(false);
        CloudScriptManager.Instance.ExecGetExpertise(expertise =>
        {
            if (!characterModels.ContainsKey(expertise))
            {
                Debug.LogWarning("No experience found!");
                return;
            }

            foreach (var model in characterModels)
            {
                model.Value.head.SetActive(model.Key == expertise);
                model.Value.leftArm.SetActive(model.Key == expertise);
                model.Value.rightArm.SetActive(model.Key == expertise);
                model.Value.body.SetActive(model.Key == expertise);
            }

            characterText.text = expertise switch
            {
                "Software Engineer" => "Byte-Bear",
                "Hardware Engineer" => "Canine Envoy",
                _ => "Stalwart Bill-der"
            };

            loadingManager.gameObject.SetActive(false);
            geeklingPanel.gameObject.SetActive(true);
        }, e => Debug.LogError(e));

    }

    public void OnPressBack()
    {
        SceneManager.LoadScene("MainPage");
    }

    [System.Serializable]
    public struct CharacterModel
    {
        public GameObject head;
        public GameObject leftArm, rightArm;
        public GameObject body;
    }

    [System.Serializable]
    public struct OutfitModel
    {
        public GameObject hat;
        public GameObject body;
    }
}
