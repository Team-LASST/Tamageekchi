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
    [SerializeField]
    [SerializedDictionary("Key", "Models")]
    private SerializedDictionary<int, OutfitModel> outfitModels;

    [SerializeField]
    private List<ShopItem> shopItems;

    int currSelectedHelmetIndex = 0;

    private void Start()
    {
        loadingManager.gameObject.SetActive(true);
        geeklingPanel.gameObject.SetActive(false);
        for (int i = 0; i < shopItems.Count; i++)
        {
            int temp = i;
            shopItems[i].button.onClick.AddListener(() => UpdateHelmet(temp));
        }

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

            // Update helmet
            CloudScriptManager.Instance.ExecGetHelmetIndex(helmet =>
            {
                UpdateHelmet(helmet);

                loadingManager.gameObject.SetActive(false);
                geeklingPanel.gameObject.SetActive(true);

            }, e => Debug.LogError(e));
        }, e => Debug.LogError(e));

    }

    public void UpdateHelmet(int index)
    {
        currSelectedHelmetIndex = index;
        foreach (var model in outfitModels)
        {
            if (model.Value.hat == null)
                continue;

            model.Value.hat.gameObject.SetActive(model.Key == index);
            model.Value.body.gameObject.SetActive(false);
        }

        for (int i = 0; i < shopItems.Count; i++)
        {
            shopItems[i].UpdateSelection(i == index);
        }
    }

    public void OnPressBack()
    {
        CloudScriptManager.Instance.ExecUpdateHelmet(currSelectedHelmetIndex, _ => SceneManager.LoadScene("MainPage"), e => Debug.LogError(e));
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
