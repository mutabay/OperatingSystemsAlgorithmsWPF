using System;

namespace FullProject.BankersAlgorithm_6
{
    public class Bankers
    {
        public int numberOfProcesses { get; set; }
        public int numberOfResources { get; set; }

        public int[,] needMatrix { get; set; }
        public int[,] maxDemandMatrix { get; set; }
        public int[,] allocationMatrix { get; set; }
        public int[] availableResources { get; set; }
        public int[] safeSequence { get; set; }

        public string showSafeSequence { get; set; }

        public Bankers(int numberOfProcesses, int numberOfResources,
            int[,] maxDemandMatrix, int[,] allocationMatrix, int[] availableResources)
        {
            this.numberOfProcesses = numberOfProcesses;
            this.numberOfResources = numberOfResources;
            this.maxDemandMatrix = maxDemandMatrix;
            this.allocationMatrix = allocationMatrix;
            this.availableResources = availableResources;
            safeSequence = new int[numberOfProcesses];
            needMatrix = new int[numberOfProcesses, numberOfResources];
            FillNeedMatrix();
            showSafeSequence = "";
        }


        private void FillNeedMatrix()
        {
            for (int i = 0; i < numberOfProcesses; i++)
            {
                for (int j = 0; j < numberOfResources; j++)
                    needMatrix[i, j] = maxDemandMatrix[i, j] - allocationMatrix[i, j];
            }
        }

        public bool IsSafeState()
        {
            int count = 0;

            // visited array to find the 
            // already allocated process
            Boolean[] visited = new Boolean[numberOfProcesses];
            for (int i = 0; i < numberOfProcesses; i++)
            {
                visited[i] = false;
            }

            // work array to store the copy of 
            // available resources
            int[] work = new int[numberOfResources];
            for (int i = 0; i < numberOfResources; i++)
            {
                work[i] = availableResources[i];
            }

            while (count < numberOfProcesses)
            {
                Boolean flag = false;
                for (int i = 0; i < numberOfProcesses; i++)
                {
                    if (visited[i] == false)
                    {
                        int j;
                        for (j = 0; j < numberOfResources; j++)
                        {
                            if (work[j] < needMatrix[i, j])
                                break;
                        }
                        if (j == numberOfResources)
                        {
                            safeSequence[count++] = i;
                            visited[i] = true;
                            flag = true;
                            for (j = 0; j < numberOfResources; j++)
                            {
                                work[j] = work[j] + allocationMatrix[i, j];
                            }
                        }
                    }
                }
                if (flag == false)
                {
                    break;
                }
            }

            if (count < numberOfProcesses)
                return false;
            else
            {
                // Fill showSafeSequence
                for (int i = 0; i < numberOfProcesses; i++)
                {
                    showSafeSequence += "P" + safeSequence[i];
                    if (i != numberOfProcesses - 1)
                        showSafeSequence += " -> ";
                }
                return true;
            }
        }



    }
}
