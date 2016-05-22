/*
 * A bookmark corresponds to a word that can be used in the games.
 * it contains relevant information about that word,
 * like a translation and a context sentence.
 */
using System.Collections;
using UnityEngine;
using Boomlagoon.JSON;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

[System.Serializable]
public class Bookmark {
    
    public string word { get; set; }
    public string translation { get; set; }
    public string context { get; set; }
    public uint id { get; set; }
    public bool isLearned;
    public DateTime bookmarkDate;

    public Bookmark(string word, string translation, string context, uint id, DateTime bookmarkDate) {
        this.word = word;
        this.translation = translation;
        this.context = context;
        this.id = id;
        this.isLearned = false;
        this.bookmarkDate = bookmarkDate;
    }

	/* Changes the bookmarks in Json-format gotten from zeeguu into a bookmark-list */
    public static List<Bookmark> ListFromJson(string json) {
        List<Bookmark> bookmarks = new List<Bookmark>();
        JSONObject bm;

        JSONArray dates = JSONArray.Parse(json);

        foreach (JSONValue date in dates) {
            foreach (JSONValue dateBookmark in date.Obj.GetArray("bookmarks")) {
                bm = dateBookmark.Obj;
                foreach(JSONValue translation in bm.GetArray("to")) {
                    bookmarks.Add(new Bookmark(bm.GetString("from"),
                        translation.Str,
                        bm.GetString("context"),
                        System.Convert.ToUInt32(bm.GetNumber("id")),
                        DateTime.Parse(date.Obj.GetString("date"))));
                }
            }
        }
        return bookmarks;
    }
}
