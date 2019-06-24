using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace acintratefix
{
    class Program
    {
        static void Main(string[] args)
        {
            // read in the file
            // regex to find all the matching instances
            // load relevant line... 
            // change the value: divide by 100, format as NN.NNNNNN
            // stream back to new file

            Console.WriteLine("Enter the full filename, path included: ");
            string fileName = Console.ReadLine();  //@"C:\_github\anacredit-interest-rate-fix\testfile.txt";
            Console.WriteLine("Enter the output filename, path included: ");
            string outFileName = Console.ReadLine(); // @"C:\_github\anacredit-interest-rate-fix\outfile.txt";

            int flushCount = 0;

            using (StreamReader sr = File.OpenText(fileName))
            {

                Console.WriteLine("Opening file... wait a few seconds...");

                using (StreamWriter outfile = new StreamWriter(outFileName))
                {
                    outfile.AutoFlush = false;

                    string s = String.Empty;
                    while ((s = sr.ReadLine()) != null)
                    {
                        //do your stuff here
                        Regex r = new Regex(@"<InterestRate>(.+?)<\/InterestRate>|<InterestRateCap>(.+?)<\/InterestRateCap>|<InterestRateFloor>(.+?)<\/InterestRateFloor>|<InterestRateSpreadMargin>(.+?)<\/InterestRateSpreadMargin>");
                        if (r.IsMatch(s))
                        {
                            Console.Write(s.ToString());
                            var value = s.Split('>')[1].Split('<')[0].ToString();
                            decimal rate = 0;
                            Decimal.TryParse(value, out rate);
                            if (rate == 0)
                            {
                                Console.WriteLine($" --> {s}");
                                outfile.WriteLine(s);
                            }
                            else
                            {
                                var newRate = rate / 100;
                                var newLine = s.Replace(value, newRate.ToString("0.000000"));
                                Console.WriteLine($" --> {newLine}");
                                outfile.WriteLine(newLine);
                            }
                        }
                        else
                            outfile.WriteLine(s);

                        flushCount++;

                        // flush every 2000 lines... 
                        if (flushCount % 2000 == 0)
                        {
                            outfile.Flush();
                        }
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
