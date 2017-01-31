using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Maze
{
    [Serializable]
    public class MazeHolder
    {
        MazeObject[,] maze;
        public MazeObject[,] Mazes { get { return maze; } }
        public int DEF_BORDER = 4;
        public int REC_BORDER = 3;
        public float obj_density = 0.05f;
        public float cell_size = 1f;
        public MazeHolder(int w, int h)
        {
            IniMaze(w, h, obj_density);
        }
        bool OutOfBounds(float x, float y)
        {
            return !(x > 0 && x < (maze.GetLength(0) - 1) * cell_size
                && y > 0 && y < (maze.GetLength(1) - 1) * cell_size);
        }
        public void CalculateToPointDirection(Vector3 position, Vector3 aim_position, ref Vector3 direction)
        {
            position.y = 0;
            aim_position.y = 0;

            direction = aim_position - position;
            direction.Normalize();
            int[] aim_vector = new int[2];
            aim_vector[0] = (int)(aim_position.x - currentCenter.x)+maze.GetLength(0)/2;
            aim_vector[1] = (int)(aim_position.z - currentCenter.y) + maze.GetLength(1)/2;
            int[] start_vector = new int[2];
            start_vector[0] = (int)(position.x - currentCenter.x)+maze.GetLength(0)/2;
            start_vector[1] = (int)(position.z - currentCenter.y) + maze.GetLength(1)/2;
            if (OutOfBounds(start_vector[0]*cell_size, start_vector[1] * cell_size))
            {
                return;
            }

            int[] current_vector = new int[2];
            current_vector[0] = start_vector[0];
            current_vector[1] = start_vector[1];
            int[] tmp_mv_vector = new int[2];
            int[] current_maze_wp_vector = new int[2];
            int it_level = 0;
            int max_it_level = 20;
            int change_directions_count = 0;
            Vector3 tmp_direction = direction;
            do
            {
                calc_fw_vector(tmp_direction, ref tmp_mv_vector);
                current_maze_wp_vector[0] = current_vector[0] + tmp_mv_vector[0];
                current_maze_wp_vector[1] = current_vector[1] + tmp_mv_vector[1];
                if (!(OutOfBounds(current_maze_wp_vector[0]*cell_size, current_maze_wp_vector[1] * cell_size))
                
                    && (maze[current_maze_wp_vector[0], current_maze_wp_vector[1]].type == MazeObjectType.Empty))
                    {
                        it_level++;
                        sumInt(ref current_vector, tmp_mv_vector);
//                    change_directions_count = 0;
                    tmp_direction.x = aim_vector[0]-current_vector[0];
                    tmp_direction.z = aim_vector[1]-current_vector[1];
                    tmp_direction.Normalize();
                }
                
                else
                {
                    if (change_directions_count < 3)
                    {
                        change_directions_count++;
                    }
                    else
                    {
                        it_level--;
                        change_directions_count = 0;
                    }


                    if (it_level > 0)
                    {
                        tmp_direction = Quaternion.Euler(0, 90f * change_directions_count, 0) * direction;

                    }
                    else
                    {
                        direction = Quaternion.Euler(0, 90f * change_directions_count, 0) * direction;
                        direction.Normalize();
                        tmp_direction = direction;
                    }

                }
            } while (distance_to(current_vector, aim_vector) > 1 && it_level >= 0 && it_level <= max_it_level);
            direction.Normalize();
        }
        void sumInt(ref int[] what, int[] to_sum)
        {
            what[0] += to_sum[0];
            what[1] += to_sum[1];
        }
        int distance_to(int[] from, int[] to)
        {
            return Mathf.Abs(from[0] - to[0] + from[1] - to[1]);
        }
        void calc_fw_vector(Vector3 dir,ref int[] mv)
        {
            mv[0] = Mathf.RoundToInt(dir.x);
            mv[1] = Mathf.RoundToInt(dir.z);
            //mv[0] =(int)Mathf.Sign(dir.x)*((Mathf.Abs(dir.x)> Mathf.Abs(dir.z)) ? 1 : 0);
            //mv[1] = (int)Mathf.Sign(dir.z) * ((Mathf.Abs(dir.x) < Mathf.Abs(dir.z)) ? 1 : 0);
        }

        Vector2 currentCenter = Vector2.zero;
        void IniMaze(int h, int w,float dens)
        {
            maze = new MazeObject[w + DEF_BORDER * 2, h + DEF_BORDER * 2];
            float h_x = maze.GetLength(0) / 2f;
            float h_y = maze.GetLength(1) / 2f;
            for (int i=0;i<maze.GetLength(0);i++)
                for (int j=0;j< maze.GetLength(1); j++)
                {
                    maze[i, j] = new MazeObject
                    {
                        position = new Vector3(i - h_x + .5f, 0, j - h_y + .5f) * cell_size,
                        scales = new Vector3(1, UnityEngine.Random.Range(0.1f, 2f), 1) * cell_size,
                        type = (UnityEngine.Random.Range(0f, 1f) < dens ? MazeObjectType.Block : MazeObjectType.Empty),
                        is_Alive = true
                    };
                }
        }
        void PopulateMaze(int i, int j, float h_x, float h_y)
        {
            maze[i, j].position.x = (i + h_x+.5f )*cell_size;
            maze[i, j].position.z =( j + h_y+.5f)*cell_size;
            maze[i, j].scales.x = 1f*cell_size;
            maze[i, j].scales.y = UnityEngine.Random.Range(0.1f, 2f) * cell_size;
            maze[i, j].scales.z = cell_size;
            maze[i, j].type = (UnityEngine.Random.Range(0f, 1f) < obj_density? MazeObjectType.Block : MazeObjectType.Empty);
            maze[i, j].is_Alive = false;
        }

        void CopyMaze(int new_i, int new_j, int i,int j)
        {
            maze[new_i, new_j].position = maze[i,j].position;
            maze[new_i, new_j].scales = maze[i, j].scales;
            maze[new_i, new_j].type = maze[i, j].type; 
            maze[new_i, new_j].is_Alive = maze[i, j].is_Alive;
        }

        Vector2 tmpV=Vector2.zero;
        int[] offsets=new int[2];
        public void UpdateMaze(Vector3 newCenter)
        {
            tmpV.x = newCenter.x;
            tmpV.y = newCenter.z;
            if ((tmpV - currentCenter).magnitude < 3)
                return;
            offsets[0] = (int)Mathf.Ceil(tmpV.x - currentCenter.x);
            offsets[1] = (int)Mathf.Ceil(tmpV.y - currentCenter.y);
            currentCenter.x += offsets[0];
            currentCenter.y += offsets[1];
            int x_dim = maze.GetLength(0);
            int y_dim = maze.GetLength(1);

            if (offsets[0] != 0)
                for (int i = 0; i < x_dim - Mathf.Abs(offsets[0]); i++)
                    for (int j = 0; j < y_dim; j++)
                    {

                        if (offsets[0] > 0)
                            CopyMaze(i, j, i + offsets[0], j);
                        if (offsets[0] < 0)
                            CopyMaze(x_dim - 1 - i, j, x_dim - 1 + offsets[0], j);
                    }

            if (offsets[1] != 0)
                for (int j = 0; j < y_dim - Mathf.Abs(offsets[1]); j++)
                    for (int i = 0; i < y_dim; i++)
                    {

                        if (offsets[1] > 0)
                            CopyMaze(i, j, i, j + offsets[1]);
                        if (offsets[1] < 0)
                            CopyMaze(i, y_dim - 1 - j, i, y_dim - 1 + offsets[1]);
                    }


            if (offsets[0] != 0)
                for (int i = 0; i < Mathf.Abs(offsets[0]); i++)
                    for (int j = 0; j < maze.GetLength(1); j++)
                    {
                        if (offsets[0] < 0)
                            PopulateMaze(i, j, currentCenter.x - x_dim / 2f, currentCenter.y - y_dim / 2f);
                        if (offsets[0] > 0)
                            PopulateMaze(x_dim - 1 - i, j, currentCenter.x - x_dim / 2f, currentCenter.y - y_dim / 2f);
                    }

            if (offsets[1] != 0)
                for (int j = 0; j < Mathf.Abs(offsets[1]); j++)
                    for (int i = 0; i < maze.GetLength(0); i++)
                    {
                        if (offsets[1] < 0)
                            PopulateMaze(i, j, currentCenter.x - x_dim / 2f, currentCenter.y - y_dim / 2f);
                        if (offsets[1] > 0)
                            PopulateMaze(i, y_dim - 1 - j, currentCenter.x - x_dim / 2f, currentCenter.y - y_dim / 2f);
                    }

        }

    }
}
