using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FrequencyList {
    public string lang;
    Dictionary<string, double> words;
    public int mostCommonOccurence = 0;

    public FrequencyList(string lang) {
        words = new Dictionary<string, double>();
        this.lang = lang;
    }

    public IEnumerator initialize() {
        
        string word;
        int freq;
        string list = ((TextAsset)Resources.Load(lang)).text;

        int linenr = 0;

        string[] lines = list.Split('\n');
        foreach (string line in lines.Take(lines.Length - 1)) {
            if (linenr % 10000 == 0) {
                yield return 0;
            }
            if (mostCommonOccurence == 0) {
                mostCommonOccurence = Convert.ToInt32(line.Split(' ')[1]);
            }
            word = line.Split(' ')[0];
            freq = Convert.ToInt32(line.Split(' ')[1]);
            words.Add(word, Convert.ToDouble(freq) / mostCommonOccurence);
        }
    }

    public double Search(string word) {
        if(!word.Contains(" ")) {
            if (words.ContainsKey(word)) {
                return words[word];
            } else {
                // If the word isn't found, it's either really rare or misspelled.
                // It could of course also be a concatenation of two moderately 
                // rare words, but there's no easy way to check this.
                return 1.0/mostCommonOccurence;
            }
        } else {
            //If the "word" is actually a sentence, score it using the rarest word.
            return word.Split(' ').Min(x=>Search(x));

        }
    }
}

