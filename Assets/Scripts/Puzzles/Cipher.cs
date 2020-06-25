using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cipher {

    public enum CipherType { Shift, Mono, Poly }
    static string Alphabet = "abcdefghijklmnopqrstuvwxyz";

    static public char[] GenerateMono()
    {
        

        char[] output = new char[26];

        for(int i = 0;i < 26; i++)
        {
            char hold;
            bool NoColision;
            do {
                NoColision = false;
                hold = Alphabet.ToCharArray()[UnityEngine.Random.Range(0, 26)];
                for (int j = 0; j < i; j++)
                {
                    if (hold == output[j])
                        NoColision = true;
                }

            } while (NoColision);
            output[i] = hold;
        }

        return output;
    }

    static public string Mono(string Input, char[] SetTo)
    {
        string Output="";


        

        foreach(char Character in Input)
        {
            Debug.Log(Character - 65);

            if (Char.IsUpper(Character))
                Output += Char.ToUpper(SetTo[(Character - 65)]);
            else if (Char.IsLetter(Character))
                Output += SetTo[(Character - 97)];
            else
                Output += Character;
        }

        return Output;

    }


    static public string Offset(string Input, int Offset)
    {
        string Output = "";


        foreach (char Character in Input)
        {

            if (Char.IsLetter(Character))
            {
                bool Lower = false;
                if (Char.IsUpper(Character))
                    Lower = true;
                int hold = OutofBounds(Convert.ToInt32(Character) + Offset, Lower);
                Output += Convert.ToChar(hold);
            }
            else
            {
                Output += Character;
            }
        }
        return Output;
    }

    public static string Poly(string Input, char[] Offset)
    {
        string Output = "";
        int currentOffset = 0;


        foreach (char Character in Input)
        {
            if (Char.IsLetter(Character)){
                bool Lower = false;
                if (Char.IsUpper(Character))
                    Lower = true;
                int hold = OutofBounds(Convert.ToInt32(Character) + Offset[currentOffset]-65, Lower);
                Output += Convert.ToChar(hold);
                currentOffset++;
                if (currentOffset == Offset.Length)
                    currentOffset = 0;
            }
            else
            {
                Output += Character;
            }
        }
        return Output;
    }

    static int OutofBounds(int check,bool Lowercase)
    {
        if (Lowercase)
        {
            while (check > 90)
                check -= 26;
            while (check < 65)
                check += 26;
        }
        else
        {
            while (check > 122)
                check -= 26;
            while (check < 97)
                check += 26;
        }

        return check;
    }

}

