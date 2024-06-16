using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class PlayerTracker : MonoBehaviour
{
    [SerializeField] private LayerMask blockLayerMask;
    [SerializeField] private Vector2 trackScale;
    [SerializeField] private float nodeRadius, colliderOffset;
    [SerializeField] private float timePerNode;
    private Node[,] nodes;
    private float startX, startY;
    private Transform player;
    private Vector2 initialPosition;

    private int trackSizeX, trackSizeY;
    private List<Node> path;
    private Node playerNode;
    private Node fleeNode;
    private float playerCheckTime;

    public event Action onMissingPlayer;
    public event Action onFindPlayer;
    private bool isTracking;
    private Tween movement;
    public void RequestPath(bool isFleeing)
    {
        if (SceneManager.Instance.FieldManager.Player == null)
            return;

        if (player == null)
            player = SceneManager.Instance.FieldManager.Player.transform;

        if (IsOnNode(player.position))
        {
            if (!isTracking)
                onFindPlayer?.Invoke();
            isTracking = true;
            if (playerNode == null || playerNode != StandingNode(player.position))
            {
                Vector2 destination;
                if(!isFleeing)
                {
                    destination = player.position;
                    playerNode = StandingNode(player.position);
                }
                else
                {
                    destination = new Vector2(2 * transform.position.x - player.position.x, 2 * transform.position.y - player.position.y);
                    fleeNode = StandingNode(destination);
                }
                FindPath(transform.position, destination);

                if (path == null)
                    return;
                if (path.Count > 1)
                    MoveAlongPath();
                else
                    MoveToPosition(destination);
            }
            playerCheckTime = Time.time;
        }

        if (Time.time - playerCheckTime > 1)
        {
            if (isTracking)
                onMissingPlayer?.Invoke();
            isTracking = false;
            GoBackToStartPoint();
        }
    }

    private void MoveAlongPath()
    {
        if (path.Count > 0)
        {
            Vector3 nextPoint = path.First().point;
            float distance = Vector3.Distance(transform.position, nextPoint);
            float moveTime = distance / (nodeRadius * 2) * timePerNode;
            movement?.Kill();
            movement = transform.DOMove(nextPoint, moveTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                path.RemoveAt(0);
                MoveAlongPath();
            });
        }
    }

    private void MoveToPosition(Vector3 position)
    {
        float distance = Vector3.Distance(transform.position, position);
        float moveTime = distance / (nodeRadius * 2) * timePerNode;
        movement?.Kill();
        movement = transform.DOMove(position, moveTime).SetEase(Ease.Linear);
    }

    public void InitSettings(TopViewEnemy enemy)
    {

        trackSizeX = Mathf.RoundToInt(trackScale.x / (nodeRadius * 2));
        trackSizeY = Mathf.RoundToInt(trackScale.y / (nodeRadius * 2));
        startX = transform.position.x;
        startY = transform.position.y;
        initialPosition = transform.position;
        onFindPlayer -= enemy.Surprised;
        onFindPlayer += enemy.Surprised;
        onMissingPlayer -= enemy.Question;
        onMissingPlayer += enemy.Question;
        CreateScale();
    }

    public void FindPath(Vector2 startPosition, Vector2 targetPosition)
    {
        Node startNode = StandingNode(startPosition);
        Node targetNode = StandingNode(targetPosition);

        Heap<Node> openNodes = new Heap<Node>(trackSizeX * trackSizeY);
        List<Node> closeNodes = new List<Node>();
        openNodes.Add(startNode);

        while (openNodes.Count > 0)
        {
            Node currentNode = openNodes.RemoveFirst();
            closeNodes.Add(currentNode);

            if (currentNode == targetNode)
            {
                SetPath(startNode, targetNode);
                return;
            }

            foreach (Node node in GetNearNodes(currentNode))
            {
                if (!node.walkable || closeNodes.Contains(node))
                    continue;

                int newCost = currentNode.g + GetMoveCost(currentNode, node);
                if (newCost < node.g || !openNodes.Contains(node))
                {
                    node.g = newCost;
                    node.h = GetMoveCost(node, targetNode);
                    node.parentNode = currentNode;

                    if (!openNodes.Contains(node))
                    {
                        openNodes.Add(node);
                    }
                }
            }
        }
    }

    public void SetPath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = nodes[currentNode.parentNode.x, currentNode.parentNode.y];
        }

        path.Reverse();
        this.path = path;
    }
    public void GoBackToStartPoint()
    {
        if (path != null && path.Count > 0)
            path.Clear();
        FindPath(transform.position, initialPosition);
        MoveAlongPath();
    }
    public void CreateScale()
    {
        nodes = new Node[trackSizeX, trackSizeY];
        Vector2 trackMin = (Vector2)transform.position - new Vector2(trackScale.x / 2f, trackScale.y / 2f);

        for (int x = 0; x < trackSizeX; ++x)
        {
            for (int y = 0; y < trackSizeY; ++y)
            {
                Vector2 nodePoint = trackMin + Vector2.right * (x * nodeRadius * 2 + nodeRadius) + Vector2.up * (y * nodeRadius * 2 + nodeRadius);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(nodePoint, nodeRadius - colliderOffset);
                bool walkable = true;
                foreach (Collider2D collider in colliders)
                {
                    if (1 << collider.gameObject.layer == blockLayerMask)
                    {
                        walkable = false;
                        break;
                    }
                }
                nodes[x, y] = new Node(walkable, nodePoint, x, y);
            }
        }
    }

    public List<Node> GetNearNodes(Node node)
    {
        List<Node> nearNodes = new List<Node>();

        for (int x = -1; x <= 1; ++x)
        {
            for (int y = -1; y <= 1; ++y)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.x + x;
                int checkY = node.y + y;

                if (checkX >= 0 && checkY >= 0 && checkX < trackSizeX && checkY < trackSizeY)
                    nearNodes.Add(nodes[checkX, checkY]);
            }
        }

        return nearNodes;
    }
    public int GetMoveCost(Node startNode, Node targetNode)
    {
        int distanceX = Mathf.Abs(startNode.x - targetNode.x);
        int distanceY = Mathf.Abs(startNode.y - targetNode.y);

        if (distanceX > distanceY)
            return (distanceX - distanceY) * 10 + distanceY * 14;
        else
            return (distanceY - distanceX) * 10 + distanceX * 14;
    }

    public Node StandingNode(Vector2 position)
    {
        float percentageX = Mathf.Clamp01((position.x + trackScale.x * 0.5f - startX) / trackScale.x);
        float percentageY = Mathf.Clamp01((position.y + trackScale.y * 0.5f - startY) / trackScale.y);

        int x = Mathf.RoundToInt((trackSizeX - 1) * percentageX);
        int y = Mathf.RoundToInt((trackSizeY - 1) * percentageY);

        return nodes[x, y];
    }
    private bool IsOnNode(Vector2 position)
    {
        float x = (position.x + trackScale.x * 0.5f - startX) / trackScale.x;
        float y = (position.y + trackScale.y * 0.5f - startY) / trackScale.y;

        return ((x > 0 && x < 1) && (y > 0 || y < 1));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(trackScale.x, trackScale.y));
        if (player == null)
            return;
        if (nodes != null)
        {
            Node enemyNode = StandingNode(transform.position);
            Node playerNode = StandingNode(player.position);
            Node fleeNode = StandingNode(new Vector2(2 * initialPosition.x - player.position.x, 2 * initialPosition.y - player.position.y));
            foreach (Node node in nodes)
            {
                Gizmos.color = node.walkable ? Color.green : Color.red;
                if (path != null && path.Contains(node))
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(node.point, Vector2.one * (nodeRadius * 2 - 0.1f));
                    continue;
                }
                if (node == enemyNode)
                    Gizmos.color = Color.blue;
                else if (node == playerNode)
                    Gizmos.color = Color.magenta;
                else if(node == fleeNode)
                    Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(node.point, Vector2.one * (nodeRadius * 2 - 0.1f));
            }
        }
    }
}

public class Node : IHeapItem<Node>
{
    public bool walkable;
    public Vector2 point;
    public Node parentNode;
    public int x, y;
    private int heapIndex;

    /// <summary>
    /// a* 알고리즘에서 가중치. 여기서 가로, 세로로 이동시 10, 대각선 이동시 14를 부여함
    /// </summary>
    public int g;
    /// <summary>
    /// a* 알고리즘에서 목적지까지 도달하는 데 소요되는 비용
    /// </summary>
    public int h;
    public int f { get => g + h; }
    public int HeapIndex { get => heapIndex; set => heapIndex = value; }

    public Node(bool walkable, Vector2 point, int x, int y)
    {
        this.walkable = walkable;
        this.point = point;
        this.x = x;
        this.y = y;
    }

    public int CompareTo(Node compareNode)
    {
        int compare = compareNode.f.CompareTo(f);
        //만약 두 노드의 f값이 같다면
        if (compare == 0)
            compare = compareNode.h.CompareTo(h);

        return compare;
    }
}