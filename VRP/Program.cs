using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        
        double[,] coordinates = { { 38.36269, 34.00775 }, { 38.3629142554386, 34.0286123907685 }, { 38.3905898269593, 34.1367241888959 }, { 38.1989454804089, 33.9024377389872 }, { 38.1989454804089, 33.9024377389872 }, { 38.1643988720867, 33.8073766968878 }, { 38.1617856340388, 33.7982775554556 }, { 38.2987059968752, 33.8426425828776 }, { 38.2992420565508, 33.8508771812444 } };
        
        int pointCount = coordinates.GetLength(0);
        int length = pointCount;
        int countList = 0;
        int population = 20 * length;
        int generations = 200 * length;
        double mutationRate = 0.1;

        double[] aroundDistance = new double[length]; // uzak mesafe çevresindeki noktalar
        List<double> firstDistance = new List<double>(); // işletmeye uzaklıklar
        List<int> indexesToRemove = new List<int>();
        List<int> remainingIndexes = new List<int>();
        List<int> sortedIndexes = new List<int>();
        List<int> workTime = new List<int>(); // çalışma süresi


        double[] totalDistance = new double[population];
        double fDistance = 0.0;

        Random random = new Random();

        int maxDistance = 6; //uzak noktam
        int minDistance = 2; //uzak noktamın çevresindeki uzaklık
        int number = 0; // uzak noktam için kullandığım sayaç
        int pop = 2; //yoğunluk
        int k = 0;
        int l = 0;


        int parent1 = 0;
        int parent2 = 0;
        int selection1 = 0;
        int selection2 = 0;
        int selection3 = 0;
        int selection4 = 0;


        int maxTime = 20; //mesai


        //işletmeye göre uzak nokta bulma
        for (int j = 0; j < length; j++)
        {
            fDistance = Math.Sqrt(Math.Pow(coordinates[0, 0] - coordinates[j, 0], 2) + Math.Pow(coordinates[0, 1] - coordinates[j, 1], 2));
            firstDistance.Add(fDistance);

            if (fDistance >= maxDistance)
            {
                for (int i = 0; i < length; i++)
                {
                    aroundDistance[i] = Math.Sqrt(Math.Pow(coordinates[j, 0] - coordinates[i, 0], 2) + Math.Pow(coordinates[j, 1] - coordinates[i, 1], 2));
                    if (aroundDistance[i] < minDistance)
                    {
                        number++;
                    }
                }
                //yoğunluğumu sağlamıyorsa rota listemden siliyorum
                if (number <= pop)
                {
                    firstDistance.RemoveAt(firstDistance.Count - 1);
                    indexesToRemove.Add(j);
                }
            }
        }

        //atanabilecek indeks değerlerim
        for (int i = 0; i < length; i++)
        {
            if (!indexesToRemove.Contains(i)) // Eğer indexesToRemove listesinde değilse
            {
                remainingIndexes.Add(i);
            }
        }

        //------------------------------Genetik Algoritma------------------------------------------

        // Popülasyon oluşturma 

        int[,] listIndexes = new int[0, 0]; //popülasyon sayım kadar rota sıralamalarımı tutuyorum

        do
        {
            totalDistance = new double[population];
            listIndexes = new int[population, remainingIndexes.Count + 1];

            for (int p = 0; p < population; p++)
            {

                sortedIndexes = remainingIndexes.Skip(1).OrderBy(x => random.Next()).ToList();
                sortedIndexes.Insert(0, 0); // her zaman işletmeden başlasın
                sortedIndexes.Add(sortedIndexes[0]);
                countList = sortedIndexes.Count;

                double[] distance = new double[countList];

                //her bir popülasyon üyesi için toplam mesafe hesaplama
                for (int j = 0; j < countList; j++)
                {
                    listIndexes[p, j] = sortedIndexes[j];
                }
                //mesafelerin hesaplanması
                for (int i = 0; i < countList - 1; i++)
                {
                    k = sortedIndexes[i];
                    l = sortedIndexes[i + 1];
                    distance[i] = Math.Sqrt(Math.Pow(coordinates[k, 0] - coordinates[l, 0], 2) + Math.Pow(coordinates[k, 1] - coordinates[l, 1], 2));
                    totalDistance[p] += distance[i];
                }
                if (totalDistance[p] <= maxTime)
                {
                    workTime.Add(p);
                }

            }
            //mesai kısıtımı aşıyorsa
            if (workTime.Count == 0)
            {
                int removeIndex = random.Next(countList);
                sortedIndexes.RemoveAt(removeIndex);
                remainingIndexes.RemoveAt(removeIndex);
                countList = sortedIndexes.Count;
            }

        } while (workTime.Count < 1); //mesai kısıtımı sağlayasıya kadar hesaplar

        //başlangıç çözümleri
        Console.WriteLine("Başlangıç Çözümleri:");
        for (int i = 0; i < population; i++)
        {
            Console.WriteLine("{0}.çözüm: {1}", i + 1, totalDistance[i]);
        }

        //jenerasyon sayısı kadar tekrarla
        for (int generation = 0; generation < generations; generation++)
        {

            int[] chosenParent1 = new int[countList];
            int[] chosenParent2 = new int[countList];
            //çocuklar
            int[] child1 = new int[countList];
            int[] child2 = new int[countList];
            int[] bestChild = new int[countList];

            //ebevyn oluşturma
            do
            {
                int randomIndex = random.Next(workTime.Count); //kısıtlarımı sağlayan listemden rastgele bir değer seçiyorum
                selection1 = workTime[randomIndex];
                do
                {
                    int randomIndex2 = random.Next(workTime.Count);
                    selection2 = workTime[randomIndex2];

                } while (selection1 == selection2);
                if (totalDistance[selection1] <= totalDistance[selection2])
                {
                    parent1 = selection1;
                }
                else
                {
                    parent1 = selection2;

                }
                for (int i = 0; i < countList; i++)
                {
                    chosenParent1[i] = listIndexes[parent1, i];
                }
                int randomIndex3 = random.Next(workTime.Count);
                selection3 = workTime[randomIndex3];
                do
                {
                    int randomIndex4 = random.Next(workTime.Count);
                    selection4 = workTime[randomIndex4];

                } while (selection3 == selection4);

                if (totalDistance[selection3] <= totalDistance[selection4])
                {
                    parent2 = selection3;
                }
                else
                {
                    parent2 = selection4;
                }
                for (int j = 0; j < countList; j++)
                {
                    chosenParent2[j] = listIndexes[parent2, j];
                }

            } while (parent1 == parent2); //birbirinden farklı iki tane ebevyn seçtik

            // Çaprazlama

            int startIndex = 1; // İlk indeksin korunması
            int endIndex = sortedIndexes.Count - 1; // Son indeks

            // İki farklı çaprazlama noktası seçme
            int crossOverPoint1 = random.Next(startIndex, endIndex);
            int crossOverPoint2 = random.Next(startIndex, endIndex);

            // Çaprazlama işlemi
            Array.Copy(chosenParent1, child1, chosenParent1.Length);
            Array.Copy(chosenParent2, child2, chosenParent2.Length);
            if (crossOverPoint1 > crossOverPoint2)
            {
                int temp = crossOverPoint1;
                crossOverPoint1 = crossOverPoint2;
                crossOverPoint2 = temp;
            }
            int crossLength = crossOverPoint2 - crossOverPoint1;
            int[] parent1Order = new int[crossLength + 1];
            int[] parent2Order = new int[crossLength + 1];

            //çaprazlanacak kısımlar
            int index = 0;
            for (int j = crossOverPoint1; j <= crossOverPoint2; j++)
            {
                parent1Order[index] = chosenParent1[j];
                parent2Order[index] = chosenParent2[j];

                index++;
            }
            var sameValue = parent1Order.Intersect(parent2Order);
            List<int> e1Order = new List<int>();
            List<int> e2Order = new List<int>();
            //sıralamayı koruyarak çaprazlama yapma
            foreach (int find in sameValue)
            {
                int e1 = Array.IndexOf(chosenParent1, find);
                int e2 = Array.IndexOf(chosenParent2, find);
                e1Order.Add(e1);
                e2Order.Add(e2);
            }
            e2Order.Sort();
            for (int i = 0; i < e1Order.Count; i++)
            {
                child1[e1Order[i]] = chosenParent2[e2Order[i]];
            }
            for (int i = 0; i < e1Order.Count; i++)
            {
                child2[e2Order[i]] = chosenParent1[e1Order[i]];
            }

            // Mutasyon

            if (random.NextDouble() < mutationRate) //çocuk1 için mutasyon olasılığı kontrol ediliyor
            {
                // Rastgele iki gen değeri seçilerek değiştiriliyor
                int mutation1Point1 = random.Next(startIndex, endIndex);
                int mutation1Point2 = random.Next(startIndex, endIndex);
                int temp2 = child1[mutation1Point1];
                child1[mutation1Point1] = child1[mutation1Point2];
                child1[mutation1Point2] = temp2;
            }
            if (random.NextDouble() < mutationRate) //çocuk2 için mutasyon olasılığı kontrol ediliyor
            {
                // Rastgele iki gen değeri seçilerek değiştiriliyor
                int mutation2Point1 = random.Next(startIndex, endIndex);
                int mutation2Point2 = random.Next(startIndex, endIndex);
                int temp2 = child2[mutation2Point1];
                child2[mutation2Point1] = child2[mutation2Point2];
                child2[mutation2Point2] = temp2;
            }

            //çocukların amaç fonksiyonunu bulma
            double[] child1Distance = new double[countList];
            double[] child2Distance = new double[countList];
            double totalDistance1 = 0.0;
            double totalDistance2 = 0.0;


            for (int i = 0; i < countList - 1; i++)
            {
                int child1Point1 = child1[i];
                int child1Point2 = child1[i + 1];
                int child2Point1 = child1[i];
                int child2Point2 = child1[i + 1];
                child1Distance[i] = Math.Sqrt(Math.Pow(coordinates[child1Point1, 0] - coordinates[child1Point2, 0], 2) + Math.Pow(coordinates[child1Point1, 1] - coordinates[child1Point2, 1], 2));
                child2Distance[i] = Math.Sqrt(Math.Pow(coordinates[child2Point1, 0] - coordinates[child2Point2, 0], 2) + Math.Pow(coordinates[child2Point1, 1] - coordinates[child2Point2, 1], 2));
                totalDistance1 += child1Distance[i];
                totalDistance2 += child2Distance[i];

            }
            //çocukların amaç fonksiyonunu kıyaslayıp en iyi çocuğu bulma
            if (totalDistance1 <= totalDistance2)
            {
                bestChild = child1;
            }
            else
            {
                bestChild = child2;
                totalDistance1 = totalDistance2;
            }

            //en kötü atılır 
            double maxVal = totalDistance.Max();
            int maxValIndex = Array.IndexOf(totalDistance, maxVal);
            //çocuk en kötüden iyiyse listeye eklenir
            if (totalDistance1 <= totalDistance[maxValIndex])
            {

                for (int i = 0; i < countList; i++)
                {
                    listIndexes[maxValIndex, i] = bestChild[i];

                }
                totalDistance[maxValIndex] = totalDistance1;
            }
        }

        //optimal rotam
        double minVal = totalDistance.Min();
        int minValIndex = Array.IndexOf(totalDistance, minVal);
        Console.WriteLine();
        Console.WriteLine("En Kısa Uzaklık: " + totalDistance[minValIndex]);
        Console.Write("En Kısa Rota: ");
        for (int i = 0; i < countList; i++)
        {
            Console.Write(listIndexes[minValIndex, i]);
            if (i != countList - 1)
            {
                Console.Write("->");
            }
        }
        Console.ReadLine();
    }
}
