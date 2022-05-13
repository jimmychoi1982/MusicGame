using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System.Linq;

public class NotesGenerator : SingletonMonoBehaviour<NotesGenerator>
{
    public enum NoteType
    {
        Key0,
        Key1,
        Key2,
        Key3,
        Key4,
    }

    [Serializable]
    public class InputJson
    {
        public Notes[] notes;
        public int BPM;
    }

    [Serializable]
    public class Notes
    {
        public int num;
        public int block;
        public int LPB;
    }

    private int[] scoreNum;//ノーツの番号を順に入れる
    private int[] scoreBlock;//ノーツの種類を順に入れる
    private int BPM;
    private int LPB;

    [SerializeField]
    private NoteEditorNote notesPre;
    [SerializeField]
    float noteStartPositionY = 230;

    private float moveSpan = 0.01f;
    private float nowTime;// 音楽の再生されている時間
    private int beatNum;// 今の拍数
    private int beatCount;// json配列用(拍数)のカウント
    private bool isBeat;// ビートを打っているか(生成のタイミング)

    [SerializeField]
    private AudioSource gameAudio;

    public static bool isAudioPlay=false;

    NotesData noteData; // Delete

    [SerializeField]
    TextAsset jsonData;

    Vector2 noteStartPos = Vector2.zero;
    public Transform noteParent;

    List<NoteEditorNote> key0NoteViewDataList;
    List<NoteEditorNote> key1NoteViewDataList;
    List<NoteEditorNote> key2NoteViewDataList;

    void Awake()
    {
        MusicReading(); // 数据读取成功

        InvokeRepeating("NotesIns", 0f, moveSpan);

        key0NoteViewDataList = new List<NoteEditorNote>();
        key1NoteViewDataList = new List<NoteEditorNote>();
        key2NoteViewDataList = new List<NoteEditorNote>();
    }

    /// <summary>
    /// 譜面の読み込み
    /// </summary>
    void MusicReading()
    {
        string inputString = jsonData.ToString();
        InputJson inputJson = JsonUtility.FromJson<InputJson>(inputString);

        scoreNum = new int[inputJson.notes.Length];
        scoreBlock = new int[inputJson.notes.Length];
        BPM = inputJson.BPM;
        LPB = inputJson.notes[0].LPB;

        for (int i = 0; i < inputJson.notes.Length; i++)
        {
            //ノーツがある場所を入れる
            scoreNum[i] = inputJson.notes[i].num;
            //ノーツの種類を入れる(scoreBlock[i]はscoreNum[i]の種類)
            scoreBlock[i] = inputJson.notes[i].block;
        }

        StartCoroutine("StartBGM");
    }

    IEnumerator StartBGM()
    {
        yield return new WaitForSeconds(1.0f);
        NoteSoundManager.Instance.PlayBGM();
    }

    /// <summary>
    /// 譜面上の時間とゲームの時間のカウントと制御
    /// </summary>
    void GetScoreTime()
    {
        //今の音楽の時間の取得
        nowTime += moveSpan; //(1)

        //ノーツが無くなったら処理終了
        if (beatCount > scoreNum.Length) return;

        //楽譜上でどこかの取得
        beatNum = (int)(nowTime * BPM / 60 * LPB); //(2)
    }

    /// <summary>
    /// ノーツを生成する
    /// </summary>
    void NotesIns()
    {
        GetScoreTime();

        //json上でのカウントと楽譜上でのカウントの一致
        if (beatCount < scoreNum.Length)
        {
            isBeat = (scoreNum[beatCount] == beatNum); //(3)
        }

        //生成のタイミングなら
        if (isBeat)
        {
            var obj = Instantiate(notesPre, noteParent);

            //ノーツ0の生成
            if (scoreBlock[beatCount] == (int)NoteType.Key0)
            {
                obj.transform.localPosition = new Vector3(-146, noteStartPositionY, 0);
                obj.gameObject.SetActive(true);
                obj.noteType = NoteType.Key0;

                key0NoteViewDataList.Add(obj);
            }

            //ノーツ1の生成
            if (scoreBlock[beatCount] == (int)NoteType.Key1)
            {
                obj.transform.localPosition = new Vector3(0, noteStartPositionY, 0);
                obj.gameObject.SetActive(true);
                obj.noteType = NoteType.Key1;
                
                key1NoteViewDataList.Add(obj);
            }

            //ノーツ1の生成
            if (scoreBlock[beatCount] == (int)NoteType.Key2)
            {
                obj.transform.localPosition = new Vector3(146, noteStartPositionY, 0);
                obj.gameObject.SetActive(true);
                obj.noteType = NoteType.Key2;

                key2NoteViewDataList.Add(obj);
            }

            beatCount++; //(5)
            isBeat = false;

        }
    }

    public void RemoveNote(NoteEditorNote note)
    {
        if(note.noteType == NoteType.Key0)
        {
            key0NoteViewDataList.Remove(note);
        }
        else if(note.noteType == NoteType.Key1)
        {
            key1NoteViewDataList.Remove(note);
        }
        else if (note.noteType == NoteType.Key2)
        {
            key2NoteViewDataList.Remove(note);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            var firstNoteObj = key0NoteViewDataList.First();
            var distance = Mathf.Abs(firstNoteObj.transform.localPosition.y - NoteJudgeBar.Instance.MyTransform.localPosition.y);
            if (distance <= 50)
            {
                RemoveNote(firstNoteObj);
                Destroy(firstNoteObj.gameObject);
            }
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            var firstNoteObj = key1NoteViewDataList.First();
            var distance = Mathf.Abs(firstNoteObj.transform.localPosition.y - NoteJudgeBar.Instance.MyTransform.localPosition.y);
            if (distance <= 50)
            {
                RemoveNote(firstNoteObj);
                Destroy(firstNoteObj.gameObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            var firstNoteObj = key2NoteViewDataList.First();
            var distance = Mathf.Abs(firstNoteObj.transform.localPosition.y - NoteJudgeBar.Instance.MyTransform.localPosition.y);
            if (distance <= 50)
            {
                RemoveNote(firstNoteObj);
                Destroy(firstNoteObj.gameObject);
            }
        }
    }

}