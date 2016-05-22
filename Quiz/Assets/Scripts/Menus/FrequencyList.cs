using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using UnityEngine;

public class FrequencyList {

    Dictionary<string, double> words;
    public int mostCommonOccurence = 0;

    public FrequencyList(string lang) {
        Debug.Log("Making frequency list for " + lang);
        words = new Dictionary<string, double>();

        string line, word;
        int freq;
        StreamReader file = new StreamReader("Assets/Wordlists/"+ lang + ".txt");

        int linenr = 0;
        while ((line = file.ReadLine()) != null) {
            if(linenr++ % 10000 == 0) {
                Debug.Log("Number crunching at line" + linenr);
            }
            if (mostCommonOccurence == 0) {
                mostCommonOccurence = Convert.ToInt32(line.Split(' ')[1]);
            }
            word = line.Split(' ')[0];
            freq = Convert.ToInt32(line.Split(' ')[1]);
            words.Add(word, Convert.ToDouble(freq)/mostCommonOccurence);
        }

        file.Close();
    }

    public double Search(string word) {
        Debug.Log("Searching in frequency list for '" + word + "'");
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

