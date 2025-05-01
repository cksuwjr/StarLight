using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    public PuzzleSlot puzzleSlot;

    public void Init(PuzzleSlot puzzleSlot)
    {
        this.puzzleSlot = puzzleSlot;
    }
}
