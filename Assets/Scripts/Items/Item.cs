using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public ItemData data;
    public string GetInteractPrompt()
    {
        return $"{data.name}\n{data.desc}";
    }

    public void OnInteract()
    {
        PlayerManager.Instance.Player.itemData = data;
        PlayerManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }

}
