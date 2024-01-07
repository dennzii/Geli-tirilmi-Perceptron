using System;

public class Decider
{
	//Bölüm sabitleri


	public double lambda;//Ögrenme katsayısı
	

	public double inp1_weight;
	public double inp2_weight;
	public double inp3_weight;
	public double inp4_weight;
	public double inp5_weight;


	public Decider(double _lambda, double inp1_wei, double inp2_wei, double inp3_wei,double  inp4_wei, double inp5_wei)
	{
		this.lambda = _lambda;
		this.inp1_weight = inp1_wei;
		this.inp2_weight = inp2_wei;
		this.inp3_weight = inp3_wei;
		this.inp4_weight = inp4_wei;
		this.inp5_weight = inp5_wei;
	}

	public void Train(double inp1, double inp2, double inp3, double inp4,double inp5, double target)
	{
		//Outputun, input ve ağırlıkların çarpımıyla hesaplanması
		double output = inp1 * inp1_weight +
			inp2 * inp2_weight + inp3 * inp3_weight +inp4 * inp4_weight;

		//Console.WriteLine("inp1_wei:" + inp1_weight  + " inp2_wei:" + inp2_weight+" inp3_wei:"+inp3_weight+" Target");
		ChangeWeight(target, output, inp1, ref inp1_weight);
		ChangeWeight(target, output, inp2, ref inp2_weight);
		ChangeWeight(target, output, inp3, ref inp3_weight);
		ChangeWeight(target, output, inp4, ref inp4_weight);
		ChangeWeight(target, output, inp5, ref inp5_weight);

		//Ağırlık atamaları

	}

	public double calc_output(double inp1, double inp2, double inp3,double inp4,double inp5)
	{
		return (inp1 * inp1_weight + inp2 * inp2_weight + inp3 * inp3_weight + inp3 * inp3_weight + inp4 * inp4_weight + inp5 *inp5_weight);
	}

	public void ChangeWeight(double target, double output, double input, ref double weight)
	{
		weight = weight + lambda * (target - output) * input;
	}


}
