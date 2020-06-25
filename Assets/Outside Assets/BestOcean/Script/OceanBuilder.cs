

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class OceanBuilder
{
    public static void GenerateMesh(Ocean ocean, float baseVertDensity, int lodCount)
    {
        int oceanLayer = LayerMask.NameToLayer(ocean.LayerName);
        Mesh[] meshInsts = new Mesh[4];
        for (int i = 0; i < 4; i++)
        {
            meshInsts[i] = BuildOceanPatch(i, baseVertDensity);
        }

        ocean._lods = new LodTransform[lodCount];
        
        ocean._lodDataAnimWaves = LodDataMgr.Create<LodDataMgrAnimWaves, SimSettingsAnimatedWaves>(ocean.gameObject, ref ocean._simSettingsAnimatedWaves);
        
        if (ocean._createFoamSim)
        {
            ocean._lodDataFoam = LodDataMgr.Create<LodDataMgrFoam, SimSettingsFoam>(ocean.gameObject, ref ocean._simSettingsFoam);
        }

        if (ocean._createSeaFloorDepthData)
        {
            ocean._lodDataSeaDepths = ocean.gameObject.AddComponent<LodDataMgrSeaFloorDepth>();
        }
        

        
        // Remove existing LODs
        for (int i = 0; i < ocean.transform.childCount; i++)
        {
            var child = ocean.transform.GetChild(i);
            if (child.name.StartsWith("LOD"))
            {
                child.parent = null;
                Object.Destroy(child.gameObject);
                i--;
            }
        }

        int startLevel = 0;
        for (int i = 0; i < lodCount; i++)
        {
            bool biggestLOD = i == lodCount - 1;
            GameObject nextLod = CreateLOD(ocean, i, lodCount, biggestLOD, meshInsts, baseVertDensity, oceanLayer);
            nextLod.transform.parent = ocean.transform;

            // scale only horizontally, otherwise culling bounding box will be scaled up in y
            float horizScale = Mathf.Pow(2f, (float)(i + startLevel));
            nextLod.transform.localScale = new Vector3(horizScale, 1f, horizScale);
        }
    }

    static Mesh BuildOceanPatch(int index, float baseVertDensity)
    {
        ArrayList verts = new ArrayList();
        ArrayList indices = new ArrayList();

        float dx = 1f / baseVertDensity;

        float skirtXminus = 1f, skirtXplus = 1f;
        float skirtZminus = 1f, skirtZplus = 1f;

        float sideLength_verts_x = 1f + baseVertDensity + skirtXminus + skirtXplus;
        float sideLength_verts_z = 1f + baseVertDensity + skirtZminus + skirtZplus;

        float start_x = -0.5f - skirtXminus * dx;
        float start_z = -0.5f - skirtZminus * dx;
        float end_x = 0.5f + skirtXplus * dx;
        float end_z = 0.5f + skirtZplus * dx;
        float times = 10f;
        for (float j = 0; j < sideLength_verts_z; j++)
        {
            float z = Mathf.Lerp(start_z, end_z, j / (sideLength_verts_z - 1f));

            if ((index == 1 || index == 2)&& j == sideLength_verts_z - 1f)
                z *= times;

            for (float i = 0; i < sideLength_verts_x; i++)
            {
                float x = Mathf.Lerp(start_x, end_x, i / (sideLength_verts_x - 1f));

                if (i == sideLength_verts_x - 1f && (index == 1 || index == 3))
                    x *= times;

                verts.Add(new Vector3(x, 0f, z));
            }
        }


        int sideLength_squares_x = (int)sideLength_verts_x - 1;
        int sideLength_squares_z = (int)sideLength_verts_z - 1;

        for (int j = 0; j < sideLength_squares_z; j++)
        {
            for (int i = 0; i < sideLength_squares_x; i++)
            {
                bool flipEdge = false;

                if (i % 2 == 1) flipEdge = !flipEdge;
                if (j % 2 == 1) flipEdge = !flipEdge;

                int i0 = i + j * (sideLength_squares_x + 1);
                int i1 = i0 + 1;
                int i2 = i0 + (sideLength_squares_x + 1);
                int i3 = i2 + 1;

                if (!flipEdge)
                {
                    // tri 1
                    indices.Add(i3);
                    indices.Add(i1);
                    indices.Add(i0);

                    // tri 2
                    indices.Add(i0);
                    indices.Add(i2);
                    indices.Add(i3);
                }
                else
                {
                    // tri 1
                    indices.Add(i3);
                    indices.Add(i1);
                    indices.Add(i2);

                    // tri 2
                    indices.Add(i0);
                    indices.Add(i2);
                    indices.Add(i1);
                }
            }
        }

        Mesh mesh = new Mesh();
        if (verts != null && verts.Count > 0)
        {
            Vector3[] arrV = new Vector3[verts.Count];
            verts.CopyTo(arrV);

            int[] arrI = new int[indices.Count];
            indices.CopyTo(arrI);

            mesh.SetIndices(null, MeshTopology.Triangles, 0);
            mesh.vertices = arrV;
            mesh.normals = null;
            mesh.SetIndices(arrI, MeshTopology.Triangles, 0);

            mesh.RecalculateBounds();
            Bounds bounds = mesh.bounds;
            bounds.extents = new Vector3(bounds.extents.x + dx, 100f, bounds.extents.z + dx);
            mesh.bounds = bounds;
            mesh.name = index.ToString();
        }
        return mesh;
    }

    static GameObject CreateLOD(Ocean ocean, int lodIndex, int lodCount, bool biggestLOD, Mesh[] meshData, float baseVertDensity, int oceanLayer)
    {
        GameObject parent = new GameObject();
        parent.name = "LOD" + lodIndex;
        parent.layer = oceanLayer;
        parent.transform.parent = ocean.transform;
        parent.transform.localPosition = Vector3.zero;
        parent.transform.localRotation = Quaternion.identity;

        ocean._lods[lodIndex] = parent.AddComponent<LodTransform>();
        ocean._lods[lodIndex].InitLODData(lodIndex, lodCount);
        Vector2[] offsets;
        int[] indexs = null;
        float[] angles = null;
        if (lodIndex != 0)
        {
            offsets = new Vector2[] {
                    new Vector2(-1.5f,1.5f),    new Vector2(-0.5f,1.5f),    new Vector2(0.5f,1.5f),     new Vector2(1.5f,1.5f),
                    new Vector2(-1.5f,0.5f),                                                            new Vector2(1.5f,0.5f),
                    new Vector2(-1.5f,-0.5f),                                                           new Vector2(1.5f,-0.5f),
                    new Vector2(-1.5f,-1.5f),   new Vector2(-0.5f,-1.5f),   new Vector2(0.5f,-1.5f),    new Vector2(1.5f,-1.5f),
                };
            indexs = new int[] {
                1, 2, 2, 1,
                3,       3,
                3,       3,
                1, 2, 2, 1,
            };

            angles = new float[] {
                -90f, 0f, 0f, 0f,
                180f,             0f,
                180f,             0f,
                180f, 180f, 180f, 90f,
            };

        }
        else
        {
            offsets = new Vector2[] {
                    new Vector2(-1.5f,1.5f),    new Vector2(-0.5f,1.5f),    new Vector2(0.5f,1.5f),     new Vector2(1.5f,1.5f),
                    new Vector2(-1.5f,0.5f),    new Vector2(-0.5f,0.5f),    new Vector2(0.5f,0.5f),     new Vector2(1.5f,0.5f),
                    new Vector2(-1.5f,-0.5f),   new Vector2(-0.5f,-0.5f),   new Vector2(0.5f,-0.5f),    new Vector2(1.5f,-0.5f),
                    new Vector2(-1.5f,-1.5f),   new Vector2(-0.5f,-1.5f),   new Vector2(0.5f,-1.5f),    new Vector2(1.5f,-1.5f),
                };
        }


        // create the ocean patches
        for (int i = 0; i < offsets.Length; i++)
        {
            // instantiate and place patch
            var patch = new GameObject(string.Format("Tile_L{0}", lodIndex));
            patch.layer = oceanLayer;
            patch.transform.parent = parent.transform;
            Vector2 pos = offsets[i];
            patch.transform.localPosition = new Vector3(pos.x, 0f, pos.y);
            patch.transform.localScale = Vector3.one;

            patch.AddComponent<OceanChunkRenderer>().SetInstanceData(lodIndex, lodCount, baseVertDensity);
            int index = 0;
            Quaternion r = Quaternion.identity;
            if(biggestLOD)
            {
                index = indexs[i];
                r = Quaternion.Euler(0f, angles[i], 0f);
                patch.transform.localRotation = r;
            }
            patch.AddComponent<MeshFilter>().mesh = meshData[index];

            var mr = patch.AddComponent<MeshRenderer>();
           
            mr.sortingOrder = -lodCount + lodIndex;
            if (lodIndex == 0)
            {
                mr.sortingOrder--;
            }
            mr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mr.receiveShadows = false; 
            mr.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
            mr.material = ocean.OceanMaterial;
        }

        return parent;
    }
}
