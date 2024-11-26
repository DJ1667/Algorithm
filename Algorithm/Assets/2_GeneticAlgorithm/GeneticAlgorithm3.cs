using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//https://www.luogu.com.cn/problem/P5544

public class GeneticAlgorithm3 : MonoBehaviour
{
    const int N = 10010;
    const float eps = 1e-6f;
    const float limInitial = 1e4f;
    static int n, m;
    static float R, lim = limInitial;

    // Enemies class to store enemy positions
    struct Enemies
    {
        public float x, y;
    }

    // Buildings class to store buildings information
    struct Buildings
    {
        public float x, y, r;
    }

    static Enemies[] Enemy = new Enemies[N];
    static Buildings[] Build = new Buildings[N];

    // Calculate distance between two points (x1, y1) and (x2, y2)
    static float Dist(float x1, float y1, float x2, float y2)
    {
        float dx = x1 - x2;
        float dy = y1 - y2;
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    // Individual class to store each individual in the population
    class Individual : IComparable<Individual>
    {
        public float x, y, r;
        public int fitness;

        // Constructor to initialize an individual
        public Individual(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.r = CalcR(x, y);
            this.fitness = CalcFitness();
        }

        // Method to calculate the radius r for this individual
        public float CalcR(float x, float y)
        {
            float r = R;
            for (int i = 1; i <= n; i++)
                r = Math.Min(r, Dist(x, y, Build[i].x, Build[i].y) - Build[i].r);
            return Mathf.Max(r, 0.0f);
        }

        // Method to mate this individual with another one
        public Individual Mate()
        {
            float chx = x, chy = y;
            float dx = Random.Range(0, lim), dy = Random.Range(0, lim);
            bool opx = Random.Range(0, 2) == 0, opy = Random.Range(0, 2) == 0;

            chx += dx * (opx ? 1 : -1);
            chy += dy * (opy ? 1 : -1);

            return new Individual(chx, chy);
        }

        // Method to calculate the fitness of this individual
        public int CalcFitness()
        {
            int cnt = 0;
            for (int i = 1; i <= m; i++)
                if (Dist(this.x, this.y, Enemy[i].x, Enemy[i].y) - this.r <= eps)
                    cnt++;
            return cnt;
        }

        // IComparable implementation to sort individuals based on their fitness
        public int CompareTo(Individual other)
        {
            return other.fitness.CompareTo(this.fitness);
        }
    }

    void Start()
    {
        n = 30; //建筑数量
        m = 50; //敌人数量
        R = 5; //炸弹半径

        // Read building data
        for (int i = 1; i <= n; i++)
        {
            Build[i].x = Random.Range(0, 300);
            Build[i].y = Random.Range(0, 300);
            Build[i].r = Random.Range(0, 10);
        }

        // Read enemy data
        for (int i = 1; i <= m; i++)
        {
            Enemy[i].x = Random.Range(-150, 150);
            Enemy[i].y = Random.Range(-150, 150);
        }

        // Calculate center of mass of enemies
        float sx = 0.0f, sy = 0.0f;
        for (int i = 1; i <= m; i++)
        {
            sx += Enemy[i].x;
            sy += Enemy[i].y;
        }
        sx /= m;
        sy /= m;

        // Create initial population
        List<Individual> population = new List<Individual>();
        int POPULATION = 100, TIMES = 1000; // Assume these are given constants
        for (int i = 0; i < POPULATION; i++)
        {
            float x = Random.Range(0.0f, sx * 3.0f) * (Random.Range(0, 2) == 0 ? 1 : -1);
            float y = Random.Range(0.0f, sy * 3.0f) * (Random.Range(0, 2) == 0 ? 1 : -1);
            population.Add(new Individual(x, y));
        }

        // Genetic algorithm main loop
        for (int t = TIMES; t >= 1; t--)
        {
            // Sort population by fitness
            population.Sort();

            // Create new population
            List<Individual> newPopulation = new List<Individual>();
            int s = (10 * POPULATION) / 100;

            // Keep the top 10% as elite
            for (int i = 0; i < s; i++)
                newPopulation.Add(population[i]);

            // Fill the remaining population
            s = POPULATION - s;
            while (newPopulation.Count < POPULATION)
            {
                int len = population.Count;
                Individual p = population[Random.Range(0, 50)];
                Individual q = p.Mate();
                float delta = q.fitness - p.fitness;

                if (delta < 0)
                    newPopulation.Add(q);
                else if (Math.Exp(delta / t) > Random.Range(0.0f, 1.0f))
                    newPopulation.Add(q); // Accept with probability

            }

            // Replace old population with new one
            population = newPopulation;
            lim *= 0.75f;
        }

        // Output the best fitness
        Debug.Log(population[0].fitness);
    }
}
