using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Random = UnityEngine.Random;

public class PuzzleManager : MonoBehaviour
{
    private Dictionary<PuzzleSlot, Puzzle> correct = new Dictionary<PuzzleSlot, Puzzle>();
    private Dictionary<PuzzleSlot, Puzzle> slots = new Dictionary<PuzzleSlot, Puzzle>();

    private PuzzleSlot nowPuzzleSlot;
    private Puzzle selectedPuzzleSlice;

    private GameObject player;

    public UnityEvent OnFaze1End;
    public UnityEvent OnFaze2End;

    private Action action;

    private GameObject puzzle1;
    private GameObject puzzle2;

    private void Awake()
    {
        puzzle1 = GameObject.Find("Puzzle");
        for(int i = 0; i < puzzle1.transform.childCount; i++)
        {
            if (puzzle1.transform.GetChild(i).TryGetComponent<PuzzleSlot>(out var puzzleSlot))
            {
                puzzleSlot.Init(this);

                correct.Add(puzzleSlot, puzzleSlot.puzzle);
                slots.Add(puzzleSlot, puzzleSlot.puzzle);
            }
        }

        puzzle2 = GameObject.Find("Puzzle2");
        puzzle2.SetActive(false);
    }

    private void Start()
    {
        Invoke("MixPuzzle", 1f);
    }

    private IEnumerator PuzzleFaze2()
    {
        puzzle1.SetActive(false);
        puzzle2.SetActive(true);

        for (int i = 0; i < puzzle2.transform.childCount; i++)
        {
            if (puzzle2.transform.GetChild(i).TryGetComponent<PuzzleSlot>(out var puzzleSlot))
            {
                puzzleSlot.Init(this);

                correct.Add(puzzleSlot, puzzleSlot.puzzle);
                slots.Add(puzzleSlot, puzzleSlot.puzzle);
            }
        }

        yield return null;
        action = Puzzle2;
        Invoke("MixPuzzle", 3f);
    }


    private void Update()
    {
        if (!selectedPuzzleSlice) return;

        selectedPuzzleSlice.transform.position = player.transform.position;
    }

    public void Current(PuzzleSlot slot)
    {
        nowPuzzleSlot = slot;
    }

    public void CurrentOut(PuzzleSlot slot)
    {
        if (slot == nowPuzzleSlot)
            nowPuzzleSlot = null;
    }

    public void Select()
    {
        if (selectedPuzzleSlice)
            PutDownPuzzle(nowPuzzleSlot);
        else
            PutUpPuzzle(nowPuzzleSlot);
    }

    public void PutUpPuzzle(PuzzleSlot slot)
    {
        if (!slot) return;

        if (slot.puzzle)
        {
            selectedPuzzleSlice = slot.puzzle;
            selectedPuzzleSlice.GetComponent<SpriteRenderer>().sortingOrder = 1;

            PutPuzzleSlice(slot, null);
        }
    }

    public void PutDownPuzzle(PuzzleSlot slot)
    {
        if (!slot) return;

        var nowPuzzle = selectedPuzzleSlice;
        selectedPuzzleSlice.GetComponent<SpriteRenderer>().sortingOrder = 0;

        //if (selectedPuzzleSlice.transform.GetChild(0).TryGetComponent<OutLineEffecter>(out var lineEffecter))
        //    lineEffecter.SetEffect(false);
        selectedPuzzleSlice = null;

        PutUpPuzzle(slot);

        PutPuzzleSlice(slot, nowPuzzle);

        //slot.puzzle = nowPuzzle;
        //nowPuzzle.transform.SetParent(slot.transform);
        //nowPuzzle.transform.localPosition = Vector3.zero;

        //slots[slot] = nowPuzzle;
    }

    private void MixPuzzle()
    {
        List<PuzzleSlot> puzzleSlots = new List<PuzzleSlot>();
        List<Puzzle> puzzleSlices = new List<Puzzle>();
        Puzzle puzzleSlice;
        foreach(var key in slots.Keys)
        {
            slots.TryGetValue(key, out puzzleSlice);
            puzzleSlots.Add(key);
            puzzleSlices.Add(puzzleSlice);
        }

        for(int i = 0; i < puzzleSlots.Count; i++)
        {
            puzzleSlice = puzzleSlices[Random.Range(0, puzzleSlices.Count)];
            puzzleSlices.Remove(puzzleSlice);

            slots[puzzleSlots[i]] = puzzleSlice;
        }

        ArrangePuzzle();
    }

    private void ArrangePuzzle()
    {
        List<PuzzleSlot> puzzleSlots = new List<PuzzleSlot>();

        Puzzle puzzleSlice;
        foreach (var key in slots.Keys)
        {
            puzzleSlots.Add(key);
        }

        for (int i = 0; i < puzzleSlots.Count; i++)
        {
            slots.TryGetValue(puzzleSlots[i], out puzzleSlice);
            //PutPuzzleSlice(puzzleSlots[i], puzzleSlice);
            StartCoroutine(PutPuzzleSliceTimer(puzzleSlots[i], puzzleSlice, 0.5f));
        }
    }

    private void PutPuzzleSlice(PuzzleSlot slot, Puzzle slice)
    {
        slots[slot] = slice;
        slot.puzzle = slice;

        if (!slice) return;

        slice.transform.SetParent(slot.transform);
        slice.transform.localPosition = Vector3.zero;

        Grading();
    }

    private void ClearPuzzle()
    {
        correct.Clear();
        slots.Clear();
    }

    private IEnumerator PutPuzzleSliceTimer(PuzzleSlot slot, Puzzle slice, float time)
    {
        slots[slot] = slice;
        slot.puzzle = slice;

        if (slice)
        {
            slice.transform.SetParent(slot.transform);

            var timer = 0f;
            var currentPos = slice.transform.localPosition;
            var endPos = Vector3.zero;

            while (timer < time)
            {
                timer += Time.deltaTime;

                slice.transform.localPosition = Vector3.Lerp(currentPos, endPos, timer / time);
                yield return null;
            }
        }

        player = GameManager.Instance.Player;
        if (player.TryGetComponent<PlayerController>(out var controller))
            controller.RemoveButtonInteraction();

        UIManager.OnPressBtnSlot1 -= Select;
        UIManager.OnPressBtnSlot1 += Select;
        action = Puzzle1;


        GameManager.Instance.SetTimer(61f,
                () => UIManager.Instance.OpenClearPopup(false)
            );
    }

    private void Grading()
    {
        Puzzle correctSlice;
        Puzzle nowSlice;
        foreach(var key in correct.Keys)
        {
            correct.TryGetValue(key, out correctSlice);
            slots.TryGetValue(key, out nowSlice);

            //Debug.Log(correctSlice.transform.parent.name + "/" + nowSlice.transform.parent.name);
            //Debug.Log(correctSlice.GetComponent<Puzzle>().puzzleSlot.name + "/" + nowSlice.GetComponent<Puzzle>().puzzleSlot.name);


            if (nowSlice != correctSlice)
            {
                return;
            }
        }

        if (puzzle1.activeSelf)
        {
            ClearPuzzle();
            ScenarioManager.Instance.OnStoryEnd += () => { StartCoroutine("PuzzleFaze2"); };
            OnFaze1End?.Invoke();
        }
        else
        {
            OnFaze2End?.Invoke();
            ScenarioManager.Instance.OnStoryEnd += () => { GameManager.Instance.LoadScene("1-2Stage"); };
        }
        //UIManager.Instance.OpenClearPuzzlePopup(true, () => { GameManager.Instance.LoadScene("1-2Stage"); });

        GameManager.Instance.StopTimer();
    }

    private void Puzzle1()
    {
        OnFaze1End?.Invoke();
    }

    private void Puzzle2()
    {
        ClearPuzzle();
        OnFaze2End?.Invoke();
    }
}
