using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class OutLineDrawer : Image
{
    [Tooltip("�߰� ������")]
    public RectTransform[] points;

    [Header("Texture")]
    [Tooltip("����� Sprite")]
    public Sprite targetSprite;
    [Tooltip("ƾƮ ����")]
    public Color tintColor = Color.white;

    [Header("Mesh")]
    [Tooltip("�β�")]
    [Min(0f)] public float thickness = 50f;
    [Tooltip("���� �� ����")]
    [Min(1)] public int lineSegment = 5;
    [Tooltip("� ��뿩��")]
    public bool useCurve = true;
    [Tooltip("� �� ���� (� ��� �� ����)")]
    [Min(1)] public int curveSegment = 10;

    [Header("UV")]
    [Tooltip("UV �ݺ��ϴ� �Ÿ�")]
    public float uvDistance = 50f;
    [Tooltip("UV �帣�� �ӵ�")]
    public float uvFlowSpeed = 1f;

    [Header("Option")]
    [Tooltip("�ּ� �Ҽ���(���� ����)")]
    [Min(0f)] public float epsilon = 0.001f;

    // Mesh
    private List<(Vector2 pos, Color32 color, Vector2 uv)> _vertices;
    private List<(int idx, int idx2, int idx3)> _triangles;

    // UV
    private float _uv;
    private float _uvFlow;

    //__________________________________________________________________________ Draw
    protected override void OnPopulateMesh(VertexHelper vh) // UI ��ҿ� ������ ������ �� ����Ǵ� �Լ�
    {
        base.OnPopulateMesh(vh);
        if (this.points == null) return;

        // ������ ������ �ϱ� ���� ���� ������ ��� �����Ѵ�.
        vh.Clear();

        // ������ �ﰢ�� �����͸� ���� ����Ʈ���� �ʱ�ȭ�Ѵ�.
        if (_vertices == null) _vertices = new List<(Vector2 pos, Color32 color, Vector2 uv)>();
        if (_triangles == null) _triangles = new List<(int idx, int idx2, int idx3)>();

        // ���� Transform �������� ����Ʈ���� ��ȯ�ϰ�, ��ġ�� ����Ʈ���� �����Ѵ�.
        Vector2[] points = getLinePoints();

        // ���� ���� ������ �� ������ ��� Ʃ�÷ν�, ���� ������ ������ ������ ��´�.
        (Vector2 left, Vector2 right) start, end = default;

        // ����Ʈ���� �ݺ��ϸ鼭, ���� ������ �� ������ ���� ������ �׾��ش�.
        for (int i = 0, l = points.Length - 1; i < l; ++i)
        {
            start = GetLineSide(points[i], points[i + 1], 0f, thickness);
            // ������ �� ������ ������ ���� �������� Ŀ�긦 �׾��ش�.
            if (useCurve && i > 0) stackMeshCurve(end, start);
            end = GetLineSide(points[i], points[i + 1], 1f, thickness);

            stackMeshLine(start, end);
        }

        // ������� ���� �����ͷ� ������ �籸���Ѵ�.
        applyMesh(vh);
    }
    private Vector2[] getLinePoints() // ����Ʈ�� �����ϴ� �Լ�
    {
        // ����Ʈ�� ��ġ�� ���� Transform �������� ��ȯ�Ѵ�.
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < this.points.Length; ++i)
        {
            if (this.points[i]) points.Add(InverseRectTransformPoint(transform, this.points[i].position));
        }

        // ��ġ�� ����Ʈ�� �����Ѵ�.
        for (int i = 0; i < points.Count - 1; ++i)
        {
            if (Approximately(points[i], points[i + 1], epsilon))
            {
                points.RemoveAt(i + 1);
                --i;
            }
        }

        return points.ToArray();
    }

    //__________________________________________________________________________ Mesh
    private void stackMeshLine((Vector2 left, Vector2 right) start, (Vector2 left, Vector2 right) end) // ������ �״� �Լ�
    {
        // ���� �� ������ŭ ������, ���� ������ �� ������ �״´�.
        for (int i = 0; i < lineSegment; ++i)
        {
            Vector2 startLeft = Vector2.Lerp(start.left, end.left, (float)i / lineSegment);
            Vector2 startRight = Vector2.Lerp(start.right, end.right, (float)i / lineSegment);
            Vector2 endLeft = Vector2.Lerp(start.left, end.left, ((float)i + 1) / lineSegment);
            Vector2 endRight = Vector2.Lerp(start.right, end.right, ((float)i + 1) / lineSegment);

            stackMeshSquare((startLeft, startRight), (endLeft, endRight));
        }
    }
    private void stackMeshCurve((Vector2 left, Vector2 right) start, (Vector2 left, Vector2 right) end) // Ŀ�긦 �״� �Լ�
    {
        // Ŀ�� ������ �� ���⺤�Ͱ� ������ Ŀ�갡 �ʿ�����Ƿ� �����Ѵ�.
        Vector2 startDir = (start.right - start.left).normalized;
        Vector2 endDir = (end.right - end.left).normalized;
        if (Approximately(startDir, endDir, epsilon)) return;

        // �� ���⺤���� ������ �̸� ����Ѵ�.
        float signedAngle = Vector2.SignedAngle(startDir, endDir);
        float angleSegment = signedAngle / curveSegment;

        // ���� ������ �������� �ٶ󺸴� ������ ���� ������ �����Ѵ�.
        Quaternion baseLookRot = Quaternion.LookRotation(startDir, -Vector3.forward);
        Vector2 center = (start.left + start.right) * 0.5f;

        // ��� �� ������ŭ �ݺ��ϸ鼭, ���� ������ �� ������ �׾��ش�.
        for (int i = 0; i < curveSegment; ++i)
        {
            // �� ���⺤���� ������ �̸� ��Ƶд�.
            Vector2 startLeft, startRight, endLeft, endRight;
            startLeft = startRight = endLeft = endRight = center;

            // ���� ������ ������ �������̹Ƿ�, ������ ������, �ĸ��� ������ �ȴ�.
            Quaternion startLookRot = baseLookRot * Quaternion.Euler(0f, -angleSegment * i, 0f);
            startLeft += (Vector2)(startLookRot * Vector3.back * thickness * 0.5f);
            startRight += (Vector2)(startLookRot * Vector3.forward * thickness * 0.5f);

            Quaternion endLookRot = baseLookRot * Quaternion.Euler(0f, -angleSegment * (i + 1), 0f);
            endLeft += (Vector2)(endLookRot * Vector3.back * thickness * 0.5f);
            endRight += (Vector2)(endLookRot * Vector3.forward * thickness * 0.5f);

            stackMeshSquare((startLeft, startRight), (endLeft, endRight));
        }
    }
    private void stackMeshSquare((Vector2 left, Vector2 right) start, (Vector2 left, Vector2 right) end) // �簢���� �״� �Լ�
    {
        // �Ÿ��� ���� UV�� �����Ͽ�, ���ο� UV�� �����Ѵ�.
        float distance = (Vector2.Distance(start.left, end.left) + Vector2.Distance(start.right, end.right)) * 0.5f;
        float newUV = _uv + distance / uvDistance;

        // ������ ������ �״´�. (UV Flow ���� ����)
        _vertices.Add((start.left, color, new Vector2(0f, _uv - _uvFlow)));
        _vertices.Add((end.left, color, new Vector2(0f, newUV - _uvFlow)));
        _vertices.Add((end.right, color, new Vector2(1f, newUV - _uvFlow)));
        _vertices.Add((start.right, color, new Vector2(1f, _uv - _uvFlow)));

        // �ﰢ���� �״´�. (���� �Ʒ����� �ð��������)
        int count = _vertices.Count;
        _triangles.Add((count - 4, count - 3, count - 2));
        _triangles.Add((count - 2, count - 1, count - 4));

        // ���ο� UV�� �����Ѵ�.
        _uv = newUV;
    }
    private void applyMesh(VertexHelper vh) // ������ �����ϴ� �Լ�
    {
        // ������� ���� �����͸� �������� ������ �����Ѵ�.
        for (int i = 0, l = _vertices.Count; i < l; ++i)
            vh.AddVert(_vertices[i].pos, _vertices[i].color, _vertices[i].uv);
        for (int i = 0, l = _triangles.Count; i < l; ++i)
            vh.AddTriangle(_triangles[i].idx, _triangles[i].idx2, _triangles[i].idx3);

        // �����͸� ���� �������� �ʱ�ȭ�Ѵ�.
        _vertices.Clear();
        _triangles.Clear();
        _uv = 0f;
        _uvFlow = (_uvFlow + uvFlowSpeed * Time.deltaTime) % 1f;
    }

    //__________________________________________________________________________ Util
    public static bool Approximately(Vector2 a, Vector2 b, float epsilon) // �� ������ ���� ��ġ�ϴ��� �Ǵ��ϴ� �Լ�(Vector2)
    {
        return Approximately(a.x, b.x, epsilon) && Approximately(a.y, b.y, epsilon);
    }
    public static bool Approximately(float a, float b, float epsilon) // �� ������ ���� ��ġ�ϴ��� �Ǵ��ϴ� �Լ�(Float)
    {
        // Epsilon �̳��� �����̸� ���� ������ �Ǵ��Ѵ�.
        return Mathf.Abs(a - b) <= epsilon;
    }
    public static Vector2 InverseRectTransformPoint(Transform tr, Vector2 world) // Transform�� Local Point���� �����ϴ� �Լ�
    {
        // Canvas�� ũ�Ⱑ ����� �� �����Ƿ�, LossyScale�� �����ش�.
        Vector2 scale = tr.lossyScale;
        world -= (Vector2)tr.position;
        world.x /= scale.x;
        world.y /= scale.y;
        return world;
    }
    public static (Vector2 left, Vector2 right) GetLineSide(Vector2 start, Vector2 end, float t, float thickness) // ������ ���� ������ ���ϴ� �Լ�
    {
        // ���⺤�͸� ���� ������ ���Ѵ�.
        Vector3 dir = end - start;
        Quaternion lookRot = Quaternion.LookRotation(dir, -Vector3.forward);

        // �߰� �������� ���� -90, 90�� ȸ���Ͽ� ���� ������ ������ ������ ���Ѵ�.
        Vector2 center = Vector2.Lerp(start, end, t);
        Vector2 left = center + (Vector2)(lookRot * Quaternion.Euler(0f, -90f, 0f) * Vector3.forward * thickness * 0.5f);
        Vector2 right = center + (Vector2)(lookRot * Quaternion.Euler(0f, 90f, 0f) * Vector3.forward * thickness * 0.5f);

        return (left, right);
    }

    //__________________________________________________________________________ Update
    protected virtual void Update()
    {
        // OnPopulateMesh�� UI ��ҿ� ����� ����(ũ��, �Ǻ�, ��Ŀ ��) ����ǹǷ�,
        // �� ������ �����ϱ� ���� SetVerticesDirty �Լ��� �����Ѵ�.
        SetVerticesDirty();
    }

    //__________________________________________________________________________ Editor
    //protected override void OnValidate() // �ν����� â���� ������Ƽ�� ������ �� ����Ǵ� �Լ�
    //{
    //    base.OnValidate();

    //    // Sprite�� �����Ѵ�.
    //    if (targetSprite != sprite)
    //        sprite = targetSprite;

    //    // WrapMode�� Repeat�� �Ѵ�.
    //    if (targetSprite && targetSprite.texture.wrapMode != TextureWrapMode.Repeat)
    //        targetSprite.texture.wrapMode = TextureWrapMode.Repeat;

    //    // Color�� �����Ѵ�.
    //    if (tintColor != color)
    //        color = tintColor;
    //}
}

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(OutLineDrawer))]
public class LineDrawerInspector : UnityEditor.Editor
{
    public override void OnInspectorGUI() // �ν����͸� ���� �����ϴ� �Լ�
    {
        // LineDrawer Ŭ�������� ����ϴ� ������Ƽ���� ǥ���Ѵ�.
        foreach (var field in typeof(OutLineDrawer).GetFields())
            UnityEditor.EditorGUILayout.PropertyField(serializedObject.FindProperty(field.Name), true);
        serializedObject.ApplyModifiedProperties();
    }
}
#endif