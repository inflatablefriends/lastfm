using System.IO;
using System.Text;

namespace IF.Lastfm.Core.Tests
{
    public static class TestHelper
    {
        public static string DifferencesTo(this string first, string second)
        {
            const string start = "\n\nDifferences:\n";

            var sb = new StringBuilder(start);

            var sr1 = new StringReader(first);
            var sr2 = new StringReader(second);

            int count = 1;
            using (sr1)
            {
                using (sr2)
                {
                    while (true)
                    {
                        string line1, line2;
                        var lineWasRead = sr1.LineRead(out line1);
                        lineWasRead &= sr2.LineRead(out line2);

                        if (!lineWasRead)
                        {
                            break;
                        }

                        if (line1 != line2)
                        {
                            var line = string.Format("{0}A: {1}\n{0}B: {2}", count, line1, line2);
                            sb.AppendLine(line);
                        }

                        count++;
                    }
                }
            }

            return sb.ToString();
        }

        public static bool LineRead(this StringReader reader, out string line)
        {
            line = reader.ReadLine();

            return !string.IsNullOrEmpty(line);
        }

    }
}
