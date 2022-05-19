using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnerController : MonoBehaviour
{
    const int CIRCLE_DEGREE = 360;
    public int Radius;
    public bool Spin;
    public float Speed;
    public int MinTimes;
    public int MaxTiems;
    public AnimationCurve SpeedCurve;
    public int Index;
    public Sprite Background;
    public Sprite SpinButton;
    public Sprite Pointer;
    public float PointerOffset;
    

    public Piece[] Pieces;

    private GameObject pieceContainer;

    private GameObject spinButtonGO;
    private SpriteRenderer spinButtonSpriteRenderer;

    private GameObject pointerGO;
    private SpriteRenderer pointerSpriteRenderer;

    private SpriteRenderer spriteRenderer;

    private AudioSource audioSource;

    private bool isSpin;
    private float currentDegree;
    private float targetDegree;
    public float time;
    void Start()
    {
        pieceContainer = new GameObject();
        pieceContainer.name = "Pieces";
        pieceContainer.transform.SetParent(transform, true);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = -2;

        spinButtonGO = new GameObject();
        spinButtonGO.name = "Spin Button";
        spinButtonGO.transform.SetParent(transform, true);
        spinButtonSpriteRenderer = spinButtonGO.AddComponent<SpriteRenderer>();
        spinButtonSpriteRenderer.sprite = SpinButton;
        spinButtonSpriteRenderer.sortingOrder = 1;

        pointerGO = new GameObject();
        pointerGO.name = "Pointer";
        pointerGO.transform.Translate(new Vector3(0, PointerOffset, 0), Space.Self);
        pointerGO.transform.SetParent(transform, true);
        pointerSpriteRenderer = pointerGO.AddComponent<SpriteRenderer>();
        pointerSpriteRenderer.sprite = Pointer;
        pointerSpriteRenderer.sortingOrder = 1;

        audioSource = GetComponent<AudioSource>();

        isSpin = false;
        int angle = CIRCLE_DEGREE / Pieces.Length;
        GameObject piece;
        for (int i = 0; i < Pieces.Length; i++)
        {
            piece = new GameObject();
            piece.name = "Piece " + i;
            piece.transform.position = transform.position;
            piece.transform.rotation = Quaternion.identity;
            piece.transform.SetParent(pieceContainer.transform, true);
            Pieces[i].PieceRadius = Radius;
            Pieces[i].PieceAngle = angle;
            piece.AddComponent<PieceRenderer>().ItemPiece = Pieces[i];
            piece.transform.Rotate(new Vector3(0, 0, angle * i));
        }
    }
    void Update()
    {
        spriteRenderer.sprite = Background;

        if (Spin)
        {
            Spin = false;
            isSpin = true;
            transform.Rotate(Vector3.zero, Space.Self);
            targetDegree = CIRCLE_DEGREE * Random.Range(MinTimes, MaxTiems) + ((CIRCLE_DEGREE / Pieces.Length)) * (Index + 1) - ((CIRCLE_DEGREE / Pieces.Length) / 2);
            currentDegree = 0;
            audioSource.Play();
            audioSource.loop = true;
        }
        if (isSpin)
        {
            pieceContainer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, currentDegree));
            currentDegree += Time.deltaTime * Speed + Speed * SpeedCurve.Evaluate(currentDegree / targetDegree);
            if (currentDegree >= targetDegree)
            {
                isSpin = false;
                audioSource.Stop();
            }
        }
    }
}

