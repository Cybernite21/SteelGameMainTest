using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //public static event System.Action plrCaught;

    public bool attacking = false;

    public float viewDistance = 3f;
    public float attackDelay = 2f;
    public float viewAngle = 45f;
    public static float plrCatchTimer = 1f;

    [Range(4, 100)]
    public int fovMeshRes;

    public int damage = 5;

    Vector3[] verticiesAdv; 

    float plrVisibleTimer = 0;

    public LayerMask viewBlockingMask;
    public LayerMask fovMask;

    [ColorUsage(true, true)]
    public Color caughtPlrColor;
    Color originalColor;

    public LineRenderer fillLine;
    LineRenderer lines;

    Transform plr;

     float meshResolution;
     int edgeResolveIterations;
     float edgeDstThreshold;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    // Start is called before the first frame update
    void Start()
    {
        plr = GameObject.FindGameObjectWithTag("Player").transform;
        originalColor = GetComponent<Renderer>().material.color;
        lines = GetComponent<LineRenderer>();

        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
    }

    // Update is called once per frame
    void Update()
    {
        plrCheck();
    }
    void LateUpdate()
    {
        //DrawFieldOfView();
        drawLineAdv();
    }

    void plrCheck()
    {
        if (canSeePlr())
        {
            plrVisibleTimer += Time.deltaTime;
        }
        else
        {
            plrVisibleTimer -= Time.deltaTime;
        }
        plrVisibleTimer = Mathf.Clamp(plrVisibleTimer, 0, plrCatchTimer);
        GetComponent<Renderer>().material.color = Color.Lerp(originalColor, caughtPlrColor, plrVisibleTimer / plrCatchTimer);
        if(plrVisibleTimer == plrCatchTimer)
        {
            if(!attacking)
            {
                StartCoroutine(dealDamage());
            }
        }
    }

    IEnumerator dealDamage()
    {
        attacking = true;
        while(plrVisibleTimer == plrCatchTimer)
        {
            plr.gameObject.GetComponent<PlayerControler>().health -= damage;
            yield return new WaitForSecondsRealtime(attackDelay);
            yield return new WaitForEndOfFrame();
        }
        attacking = false;
        yield return null;
    }

    void drawLine()
    {
        Vector3[] verticies = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        LineRenderer[] lines = GetComponentsInChildren<LineRenderer>();
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].SetPosition(0, transform.position);
            if (i == 0)
                lines[i].SetPosition(1, transform.position + Quaternion.AngleAxis(viewAngle / 2f, transform.up) * transform.forward * viewDistance);
            else if(i == 1)
                lines[i].SetPosition(1, transform.position + Quaternion.AngleAxis(viewAngle / -2f, transform.up) * transform.forward * viewDistance);
            else
                lines[i].SetPosition(1, transform.position + transform.forward * viewDistance);
        }
        fillLine.positionCount = lines.Length - 1;
        for (int i = 0; i < lines.Length - 1; i++)
        {
            fillLine.SetPosition(i, lines[i].GetPosition(1));
            print(i);
        }
        Vector3 tmp = fillLine.GetPosition(1);
        fillLine.SetPosition(1, fillLine.GetPosition(2));
        fillLine.SetPosition(2, tmp);

        //mesh Verticies locations
        verticies[1] = transform.position - transform.position;
        verticies[0] = lines[0].GetPosition(1) - transform.position;
        verticies[2] = lines[1].GetPosition(1) - transform.position;
        verticies[3] = lines[2].GetPosition(1) - transform.position;

        //mesh uv locations
        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(1, 0);

        //mesh triangles
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 3;
        triangles[4] = 0;
        triangles[5] = 2;

        //making the mesh
        Mesh fovMeshGenerated = new Mesh();
        fovMeshGenerated.Clear();
        fovMeshGenerated.vertices = verticies;
        fovMeshGenerated.uv = uv;
        fovMeshGenerated.triangles = triangles;
        fovMeshGenerated.RecalculateBounds();
        fovMeshGenerated.RecalculateNormals();
        fovMeshGenerated.Optimize();
        fovMeshGenerated.MarkDynamic();
        
        viewMeshFilter.mesh = fovMeshGenerated;
    }

    void drawLineAdv()
    {
        float angle = viewAngle / -2f;
        //Vector3[] 
        verticiesAdv = new Vector3[fovMeshRes + 2];
        //Vector2[] uv = new Vector2[fovMeshRes + 2]; 
        int[] trianglesAdv = new int[(verticiesAdv.Length - 2)*3];
        float stepDegree = viewAngle / fovMeshRes;
        Ray ray = new Ray(transform.position, Quaternion.AngleAxis(viewAngle / -2f, transform.up) * transform.forward);
        RaycastHit rayInfo;

        //Verticies
        verticiesAdv[0] = transform.position;
        for (int i = 0; i <= fovMeshRes; i++)
        {
            if(Physics.Raycast(ray, out rayInfo, viewDistance, fovMask, QueryTriggerInteraction.Ignore))
            {
                verticiesAdv[i + 1] = rayInfo.point;
            }
            else
            {
                verticiesAdv[i + 1] = (Quaternion.AngleAxis(angle, transform.up) * transform.forward * viewDistance) + transform.position;
            }
            
            angle += stepDegree;
            ray.direction = Quaternion.AngleAxis(angle, transform.up) * transform.forward;
        }        

        //triangles
        int tmp = 0;
        for (int i = 0; i < trianglesAdv.Length; i++)
        {
            if (i == 0)
            {
                trianglesAdv[i] = 0;
            }
            else if(i % 3 == 0)
            {
                trianglesAdv[i] = 0;
                tmp = trianglesAdv[i - 1];
            }
            else if(tmp == 0)
            {
                trianglesAdv[i] = i;
            }
            else
            {
                trianglesAdv[i] = tmp;
                tmp += 1;
            }
        }
        
        //fix position
        for (int i = 0; i < verticiesAdv.Length; i++)
        {
            verticiesAdv[i] = transform.InverseTransformPoint(verticiesAdv[i]);
        }

        Mesh fovMeshGenerated = new Mesh();
        fovMeshGenerated.Clear();
        fovMeshGenerated.vertices = verticiesAdv;
        //fovMeshGenerated.uv = uv;
        fovMeshGenerated.triangles = trianglesAdv; 
        fovMeshGenerated.RecalculateBounds();
        fovMeshGenerated.RecalculateNormals();
        fovMeshGenerated.Optimize();
        fovMeshGenerated.MarkDynamic();

        viewMeshFilter.mesh = fovMeshGenerated;
    }

    bool canSeePlr()
    {
        if(Vector3.Distance(transform.position, plr.position) < viewDistance)
        {
            Vector3 dirToPlr = (plr.position - transform.position).normalized;
            float angleBetweenMeAndPlr = Vector3.Angle(transform.forward, dirToPlr);
            if(angleBetweenMeAndPlr < viewAngle/2f)
            {
                Vector3 correctedPlrPos = new Vector3(plr.position.x, transform.position.y, plr.position.z);
                if(!Physics.Linecast(transform.position, correctedPlrPos, viewBlockingMask))
                {
                    return true;
                }
            }
        }

        return false;
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewDistance, fovMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * viewDistance, viewDistance, globalAngle);
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(viewAngle/2f, transform.up) * transform.forward * viewDistance);
        Gizmos.DrawRay(transform.position, Quaternion.AngleAxis(viewAngle / -2f, transform.up) * transform.forward * viewDistance);

        if (verticiesAdv != null)
        {
            for (int i = 0; i < verticiesAdv.Length; i++)
            {
                Gizmos.DrawSphere(verticiesAdv[i], 0.1f);
            }
        }
    }
}
