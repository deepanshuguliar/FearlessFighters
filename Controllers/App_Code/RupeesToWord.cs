using System;
using System.Collections.Generic;
using System.Text;

namespace ATS
{
  public  class Class1
    {
        public  string RupeesToWord(string MyNumber)
        {
            string Temp, Paisa = "", Hundreds, Words = "";
            int DecimalPlace, iCount;
            string[] place = new string[9];
            place[0] = " Thousand ";
            place[2] = " Lakh ";
            place[4] = " Crore ";
            place[6] = " Arab ";
            place[8] = " Kharab ";

            DecimalPlace = MyNumber.IndexOf(".");

            if (DecimalPlace > 0)
            {
                Temp = MyNumber.Substring(DecimalPlace + 1, 2);
                Paisa = " and " + ConvertTens(Temp) + " Paisa";

                MyNumber = MyNumber.Substring(0, DecimalPlace);
            }

            if (MyNumber.Length > 0 && MyNumber.Length <= 2)
            {
                if (MyNumber.Length == 1)
                    return ConvertDigit(MyNumber) + Paisa + " Only.";
                else
                    return ConvertTens(MyNumber) + Paisa + " Only.";
            }

            Hundreds = ConvertHundreds(MyNumber.Substring((MyNumber.Length - 3)));
            MyNumber = MyNumber.Substring(0, (MyNumber.Length - 3));
            iCount = 0;
            while ((MyNumber != ""))
            {
                if (MyNumber.Length == 1)
                {
                    Temp = MyNumber.Substring((MyNumber.Length - 1));
                    Words = Words.Insert(0, (ConvertDigit(Temp) + place[iCount]));
                    MyNumber = MyNumber.Substring(0, (MyNumber.Length - 1));
                }
                else
                {
                    Temp = MyNumber.Substring((MyNumber.Length - 2));
                    Words = Words.Insert(0, (ConvertTens(Temp) + place[iCount]));
                    MyNumber = MyNumber.Substring(0, (MyNumber.Length - 2));
                }
                iCount = (iCount + 2);
            }
            return Words + (Hundreds + (Paisa + " Only"));
        }

        private string ConvertHundreds(string MyNumber)
        {
            string Result = "";
            MyNumber = ("000" + MyNumber).Substring((("000" + MyNumber).Length - 3));
            if ((MyNumber.Substring(0, 1) != "0"))
            {
                Result = (ConvertDigit(MyNumber.Substring(0, 1)) + " Hundreds ");
            }
            if ((MyNumber.Substring(1, 1) != "0"))
            {
                Result = (Result + ConvertTens(MyNumber.Substring(1)));
            }
            else
            {
                Result = (Result + ConvertDigit(MyNumber.Substring(2)));
            }
            return Result.Trim();
        }

        private string ConvertTens(string MyTens)
        {
            string Result = "";
            if (int.Parse(MyTens.Substring(0, 1)) == 1)
            {
                switch (int.Parse(MyTens.ToString()))
                {
                    case 10: Result = "Ten"; break;
                    case 11: Result = "Eleven"; break;
                    case 12: Result = "Twelve"; break;
                    case 13: Result = "Thirteen"; break;
                    case 14: Result = "Fourteen"; break;
                    case 15: Result = "Fifteen"; break;
                    case 16: Result = "Sixteen"; break;
                    case 17: Result = "Seventeen"; break;
                    case 18: Result = "Eighteen"; break;
                    case 19: Result = "Nineteen"; break;
                    default: break;
                }
            }
            else
            {
                switch (int.Parse(MyTens.Substring(0, 1)))
                {
                    case 2: Result = "Twenty "; break;
                    case 3: Result = "Thirty "; break;
                    case 4: Result = "Forty "; break;
                    case 5: Result = "Fifty "; break;
                    case 6: Result = "Sixty "; break;
                    case 7: Result = "Seventy "; break;
                    case 8: Result = "Eighty "; break;
                    case 9: Result = "Ninety "; break;
                    default: break;
                }
                Result = Result + ConvertDigit(MyTens.Substring(1, 1));
            }
            return Result;
        }
        private string ConvertDigit(string MyDigit)
        {
            switch (int.Parse(MyDigit))
            {
                case 1: return "One";
                case 2: return "Two";
                case 3: return "Three";
                case 4: return "Four";
                case 5: return "Five";
                case 6: return "Six";
                case 7: return "Seven";
                case 8: return "Eight";
                case 9: return "Nine";
                default: return "";
            }
        }
    }
}
