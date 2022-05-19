using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerController : MonoBehaviour
{
    const int CIRCLE_DEGREE = 360;
    public int Radius;
    public Piece[] Pieces;
    void Start()
    {
        int angle = CIRCLE_DEGREE / Pieces.Length;
        GameObject piece;
        for (int i = 0; i < Pieces.Length; i++)
        {
            piece = new GameObject();
            piece.name = "Piece " + i;
            piece.transform.position = transform.position;
            piece.transform.rotation = Quaternion.identity;
            piece.transform.SetParent(transform, true);
            Pieces[i].PieceRadius = Radius;
            Pieces[i].PieceAngle = angle;
            piece.AddComponent<PieceRenderer>().ItemPiece = Pieces[i];
            piece.transform.Rotate(new Vector3(0, 0, angle * i));
        }
    }

}
