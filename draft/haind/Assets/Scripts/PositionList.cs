using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PositionList
{
    //Cái class này sẽ chốt luôn những toạ độ đi trong map k fix gì hết
    public readonly SortedList<int, Vector3> List = new SortedList<int, Vector3>();
    //List di binh thương
    // tạo list 2 cho quân đỏ,...list 3....
    public PositionList()
    {
        List.Add(0, new Vector3(2.97f, 2.9f));
        List.Add(1, new Vector3(2.37f, 2.71f));
        List.Add(2, new Vector3(1.79f, 2.5f));
        List.Add(3, new Vector3(1.21f, 2.29f));
        List.Add(4, new Vector3(0.63f, 2.1f));
        List.Add(5, new Vector3(0.05f, 1.89f));
        List.Add(6, new Vector3(-0.74f, 2.08f));
        List.Add(7, new Vector3(-1.37f, 2.3f));
        List.Add(8, new Vector3(-2.01f, 2.53f));
        List.Add(9, new Vector3(-2.6f, 2.68f));
        List.Add(10, new Vector3(-3.31f, 2.87f));
        List.Add(11, new Vector3(-4.53f, 2.73f));
        List.Add(12, new Vector3(-4.96f, 2.37f));
        List.Add(13, new Vector3(-4.39f, 2.16f));
        List.Add(14, new Vector3(-3.7f, 1.89f));
        List.Add(15, new Vector3(-3.11f, 1.66f));
        List.Add(16, new Vector3(-2.44f, 1.41f));
        List.Add(17, new Vector3(-1.96f, 1.12f));
        List.Add(18, new Vector3(-2.48f, 0.78f));
        List.Add(19, new Vector3(-3.09f, 0.44f));
        List.Add(20, new Vector3(-3.7f, 0.13f));
        List.Add(21, new Vector3(-4.31f, -0.18f));
        List.Add(22, new Vector3(-4.92f, -0.52f));
        List.Add(23, new Vector3(-4.63f, -1.15f));
        List.Add(24, new Vector3(-3.52f, -1.34f));
        List.Add(25, new Vector3(-2.85f, -1f));
        List.Add(26, new Vector3(-2.16f, -0.6f));
        List.Add(27, new Vector3(-1.55f, -0.31f));
        List.Add(28, new Vector3(-0.92f, 0.05f));
        List.Add(29, new Vector3(-0.18f, 0.24f));
        List.Add(30, new Vector3(0.51f, 0.01f));
        List.Add(31, new Vector3(1.14f, -0.28f));
        List.Add(32, new Vector3(1.81f, -0.66f));
        List.Add(33, new Vector3(2.4f, -1.02f));
        List.Add(34, new Vector3(3.11f, -1.37f));
        List.Add(35, new Vector3(4.6f, -1.2f));
        List.Add(36, new Vector3(4.78f, -0.53f));
        List.Add(37, new Vector3(4.09f, -0.15f));
        List.Add(38, new Vector3(3.51f, 0.11f));
        List.Add(39, new Vector3(2.91f, 0.5f));
        List.Add(40, new Vector3(2.26f, 0.75f));
        List.Add(41, new Vector3(1.66f, 1.08f));
        List.Add(42, new Vector3(2.24f, 1.32f));
        List.Add(43, new Vector3(2.77f, 1.57f));
        List.Add(44, new Vector3(3.37f, 1.88f));
        List.Add(45, new Vector3(3.96f, 2.06f));
        List.Add(46, new Vector3(4.49f, 2.24f));
        List.Add(47, new Vector3(4.17f, 2.7f));
        List.Add(101, new Vector3(3.65f, -0.8f));
        List.Add(102, new Vector3(3.18f, -0.49f));
        List.Add(103, new Vector3(2.7f, -0.2f));
        List.Add(104, new Vector3(2.25f, -0.08f));
        List.Add(105, new Vector3(1.72f, 0.43f));
        List.Add(106, new Vector3(1.24f, 0.68f));

    }
    }

// Update is called once per frame


