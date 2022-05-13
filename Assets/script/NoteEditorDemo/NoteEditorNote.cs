using UnityEngine;
using System;
using UnityEngine.EventSystems;// Required when using Event data.

public class NoteEditorNote : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private float notesSpeed;

    [SerializeField]
    private Vector2 startPos;//ノーツの開始位置
    [SerializeField]
    private Vector2 judgePos;//判定したい場所

    public static float moveSpan = 0.01f;//回すスパン 

    private float notesTime;

    public NotesGenerator.NoteType noteType;

    void Start()
    {
        notesTime = (startPos.x - judgePos.x) / notesSpeed;
        InvokeRepeating("MakeNotesMove", 0, moveSpan);
    }

    void MakeNotesMove()
    {
        transform.position += new Vector3(0f, -notesSpeed, 0f);
        notesTime -= moveSpan;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        var distance = Mathf.Abs(transform.localPosition.y - NoteJudgeBar.Instance.MyTransform.localPosition.y);
        Debug.Log(distance);

        NoteSoundManager.Instance.PlaySE();

        if (distance <= 50)
        {
            NotesGenerator.Instance.RemoveNote(this);
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if(this.transform.localPosition.y <= -Screen.height/2)
        {
            NotesGenerator.Instance.RemoveNote(this);
            Destroy(this.gameObject);
        }
    }
}