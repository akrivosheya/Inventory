using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]

public class UIItem : MonoBehaviour
{
    public int Index { get; set; }
    [SerializeField] private Text Count;
    [SerializeField] private string ItemsImagesFile = "Sprites";
    [SerializeField] private string ErrorImage = "error";
    [SerializeField] private string PathSeparator = "/";
    private Image ItemImage;
    private readonly string EmptyString = "";

    void Start()
    {
        ItemImage = GetComponent<Image>();
        UpdateItem();
    }

    public void UpdateItem()
    {
        int count = 0;
        try
        {
            count = Managers.Inventory.GetItemCount(Index);
        }
        catch(System.ArgumentException ex)
        {
            Debug.Log("Can't update data about item " + Index + ": " + ex);
            Count.gameObject.SetActive(false);
            ItemImage.gameObject.SetActive(false);
            return;
        }
        Count.gameObject.SetActive(true);
        
        var id = Managers.Inventory.GetItemId(Index);
        if(id == null)
        {
            id = ErrorImage;
        }
        if(id == Item.EmptyItemId)
        {
            ItemImage.sprite = null;
        }
        else
        {
            var image = Resources.Load<Sprite>(ItemsImagesFile + PathSeparator + id);
            ItemImage.sprite = image;
        }
        if(id == Item.EmptyItemId || count == 0)
        {
            Count.text = EmptyString;
        }
        else
        {
            Count.text = count.ToString();
        }
    }
}
