using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NotesData
{
    public string name;
    public int maxBlock;
    public int BPM;
    public long offset;
    public List<Notes> notes;
}

[Serializable]
public class Notes
{
    public int LPB;
    public int num;
    public int block;
    public int type;
    public List<Notes> notes;
}