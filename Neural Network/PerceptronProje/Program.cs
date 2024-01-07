using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerceptronProje
{
    class Program
    {
        //İterasyon sayısı
        public static int EPOCH = 100000;

        public static double[] age_list_max_min = new double[2];
        public static double[] Age_list = new double[1339];

        public static bool[,] Sex_list = new bool[1339,2];

        public static double[] bmi_list_max_min = new double[2];
        public static double[] BMIs_list = new double[1339];

        public static bool[,] Smokes_list = new bool[1339,2];

        public static double[] children_list_max_min = new double[2];
        public static double[] Childeren_list = new double[1339];

        public static bool[,] Region_list = new bool[1339,4];

        public static double[] charges_list_max_min = new double[2];
        public static double[] Charges_list = new double[1339];

        public static double[] MSEs = new double[EPOCH];

        public static Neuron neuron1;
        public static Neuron neuron2;
        public static OneHotEncodingNeuron region_neuron;
        public static OneHotEncodingNeuron sex_neuron;
        public static OneHotEncodingNeuron smokes_neuron;

        public static Decider neuron_decider;
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            //epoch sayısı 10 ve ogrene katsayısı 0.05 olarak verildi.
            using (var reader = new StreamReader(@"C:\Users\deniz\Desktop\insurance.csv"))
            {
                int i = 0;

                reader.ReadLine();//ilk satırı atla

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    Age_list[i] = double.Parse(values[0]);


                    //One Hot Encoding
                    if (values[1] == "female")// erkek, kadın => [0,1] kadın
                    {
                        Sex_list[i, 0] = false;
                        Sex_list[i, 1] = true;
                    } 
                    else if (values[1] == "male")
                    {
                        Sex_list[i, 0] = true;
                        Sex_list[i, 1] = false;
                    }
                       

                    BMIs_list[i] = double.Parse(values[2]);
                    Childeren_list[i] = double.Parse(values[3]);

                    if (values[4] == "no")
                    {
                        Smokes_list[i,0] = true;
                        Smokes_list[i, 0] = false;
                    }
                    else if (values[4] == "yes")
                    {
                        Smokes_list[i,0] = false;
                        Smokes_list[i, 0] = true;
                    }
                        

                    //One Hot coding
                    if (values[5] == "northwest")// northwest = 1,0,0,0 northeast = 0,1,0,0 southwest = 0,0,1,0 southeast = 0,0,0,1
                    {
                        Region_list[i, 0] = true;
                        Region_list[i, 1] = false;
                        Region_list[i, 2] = false;
                        Region_list[i, 3] = false;
                    }
                    else if (values[5] == "northeast")
                    {
                        Region_list[i, 0] = false;
                        Region_list[i, 1] = true;
                        Region_list[i, 2] = false;
                        Region_list[i, 3] = false;
                    }
                    else if (values[5] == "southwest")
                    {
                        Region_list[i, 0] = false;
                        Region_list[i, 1] = false;
                        Region_list[i, 2] = true;
                        Region_list[i, 3] = false;
                    }
                    else if (values[5] == "southeast")
                    {
                        Region_list[i, 0] = false;
                        Region_list[i, 1] = false;
                        Region_list[i, 2] = false;
                        Region_list[i, 3] = true;
                    }

                    Charges_list[i] = double.Parse(values[6]);
                
                    i++;
                }
            }
            Console.WriteLine("Age list normalizasyonu yapılıyor..");
            NormalizeDataset(Age_list,age_list_max_min);
            Console.WriteLine("BMI list normalizasyonu yapılıyor..");
            NormalizeDataset(BMIs_list,bmi_list_max_min);
            Console.WriteLine("Children list normalizasyonu yapılıyor..");
            NormalizeDataset(Childeren_list,children_list_max_min);
            Console.WriteLine("Charges list normalizasyonu yapılıyor..");
            NormalizeDataset(Charges_list, charges_list_max_min);



            Trainer(EPOCH, 0.000001f);

            foreach (float f in MSEs)
                Console.WriteLine(f);

            Console.WriteLine("Tests");

            for (int i = 1300; i < 1339; i++)
                Console.WriteLine("Target:" +Denormalize(charges_list_max_min, Charges_list[i]) + "Precision:" +
                    Denormalize(charges_list_max_min, neuron_decider.calc_output(neuron1.calc_output(Age_list[i], Childeren_list[i]),
                    neuron2.calc_output(BMIs_list[i] , 0),
                    region_neuron.calc_output(Region_list[i, 0], Region_list[i, 1], Region_list[i, 2], Region_list[i, 3]),
                    sex_neuron.calc_output(Sex_list[i, 0], Sex_list[i, 1], false, false),
                    smokes_neuron.calc_output(Smokes_list[i,0],Smokes_list[i,1],false,false))));

            Console.ReadLine();
        }

        static void NormalizeDataset(double[] dataset,double[] min_max_arr)//Daha isabetli sonuçlar için min-max normalizasyonu
        {
           
            (double min, double max) = getMinMax(dataset);

            min_max_arr[0] = min;
            min_max_arr[1] = max;

            for(int i = 0;i<dataset.Length;i++)
            {
                double val = dataset[i];

               double newVal = NormalizeDouble(min, max,  val);
                
                dataset[i] = newVal;
            }
        }

        static double NormalizeDouble(double min, double max,double val)
        {
            return (val - min) / (max - min);
        }

        static double Denormalize(double[] arr, double normal_val)
        {
            //arr[0] => min
            //arr[1] => max
            return normal_val * (arr[1] - arr[0]) + arr[0];
        }

        static (double,double) getMinMax(double[] dataset)
        {
            // 0. indis => min,
            // 1. indis => max değeri döndürü.

            double min = double.MaxValue;
            double max = -1;

            for(int i = 0;i<dataset.Length;i++)
            {
                if (dataset[i] < min)
                    min = dataset[i];
                else if (dataset[i] > max)
                    max = dataset[i];
            }
           

            return (min,max);
        }

        static void Trainer(int epoch, double lambda)
        {
            Console.WriteLine("Model, Epoch = "+ EPOCH.ToString() +" λ =" + lambda.ToString() + "konfigurasyonuyla eğitiliyor...");

            neuron1 = new Neuron(0.001f, 0.2, "age", 0.2, "children");
            neuron2 = new Neuron(0.001f, 0.2, "bmi", 0.2, "smoke");

            region_neuron = new OneHotEncodingNeuron(0.0001f, 0.2, 0.2, 0.2, 0.2);
            sex_neuron = new OneHotEncodingNeuron(0.0001f, 0.2, 0.2, 0.2, 0.2);
            smokes_neuron = new OneHotEncodingNeuron(0.0001f, 0.2, 0.2, 0.2, 0.2);

            neuron_decider = new Decider(lambda, 0.2, 0.2, 0.2,0.2,0.2);


            for(int i = 0; i < epoch;i++)
            {
                //  Console.WriteLine(Age_list[i] + " " + BMIs_list[i] + " " + Childeren_list[i] + " " + Smokes_list[i] + " " + Region_list[i] + " " + Charges_list[i]);
                double mse = 0;

                if(i%10000 == 0)
                Console.WriteLine("epoch " + i.ToString());

                for(int j = 0; j < 1339;j++)
                {

                    double inp1_de = neuron1.calc_output(Age_list[j], Childeren_list[j]);
                    double inp2_de = neuron2.calc_output(BMIs_list[j],0);

                    double region_inp_de = region_neuron.calc_output(Region_list[j, 0], Region_list[j, 0], Region_list[j, 0], Region_list[j, 0]);
                    double sex_inp_de = sex_neuron.calc_output(Sex_list[j, 0], Sex_list[j, 1], false, false);//cinsiyette iki boolean yetiyor.
                    double smokes_inp_de = sex_neuron.calc_output(Smokes_list[j, 0], Smokes_list[j, 1], false, false); 


                    double wei1 = neuron_decider.inp1_weight;
                    double wei2 = neuron_decider.inp2_weight;
                    double wei3 = neuron_decider.inp3_weight;
                    double wei4 = neuron_decider.inp4_weight;
                    double wei5 = neuron_decider.inp5_weight;


                    neuron_decider.Train(inp1_de, inp2_de, region_inp_de,sex_inp_de,smokes_inp_de ,Charges_list[j]);

                    double outp = neuron_decider.calc_output(inp1_de, inp2_de, region_inp_de,sex_inp_de,smokes_inp_de);

                    mse += (Charges_list[j]  - outp) * (Charges_list[j] - outp);

                    //Console.WriteLine("Target:" + (Charges_list[j]/100) + "out:" + outp.ToString());
                    // Console.WriteLine("MSE:")

                    neuron1.ChangeWeight(neuron_decider.inp1_weight,wei1, Age_list[j], ref neuron1.inp1_weight);
                    neuron1.ChangeWeight(neuron_decider.inp1_weight , wei1, Childeren_list[j], ref neuron1.inp2_weight);

                    neuron2.ChangeWeight(neuron_decider.inp2_weight , wei2 , BMIs_list[j], ref neuron2.inp1_weight);
                    //neuron2.ChangeWeight(neuron_decider.inp2_weight , wei2 , Smokes_list[j], ref neuron2.inp2_weight);

                    region_neuron.ChangeWeight(neuron_decider.inp3_weight , wei3 ,Convert.ToDouble( Region_list[j,0]), ref region_neuron.inp1_weight);
                    region_neuron.ChangeWeight(neuron_decider.inp3_weight , wei3 , Convert.ToDouble(Region_list[j, 1]), ref region_neuron.inp2_weight);
                    region_neuron.ChangeWeight(neuron_decider.inp3_weight, wei3, Convert.ToDouble(Region_list[j, 2]), ref region_neuron.inp3_weight);
                    region_neuron.ChangeWeight(neuron_decider.inp3_weight, wei3, Convert.ToDouble(Region_list[j, 3]), ref region_neuron.inp4_weight);

                    sex_neuron.ChangeWeight(neuron_decider.inp4_weight, wei4, Convert.ToDouble(Sex_list[j, 0]), ref sex_neuron.inp1_weight);
                    sex_neuron.ChangeWeight(neuron_decider.inp4_weight, wei4, Convert.ToDouble(Sex_list[j, 1]), ref sex_neuron.inp2_weight);

                    smokes_neuron.ChangeWeight(neuron_decider.inp5_weight, wei5, Convert.ToDouble(Smokes_list[j, 0]), ref smokes_neuron.inp1_weight);
                    smokes_neuron.ChangeWeight(neuron_decider.inp5_weight, wei5, Convert.ToDouble(Smokes_list[j, 1]), ref smokes_neuron.inp2_weight);

                }

                MSEs[i] = mse / 1339;
                Console.WriteLine("Epoch:"+i+" mse:"+MSEs[i]);
                mse = 0;
            }

          
        }

        //Mean Square Error Hesaplaması
        /*
        static float CalculateMSE()
        {
            float mse = 0;

            for (int j = 0; j < DATASET_SIZE; j++)
                mse += (dataset[j, 2] / 100 - perceptron.prec(dataset[j, 0], dataset[j, 1])) * (dataset[j, 2] / 100 - perceptron.prec(dataset[j, 0], dataset[j, 1]));

            mse = mse / DATASET_SIZE;

            return mse;
        }
        */
    }

}
