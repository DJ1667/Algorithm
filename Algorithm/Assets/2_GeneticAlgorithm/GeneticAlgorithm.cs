using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    const int POPULATION = 1000;
    const string target = "I've got it!";
    const string GeneBase = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOP" +
                           "QRSTUVWXYZ 1234567890, .-;:_!\"#%&/()=?@${[]}'"; // 基因库
    static char Mutate()
    {
        return GeneBase[Random.Range(0, GeneBase.Length)];
    }

    static string Create() // 产生新的染色体
    {
        string chromosome = "";
        int len = target.Length;
        for (int i = 0; i < len; i++)
        {
            chromosome += Mutate();
        }
        return chromosome;
    }

    class Individual // 个体
    {
        public string Chromosome { get; set; } // 染色体
        public int Fitness { get; set; } // 适应度

        public Individual(string chromosome)
        {
            Chromosome = chromosome;
            Fitness = CalcFitness();
        }

        public Individual Mate(Individual parent) // 父母双方交配产生后代
        {
            string child = ""; // 后代
            int len = Chromosome.Length;
            for (int i = 0; i < len; i++)
            {
                float p = Random.Range(0, 100) / 100f; // 计算交配概率
                if (p <= 0.5f) // 如果概率小于等于 0.5，则从母亲(chromosome)遗传该基因
                    child += Chromosome[i];
                else // 从父亲遗传(parent)
                    child += parent.Chromosome[i];
            }
            return new Individual(child);
        }

        public int CalcFitness() // 计算适应度
        {
            int len = target.Length;
            int nowFitness = 0;
            for (int i = 0; i < len; i++)
            {
                if (Chromosome[i] != target[i])
                    nowFitness++;
            }
            return nowFitness;
        }

        public void Mutation() // 随机变异
        {
            int P = Random.Range(1, 3), LEN = target.Length;
            for (int i = 1; i <= P; i++)
            {
                int pos = Random.Range(0, LEN);
                char newGene = Mutate();
                char[] chromosomeArray = Chromosome.ToCharArray();
                chromosomeArray[pos] = newGene;
                Chromosome = new string(chromosomeArray);
            }
            Fitness = CalcFitness();
        }
    }

    void Start()
    {
        int cnt = 0; // 计算当前迭代到第几代
        bool found = false;
        List<Individual> population = new List<Individual>();

        for (int i = 0; i < POPULATION; i++) // 随机个体
        {
            population.Add(new Individual(Create()));
        }

        while (!found)
        {
            population.Sort((a, b) => a.Fitness.CompareTo(b.Fitness)); // 按照适应度排序
            if (population[0].Fitness == 0)
            {
                found = true;
                break;
            }

            List<Individual> newPopulation = new List<Individual>();
            int s = POPULATION / 10; // 保留精英种子
            for (int i = 0; i < s; i++)
            {
                newPopulation.Add(population[i]);
            }

            s = POPULATION - s; // 剩下的随机交配
            for (int i = 0; i < s; i++)
            {
                int len = population.Count;
                //只从前一半的种群中选择父母
                Individual p1 = population[Random.Range(0, len / 2)];
                Individual p2 = population[Random.Range(0, len / 2)];
                Individual ch = p1.Mate(p2);
                int P = Random.Range(0, 100);
                if (P <= 20) ch.Mutation();
                newPopulation.Add(ch);
            }

            population = newPopulation;

            Debug.Log($"Generation: {cnt}  String: {population[0].Chromosome}  Fitness: {population[0].Fitness}");
            cnt++;
        }

        Debug.Log($"Generation: {cnt}  String: {population[0].Chromosome}  Fitness: {population[0].Fitness}");
    }
}
