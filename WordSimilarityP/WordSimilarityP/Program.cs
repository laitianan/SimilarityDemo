using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucene.Net.Analysis.Cn.PowerAnalyzers.Configurantions;
using WordSimilarity;
using System.Text.RegularExpressions;
using LSH;
namespace WordSimilarityP
{
    class Program
    {
        static void Main(string[] args)
        {

            int a = LevenshteinDistance.Compute(@"D:\VSSource\WordSimilarityP\WordSimilarityP\package", @"D:\VSSource\WordSimilarityP\WordSimilarityP\package\Dicts\");
            Console.WriteLine(a);
            int a2 = LevenshteinDistance.Compute("aaa", "aa");
            Console.WriteLine(a2);
            Console.ReadKey();
            return;

            Configurantions.DictionaryHome = @"D:\VSSource\WordSimilarityP\WordSimilarityP\package\Dicts\";

            var unit = UnitConversion.unitTranslation("100.23kg");
            var equ = AnnlyzerWord.specEqual("480g", "80gx6", "");
            Console.WriteLine(equ);
            var equ2 = AnnlyzerWord.specEqual("60g", "6*10g", "");
            Console.WriteLine(equ2);
            var equ3 = AnnlyzerWord.specEqual("6g*6", "6*6g", "");
            Console.WriteLine(equ3);
            var equ4 = AnnlyzerWord.specEqual("480g", "480g", "");
            Console.WriteLine(equ4);
            var equ5 = AnnlyzerWord.specEqual("3*6*120g", "6*3*120g", "");

            Console.WriteLine(equ5);
            var equ6 = AnnlyzerWord.specEqual("60*10g", "10*60g", "");
            Console.WriteLine(equ6);
            if (true) return;
            //Console.WriteLine(unit);
            var lda1 = "近日，国内一项调查结果表明：10%的人喜欢开轿车，13%的人喜欢开面包车，22%的人喜欢开越野车，55%的人喜欢多功能车。他们对爱车的选择标准从以前的感性转为务实、审慎和理性。对于中国广大消费者而言，一辆既能作为日常家庭用车又兼具部分商务功能的多功能轿车，理所当然是他们的首选。　　由世界顶级汽车设计师JustynNorek操刀设计，呈现我们眼前的陆风风尚造型既不乏东方的质朴含蓄，又彰显了西方的尊荣大方，是东方文化和西方风韵碰撞的智慧的结晶。配置上非常丰富，完全达到目前国内中档轿车水平；而在安全性上，该车型为驾乘者提供全面的呵护，先进而完善的装备提供了最佳的主被动安全保障。陆风风尚(报价;图片)作为一款设计理念先进、功能完善、品质优良的车型，历时四年开发，向国家专利部门申请了133项专利，另一款具有欧洲血统的“全球资源车”。凭借陆风的强势品牌形象和完善服务网络，陆风今后将极有可能成为多功能轿车市场上的领导者。";
            var lda2 = "在政府部门明确表示汽车投资过热、产能过剩的背景下，日前新飞集团却逆流而上：专用汽车工业园在河南新乡市开发区正式开始建设。该项目占地８００余亩，计划总投资１０亿元，其中一期工程计划投资３亿元，年产专用汽车１万辆，计划２００７年年底建成；二期工程计划投资７亿元，年产专用汽车３万辆，计划２０１０年建成；预计年销售收入２０亿元、年利税２亿元。　　据了解，新飞集团在２００３年开始投资冷藏专用车。２００５年河南新乡专用汽车厂被划归新飞集团，其总部地处河南新乡高新技术开发区，拥有新乡、鹤壁两处生产基地，专业研发生产冷藏车(报价;图片)、保温车、邮政车、军用通信车、半挂车等厢式、工程、军用三大系列３００多个品种。目前，新飞集团以冷藏车为主导的专用汽车项目年生产规模为５０００辆。　　新飞一高层曾表示，冰箱行业利润越来越薄，很难带动新飞下一步的高速发展，新飞必须有新的发展空间。“国有股退出肯定是正确的，可以把资金用于新的利润增长点”。２００５年，在将新飞公司股权大量转让之后，新飞集团把主要精力放在信息安全产业和专用车产业上，５亿元转让款则用于集团新业务的发展，也就是新飞所谓的“二次创业”。　　有家电专家提出，虽然汽车业利润逐渐摊薄，其平均利润率仍比家电业高出３倍以上。或许，这正是家电企业前赴后继地投奔汽车业的主要原因，江苏春兰、宁波波导、广东美的、河南新飞、宁波奥克斯等等都曾与汽车有着千丝万缕的关系。　　“资金对我们而言不成问题。”奥克斯的一位负责人曾这样告诉记者。在上世纪９０年代中后期，国内家电巨头大都赚得盆盈钵满，有雄厚资金做支撑的他们在企业发展的交叉口，不约而同地选择了多元化发展来避开行业风险，而同属制造业、利润相对可观的汽车业成为其首选。然而，在消费日趋理性、产业生存的起点拔高、新汽车产业政策对跨产品类别生产轿车的门槛增高的前提下，短时期内，“外行”要在技术研发水平、产品制造工艺、质量保证体系和营销服务网络方面形成竞争优势谈何容易。曾高调进军汽车业的奥克斯最后也只落得一个黯然离开的下场。　　“到２００６年，我们要争做中国冷藏车第一品牌。”新飞高层表示。然而，不立足核心技术的开发，仅仅以利润获取为直接目的，如何成就第一？　　奥克斯的退出曾引起人们对企业多元化的思考。众所周知，企业寻求的第二产业最好与第一产业有互通性。对此，新飞认为进军冷藏车与制造冰箱有异曲同工之妙，都是围绕“冷”作文章。“看似冰箱与汽车不搭界，但是我们做冷藏车，其实就是把冰箱、冰柜放大后搬到汽车上。”新飞一人士称。言语简单，然而造车能如此简单？　　“家电企业在市场拓展方面积累了不少经验，这是家电企业进入汽车业的优势之一。但是，汽车行业是一个资金和技术双密集型的产业，只有兼备雄厚的技术储备和资金实力的企业才能保持恒久的优势，也更有持久作战的本钱。”中央财经大学金融学院教授、金融证券研究所副所长韩复龄曾在接受本报记者采访时称，倘若新飞也如奥克斯、春兰般只在汽车业边缘轰轰烈烈地走一遭，实无必要费时费力。另一方面，虽然专用车的市场潜力被业内看好，但２００５年全国专用车市场销量下降是不争的事实，缺乏品牌价值支撑的新飞能否冲出重围还得打一个问号。";
            var list1 = AnnlyzerWord.wordSegmentationToList(lda1);
            var list2 = AnnlyzerWord.wordSegmentationToList(lda2);
            var dict1 = AnnlyzerWord.getDictStrNum(list1);
            var dict2 = AnnlyzerWord.getDictStrNum(list2);
            var rep = AnnlyzerWord.repetitiveRate(dict1, dict2);
            Console.WriteLine(rep);
            //Console.ReadKey();
            //if (true) return;

            var now1 = DateTime.Now;
            Console.WriteLine(now1.ToString("yyyy/MM/dd HH:mm:ss:fffffff "));

            var now2 = DateTime.Now;
            Console.WriteLine(now2.ToString("yyyy/MM/dd HH:mm:ss:fffffff "));
            Console.WriteLine((now2 - now1));
            //var list = new List<string>() {
            //    "洗面奶120g控油补水温和修护保湿",

            //    "【天猫超市】郁美净鲜奶橄榄润颜洗面奶130g控油补水温[dfdafdsa]",
            //    "【天猫超市】郁美净鲜奶橄榄润颜洗面奶100g控油补水温和修护保k湿",
            //     "【天猫超市】净鲜奶橄榄润颜洗面奶140g控油补水温和修护保湿",
            //     "【天猫超市】郁美净鲜奶橄榄润颜洗面奶100g控油补水温和修护保s湿[dfdafdsa]"

            //};


            string s2 = "潘婷洗发露乳液修护型750ml+护发素400ml+80ml*3 潘婷洗发露乳液修护型750ml+护发素400ml+80ml/瓶*3";
            string s1 = "多芬 滋养水润洗发乳/水/露700ml+护发素195ml*2";
            string s3 = "高姿COGI匀净萃白10件套（修容霜5g*2+洁面15g+水10ml*2+乳10g+霜5g+面贴膜20ml*3）（小样套装）不建议购买";

            string s5 = "丝蕴臻粹莹润套装500ml洗发水500ml护发素精油滋养";
            string s4 = "施华蔻多效修护19套装600ml洗发水700ml护发素送小样";

            string s6 = "海飞丝男士洗发水净爽去油套装（500ml洗发水*2+380ml" +
                    "洗发水*1+80ml丝质柔滑洗发露*2）";

            string s7 = "欧莱雅 (LOREAL)多效修复洗护套装（多效修复洗发露400mlx2+多效修复润发乳 200ml）";

            string s8 = "潘婷洗发水 洗护套装乳液750ml护发素400ml赶紧80ml*5";
            string s9 = "潘婷洗护发套装 乳液修复500ml护发素500ml送小样";

            string s10 = "卡乐比薯条 日本进口零食 卡乐比 薯条三兄弟 90克黄油酱油味 休闲食品 黄油90克/袋";
            string s11 = "Schwarzkopf施华蔻羊绒脂滋养套装（200+200+50+10）";
            Console.WriteLine("1: " + s1 + "\t" + PatterSpec.patterSpec(s1));
            Console.WriteLine("2: " + s2 + "\t" + PatterSpec.patterSpec(s2));
            Console.WriteLine("3: " + s3 + "\t" + PatterSpec.patterSpec(s3));
            Console.WriteLine("4: " + s4 + "\t" + PatterSpec.patterSpec(s4));
            Console.WriteLine("5: " + s5 + "\t" + PatterSpec.patterSpec(s5));
            Console.WriteLine("6: " + s6 + "\t" + PatterSpec.patterSpec(s6));
            Console.WriteLine("7: " + s7 + "\t" + PatterSpec.patterSpec(s7));
            Console.WriteLine("8: " + s8 + "\t" + PatterSpec.patterSpec(s8));
            Console.WriteLine("9: " + s9 + "\t" + PatterSpec.patterSpec(s9));
            Console.WriteLine("10: " + s10 + "\t" + PatterSpec.patterSpec(s10));
            Console.WriteLine("11: " + s11 + "\t" + PatterSpec.patterSpec(s11));


            //Console.WriteLine(patterSpec(s11 ));
            string s12 = "【天猫超市】日本进口 AGF马克西姆冻干速溶咖啡粉（瓶装）3*80g/瓶";
            string s13 = "谜尚（MISSHA）舒缓补水芦荟面膜（5片*25ml）";
            string s14 = "【京东超市】滋源无患子控油清爽洗护特惠套装（ 535ml+265ml）";
            string s15 = "【京东超市】丝蓓绮炫魅滋养洗发露3支分享装（洗发露550ml*3）";
            string s16 = "资生堂沐浴露可悠然美肌沐浴露550mlX3瓶 Item#:147556";
            string s17 = "【京东超市】威露士（walch）健康香皂 清新青柠 125g×4";
            string s18 = "【京东超市】威露士（walch）健康香皂 清新青柠 125g";
            string s19 = "简佳防水卷纸架(JJ-6031) 17.5*13.6*15.5cm";

            string s20 = "简佳防水40g+40g*3";
            string s21 = "简佳防水1刀架+1刀头";
            Console.WriteLine("s11: " + s11 + "\t" + PatterSpec.patterSpec(s11));
            Console.WriteLine("12: " + s12 + "\t" + PatterSpec.patterSpec(s12));
            Console.WriteLine("s13: " + s13 + "\t" + PatterSpec.patterSpec(s13));
            Console.WriteLine("s14: " + s14 + "\t" + PatterSpec.patterSpec(s14));
            Console.WriteLine("s15: " + s15 + "\t" + PatterSpec.patterSpec(s15));
            Console.WriteLine("s16: " + s16 + "\t" + PatterSpec.patterSpec(s16));
            Console.WriteLine("s17: " + s17 + "\t" + PatterSpec.patterSpec(s17));
            Console.WriteLine("s18: " + s18 + "\t" + PatterSpec.patterSpec(s18));
            Console.WriteLine("s19: " + s19 + "\t" + PatterSpec.patterSpec(s19));
            Console.WriteLine("s20: " + s20 + "\t" + PatterSpec.patterSpec(s20));
            Console.WriteLine("s21: " + s21 + "\t" + PatterSpec.patterSpec(s21));
            string s22 = "【京东超市】马来西亚进口可康（cocon）多口味优格果冻210g（6*35g）";
            Console.WriteLine("s22: " + s22 + "\t" + PatterSpec.patterSpec(s22));
            Console.WriteLine(UnitConversion.translationLtoML("3*3*2L"));
            Console.WriteLine(UnitConversion.translationLtoML("3*3*2L"));
            // var set = AnnlyzerWord.wordSegmentation("【天猫超市】Schwarzkopf施华蔻斐丝丽睡莲洗发露450ml 花香补水");
            //foreach (var str in set)
            //{
            //    Console.Write(str + "   ");
            //}

            //var list = new List<string>()
            //{
            //    "洗面奶",
            //    "沐浴露",
            //    "牙膏"
            //};


            var list = new List<string>() {
                "德芙巧克力M豆牛奶彩豆分享装160g 节日礼物零食"

            };
            Regex regex = new Regex(@"[\[(（【](.)*?[)）】\]]");

            regex.Replace("", "");

            var now3 = DateTime.Now;
            Console.WriteLine(now3.ToString("yyyy/MM/dd HH:mm:ss:fffffff "));
            Console.WriteLine(AnnlyzerWord.wordSimilarityIndexBySpecAndSimilarity("一皇香脆薯片(酸奶油口味)", "160g", list));
            var now4 = DateTime.Now;
            Console.WriteLine(now2.ToString("yyyy/MM/dd HH:mm:ss:fffffff "));
            Console.WriteLine((now4 - now3));

            Console.WriteLine(AnnlyzerWord.specEqual("200g", "0.2kg", "荷裕烟熏三文鱼M-XL"));
            //   Console.ReadKey();
        }
    }
}
