using System;

public class Neuron
{
	//Bölüm sabitleri
	public const int CALISMA_BOLUM_DEGERI = 10;
	public const int DERSE_DEVAM_BOLUM_DEGERI = 15;
	public const int SINAV_SONUCU_BOLUM_DEGERI = 100;

	public float lambda;
	public float calisma_suresi_weight;
	public float derse_devam_suresi_weight;

	public Neuron(float _lambda,float _calisma_suresi_weight,float _derse_devam_suresi_weight)
    {
        this.lambda = _lambda;
		this.calisma_suresi_weight = _calisma_suresi_weight;
		this.derse_devam_suresi_weight = _derse_devam_suresi_weight;
    }

    public void Train(float calisma_sur_inp,float derse_dev_inp,float target)
    {
		//Outputun, input ve ağırlıkların çarpımıyla hesaplanması
		float output = calisma_sur_inp / CALISMA_BOLUM_DEGERI * calisma_suresi_weight + 
			derse_dev_inp / DERSE_DEVAM_BOLUM_DEGERI * derse_devam_suresi_weight;

		//Ağırlık atamaları
		ChangeWeight(target / SINAV_SONUCU_BOLUM_DEGERI , output, calisma_sur_inp, calisma_suresi_weight);
		ChangeWeight(target / SINAV_SONUCU_BOLUM_DEGERI, output, derse_dev_inp, derse_devam_suresi_weight);
    }

	public float prec(float calisma_sur_inp, float derse_dev_inp)
	{
		return (calisma_suresi_weight * calisma_sur_inp + derse_devam_suresi_weight * derse_dev_inp);
	}

	public void ChangeWeight(float target,float output,float input,ref float weight)
    {
		weight = lambda * (target - output) * input; 
    }

	
}
