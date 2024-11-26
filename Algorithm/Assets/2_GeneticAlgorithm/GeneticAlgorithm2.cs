using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.luogu.com.cn/problem/UVA10228

public class GeneticAlgorithm2 : MonoBehaviour
{
    const int N = 110;
    const int POPULATION = 100;
    const int TIMES = 1000;
    const int LEN = 10;

    static int n;
    static (float x, float y)[] a = new (float, float)[N]; // 存储 n 个点


    // 计算两点之间的距离
    static float Dist(float x1, float y1, float x2, float y2)
    {
        float dx = x1 - x2;
        float dy = y1 - y2;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    // 存储个体信息的结构体
    public class Individual
    {
        public int x, y; // 个体的横纵坐标
        public float fitness; // 适应度

        public Individual(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.fitness = CalcFitness();
        }

        // 交配
        //针对这个问题，这个算法进行改进：
        //为了避免答案精度过低，我们可以将父母双亲交配改成父代自交（也就是复制自己，随机变异几个点）产生后代。
        //当然，我们会调高变异率。
        public Individual Mate()
        {
            int chx = x, chy = y;
            int p1 = Random.Range(0, 4); // 将 x 随机变异三个二进制位
            for (int i = 1; i <= p1; i++)
            {
                int p = Random.Range(0, LEN + 1);
                if ((chx >> p & 1) == 1)
                    chx -= (1 << p);
                else
                    chx += (1 << p);
            }

            int p2 = Random.Range(0, 4);
            for (int i = 1; i <= p2; i++)
            {
                int p = Random.Range(0, LEN + 1);
                if ((chy >> p & 1) == 1)
                    chy -= (1 << p);
                else
                    chy += (1 << p);
            }

            return new Individual(chx, chy);
        }

        // 计算适应度
        public float CalcFitness()
        {
            float ans = 0;
            for (int i = 1; i <= n; i++)
            {
                ans += Dist(a[i].x, a[i].y, x, y);
            }
            return ans;
        }

        public static int CompareFitness(Individual a, Individual b)
        {
            return a.fitness.CompareTo(b.fitness);
        }
    }

    void Start()
    {

        n = 6; //随机生成 n 个点

        for (int i = 1; i <= n; i++)
        {
            a[i] = (Random.Range(0, 1000), Random.Range(0, 1000));
        }

        List<Individual> population = new List<Individual>();

        // 初始化种群
        for (int i = 0; i < POPULATION; i++)
        {
            int x = Random.Range(0, 1000);
            int y = Random.Range(0, 1000);
            population.Add(new Individual(x, y));
        }

        // 遗传算法主循环
        for (int i = TIMES; i >= 1; i--)
        {
            population.Sort(Individual.CompareFitness);
            List<Individual> newPopulation = new List<Individual>();

            int s = POPULATION / 10;
            // 保留精英种子
            for (int j = 0; j < s; j++)
                newPopulation.Add(population[j]);

            s = POPULATION - s;
            while (newPopulation.Count < POPULATION)
            {
                int len = population.Count;
                Individual p = population[Random.Range(0, len / 2)];
                Individual q = p.Mate();

                float delta = q.fitness - p.fitness;
                if (delta < 0)
                    newPopulation.Add(q);
                else if (Mathf.Exp(-delta / i) / 2.0 > Random.Range(0, 1f))
                    newPopulation.Add(q); // 概率接受
            }

            population = newPopulation;
        }

        // 输出最优适应度
        Debug.Log($"{population[0].fitness:F0}");

    }
}
