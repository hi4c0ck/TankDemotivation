using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Utils
{
    class GameFieldGenerator
    {



        public static GameObject GenPlane(int widthMap, int heightMap, float cellSize)
        {
            return GenPlane(widthMap, heightMap, cellSize, new Material("ff"));
        }
        public static GameObject GenPlane(int widthMap, int heightMap, float cellSize, Material materialOfEarth)
        {

            int x = (int)(widthMap);
            int y = (int)(heightMap);
            List<Vector3> vertices = new List<Vector3>();

            List<int> triangles = new List<int>();
            List<Vector2> uv = new List<Vector2>();
            int stek = 0;

            float doubleTileX = 1f/widthMap;
            float doubleTileY = 1f / heightMap;
            for (int i = 0; i < x; i++)
            {
                float floorLevel = 0;
//                doubleTileY = 0;
                for (int j = 0; j < y; j++)
                {
                    vertices.AddRange(new Vector3[]
                    {
                    new Vector3(i-x/2f, floorLevel,0+j-y/2f)*cellSize,
                     new Vector3(i+1-x/2f,floorLevel,0+j-y/2f)*cellSize,
                     new Vector3(i-x/2f,floorLevel,1+j-y/2f)*cellSize,
                   new Vector3(i+1-x/2f,floorLevel,1+j-y/2f)*cellSize
                    });


                    triangles.AddRange(new int[]
                        {
                       stek+0,stek+2,stek+3,
                     stek+3,stek+1,stek+0
                        });
                    stek += 4;

                    uv.AddRange(new Vector2[]
                        {
                         new Vector2(i*doubleTileX,j*doubleTileY),
                      new Vector2((i+1)*doubleTileX,j*doubleTileY),
                       new Vector2(i*doubleTileX,(j+1)*doubleTileY),
                       new Vector2((i+1)*doubleTileX,(j+1)*doubleTileY)
                    });

   //                 if (doubleTileY > 0) { doubleTileY = 0; } else { doubleTileY++; }
                }

 //               if (doubleTileX > 0) { doubleTileX = 0; } else { doubleTileX++; }
            }
            /*
            for (int zzz = 0; zzz < vertices.Count; zzz++)
            {
                vertices[zzz] = new Vector3(vertices[zzz].x * 11, vertices[zzz].y, vertices[zzz].z * 7);
            }
            */
            //  GameObject newHex = (GameObject)Instantiate(spawnThis, new Vector3(0, -4, 0), Quaternion.identity);
            GameObject g = new GameObject();
            g.AddComponent<MeshFilter>();
            g.AddComponent<MeshRenderer>();
            g.AddComponent<MeshFilter>();
            MeshFilter meshFilter = g.GetComponent<MeshFilter>();
            MeshRenderer renderer = g.GetComponent<MeshRenderer>();

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            //add our triangles to the mesh
            mesh.triangles = triangles.ToArray();
            mesh.uv = uv.ToArray();

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();
            meshFilter.mesh = mesh;
            meshFilter.sharedMesh = mesh;
            //  collider.sharedMesh = mesh;
            renderer.sharedMaterial = materialOfEarth;
            return g;

        }


        public static GameObject GenOuterPlane(int widthMap, int heightMap, float cellSize, Material materialOfEarth, int outer_width)
        {
            

            int x, y, start_x, start_y;
            x = widthMap+2*outer_width;
            y = heightMap + 2 * outer_width;
            float doubleTileX = 1f / widthMap;
            float doubleTileY = 1f / heightMap;
            
            List<Vector3> vertices = new List<Vector3>();

            List<int> triangles = new List<int>();
            List<Vector2> uv = new List<Vector2>();
            int stek = 0;

            float floorLevel = 0;

            for (int i = 0; i < widthMap+ 2*outer_width; i++)
            {
                //                doubleTileY = 0;
                for (int j = 0; j < heightMap+ 2*outer_width; j++)
                    if ((j<outer_width || j>heightMap) ||
                            (i < outer_width || i >widthMap))
                {
                    vertices.AddRange(new Vector3[]
                    {
                    new Vector3(i-x/2f, floorLevel,0+j-y/2f)*cellSize,
                     new Vector3(i+1-x/2f,floorLevel,0+j-y/2f)*cellSize,
                     new Vector3(i-x/2f,floorLevel,1+j-y/2f)*cellSize,
                   new Vector3(i+1-x/2f,floorLevel,1+j-y/2f)*cellSize
                    });


                    triangles.AddRange(new int[]
                        {
                       stek+0,stek+2,stek+3,
                     stek+3,stek+1,stek+0
                        });
                    stek += 4;


                    uv.AddRange(new Vector2[]
                        {
                         new Vector2(i*doubleTileX,j*doubleTileY),
                      new Vector2((i+1)*doubleTileX,j*doubleTileY),
                       new Vector2(i*doubleTileX,(j+1)*doubleTileY),
                       new Vector2((i+1)*doubleTileX,(j+1)*doubleTileY)
                    });

                    //                 if (doubleTileY > 0) { doubleTileY = 0; } else { doubleTileY++; }
                }

                //               if (doubleTileX > 0) { doubleTileX = 0; } else { doubleTileX++; }
            }
            /*
            for (int zzz = 0; zzz < vertices.Count; zzz++)
            {
                vertices[zzz] = new Vector3(vertices[zzz].x * 11, vertices[zzz].y, vertices[zzz].z * 7);
            }
            */
            //  GameObject newHex = (GameObject)Instantiate(spawnThis, new Vector3(0, -4, 0), Quaternion.identity);
            GameObject g = new GameObject();
            g.AddComponent<MeshFilter>();
            g.AddComponent<MeshRenderer>();
            g.AddComponent<MeshFilter>();
            MeshFilter meshFilter = g.GetComponent<MeshFilter>();
            MeshRenderer renderer = g.GetComponent<MeshRenderer>();

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            //add our triangles to the mesh
            mesh.triangles = triangles.ToArray();
            mesh.uv = uv.ToArray();

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();
            meshFilter.mesh = mesh;
            meshFilter.sharedMesh = mesh;
            //  collider.sharedMesh = mesh;
            renderer.sharedMaterial = materialOfEarth;
            return g;

        }



        public static GameObject GenOuterStripe(int widthMap, int heightMap, float cellSize, Material materialOfEarth,Vector2 side)
        {
            widthMap += 2;
            heightMap += 2;

            int x, y,start_x,start_y;
            float doubleTileX = 1f / widthMap;
            float doubleTileY = 1f / heightMap;
            if (Mathf.Abs(side.x)== Mathf.Abs(side.y))
            {
                x = side.x == 1 ? (widthMap/2) : (int)(widthMap / 2+1);
                y = side.y == 1 ? heightMap : 1;
                start_x = x - 1;
                start_y = y - 1;
            }
            else
            {
                x = Mathf.Abs(side.x)==1?1:widthMap;
                y = Mathf.Abs(side.y) == 1 ? 1 : heightMap;
                start_x = 0;
                start_y = 0;
            }
            List<Vector3> vertices = new List<Vector3>();

            List<int> triangles = new List<int>();
            List<Vector2> uv = new List<Vector2>();
            int stek = 0;

            float floorLevel = 0;

            for (int i = start_x; i < x; i++)
            {
                //                doubleTileY = 0;
                for (int j = start_y; j < y; j++)
                {
                    vertices.AddRange(new Vector3[]
                    {
                    new Vector3(i-x/2f, floorLevel,0+j-y/2f)*cellSize,
                     new Vector3(i+1-x/2f,floorLevel,0+j-y/2f)*cellSize,
                     new Vector3(i-x/2f,floorLevel,1+j-y/2f)*cellSize,
                   new Vector3(i+1-x/2f,floorLevel,1+j-y/2f)*cellSize
                    });


                    triangles.AddRange(new int[]
                        {
                       stek+0,stek+2,stek+3,
                     stek+3,stek+1,stek+0
                        });
                    stek += 4;

                    int _i = side.x == 1 ? widthMap - 1 : i;
                    int _j = side.y == 1 ? heightMap - 1 : j;

                    uv.AddRange(new Vector2[]
                        {
                         new Vector2(_i*doubleTileX,_j*doubleTileY),
                      new Vector2((_i+1)*doubleTileX,_j*doubleTileY),
                       new Vector2(_i*doubleTileX,(_j+1)*doubleTileY),
                       new Vector2((_i+1)*doubleTileX,(_j+1)*doubleTileY)
                    });

                    //                 if (doubleTileY > 0) { doubleTileY = 0; } else { doubleTileY++; }
                }

                //               if (doubleTileX > 0) { doubleTileX = 0; } else { doubleTileX++; }
            }
            /*
            for (int zzz = 0; zzz < vertices.Count; zzz++)
            {
                vertices[zzz] = new Vector3(vertices[zzz].x * 11, vertices[zzz].y, vertices[zzz].z * 7);
            }
            */
            //  GameObject newHex = (GameObject)Instantiate(spawnThis, new Vector3(0, -4, 0), Quaternion.identity);
            GameObject g = new GameObject();
            g.AddComponent<MeshFilter>();
            g.AddComponent<MeshRenderer>();
            g.AddComponent<MeshFilter>();
            MeshFilter meshFilter = g.GetComponent<MeshFilter>();
            MeshRenderer renderer = g.GetComponent<MeshRenderer>();

            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            //add our triangles to the mesh
            mesh.triangles = triangles.ToArray();
            mesh.uv = uv.ToArray();

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();
            meshFilter.mesh = mesh;
            meshFilter.sharedMesh = mesh;
            //  collider.sharedMesh = mesh;
            renderer.sharedMaterial = materialOfEarth;
            return g;

        }
    }
}
