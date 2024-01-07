using System;

public class OneHotEncodingNeuron
{
	//Bölüm sabitleri


	public double lambda;//Ögrenme katsayısı

	public double inp1_weight;
	public double inp2_weight;
	public double inp3_weight;
	public double inp4_weight;


	public OneHotEncodingNeuron(double _lambda, double inp1_wei, double inp2_wei, double inp3_wei, double inp4_wei)
	{
		//4 bit giriş 4 farklı durumu ifade eder.
		this.lambda = _lambda;
		this.inp1_weight = inp1_wei;
		this.inp2_weight = inp2_wei;
		this.inp1_weight = inp1_wei;
		this.inp2_weight = inp2_wei;
	}

	public void Train(double inp1, double inp2, double target)
	{
		//Outputun, input ve ağırlıkların çarpımıyla hesaplanması
		double output = inp1 * inp1_weight +
			inp2 * inp2_weight;

	

		//Ağırlık atamaları

	}

	public double calc_output(bool _inp1, bool _inp2,bool _inp3, bool _inp4)
	{
		return (Convert.ToDouble(_inp1) * inp1_weight + Convert.ToDouble(_inp2) * inp2_weight + Convert.ToDouble(_inp3) * inp3_weight + Convert.ToDouble(_inp4) * inp4_weight);
	}

	public void ChangeWeight(double target, double output, double input, ref double weight)
	{
		weight = weight + lambda * (target - output) * input;
		//Console.WriteLine(inp1_description + " " + inp1_weight + inp2_description + " " + inp2_weight);
	}


}
