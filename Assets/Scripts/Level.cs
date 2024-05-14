using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    private IMatchGameItem item1;
    private IMatchGameItem item2;

    [SerializeField] MatchGameCore.MatchGameSetup setup;
    private readonly List<IMatchGameItem> items = new List<IMatchGameItem>();

    private void Awake()
    {
        MatchGameCore.OnItemSpawned += MatchGameCore_OnItemSpawned;

        MatchGameCore.OnFindMatches += MatchGameCore_OnFindMatches;
        MatchGameCore.OnItemMoveDown += MatchGameCore_OnItemMoveDown;
    }

    private void OnDestroy()
    {
        MatchGameCore.OnItemSpawned -= MatchGameCore_OnItemSpawned;

        MatchGameCore.OnFindMatches -= MatchGameCore_OnFindMatches;
        MatchGameCore.OnItemMoveDown -= MatchGameCore_OnItemMoveDown;
    }

    private void MatchGameCore_OnItemSpawned(Vector2 target, Vector2 start, Vector2Int index, int id)
    {
        items.Add(Item.Instant(target, start, index, id));
    }

    private void MatchGameCore_OnFindMatches(List<Vector2Int> match)
    {
        Sfx.Instant();
        CameraShake.Make();
        BalanceText.Count += match.Count;

        foreach (Vector2Int index in match)
        {
            var find = items.FindItemByIndex(index);
            Vfx.Instant(find.GameObject);

            items.Remove(find);
            Destroy(find.GameObject);
        }
    }

    private void MatchGameCore_OnItemMoveDown(Vector2Int index, Vector2Int nextIndex, Vector2 next)
    {
        var find = items.FindItemByIndex(index);
        if(find == null)
        {
            return;
        }

        find.Index = nextIndex;
        find.TargetPosition = next;
    }

    private void Start()
    {
        MatchGameCore.MatchGameInit(setup, this);
    }

    private void Update()
    {
        if(Result.gameOver || SettingsBtn.IsPressed || ExitPopup.IsOpened || PausePopup.isActive)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            var m_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            var hit = Physics2D.Raycast((Vector2)m_position, Vector2.zero, 0.0f);
            if (hit.collider)
            {
                item1 = hit.collider.GetComponent<IMatchGameItem>();
            }
        }

        if (item1 != null && Input.GetMouseButtonUp(0))
        {
            var m_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var direction = m_position - item1.GameObject.transform.position;

            Vector3 findDirection = Vector3.zero;
            if (Vector2.Dot(direction, Vector2.up) > 0.8f)
            {
                findDirection = Vector3.up;
            }
            else if (Vector2.Dot(direction, Vector2.down) > 0.8f)
            {
                findDirection = Vector3.down;
            }
            else if (Vector2.Dot(direction, Vector2.left) > 0.8f)
            {
                findDirection = Vector3.left;
            }
            else if (Vector2.Dot(direction, Vector2.right) > 0.8f)
            {
                findDirection = Vector3.right;
            }

            findDirection *= 1.1f;
            var hit = Physics2D.Raycast(item1.GameObject.transform.position + findDirection, Vector2.zero);
            if (hit.collider)
            {
                item2 = hit.collider.GetComponent<IMatchGameItem>();
            }

            if (item1 == null || item2 == null || findDirection == Vector3.zero)
            {
                item1 = item2 = null;
                return;
            }

            if (item1.ID == item2.ID)
            {
                item1 = item2 = null;
                return;
            }

            SetPositionSfx.Instant();
            item1.ChangeWith(item2);
            
            item1 = item2 = null;
        }
    }
}
