using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace WordSimilarityP
{

    /// <summary>
    /// 单位换算
    /// </summary>
    public static class UnitConversion
    {

        private static Regex pattern = new Regex("(\\d+[+*])?(\\d)+l([+*]\\d+)?");
        /// <summary>
        /// l 转ml 的单位换算
        /// </summary>
        /// <param name="strProductSpecs"></param>
        /// <returns></returns>
        public static string translationLtoML(string strProductSpecs)
        {
            strProductSpecs = strProductSpecs.ToLower();
            Match match = pattern.Match(strProductSpecs);
            string numStr = "";
            if (match.Success)
            {
                numStr = match.Groups[2].Value;
                string r = (int.Parse(numStr) * 1000).ToString() + "ml";
                strProductSpecs = strProductSpecs.Replace(numStr + "l", r);
            }
            return strProductSpecs;
        }
    }
}
