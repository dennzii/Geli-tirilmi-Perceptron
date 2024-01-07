using System;

public class Neuron
{
	//Bölüm sabitleri
	

	public double lambda;//Ögrenme katsayısı

	public double inp1_weight;
	public double inp2_weight;

	public string inp1_description;
	public string inp2_description;

	public Neuron(double _lambda, double inp1_wei,string inp1_description, double inp2_wei,string inp2_description)
	{
		this.lambda = _lambda;
		this.inp1_weight = inp1_wei;
		this.inp2_weight = inp2_wei;

		this.inp1_description = inp1_description;
		this.inp2_description = inp2_description;
	}

	public void Train(double inp1, double inp2, double target)
	{
		//Outputun, input ve ağırlıkların çarpımıyla hesaplanması
		double output = inp1 * inp1_weight +
			inp2 * inp2_weight;

		

		//Ağırlık atamaları
		
	}

	public double calc_output(double _inp1, double _inp2)
	{
		return (_inp1 * inp1_weight + _inp2 * inp2_weight);
	}

	public void ChangeWeight(double target, double output, double input, ref double weight)
	{
		weight = weight + lambda * (target - output) * input;
		//Console.WriteLine(inp1_description + " " + inp1_weight + inp2_description + " " + inp2_weight);
	}


}
