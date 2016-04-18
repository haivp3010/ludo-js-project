using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedKnight : MonoBehaviour
{
    public Animator anim;
    private GameObject clicked;
    public int distance;
    private static int i=30;
    static PositionList PL = new PositionList();
    public SortedList<int, Vector3> positions = PL.List;
    public float speed;
    public static Dice dice1=new Dice();
    private int iClick = i + dice1.Number; 
    //private SortedList SL = PositionList;
    // Use this for initialization
    private void Start()
    {

        Debug.Log("Game Start!");
        anim = GetComponent<Animator>();
        anim.enabled = false;
        Debug.Log(gameObject.ToString());
        //PositionList.
        // Setting up coordinates
        #region PositionList cũ
        /*
        positions = new Vector3[48];
        positions[0] = new Vector3(2.97f, 2.9f);
        positions[1] = new Vector3(2.37f, 2.71f);
        positions[2] = new Vector3(1.79f, 2.5f);
        positions[3] = new Vector3(1.21f, 2.29f);
        positions[4] = new Vector3(0.63f, 2.1f);
        positions[5] = new Vector3(0.05f, 1.89f);
        positions[6] = new Vector3(-0.74f, 2.08f);
        positions[7] = new Vector3(-1.37f, 2.3f);
        positions[8] = new Vector3(-2.01f, 2.53f);
        positions[9] = new Vector3(-2.6f, 2.68f);
        positions[10] = new Vector3(-3.31f, 2.87f);
        positions[11] = new Vector3(-4.53f, 2.73f);
        positions[12] = new Vector3(-4.96f, 2.37f);
        positions[13] = new Vector3(-4.39f, 2.16f);
        positions[14] = new Vector3(-3.7f, 1.89f);
        positions[15] = new Vector3(-3.11f, 1.66f);
        positions[16] = new Vector3(-2.44f, 1.41f);
        positions[17] = new Vector3(-1.96f, 1.12f);
        positions[18] = new Vector3(-2.48f, 0.78f);
        positions[19] = new Vector3(-3.09f, 0.44f);
        positions[20] = new Vector3(-3.7f, 0.13f);
        positions[21] = new Vector3(-4.31f, -0.18f);
        positions[22] = new Vector3(-4.92f, -0.52f);
        positions[23] = new Vector3(-4.63f, -1.15f);
        positions[24] = new Vector3(-3.52f, -1.34f);
        positions[25] = new Vector3(-2.85f, -1f);
        positions[26] = new Vector3(-2.16f, -0.6f);
        positions[27] = new Vector3(-1.55f, -0.31f);
        positions[28] = new Vector3(-0.92f, 0.05f);
        positions[29] = new Vector3(-0.18f, 0.24f);
        positions[30] = new Vector3(0.51f, 0.01f);
        positions[31] = new Vector3(1.14f, -0.28f);
        positions[32] = new Vector3(1.81f, -0.66f);
        positions[33] = new Vector3(2.4f, -1.02f);
        positions[34] = new Vector3(3.11f, -1.37f);
        positions[35] = new Vector3(4.6f, -1.2f);
        positions[36] = new Vector3(4.78f, -0.53f);
        positions[37] = new Vector3(4.09f, -0.15f);
        positions[38] = new Vector3(3.51f, 0.11f);
        positions[39] = new Vector3(2.91f, 0.5f);
        positions[40] = new Vector3(2.26f, 0.75f);
        positions[41] = new Vector3(1.66f, 1.08f);
        positions[42] = new Vector3(2.24f, 1.32f);
        positions[43] = new Vector3(2.77f, 1.57f);
        positions[44] = new Vector3(3.37f, 1.88f);
        positions[45] = new Vector3(3.96f, 2.06f);
        positions[46] = new Vector3(4.49f, 2.24f);
        positions[47] = new Vector3(4.17f, 2.7f);
        */
        #endregion
        
        //PositionList.List[i]=new Vector3();
    }

    // Update is called once per frame
    private void Update()
    {
        var step = speed * Time.deltaTime;

        if (clicked != null)
        {
            anim.enabled = true;
            clicked.transform.position = Vector3.MoveTowards(transform.position, positions[i], step);
            
            if (clicked.transform.position == positions[i])
            {
                i++;
                Debug.Log("IClick: "+iClick);
                if (i > iClick)
                {
                    Debug.Log(iClick);
                    clicked = null;
                    
                    anim.enabled = false;
                }
                if (i == 48)
                {
                    i = 0;
                    iClick -= 48;
                }
                if(i==36)
                {
                    i = 101;
                    iClick += 65;
                }


            }
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Sprite Clicked!");
        if (clicked == null)
            Debug.Log("null");
        clicked = gameObject;
        Debug.Log(clicked.ToString());
    }

    private void OnMouseEnter()
    {
        anim.enabled = true;

        Debug.Log("MouseEnter");
    }

    private void OnMouseExit()
    {
        anim.enabled = false;
        Debug.Log("MouseExit");
    }
}