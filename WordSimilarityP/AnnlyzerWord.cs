using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Cn.PowerAnalyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Lucene.Net.Analysis.Tokenattributes;
using System.Text.RegularExpressions;
namespace WordSimilarity
{

    /// <summary>
    /// 分词并处理相似度
    /// </summary>
    public class AnnlyzerWord
    {

        private static Regex regex = new Regex(@"[\[(（【](.)*?[)）】\]]");
        /// <summary>
        ///字符串分词之后返回一个不重复的数组
        /// </summary>
        /// <param name="text">待分词的字符串</param>
        /// <returns></returns>   wordSegmentation
        public static SortedSet<string> wordSegmentationToSet(string text)
        {

            SortedSet<string> set = new SortedSet<string>();
            using (TextReader reader = new StringReader(text))
            {
                Analyzer analyzer = BeyebeAnalyzer.ReadAnalyzer;
                TokenStream stream = analyzer.ReusableTokenStream(string.Empty, reader);
                //Lucene.Net.Analysis.Token token = null;
                while (stream.IncrementToken())
                {
                    ITermAttribute termAttribute = stream.GetAttribute<ITermAttribute>();
                    set.Add(termAttribute.Term);
                }
            }
            return set;
        }


        /// <summary>
        ///字符串分词之后返回一个重复的数组
        /// </summary>
        /// <param name="text">待分词的字符串</param>
        /// <returns></returns>   wordSegmentation
        public static List<string> wordSegmentationToList(string text)
        {

            List<string> list = new List<string>();
            using (TextReader reader = new StringReader(text))
            {
                Analyzer analyzer = BeyebeAnalyzer.ReadAnalyzer;
                TokenStream stream = analyzer.ReusableTokenStream(string.Empty, reader);
                //Lucene.Net.Analysis.Token token = null;
                while (stream.IncrementToken())
                {
                    ITermAttribute termAttribute = stream.GetAttribute<ITermAttribute>();
                    list.Add(termAttribute.Term);
                }
            }
            //list.ForEach(R => Console.Write(R + " | "));
            //Console.WriteLine();
            return list;
        }


        /// <summary>
        /// 字符串分词之后的列表，构建一个由数据字典构成的向量
        /// </summary>
        /// <param name="list1">分词列表，重复</param>
        /// <returns>数据字典向量向量</returns>
        public static Dictionary<string, int> getDictStrNum(List<string> list1)
        {
            Dictionary<string, int> dict1 = new Dictionary<string, int>();
            list1.ForEach(r =>
            {
                if (dict1.ContainsKey(r))
                {
                    dict1[r] = dict1[r] + 1;
                }
                else
                {
                    dict1.Add(r, 1);
                }
            });
            return dict1;
        }

        /// <summary>
        /// 计算向量的余弦相似度，值越高相似度越高，使用数据字典表示向量
        /// </summary>
        /// <param name="dict1">字符串向量1</param>
        /// <param name="dict2">字符串向量2</param>
        /// <returns></returns>
        public static double repetitiveRate(Dictionary<string, int> dict1, Dictionary<string, int> dict2)
        {
            double f1 = dict1.Values.Sum(r => r * r);
            double f2 = dict2.Values.Sum(r => r * r);
            f1 = Math.Sqrt(f1);
            f2 = Math.Sqrt(f2);
            int elem = dict1.Aggregate(0, (sum, kv) => sum + kv.Value * (dict2.ContainsKey(kv.Key) ? dict2[kv.Key] : 0));
            //   Console.WriteLine(elem / (f1 * f2));
            return elem / (f1 * f2);
        }

        /// <summary>
        /// 计算两个列别中词的重复率
        /// </summary>
        /// <param name="set1">列表一</param>
        /// <param name="set2">列表二</param>
        /// <returns>重复率</returns>
        public static double repetitiveRate(SortedSet<string> set1, SortedSet<string> set2)
        {
            double v = set1.Intersect(set2).Count();
            double u = set1.Union(set2).Count();
            double p = v / u;
            return p;
        }

        private static String regStr = PatterSpec.constReigex;

        /// <summary>
        /// 3*6*3*6*9 格式
        /// </summary>
        private static Regex regMul = new Regex(String.Format(@"^(\d+)([*Xx+]\d+)+$"));

        /// <summary>
        /// 3*6*3*6*9 获取所有的数字3 6 3 6 9 
        /// </summary>
        private static Regex regMulNum = new Regex(String.Format(@"\d+"));


        /// <summary>
        /// g或g/盒，获取单位后面的度量名称
        /// </summary>
        private static Regex unitName = new Regex(String.Format(@"({0})(/({1}))?", regStr, regStr));



        /// <summary>
        /// 匹配 3*6*9g/盒  的规格是否相同
        /// </summary>
        /// <param name="spec1"></param>
        /// <param name="spec2"></param>
        /// <returns></returns>
        public static bool getMulSpecisEqual(String spec1, String spec2)
        {
            if ((spec1 == null && spec2 == null) || (spec1 == spec2)) return true;
            if (spec1 == "" || spec2 == "") return false;
            spec1 = spec1.ToLower();
            spec2 = spec2.ToLower();

            String uniTone = unitName.Match(spec1).Value;
            String uniTtwo = unitName.Match(spec2).Value;

            spec1 = (uniTone == "") ? spec1 : spec1.Replace(uniTone, "");
            spec2 = (uniTone == "") ? spec2 : spec2.Replace(uniTtwo, "");


            double sum1 = -1;
            double sum2 = -1;
            int k = 0;
            if (regMul.Match(spec1).Success)
            {
                var m1 = regMulNum.Matches(spec1);
                for (int i = 0; i < m1.Count; i++)
                {
                    var m = (m1[i].Value == null || m1[i].Value == "") ? 1 : int.Parse(m1[i].Value);
                    sum1 *= m;
                    k++;

                }
                if (k == 2) return false;
            }


            if (regMul.Match(spec2).Success)
            {
                var m2 = regMulNum.Matches(spec2);
                for (int i = 0; i < m2.Count; i++)
                {
                    var m = (m2[i].Value == null || m2[i].Value == "") ? 1 : int.Parse(m2[i].Value);
                    sum2 *= m;
                }
            }

            if (!uniTone.Contains(uniTtwo) && !uniTtwo.Contains(uniTone)) return false;

            if (sum1 != -1 && sum1 == sum2) return true;

            return false;
        }

        /// <summary>
        /// 字母开头的规格，就是规格 MMQ-S35C（A）
        /// </summary>
        //定义1
        private static Regex charStartP = new Regex(@"^[a-zA-Z]+\S+");

        /// <summary>
        /// 有括号的规格 320g（含4颗）
        /// </summary>
        private static Regex haveBracket = new Regex(String.Format(@"\d+({0})?[(（]", regStr));
        /// <summary>
        /// 5545-545-45
        /// </summary>
        private static Regex allNum = new Regex(@"^\d+(-?\d+)+$");
        /// <summary>
        /// 3*10g或 10g*3或10g/盒*3
        /// </summary>
        private static Regex reg = new Regex(String.Format(@"^((\d+)([*xX]))?(\d+)(({0})(/({1}))?)(([*xX])(\d+))?$", regStr, regStr));



        /// <summary>
        /// 判断两个规格是否相同
        /// </summary>
        /// <param name="spec1"></param>
        /// <param name="spec2"></param>
        /// <param name="specName2">用于判断规格不存在情况下，型号是否存在问题</param>
        /// <returns></returns>
        public static bool specEqual(String spec1, String spec2, string specName2)
        {
            spec1 = spec1.ToLower().Replace("克", "g");
            spec2 = spec2.ToLower().Replace("克", "g");
            if ((spec1 == null && spec2 == null) || (spec1 == spec2)) return true;
            if (spec1 == null || spec2 == null) return false;
            spec1 = spec1.ToLower().Trim();
            spec2 = spec2.ToLower().Trim();
            specName2 = specName2.ToLower();
            //如果字符开头或纯数字或数字加横杠或含有括号则直接比较是否包含改规格    MMQ-S35C（A）||有括号的规格 320g（含4颗）||5545-545-45
            if (charStartP.Match(spec1).Success || haveBracket.Match(spec1).Success || allNum.Match(spec1).Success)
            {

                if (specName2.Replace(" ", "").Contains(spec1.Replace(" ", "")))
                {
                    return true;
                }
                return false;
            }

            if (spec1.Equals(spec2)) return true;

            if (getMulSpecisEqual(spec1, spec2) == true) return true;

            if (specPlusMul(spec1, spec2) == true) return true;
            var m1 = reg.Match(spec1);
            var m2 = reg.Match(spec2);
            if (m1.Success && m2.Success)
            {
                var num11 = (m1.Groups[2].Value == null || m1.Groups[2].Value == "") ? 1 : int.Parse(m1.Groups[2].Value);
                var num12 = (m1.Groups[4].Value == null || m1.Groups[4].Value == "") ? 1 : int.Parse(m1.Groups[4].Value);
                var num13 = (m1.Groups[13].Value == null || m1.Groups[13].Value == "") ? 1 : int.Parse(m1.Groups[13].Value);
                var unit1 = (m1.Groups[5].Value == null || m1.Groups[5].Value == "") ? "" : m1.Groups[5].Value;
                var unit2 = (m2.Groups[5].Value == null || m2.Groups[5].Value == "") ? "" : m2.Groups[5].Value;
                var num21 = (m2.Groups[2].Value == null || m2.Groups[2].Value == "") ? 1 : int.Parse(m2.Groups[2].Value);
                var num22 = (m2.Groups[4].Value == null || m2.Groups[4].Value == "") ? 1 : int.Parse(m2.Groups[4].Value);
                var num23 = (m2.Groups[13].Value == null || m2.Groups[13].Value == "") ? 1 : int.Parse(m2.Groups[13].Value);
                if (!unit1.Contains(unit2) && !unit2.Contains(unit1)) return false;

                //判断数字是否相等
                if (num11 == num21 && num12 == num22 && num13 == num23) return true;

                //再交换回来再比较一次
                num11 = num11 ^ num13;                      //执行变量交换         
                num13 = num13 ^ num11;
                num11 = num11 ^ num13;
                //判断数字是否相等
                if (num11 == num21 && num12 == num22 && num13 == num23) return true;
                //判断乘积是否相等
                //if (num12 == num22 && num11 * num12 * num13 == num21 * num21 * num21) return true;

                if ((num11 == 1 && num13 == 1) || (num21 == 1 && num23 == 1))
                {
                    if (num11 * num12 * num13 == num21 * num22 * num23) return true;
                }

                if (num12 == num22 && num11 * num12 * num13 == num21 * num22 * num23) return true;
            }
            return false;
        }



        public static Regex plusMul = new Regex(String.Format(@"^\d+({0})((\d+({1}))+)?$", regStr, regStr));
        public static bool specPlusMul(string spec1, string spec2)
        {
            if (plusMul.Match(spec1).Success && plusMul.Match(spec2).Success && spec1.Contains("+"))
            {

                var strArry1 = spec1.Split('*').OrderBy(r => r).ToList();
                var strArry2 = spec1.Split('*').OrderBy(r => r).ToList();
                if (strArry1.Count != strArry2.Count) return false;
                for (int i = 0; i < strArry1.Count; i++)
                {
                    if (strArry1[i] != strArry2[i]) return false;
                }
                return true;
            }

            return false;
        }




        /// <summary>
        /// 初始化list中的字符串，获取规格、以及初始化的索引位置存放到数据字典，
        /// </summary>
        /// <param name="list"></param>
        /// <returns>Dictionary<string, Tuple<string, int>>： <列表的item名称，Tuple<item提取的规格,item初始化位置>></returns>
        private static Dictionary<string, Tuple<string, int>> initDict(List<string> list)
        {
            //存放商品名称   规格  初始索引
            Dictionary<string, Tuple<string, int>> dict = new Dictionary<string, Tuple<string, int>>();
            int i = 0;
            //获取商品名称上的规格 并转换l to ml，已经存放在list位置上的索引
            list.ForEach(
                r =>
                {
                    if (!dict.ContainsKey(r.ToLower()))
                    {
                        string strs = UnitConversion.translationLtoML(PatterSpec.patterSpec(r));
                        dict.Add(r.ToLower(), Tuple.Create(strs, i));
                    }
                    i++;
                }
            );
            return dict;
        }

        /// <summary>
        /// Dictionary<string, Tuple<double, bool, int>>  string:list中item名称，double:文本相似度，规格是否相同，int:初始化位置
        /// </summary>
        /// <param name="dict">list列表初始化后的dict</param>
        /// <param name="dictDes">产品名称分词后产生的词向量数据字典 </param>
        /// <param name="productSpec">商品的规格</param>
        /// <returns>如果不匹配返回-1</returns>
        private static Dictionary<string, Tuple<double, bool, int>> afterHandleDict(Dictionary<string, Tuple<string, int>> dict, Dictionary<string, int> dictDes, string productSpec)
        {
            //商品名称对应的 文本的相似度 以及规格是否相同,初始化索引位置
            Dictionary<string, Tuple<double, bool, int>> dictMap = new Dictionary<string, Tuple<double, bool, int>>();
            foreach (var item in dict)
            {
                //获取相应的分词,分词前先去掉规格;
                var strName = (item.Value.Item1 == "" || item.Value.Item1 == null) ? item.Key : item.Key.Replace(item.Value.Item1, "");
                //去除名称的括号字符串
                //var str_ = AnnlyzerWord.regex.Replace(strName, "");
                var itemList = AnnlyzerWord.wordSegmentationToList(strName);
                var dictDes2 = AnnlyzerWord.getDictStrNum(itemList);
                //计算与目标字符串的文本相似度
                double p = AnnlyzerWord.repetitiveRate(dictDes, dictDes2);
                bool specEquals = false;
                if (productSpec != null && item.Value.Item1 != "" && item.Value.Item1 != null)
                {
                    specEquals = specEqual(productSpec, item.Value.Item1, item.Key);
                }
                dictMap.Add(item.Key, Tuple.Create(p, specEquals, item.Value.Item2));
            }
            return dictMap;
        }


        /// <summary>
        /// 通过规格来比较产品是否相同，严格规定规格一定要相同
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="productSpec"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int wordSimilarityIndexBySpec(string productName, string productSpec, List<string> list)
        {
            if (list == null || list.Count == 0) throw new Exception("列表中的数据为空");
            productName = productName.ToLower();
            productName = (productSpec == null || productSpec == "") ? productName : productName.Replace(productSpec, "");
            //  string productNameDealAfter = Regex.Replace(productName, @"[\(（【][\s\S]*?[\)）】]", "");
            string productNameDealAfter = productName;
            //存放商品名称   规格  索引
            Dictionary<string, Tuple<string, int>> dict = initDict(list);
            //语句分词后列表
            List<string> setDes = AnnlyzerWord.wordSegmentationToList(productNameDealAfter);
            //productName处理后分词后的词统计用数据字典表示
            var dictDes = AnnlyzerWord.getDictStrNum(setDes);

            //使用数据字典存放原来词汇的索引位置
            //商品名称 文本的相似度 以及规格是否相同,初始化索引位置
            Dictionary<string, Tuple<double, bool, int>> dictMap = afterHandleDict(dict, dictDes, productSpec);

            //获取含有规格字典数据
            var res = dictMap.Values.Where(r => r.Item2);
            //Tuple：相似度 以及规格是否相同,初始化索引位置
            IEnumerable<Tuple<double, bool, int>> orderResult = null;
            if (res != null && res.Count() > 0)
            {
                orderResult = res.OrderByDescending(r => r.Item1).Take(2).ToList();
                //先查找到规格相同的，规格相同的优先级别比较高，在从规格相同的按照相似度的降序排序，
                //获取出两条数据，再比较两条数据，如果相同相似度则比较初始化位置靠前的。
                if (orderResult.Count() == 1) return orderResult.First().Item3;
                else
                {
                    //返回相似度最高，如果相同则返回初始化位置最前的
                    if (orderResult.First().Item1 > orderResult.Last().Item1) return orderResult.First().Item3;
                    else if (orderResult.First().Item1 == orderResult.Last().Item1) return orderResult.Min(r => r.Item3);
                    else return orderResult.Last().Item3;
                }
            }
            return -1;
        }


        /// <summary>
        /// 返回列表中商品与productName比较相似度最高的字符串的在list中的索引,比较法则：规格相同优先->文本相似度高优先->在list中位置靠前优先
        /// </summary>
        /// <param name="productName">目标匹配商品</param>
        /// <param name="productSpec">目标匹配商品规格</param>
        /// <param name="list">待比较商品名称列表</param>
        /// <returns>列表中相似度最高的索引位置</returns>
        public static int wordSimilarityIndex(string productName, string productSpec, List<string> list)
        {
            if (list == null || list.Count == 0) throw new Exception("列表中的数据为空");
            if (list.Count == 1) return 0;
            productName = productName.ToLower();
            productName = (productSpec == null || productSpec == "") ? productName : productName.Replace(productSpec, "");
            //  string productNameDealAfter = Regex.Replace(productName, @"[\(（【][\s\S]*?[\)）】]", "");
            string productNameDealAfter = productName;
            //存放商品名称   规格  索引
            Dictionary<string, Tuple<string, int>> dict = initDict(list);


            //语句分词后列表
            List<string> setDes = AnnlyzerWord.wordSegmentationToList(productNameDealAfter);
            //productName处理后分词后的词统计用数据字典表示
            var dictDes = AnnlyzerWord.getDictStrNum(setDes);

            //使用数据字典存放原来词汇的索引位置
            //商品名称 文本的相似度 以及规格是否相同,初始化索引位置
            Dictionary<string, Tuple<double, bool, int>> dictMap = afterHandleDict(dict, dictDes, productSpec);

            //获取含有规格字典数据
            var res = dictMap.Values.Where(r => r.Item2);
            //Tuple：相似度 以及规格是否相同,初始化索引位置
            IEnumerable<Tuple<double, bool, int>> orderResult = null;
            if (res != null && res.Count() > 0)
            {
                orderResult = res.OrderByDescending(r => r.Item1).Take(2).ToList();
            }
            else
            {
                orderResult = dictMap.Values.OrderByDescending(r => r.Item1).Take(2).ToList();
            }
            //先查找到规格相同的，规格相同的优先级别比较高，在从规格相同的按照相似度的降序排序，
            //获取出两条数据，再比较两条数据，如果相同相似度则比较初始化位置靠前的。
            if (orderResult.Count() == 1) return orderResult.First().Item3;
            else
            {
                //返回相似度最高，如果相同则返回初始化位置最前的
                if (orderResult.First().Item1 > orderResult.Last().Item1) return orderResult.First().Item3;
                else if (orderResult.First().Item1 == orderResult.Last().Item1) return orderResult.Min(r => r.Item3);
                else return orderResult.Last().Item3;
            }
        }


        /// <summary>
        /// 通过规格来比较产品是否相同，严格规定规格要相同,或相似度超过0.85
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="productSpec"></param>
        /// <param name="list"></param>
        /// <param name="minSimilarity">如果不存在规格相同下，文本的相似需要不小minSimilarity，默认是0.85 </param>
        /// <returns></returns>
        public static int wordSimilarityIndexBySpecAndSimilarity(string productName, string productSpec, List<string> list, double minSimilarityAndSpec = 0.5, double minSimilarity = 0.85)
        {
            if (list == null || list.Count == 0) throw new Exception("列表中的数据为空");


            productName = productName.ToLower();
            productName = (productSpec == null || productSpec == "") ? productName : productName.Replace(productSpec, "");
            //  string productNameDealAfter = Regex.Replace(productName, @"[\(（【][\s\S]*?[\)）】]", "");
            string productNameDealAfter = productName;
            //存放商品名称   规格  索引
            Dictionary<string, Tuple<string, int>> dict = initDict(list);


            //语句分词后列表
            List<string> setDes = AnnlyzerWord.wordSegmentationToList(productNameDealAfter);
            //productName处理后分词后的词统计用数据字典表示
            var dictDes = AnnlyzerWord.getDictStrNum(setDes);

            //使用数据字典存放原来词汇的索引位置
            //商品名称 文本的相似度 以及规格是否相同,初始化索引位置
            Dictionary<string, Tuple<double, bool, int>> dictMap = afterHandleDict(dict, dictDes, productSpec);

            //获取含有规格字典数据
            var res = dictMap.Values.Where(r => ((r.Item2 && r.Item1 >= minSimilarityAndSpec) || r.Item1 >= minSimilarity));
            //Tuple：相似度 以及规格是否相同,初始化索引位置
            IEnumerable<Tuple<double, bool, int>> orderResult = null;
            if (res != null && res.Count() > 0)
            {
                orderResult = res
                    .OrderByDescending(r => r.Item2)
                    .ThenByDescending(r => r.Item1)
                    .ThenBy(r => r.Item3)
                    .Take(2).ToList();
            }
            else
            {
                orderResult = dictMap.Values.Where(r => r.Item1 >= minSimilarity).OrderByDescending(r => r.Item1).Take(2).ToList();
            }
            //先查找到规格相同的，规格相同的优先级别比较高，在从规格相同的按照相似度的降序排序，
            //获取出两条数据，再比较两条数据，如果相同相似度则比较初始化位置靠前的。
            if (orderResult.Count() == 1) return orderResult.First().Item3;
            else if (orderResult.Count() > 1)
            {
                //返回相似度最高，如果相同则返回初始化位置最前的
                if (orderResult.First().Item1 > orderResult.Last().Item1) return orderResult.First().Item3;
                else if (orderResult.First().Item1 == orderResult.Last().Item1) return orderResult.Min(r => r.Item3);
                else return orderResult.Last().Item3;
            }
            else
            {
                return -1;
            }
        }


        /// <summary>
        /// 计算目标字符串与列表字符串中的所有的相似度
        /// </summary>
        /// <param name="singleName">singleName</param>
        /// <param name="muchContextID_Text"><id,textContent></param>
        /// <returns>id,相似度</returns>
        public static Dictionary<String, double> calcSimilaritySingleToMuchJustContent(string singleName, Dictionary<string, string> muchContextID_Text)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();
            var singleNameSegmentation = wordSegmentationToList(singleName);
            var singleNameDict = getDictStrNum(singleNameSegmentation);
            foreach (var item in muchContextID_Text)
            {
                var itemSeg = wordSegmentationToList(item.Value);
                var itemDict = getDictStrNum(itemSeg);
                double similarity = repetitiveRate(singleNameDict, itemDict);
                result.Add(item.Key, similarity);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="productSpec"></param>
        /// <param name="muchContextID_Text"></param>
        /// <returns></returns>
        public static List<Tuple<int, double, decimal, string, string, DateTime>> calcSimilaritySingleToMuchJustContent(string productName, string productSpec, List<Tuple<int, string, decimal, string, DateTime>> muchContextID_Text)
        {
            List<Tuple<int, double, decimal, string, string, DateTime>> result = new List<Tuple<int, double, decimal, string, string, DateTime>>();
            var singleNameSegmentation = wordSegmentationToList(productName);
            var singleNameDict = getDictStrNum(singleNameSegmentation);
            foreach (var item in muchContextID_Text)
            {
                //获取商品的规格并进行单位转换
                var spec2 = UnitConversion.translationLtoML(PatterSpec.patterSpec(item.Item2));
                //第三个字段用于判断规格没有找到的情况下，判断是否存在一个型号相同
                var equl = specEqual(productSpec, spec2, item.Item2);

                var itemSeg = wordSegmentationToList(item.Item2);
                var itemDict = getDictStrNum(itemSeg);
                double similarity = equl == true ? 1.0 + repetitiveRate(singleNameDict, itemDict) : repetitiveRate(singleNameDict, itemDict);
                result.Add(new Tuple<int, double, decimal, string, string, DateTime>(item.Item1, similarity, item.Item3, item.Item2, item.Item4, item.Item5));
            }
            return result;
        }


    }
}
