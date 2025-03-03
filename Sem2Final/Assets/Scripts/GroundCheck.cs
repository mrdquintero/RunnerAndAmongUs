using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundCheck : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;


    public LayerMask groundLayer;

    public Collider2D referenceCollider;

    public bool drawLine;

    [SerializeField]
    float width;
    [SerializeField]
    float height;

    void Start()
    {
        width = referenceCollider.bounds.size.x;
        height = referenceCollider.bounds.size.y;
    }


    public string LeftWallCollision()
    {
        float centerX = referenceCollider.bounds.center.x;
        float centerY = referenceCollider.bounds.center.y;
        Vector2 topPos = new Vector2(centerX - width / 2 - 0.07f, centerY + height / 2);
        Vector2 midPos = new Vector2(centerX - width / 2 - 0.07f, centerY);
        Vector2 botPos = new Vector2(centerX - width / 2 - 0.07f, centerY - height / 2);

        if (drawLine)
        {
            Debug.DrawLine(player.transform.position, topPos);
            Debug.DrawLine(player.transform.position, midPos);
            Debug.DrawLine(player.transform.position, botPos);
        }

        Collider2D bot = Physics2D.Linecast(player.transform.position, botPos, groundLayer).collider;
        Collider2D mid = Physics2D.Linecast(player.transform.position, midPos, groundLayer).collider;
        Collider2D top = Physics2D.Linecast(player.transform.position, topPos, groundLayer).collider;

        if (mid != null)
        {
            return GetTile(mid, midPos);
        }
        else if (bot != null)
        {
            return GetTile(bot, botPos);
        }
        else if (top != null)
        {
            return GetTile(top, topPos);
        }

        return "";
    }

    public string RightWallCollision()
    {
        float centerX = referenceCollider.bounds.center.x;
        float centerY = referenceCollider.bounds.center.y;
        Vector2 topPos = new Vector2(centerX + width / 2 + 0.07f, centerY + height / 2);
        Vector2 midPos = new Vector2(centerX + width / 2 + 0.07f, centerY);
        Vector2 botPos = new Vector2(centerX + width / 2 + 0.07f, centerY - height / 2);

        if (drawLine)
        {
            Debug.DrawLine(player.transform.position, topPos);
            Debug.DrawLine(player.transform.position, midPos);
            Debug.DrawLine(player.transform.position, botPos);
        }

        Collider2D bot = Physics2D.Linecast(player.transform.position, botPos, groundLayer).collider;
        Collider2D mid = Physics2D.Linecast(player.transform.position, midPos, groundLayer).collider;
        Collider2D top = Physics2D.Linecast(player.transform.position, topPos, groundLayer).collider;

        if (mid != null)
        {
            return GetTile(mid, midPos);
        }
        else if (bot != null)
        {
            return GetTile(bot, botPos);
        }
        else if (top != null)
        {
            return GetTile(top, topPos);
        }

        return "";
    }

    public string GroundCollision()
    {
        float centerX = referenceCollider.bounds.center.x;
        float centerY = referenceCollider.bounds.center.y;
        Vector2 rightPos = new Vector2(centerX + width / 2, centerY - height / 2 - 0.07f);
        Vector2 midPos = new Vector2(centerX, centerY - height / 2 - 0.07f);
        Vector2 leftPos = new Vector2(centerX - width / 2, centerY - height / 2 - 0.07f);

        if (drawLine)
        {
            Debug.DrawLine(player.transform.position, rightPos);
            Debug.DrawLine(player.transform.position, midPos);
            Debug.DrawLine(player.transform.position, leftPos);
        }

        Collider2D right = Physics2D.Linecast(player.transform.position, rightPos, groundLayer).collider;
        Collider2D mid = Physics2D.Linecast(player.transform.position, midPos, groundLayer).collider;
        Collider2D left = Physics2D.Linecast(player.transform.position, leftPos, groundLayer).collider;

        if (mid != null)
        {
            return GetTile(mid, midPos);
        }
        else if (left != null)
        {
            return GetTile(left, leftPos);
        }
        else if (right != null)
        {
            return GetTile(right, rightPos);
        }

        return "";
    }

    public string CeilingCollision()
    {
        float centerX = referenceCollider.bounds.center.x;
        float centerY = referenceCollider.bounds.center.y;
        Vector2 rightPos = new Vector2(centerX + width / 2, centerY + height / 2 + 0.07f);
        Vector2 midPos = new Vector2(centerX, centerY + height / 2 + 0.07f);
        Vector2 leftPos = new Vector2(centerX - width / 2, centerY + height / 2 + 0.07f);

        if (drawLine)
        {
            Debug.DrawLine(player.transform.position, rightPos);
            Debug.DrawLine(player.transform.position, midPos);
            Debug.DrawLine(player.transform.position, leftPos);
        }

        Collider2D right = Physics2D.Linecast(player.transform.position, rightPos, groundLayer).collider;
        Collider2D mid = Physics2D.Linecast(player.transform.position, midPos, groundLayer).collider;
        Collider2D left = Physics2D.Linecast(player.transform.position, leftPos, groundLayer).collider;

        if (mid != null)
        {
            return GetTile(mid, midPos);
        }
        else if (left != null)
        {
            return GetTile(left, leftPos);
        }
        else if (right != null)
        {
            return GetTile(right, rightPos);
        }

        return "";
    }

    public string GetTile(Collider2D collision, Vector2 collisionPos)
    {
        if (collision.GetComponent<Tilemap>() != null)
        {
            Tilemap tilemap = collision.GetComponent<Tilemap>();
            GridLayout grid = tilemap.transform.parent.gameObject.GetComponent<GridLayout>();
            Vector3Int cellPos = grid.WorldToCell(collisionPos);
            if (tilemap.GetSprite(cellPos) != null)
            {
                return tilemap.GetSprite(cellPos).name;
            }
        }
        return "other";
    }
}
