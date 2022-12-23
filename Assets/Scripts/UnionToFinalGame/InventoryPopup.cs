using System.Collections.Generic;
using InteractiveElements.NativeMVC;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnionToFinalGame
{
    public class InventoryPopup : MonoBehaviour
    {
        [SerializeField] private Image[] itemIcons;
        [SerializeField] private TextMeshProUGUI[] itemLabels;
        [SerializeField] private TextMeshProUGUI curItemLabel;
        [SerializeField] private Button equipButton;
        [SerializeField] private Button useButton;

        private string _curItem;
        
        public void Open() => gameObject.SetActive(true);

        public void Close() => gameObject.SetActive(false);

        public void Refresh()
        {
            var itemList = Managers.Inventory.GetItemList();

            var len = itemIcons.Length;
            for (int i = 0; i < len; i++)
            {
                if (i < itemList.Count)
                {
                    itemIcons[i].gameObject.SetActive(true);
                    itemLabels[i].gameObject.SetActive(true);

                    var item = itemList[i];
                    var sprite = Resources.Load<Sprite>($"Icons/{item}");

                    itemIcons[i].sprite = sprite;
                    itemIcons[i].SetNativeSize();

                    var count = Managers.Inventory.GetItemCount(item);
                    var message = "x" + count;
                    if (item == Managers.Inventory.equippedItem)
                    {
                        message = "Equipped\n" + message;
                    }

                    itemLabels[i].text = message;
                    
                    EventTrigger.Entry entry = new EventTrigger.Entry();
                    entry.eventID = EventTriggerType.PointerClick;
                    entry.callback.AddListener((data => OnItem(item)));

                    var trigger = itemIcons[i].GetComponent<EventTrigger>();
                    trigger.triggers.Clear();
                    trigger.triggers.Add(entry);
                }
                else
                {
                    itemIcons[i].gameObject.SetActive(false);
                    itemLabels[i].gameObject.SetActive(false);
                }
            }

            if (!itemList.Contains(_curItem))
            {
                _curItem = null;
            }

            if (_curItem == null)
            {
                curItemLabel.gameObject.SetActive(false);
                equipButton.gameObject.SetActive(false);
                useButton.gameObject.SetActive(false);
            }
            else
            {
                curItemLabel.gameObject.SetActive(true);
                equipButton.gameObject.SetActive(true);

                if (_curItem == "healt")
                {
                    useButton.gameObject.SetActive(true);
                }
                else
                {
                    useButton.gameObject.SetActive(false);
                }

                curItemLabel.text = _curItem = ":";
            }
        }

        private void OnItem(string item)
        {
            _curItem = item;
            Refresh();
        }

        public void OnEquip()
        {
            Managers.Inventory.EquipItem(_curItem);
            Refresh();
        }

        public void OnUse()
        {
            Managers.Inventory.ConsumeItem(_curItem);
            if (_curItem == "health")
            {
                Managers.Player.ChangeHealth(25);
            }
            Refresh();
        }
    }
}