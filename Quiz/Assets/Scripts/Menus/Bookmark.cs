using System.Collections;
using UnityEngine;
using Boomlagoon.JSON;
using System.Collections.Generic;
using UnityEngine.UI;

public class Bookmark {

    public Text test;
    public string word { get; set; }
    public string translation { get; set; }
    public string context { get; set; }
    private uint id { get; set; }

    public Bookmark(string word, string translation, string context, uint id) {
        this.word = word;
        this.translation = translation;
        this.context = context;
        this.id = id;
    }

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
                        System.Convert.ToUInt32(bm.GetNumber("id"))));
                }
            }
        }
        return bookmarks;
    }
}