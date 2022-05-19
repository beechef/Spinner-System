using TMPro;
using UnityEngine;

public class PieceRenderer : MonoBehaviour
{
    public Piece ItemPiece;

    private SpriteRenderer spriteRenderer;
    private Texture2D texture;
    private Sprite sprite;

    private GameObject itemName;
    private GameObject itemQuantity;
    private GameObject itemSprite;


    void Start()
    {

        SetupTexture();
        if (ItemPiece.PieceAngle > 180)
        {
            RenderCircle();
        }
        else
        {
            RenderPiece();

        }
        itemSprite = RenderItemSprite();
        itemName = CreateText("Item Name", ItemPiece.NameColor, ItemPiece.PieceItem.Name, TextAlignmentOptions.Top, ItemPiece.NameSize, ItemPiece.NameWidth, ItemPiece.NameHeight);
        itemQuantity = CreateText("Item Quantity", ItemPiece.QuantityColor, ItemPiece.Quantity.ToString(), TextAlignmentOptions.Top, ItemPiece.QuantitySize, ItemPiece.QuantityWidth, ItemPiece.QuantityHeight);
        UpdateOffset(itemSprite);
        UpdateOffset(itemName);
        UpdateOffset(itemQuantity);
    }
    void Update()
    {
        
    }

    void UpdateOffset(GameObject go)
    {
        go.transform.Translate(new Vector3(0, ItemPiece.PieceOffset, 0), Space.Self);
    }
    GameObject CreateText(string name, Color color, string content, TextAlignmentOptions align, float fontSize, float width, float height)
    {
        GameObject text = new GameObject();
        TextMeshPro textMesh = text.AddComponent<TextMeshPro>();
        text.name = name;
        text.transform.Rotate(new Vector3(0, 0, ItemPiece.PieceAngle / 2));

        textMesh.color = color;
        textMesh.text = content;
        textMesh.alignment = align;
        textMesh.rectTransform.sizeDelta = new Vector2(width, height);
        textMesh.fontSizeMin = fontSize / 2;
        textMesh.fontSizeMax = fontSize;
        textMesh.characterWidthAdjustment = 80f;

        textMesh.enableAutoSizing = true;
        text.transform.SetParent(transform, false);
        return text;
    }

    void SetupTexture()
    {
        Vector2 pivot = new Vector2(0, 0.5f);
        if (ItemPiece.PieceAngle > 180)
        {
            pivot = new Vector2(0.5f, 0.5f);
            ItemPiece.PieceRadius *= 2;
            texture = new Texture2D(ItemPiece.PieceRadius, ItemPiece.PieceRadius);
            sprite = Sprite.Create(texture, new Rect(0, 0, ItemPiece.PieceRadius - 1, ItemPiece.PieceRadius), pivot);
        }
        else
        {
            texture = new Texture2D(ItemPiece.PieceRadius, ItemPiece.PieceRadius * 2);
            sprite = Sprite.Create(texture, new Rect(0, 0, ItemPiece.PieceRadius - 1, ItemPiece.PieceRadius * 2), pivot);
        }
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = ItemPiece.PieceColor;
        spriteRenderer.sortingOrder = -1;
    }

    GameObject RenderItemSprite()
    {
        GameObject itemSprite = new GameObject();
        itemSprite.name = "Item Sprite";
        SpriteRenderer spriteRenderer = itemSprite.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = ItemPiece.PieceItem.Image;
        spriteRenderer.sprite = Sprite.Create(spriteRenderer.sprite.texture, spriteRenderer.sprite.rect, new Vector2(0.5f, 0));
        itemSprite.transform.localScale = new Vector3(ItemPiece.SpriteWidthScale, ItemPiece.SpriteHeightScale, 0);
        itemSprite.transform.Rotate(new Vector3(0, 0, ItemPiece.PieceAngle / 2));
        itemSprite.transform.Translate(new Vector3(0, ItemPiece.SpriteOffset, 0), Space.Self);
        itemSprite.transform.SetParent(transform, false);
        return itemSprite;
    }

    void RenderCircle()
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (IsInCircle(new Vector2(ItemPiece.PieceRadius / 2, ItemPiece.PieceRadius / 2), new Vector2(x, y), ItemPiece.PieceRadius / 2))
                {
                    texture.SetPixel(x, y, ItemPiece.PieceTexture.GetPixel(x, y));
                }
                else
                    texture.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
        texture.Apply();
    }
    void RenderPiece()
    {
        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                if (CheckPoint(x, y, ItemPiece.PieceRadius, ItemPiece.PieceAngle))
                {
                    texture.SetPixel(x, y, ItemPiece.PieceTexture.GetPixel(x, y));
                }
                else
                    texture.SetPixel(x, y, new Color(0, 0, 0, 0));
            }
        }
        texture.Apply();
    }
    bool CheckPoint(int x, int y, int radius, float angle)
    {
        Vector2 aPoint = new Vector2(0, 0);
        Vector2 mPoint = new Vector2(0, radius);
        Vector2 fPoint = new Vector2(x, y);

        float fAngle = Vector2.Angle(aPoint - mPoint, mPoint - fPoint);

        return fAngle <= angle && IsInCircle(mPoint, fPoint, radius);

    }
    bool IsInCircle(Vector2 mPoint, Vector2 fPoint, int radius)
    {
        return Vector2.Distance(mPoint, fPoint) <= radius;
    }
}
