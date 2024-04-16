using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    public List<GroundMover> moversList=new();

    public Vector2 moveDirection;
    public float moverSpeed;


    
    
    private void Awake()
    {


        foreach (GroundMover mover in moversList) 
        {

           // mover.moveSpeed=moverSpeed;
            //mover.moveDir=moveDirection;
        
   
        }

       
    }
    public void UpdateGroundSpeed() 
    {
        foreach (GroundMover mover in moversList)
        {
           // mover.moveSpeed = moverSpeed;

        }
    }


}
