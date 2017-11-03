using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace WordSimilarity
{

    /// <summary>
    /// 单位换算
    /// </summary>
    public static class UnitConversion
    {

        private static Regex patternL =  new Regex("((\\d+[.])?(\\d)+)l([+*]\\d+)?");

        private static Regex patternKG = new Regex("((\\d+[.])?(\\d)+)kg([+*]\\d+)?");
        
        /// <summary>
        /// l 转ml 的单位换算
        /// </summary>
        /// <param name="strProductSpecs"></param>
        /// <returns></returns>
        public static string translationLtoML(string strProductSpecs)
        {
            strProductSpecs = strProductSpecs.ToLower();
            Match match = patternL.Match(strProductSpecs);
            string numStr = "";
            if (match.Success)
            {
                numStr = match.Groups[1].Value;
                string r = (double.Parse(numStr) * 1000).ToString() + "ml";
                strProductSpecs = strProductSpecs.Replace(numStr + "l", r);
            }
            return strProductSpecs;
        }

        /// <summary>
        /// KG 转G 的单位换算
        /// </summary>
        /// <param name="strProductSpecs"></param>
        /// <returns></returns>
        public static string translationKGtoG(string strProductSpecs)
        {
            strProductSpecs = strProductSpecs.ToLower();
            Match match = patternKG.Match(strProductSpecs);
            if (match.Success)
            {
                string numStr = match.Groups[1].Value;
                string r = (double.Parse(numStr) * 1000).ToString() + "g";
                strProductSpecs = strProductSpecs.Replace(numStr + "kg", r);
            }
            return strProductSpecs;
        }

        public static string unitTranslation(string strProductSpecs)
        {
            strProductSpecs = translationLtoML(strProductSpecs);
            strProductSpecs = translationKGtoG(strProductSpecs);
            return strProductSpecs;
        }
    }
}
