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
namespace WordSimilarityP
{
    /// <summary>
    /// 分词并处理相似度
    /// </summary>
    public class AnnlyzerWord
    {
        /// <summary>
        ///字符串分词之后返回一个不重复的数组
        /// </summary>
        /// <param name="text">待分词的字符串</param>
        /// <returns></returns>   wordSegmentation
        public static SortedSet<string> wordSegmentation(string text)
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
            return list;
        }

        private static Dictionary<string, int> getDictStrNum(List<string> list1)
        {

            Dictionary<string, int> dict1 = new Dictionary<string, int>();
            list1.ForEach(r =>
            {
                if (dict1.ContainsKey(r)) dict1[r] = dict1[r] + 1;
                else
                {
                    dict1.Add(r, 1);
                }
            });
            return dict1;
        }

        /// <summary>
        /// 余弦相似度，值越高相似度越高
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
        /// <summary>
        /// 返回列表中商品与productName比较相似度最高的字符串的在list中的索引
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
            string productNameDealAfter = Regex.Replace(productName.Replace(productSpec, ""), @"[\(（【][\s\S]*?[\)）】]", "");
            List<string> setDes = AnnlyzerWord.wordSegmentationToList(productNameDealAfter);
            var dictDes = AnnlyzerWord.getDictStrNum(setDes);
            //商品名称   规格  索引
            Dictionary<string, Tuple<string, int>> dict = new Dictionary<string, Tuple<string, int>>();
            int i = 0;
            //获取商品名称上的规格 并转换l to ml，已经存放在list位置上的索引
            list.ForEach(
                r =>
                dict.Add(r.ToLower(), Tuple.Create(UnitConversion.translationLtoML(PatterSpec.patterSpec(r)), i++))
            );  //使用数据字典存在原来词汇的索引位置

            //商品名称对应的 文本的相似度 以及规格是否相同,初始化索引位置
            Dictionary<string, Tuple<double, bool, int>> dictMap = new Dictionary<string, Tuple<double, bool, int>>();

            foreach (var item in dict)
            {
                //获取相应的分词,分词前先去掉规格;
                var itemList = AnnlyzerWord.wordSegmentationToList(Regex.Replace(item.Key.Replace(item.Value.Item1, ""), @"[\[(（【](.)*?[)）】\]]", ""));
                var dictDes2 = AnnlyzerWord.getDictStrNum(itemList);
                //计算与目标字符串的文本相似度
                double p = AnnlyzerWord.repetitiveRate(dictDes, dictDes2);
                bool specEqual = false;
                if (productSpec != null && item.Value.Item1 != "" && item.Value.Item1 != null)
                {
                    specEqual = productSpec.ToLower().Equals(item.Value.Item1);
                }
                dictMap.Add(item.Key, Tuple.Create(p, specEqual, item.Value.Item2));
            }

            var res = dictMap.Values.Where(r => r.Item2);
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
                return orderResult.First().Item1 > orderResult.Last().Item1 ? orderResult.First().Item3 : orderResult.Last().Item3;
            }


        }
    }
}
