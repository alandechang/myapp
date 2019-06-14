using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace 最佳工作序列
{
    class 最佳工作
    {
        static List<int> Require = new List<int>();//费时
        static List<int> Limit = new List<int>();//最后完成期限
        static List<int> sum = new List<int>();//前面n个任务的费时和
        static List<double> valueAverage = new List<double>();//价值
        static List<int> order = new List<int>();//最佳序列
        static string[] readText;


       
        static int Search(int[] Limit, int value)//value在此是limit.Min（），最小值
        {
            int i = -1;
            foreach (int t in Limit)
            {
                i++;
                if (t == value)
                    return i;//返回所在的位置，碰见return就跳出循环了，不执行后边的东西
            }
            return i;
        }

      
        static void Con(int[] order, int nwork, int[] sum, double[] valueAverage, int[] RequireTime, int[] Limit)
        {
           
            if (valueAverage[order[order.Length - 2]] < valueAverage[nwork])
            {
                
                if (sum[sum.Length - 1] < Limit[order[order.Length - 2]])
                {
                    int temp = order[order.Length - 2];
                    order[order.Length - 1] = temp;
                    order[order.Length - 2] = nwork;

                    if (sum.Length < 3)
                    {
                        int s = sum[sum.Length - 2];
                        sum[sum.Length - 2] = RequireTime[nwork];
                        sum[sum.Length - 1] = s;
                    }
                    else
                    {
                        int s = sum[sum.Length - 2] - sum[sum.Length - 3];
                        sum[sum.Length - 2] = sum[sum.Length - 3] + RequireTime[nwork];
                        sum[sum.Length - 1] = sum[sum.Length - 2] + s;
                    }

                    if (order.Length > 2)
                    {
                        int[] torder = new int[order.Length - 1];
                        int[] tsum = new int[sum.Length - 1];
                        for (int j = 0; j < order.Length - 1; j++)
                        {
                            torder[j] = order[j];
                            tsum[j] = sum[j];
                        }

                        Con(torder, nwork, tsum, valueAverage, RequireTime, Limit);

                        for (int j = 0; j < torder.Length; j++)
                        {
                            order[j] = torder[j];
                            sum[j] = tsum[j];
                        }
                    }
                }
            }
        }


        public static void read()
        {
            StreamReader sw = new StreamReader(@"D:\Bruce\class2second\地理信息系统算法基础\跳马\data.txt");
            string st = string.Empty;
            int m = 0;
            while (!sw.EndOfStream)//EndOfStream没有到结尾
            {
                m++;//统计行数
                string s = sw.ReadLine();//读一行字符并将数据作为字符串返回
                Console.WriteLine(s);
            }
            Console.WriteLine("");

            readText = File.ReadAllLines(@"D:\Bruce\class2second\地理信息系统算法基础\跳马\data.txt");//打开一个文本文件，读取其中的所有行，然后关闭

            try
            {

                for (int i = 0; i < m; i++)
                {
                    string data = Convert.ToString(readText[i]);//将读取的内容转换成字符串

                    if (i != 0)//i=0，完美地跳过了第一行的标题，等到i<m的时候，整个外循环就结束了，内循环不得不结束。
                    {//Substring是给出起始位置和长度进行截取，IndexOf是给出字符（串）的下标位置
                        data = data.Substring(data.IndexOf('\t') + 1, data.Length - 1 - data.IndexOf('\t'));//跳过了序号，data是从Require开始到最后
                        string d;
                        d = data.Substring(0, data.IndexOf('\t'));//截取Require字符
                        Require.Add(int.Parse(d));//将其转化为数字并压入数组
                        data = data.Substring(data.IndexOf('\t') + 1, data.Length - 1 - data.IndexOf('\t'));//在刚才提取的基础上又跳过了Require，到最后
                        d = data.Substring(0, data.IndexOf("\t"));//截取Limit的字符，这个“t”不一样
                        Limit.Add(int.Parse(d));//将其转化为数字并压入数组
                        d = data.Substring(data.IndexOf('\t') + 1, data.Length - 1 - data.IndexOf('\t'));
                        double x = Convert.ToDouble(int.Parse(d)) / Convert.ToDouble((Require[Require.Count - 1]));
                        valueAverage.Add(x);//价值比
                    }

                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("文件读取错误！");
                Console.WriteLine(ex.Message);
            }
        }


        static void Main(string[] args)
        {
            
            read();


            int[] array;
            array = new int[Limit.Count];//Count，包含的个数T
            double[] valueAver = new double[Limit.Count];
            int[] limit = new int[Limit.Count];
            Limit.CopyTo(limit, 0);//浅拷贝给刚刚实例化的limit

            for (int i = 0; i < Limit.Count; i++)
            {
                valueAver[i] = valueAverage[i];//赋值平均价值比
            }


            while (order.Count < Limit.Count)
            {
               
                int i = Search(limit, limit.Min());//返回了最小值的位置

                limit[i] = 10000;//最小期限的值无穷大
                order.Add(i);//暂时排序

                if (order.Count > 1)
                {
                    array = order.ToArray();
                    Con(array, i, sum.ToArray(), valueAver, Require.ToArray(), Limit.ToArray()); 
                    order = array.ToList();
                }
                else if (order.Count < 2)
                {
                    sum.Add(sum.Sum() + Require[i]);//总时间加上现在的
                }
                else
                {
                    sum.Add(sum[sum.Count - 1] + Require[i]);
                }
            }

            
            Console.WriteLine("最佳工作序列为：");
            foreach (int i in order)
            {
                Console.Write("{0} ", i + 1);
            }
            Console.ReadLine();
        }
    }
}

