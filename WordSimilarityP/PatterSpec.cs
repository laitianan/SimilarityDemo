using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace WordSimilarityP
{
    public class PatterSpec
    {




        private static string constReigex = "(?i)(ml|L|KG|G|毫升|升|克|千克|片|抽|瓶|罐|粒|盒|袋|块|mm)";
        public static string intRegex = "((\\d+\\.)?\\d+)";
        public static string xRegex = "(?i)(\\*|x|×|\\+)";
        public static string appendUnit = string.Format("((/({0}))?)", constReigex);
        public static string case1 = intRegex + constReigex + xRegex + intRegex + constReigex + "?";//200*3
        public static string case2 = intRegex + xRegex + intRegex + constReigex + appendUnit;//200*3L
        public static string case3 = intRegex + "(" + xRegex + intRegex + ")+";//200*3
        public static string case4 = intRegex + constReigex + appendUnit;//200L
        public static string regexString = string.Format("({0}|{1}|{2}|{3})", case1, case2, case3, case4);
        public static Regex pattern =new Regex(regexString);
        public static string getSingleSpec(string name)
        {
            string result = "";
            var  matcher = pattern.Match(name);
            if (matcher.Success)
            {
                result = matcher.Value;
            }
            return result;
        }



        public static String patterSpec(string str_)
        {
            str_ = str_.ToLower();
            string str = patternBracket(str_);
            //含有括号
            if (str.Equals(""))
            {
                //规格在名称之后
                str = patternNamePrec(str_);
            }

            if (str.Equals(""))
            {
                //规格在名称之前
                str = patternPrecName(str_);
            }
            if (str.Equals(""))
                str = getSingleSpec(str_);

            return str;
        }

     
        //private static string string_ = "高姿COGI匀净萃白10件套（修容霜5g*2+洁面15g+水10ml*2+乳10g+霜5g+面贴膜20ml*3）（小样套装）不建议购买";
        // "高姿COGI匀净萃白10件套（修容霜5g*2+洁面15g+水10ml*2+乳10g+霜5g+面贴膜20ml*3）（小样套装）不建议购买";
        private static Regex pBracket = new Regex("(\\(|（)[^)）]+\\+[^)）]+(\\)|）)");


        //用于检测是否规格在之前
        private static string str2 = string.Format("^\\d+({0})+", constReigex);
        private static Regex patte = new Regex(str2);

        /**
         * 规格在前则返回ture
         *
         * @param str
         * @return
         */
        private static bool isBefore(string str)
        {
            string string_ = str.Trim();

            Match matches = patte.Match(string_);
            if (matches.Success) return true;
            return false;
        }

        //匹配含有括号 "高姿COGI匀净萃白10件套（修容霜5g*2+洁面15g+水10ml*2+乳10g+霜5g+面贴膜20ml*3）（小样套装）不建议购买";
        private static String patternBracket(string string_)
        {

            Match m = pBracket.Match(string_);
            if (m.Success)
            {

                string str = m.Value;
                str = str.Replace("(", "").Replace("（", "").Replace(")", "").Replace("）", "");
                if (isBefore(str))
                {
                    return patternPrecName(str);
                }
                else
                {
                    return patternNamePrec(str);
                }
            }
            return "";
        }


      
        // 仅仅获取单位
        private static string unit = string.Format("(\\d+({0}){1}+([*xX]\\d+{2}?)?)([+*]?(\\d+({3}){4}+([*xX]\\d+{5}?)?))*", constReigex, appendUnit, constReigex, constReigex, appendUnit, constReigex);
        private static Regex unitPattern = new Regex(unit);


        //获取规格
        private static String getUnit(String string_)
        {

            Match m = unitPattern.Match(string_);
            //Matcher m = unitPattern.matcher(string_);
            if (m.Success)
                return m.Value;
            return "";
        }

        // 多芬 滋养水润洗发乳/水/露700ml+护发素195ml*2
        private static Regex pNamePrec = new Regex(string.Format("([/a-zA-Z\u4e00-\u9fa5])+([ ]+)?(\\d+({0}){1}+([*xX]\\d+)?)([+*]?(\\d+({2}){3}+([*xX]\\d+)?))*",
            constReigex, appendUnit, constReigex, appendUnit));

        //匹配单位在前面，名称在后面
        //(\d+%s+(\*\d+)?)([+*]?(\d+%s+(\*\d+)?))?[^ ]+
        //施华蔻多效修护19套装600ml洗发水600ml护发素送小样
        private static Regex pPrecName = new Regex(string.Format("(\\d + ({0}){1} + ([*xX]\\d +) ?)([+*] ? (\\d + ({2}){3} + ([*xX]\\d +) ?))?([a-zA-Z\\u4e00-\\u9fa5])+([*]\\d)?",
            constReigex, appendUnit, constReigex, appendUnit));

        /**
         * 匹配单位在前面，名称在后面
         *
         * @param string
         * @return
         */
        private static String patternPrecName(string string_)
        {

            MatchCollection matchers = pPrecName.Matches(string_);
            // Matcher matcher = pPrecName.matcher(string_);
            StringBuilder builder = new StringBuilder();
            int k = 0;

            for (int i = 0; i < matchers.Count; i++)
            {
                var m = matchers[i];
                if (m.Success)
                {
                    string s = m.Value;
                    string prec = getUnit(s);
                    string nameString = s.Replace(prec, "");
                    builder.Append(nameString + ":" + prec + ":");
                    k++;
                }
            }
            if (k > 1) return builder.ToString();
            return "";
        }


        /**
         * //  // 多芬 滋养水润洗发乳/水/露700ml+护发素195ml*2
         * 名称在规格之前
         *
         * @param string
         * @return
         */
        public static String patternNamePrec(String string_)
        {

            var ms = pNamePrec.Matches(string_);
            //Matcher m = pNamePrec.matcher(string_);
            StringBuilder builder = new StringBuilder();
            int k = 0;
            for (int i = 0; i < ms.Count; i++)
            {
                var m = ms[i];
                if (m.Success)
                {
                    String s = m.Value;
                    String prec = getUnit(s);
                    String nameString = s.Replace(prec, "");
                    builder.Append(nameString + ":" + prec + ":");
                    k++;
                }

            }
            if (k > 1) return builder.ToString();
            return "";
        }
    }
}
