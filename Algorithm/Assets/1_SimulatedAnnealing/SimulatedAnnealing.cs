using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulatedAnnealing : MonoBehaviour
{
    // 初始温度、冷却率、最大迭代次数等参数
    public float initialTemperature = 100f;
    public float coolingRate = 0.99f;
    public int maxIterations = 1000;

    // 当前解的坐标，目标函数可以在这里修改
    private Vector2 currentSolution;
    private Vector2 bestSolution;

    // 目标函数，这里是一个示例，可以根据需求修改
    private float ObjectiveFunction(Vector2 solution)
    {
        // 示例：一个简单的二次函数（可以自定义优化目标）
        return (solution.x - 3) * (solution.x - 3) + (solution.y - 2) * (solution.y - 2);
    }

    void Start()
    {
        // 初始解
        currentSolution = new Vector2(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f));
        bestSolution = currentSolution;

        // 执行模拟退火算法
        SimulateAnnealing();
    }

    void SimulateAnnealing()
    {
        float temperature = initialTemperature;

        for (int iteration = 0; iteration < maxIterations; iteration++)
        {
            // 随机生成一个新的邻域解
            Vector2 newSolution = GetNeighborSolution(currentSolution);

            // 计算当前解和新解的目标函数值
            float currentEnergy = ObjectiveFunction(currentSolution);
            float newEnergy = ObjectiveFunction(newSolution);

            // 如果新解更好，接受新解
            if (newEnergy < currentEnergy)
            {
                currentSolution = newSolution;
            }
            else
            {
                // 如果新解更差，根据温度决定是否接受
                float acceptanceProbability = Mathf.Exp((currentEnergy - newEnergy) / temperature);
                if (UnityEngine.Random.value < acceptanceProbability)
                {
                    currentSolution = newSolution;
                }
            }

            // 更新最佳解
            if (ObjectiveFunction(currentSolution) < ObjectiveFunction(bestSolution))
            {
                bestSolution = currentSolution;
            }

            // 降温
            temperature *= coolingRate;

            // 输出当前解
            Debug.Log($"Iteration: {iteration}, Temp: {temperature}, Best Solution: {bestSolution}, Objective: {ObjectiveFunction(bestSolution)}");
        }

        // 最终输出最优解
        Debug.Log($"Final Best Solution: {bestSolution}, Objective: {ObjectiveFunction(bestSolution)}");
    }

    // 随机生成一个邻域解
    Vector2 GetNeighborSolution(Vector2 solution)
    {
        float deltaX = UnityEngine.Random.Range(-1f, 1f);
        float deltaY = UnityEngine.Random.Range(-1f, 1f);
        return new Vector2(solution.x + deltaX, solution.y + deltaY);
    }
}
