using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private float _rotateTileByAngle = -90.0f; //Unity rotates 2D sprites Counter Clockwise
    
    public NodeValues NodeValue;

    public void RotateTile(int numberOfRotations = 1)
    {
        transform.Rotate(Vector3.forward * _rotateTileByAngle * numberOfRotations);
        NodeValue.RotateNode(numberOfRotations);
    }

    [System.Serializable]
    public struct NodeValues
    {
        public bool Top, Right, Bottom, Left;
        
        public void RotateNode(int num)
        {
            for (int n = 0; n < num; n++)
            {
                bool auz = Left;
                Left = Bottom;
                Bottom = Right;
                Right = Top;
                Top = auz;
            }
        }

        public int CalculateNodeValue()
        {
            int sum = 0;
            if(Top) sum++;
            if(Right) sum++;
            if(Bottom) sum++;
            if(Left) sum++;
            return sum;
        }
    }
}
