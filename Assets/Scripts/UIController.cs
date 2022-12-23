using System;
using InteractiveElements.NativeMVC;
using TMPro;
using UnionToFinalGame;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreLabel;
    [SerializeField] private TextMeshProUGUI healthLabel;
    [SerializeField] private SettingsPopup settingsPopup;
    [SerializeField] private InventoryPopup inventoryPopup;
    [SerializeField] private bool hideCursor;
    private int _score;
    private void Awake()
    {
        Messenger.AddListener(GameEvent.ENEMY_HIT, OnEnemyHit);
        Messenger.AddListener(GameEvent.HEALT_UPDATE, OnHealthUpdate);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.ENEMY_HIT, OnEnemyHit);
        Messenger.RemoveListener(GameEvent.HEALT_UPDATE, OnHealthUpdate);
    }
    
    private void Start()
    {
        _score = 0;
        scoreLabel.text = _score.ToString();
        
        OnHealthUpdate();
        settingsPopup.Close();
        inventoryPopup.Close();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            var isOpen = settingsPopup.gameObject.activeSelf;
            if (isOpen)
            {
                SetActiveCursor(false);
                settingsPopup.Close();
            }
            else
            {
                SetActiveCursor(true);
                settingsPopup.Open();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            var isOpen = inventoryPopup.gameObject.activeSelf;
            if (isOpen)
            {
                SetActiveCursor(false);
                inventoryPopup.Close();
            }
            else
            {
                SetActiveCursor(true);
                inventoryPopup.Open();
            }

            inventoryPopup.Refresh();
        }
    }

    private void OnEnemyHit()
    {
        _score += 1;
        scoreLabel.text = _score.ToString();
    }
    
    private void OnHealthUpdate()
    {
        var message = $"Health: {Managers.Player.Health}/{Managers.Player.MaxHealth}";
        healthLabel.text = message;
    }

    public void OnOpenSettings()
    {
        settingsPopup.Open();
        //Debug.Log("open settings");
    }

    public void OnPointerDown()
    {
        Debug.Log("pointer down");
    }

    private void SetActiveCursor(bool value)
    {
        if (hideCursor == false)
            return;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = value;
    }
}
