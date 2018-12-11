using System;
using System.Text.RegularExpressions;

namespace LogIngest
{
    public class LogLine
    {
        DateTime logTime;
        String logSource;
        String logLevel;
        String logCode;
        String logLocation;
        String logText;


        public LogLine(string log, string source)
        {
            logSource = setLogSource(source);
            logLevel = setlogLevel(log);
            logTime = doDateTimeConversion(log);
            logCode = parseLogCode(log);
            logLocation = parseLogLocation(log);
            logText = parseLogText(log);
        }

        public String setLogSource(String source)
        {
            String[] parts = source.Split('.');
            String logSource = parts[0];
            return logSource;
        }

        public DateTime doDateTimeConversion(string logString)
        {
            DateTime date = new DateTime();
            date = Convert.ToDateTime(logString.Substring(0, 23));
            return date;
        }

        public String setlogLevel(String logString)
        {
            var text = logString.Split(new[] { ' ' }, 4);
            String logLevel = text[2].ToString();
            return logLevel;
        }

        public String parseLogCode(String logString)
        {
            String code = "";
            var reg = new Regex(@"\[(.*?)\]");
            var matches = reg.Matches(logString);
            code = matches[0].ToString();
            char[] charsToTrim = { '[', ']' };
            code = code.Trim(charsToTrim);
            return code;
        }

        public String parseLogLocation(String logString)
        {
            String logLocation = "";
            String[] delimiterChars = { "-", "   " };
            String[] results = logString.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
            if (results.Length > 1)
            {
                logLocation = results[1].ToString().Trim();
            }
            else
            {
                logLocation = "Error Parsing Log Location";
            }

            return logLocation;
        }

        public String parseLogText(String logString)
        {
            String logText = "";
            var text = logString.Split(new[] { '-' }, 2);
            if (text.Length > 1)
            {
                logText = text[1].ToString();

            }

            if (logText == "" || logText == null)
            {
                logText = "No Additional Information found in log";
            }
            return logText;
        }

        public String getLog()
        {
            String logLine = (logTime + " |logSource=" + logSource + " |logLevel=" +
                logLevel + " |logCode=" + logCode + " |logLocation=" + logLocation +
                " |logText=" + logText);

            return logLine;
        }

        public DateTime getLogTime()
        {
            return logTime;
        }

        public String getLogSource()
        {
            return logSource;
        }

        public String getLogLevel()
        {
            return logLevel;
        }

        public String getLogCode()
        {
            return logCode;
        }

        public String getLogLocation()
        {
            return logLocation;
        }

        public String getLogText()
        {
            return logText;
        }

    }


}
