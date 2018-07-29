using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class RandomTest : MonoBehaviour {

    [SerializeField]
    List<string[]> numList;
    int[] was;
    public int[] cnt;
    public string[] maxList = new string[6];
    private void Start()
    {
        was = new int[]{11,30,34,35,42,44 };
//        was = new int[]{1,3,12,14,16,43 };
//        was = new int[]{8,11,19,21,36,45 };
//        was = new int[]{5,10,13,21,39,43 };
//        was = new int[]{6,11,15,17,23,40 };


        numList = CSVReader.Read("asd");

        for (int i = 0; i < cnt.Length; i++) { cnt[i] = 0; }

        for (int x = 0; x < was.Length; x++)
        {
            cnt = new int[45];
            for (int i = 1; i < 150; i++)//numList.Count
            {
                for (int j = 0; j < numList[i].Length; j++)//6번
                {
                    if (System.Convert.ToInt32(numList[i][j]) == was[x]) //was 숫자가 numList 줄에 있다면 numList[i-1]에 1~45가 나오는 횟수
                    {
                        for (int z = 0; z < cnt.Length; z++)
                        {
                            for (int k = 0; k < 6; k++)
                            {
                                if (System.Convert.ToInt32(numList[i - 1][k]) == z + 1) { cnt[z] = cnt[z] + 1; }
                            }
                        }
                    }
                }
            }
            int max = 0;
            int second = 0;
            string maxs="";
            string seconds="";
            for (int y = 0; y < cnt.Length; y++)
            {
                if (max < cnt[y])
                {
                    second = max; seconds = maxs;
                    max = cnt[y]; maxs = (y + 1).ToString();
                }
                else if (max == cnt[y]) { maxs = maxs + ", " + (y + 1).ToString(); }
                else if (second < cnt[y]) { second = cnt[y]; seconds = (y + 1).ToString(); }
                else if (second == cnt[y]) { seconds = seconds + ", " + (y + 1).ToString(); }
            }
            maxList[x] = maxs + "/ " + second;
        }


    }

}

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<string[]> Read(string file)
    {
        List<string[]> list = new List<string[]>();
        TextAsset data = Resources.Load(file) as TextAsset;

        string[] lines = Regex.Split(data.text, "\r\n");

        if (lines.Length <= 1) return list;
  
        for (var i = 0; i < lines.Length; i++)
        {
            string[] tempstrings = Regex.Split(lines[i], ",");
            list.Add(tempstrings);
            string test="";
            for (int j=0; j< tempstrings.Length; j++) {test= test + tempstrings[j] + ", "; }

            Debug.Log(lines.Length +", "+ test); 
        }

        return list;
    }
}