using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StrParseSpanTool
{
    static ReadOnlySpan<char> GetSplitSpan(string str, char symbol, int index)
    {
        if (index < 0 || index >= str.Length) return null;

        int start_idx = -1;
        int end_idx = str.IndexOf(symbol);
        if (end_idx == -1)
        {
            if (index == 0)
                return str.AsSpan();
            else
                return null;
        }

        while (start_idx != end_idx && index-- != 0)
        {
            start_idx = end_idx;
            if (end_idx != -1)
                end_idx = str.IndexOf(symbol, start_idx + 1);
        }

        if (index > 0 || (start_idx == -1 && end_idx == -1)) return null;
        return str.AsSpan().Slice(start_idx + 1, (end_idx < 0 ? str.Length : end_idx) - start_idx - 1);
    }

    public static bool SplitToStr(this string str, char symbol, int index, ref string ret)
    {
        var span = GetSplitSpan(str, symbol, index);
        if (span == null) return false;

        ret = span.ToString();
        return true;
    }

    public static void SplitToStrList(this string str, char symbol, ref List<string> refList)
    {
        int index = 0;
        string tmp_str = null;
        while (str.SplitToStr(',', index++, ref tmp_str))
        {
            if (refList == null) refList = new List<string>();
            refList.Add(tmp_str);
        }
    }

    public static bool SplitToFloat(this string str, char symbol, int index, ref float ret) 
    {
        var span = GetSplitSpan(str, symbol, index);
        if (span == null) return false;

        ret = float.Parse(span);
        return true;
    }

    public static bool SplitToInt(this string str, char symbol, int index, ref int ret)
    {
        var span = GetSplitSpan(str, symbol, index);
        if (span == null) return false;

        ret = int.Parse(span);
        return true;
    }

    public static void SplitToIntList(this string str, char symbol, ref List<int> refList)
    {
        int index = 0;
        int tmp_int = 0;
        while (str.SplitToInt(',', index++, ref tmp_int))
        {
            if (refList == null) refList = new List<int>();
            refList.Add(tmp_int);
        }
    }

    public static bool SplitToBool(this string str, char symbol, int index, ref bool ret)
    {
        var span = GetSplitSpan(str, symbol, index);
        if (span == null) return false;

        ret = bool.Parse(span);
        return true;
    }
}