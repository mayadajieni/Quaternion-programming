using UnityEngine;

public class EarthOrbit : MonoBehaviour
{
    [Header("太阳父物体")]
    public GameObject sunObj;
    [Header("太阳球体模型子物体（Sphere，公转中心）")]
    public GameObject sunSphere;
    [Header("公转速度，越小越慢")]
    public float orbitSpeed = 0.02f;
    [Header("地球自转速度")]
    public float earthRotateSpeed = 15f;
    [Header("公转轨迹线LineRenderer组件")]
    public LineRenderer orbitLine;

    private float orbitRadius;
    private float angle;

    void Start()
    {
        if (sunObj == null)
        {
            Debug.LogWarning("【错误】请给sunObj赋值！");
            enabled = false;
            return;
        }
        if (sunSphere == null)
        {
            Debug.LogWarning("【错误】请赋值sunSphere（太阳球体子物体！）");
            enabled = false;
            return;
        }
        Vector3 sunGeoCenter = sunSphere.transform.position;
        orbitRadius = Vector3.Distance(transform.position, sunGeoCenter);

        InitOrbitLine();
    }

    void Update()
    {
        if (sunObj == null || sunSphere == null) return;
        Vector3 sunGeoCenter = sunSphere.transform.position;

        // ========== 公转逻辑（四元数） ==========
        angle += orbitSpeed * Time.deltaTime;
        Quaternion orbitRot = Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0);
        Vector3 startDir = new Vector3(orbitRadius, 0, 0);
        Vector3 rotatedDir = orbitRot * startDir;
        transform.position = sunGeoCenter + rotatedDir;

        // ========== 地球自转（四元数） ==========
        Quaternion selfRot = Quaternion.Euler(0, earthRotateSpeed * Time.deltaTime, 0);
        transform.rotation *= selfRot;

        // ========== 更新轨迹线 ==========
        UpdateOrbitLine(sunGeoCenter);
    }

    #region 轨迹线相关
    void InitOrbitLine()
    {
        if (orbitLine == null) return;
        orbitLine.positionCount = 360;
        orbitLine.loop = true; // 闭合圆环
        orbitLine.startWidth = 0.1f;
        orbitLine.endWidth = 0.1f;
    }

    void UpdateOrbitLine(Vector3 center)
    {
        if (orbitLine == null) return;
        for (int i = 0; i < orbitLine.positionCount; i++)
        {
            float deg = i;
            Quaternion rot = Quaternion.Euler(0, deg, 0);
            Vector3 offset = rot * new Vector3(orbitRadius, 0, 0);
            orbitLine.SetPosition(i, center + offset);
        }
    }
    #endregion
}
